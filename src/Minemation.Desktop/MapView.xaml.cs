using MaterialDesignThemes.Wpf;
using Minemation.Desktop.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Minemation.Desktop;

public partial class MapView : UserControl, INotifyPropertyChanged
{
    private bool _isPersonnelVisible = true;
    private bool _isEquipmentVisible = true;
    private bool _isEntrancesVisible = true;
    private bool _isTaskPointsVisible = true;
    private bool _isGraphVisible = true;
    private bool _isRouteVisible = true;
    private bool _isDragging;
    private bool _isPersonnelDetailVisible;
    private bool _isPersonnelClusterVisible = true;
    private bool _isPersonnelMarkersVisible = false;
    private bool _isClusterDetailPanelVisible;
    private bool _isEquipmentClusterVisible = true;
    private bool _isEquipmentMarkersVisible;

    private string _selectedClusterTitle = string.Empty;
    private string _selectedClusterDescription = string.Empty;

    private const double MinZoom = 0.6;
    private const double MaxZoom = 3.5;
    private const double ZoomStep = 1.15;
    private double _markerVisualScale = 1;

    
    private Point _lastMousePosition;

    private readonly int _currentPersonelId;
    private readonly bool _canManage;

    private readonly HttpClient _httpClient = new()
    {
        BaseAddress = new Uri("http://localhost:5289")
    };

    public ObservableCollection<MapMarker> Entrances { get; } = new();
    public ObservableCollection<MapMarker> Equipments { get; } = new();
    public ObservableCollection<MapMarker> Personnels { get; } = new();
    public ObservableCollection<MapMarker> TaskPoints { get; } = new();
    public ObservableCollection<MapNode> Nodes { get; } = new();
    public ObservableCollection<MapEdge> Edges { get; } = new();
    public ObservableCollection<MapEdge> RouteEdges { get; } = new();
    public ObservableCollection<MapClusterMarker> PersonnelClusters { get; } = new();
    public ObservableCollection<string> SelectedClusterItems { get; } = new();
    public ObservableCollection<MapEquipmentClusterMarker> EquipmentClusters { get; } = new();

    public bool IsPersonnelVisible
    {
        get => _isPersonnelVisible;
        set
        {
            _isPersonnelVisible = value;
            OnPropertyChanged();
            UpdatePersonnelVisibilityByZoom();
        }
    }

    public bool IsEquipmentVisible
    {
        get => _isEquipmentVisible;
        set
        {
            _isEquipmentVisible = value;
            OnPropertyChanged();
            UpdateEquipmentVisibilityByZoom();
        }
    }

    public bool IsEntrancesVisible
    {
        get => _isEntrancesVisible;
        set
        {
            _isEntrancesVisible = value;
            OnPropertyChanged();
        }
    }

    public bool IsTaskPointsVisible
    {
        get => _isTaskPointsVisible;
        set
        {
            _isTaskPointsVisible = value;
            OnPropertyChanged();
        }
    }

    public bool IsGraphVisible
    {
        get => _isGraphVisible;
        set
        {
            _isGraphVisible = value;
            OnPropertyChanged();
        }
    }

    public bool IsRouteVisible
    {
        get => _isRouteVisible;
        set
        {
            _isRouteVisible = value;
            OnPropertyChanged();
        }
    }

    public MapView(int currentPersonelId = 0, bool canManage = true)
    {
        _currentPersonelId = currentPersonelId;
        _canManage = canManage;

        InitializeComponent();

        DataContext = this;
        Loaded += async (_, _) => await LoadMapDataAsync();
    }

    private async void BtnRefresh_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        await LoadMapDataAsync();
    }

    private async Task LoadMapDataAsync()
    {
        Nodes.Clear();
        Edges.Clear();
        RouteEdges.Clear();
        Entrances.Clear();
        Equipments.Clear();
        Personnels.Clear();
        PersonnelClusters.Clear();
        TaskPoints.Clear();
        EquipmentClusters.Clear();

        LoadGraph();
        BuildHighlightedRoute();

        LoadEntrances();

        await LoadEquipmentsFromApiAsync();
        await LoadPersonnelMarkersFromApiAsync();

        LoadTaskPoints();
    }

    private async Task LoadEquipmentsFromApiAsync()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<PagedResponse<MapEquipmentDto>>>(
                "/api/ekipman?SayfaNumarasi=1&SayfaBoyutu=1000");

            if (response?.Success != true || response.Data is null)
            {
                LoadEquipments();
                BuildEquipmentClustersFromCurrentMarkers();
                UpdateEquipmentVisibilityByZoom();
                return;
            }

            var visibleEquipments = response.Data.Items
                .Where(e => !IsTrackingDevice(e))
                .ToList();

            var groupedByNode = visibleEquipments
                .Select(equipment => new
                {
                    Equipment = equipment,
                    Node = GetNodeForEquipment(equipment)
                })
                .Where(x => x.Node is not null)
                .GroupBy(x => x.Node!.NodeId)
                .ToList();

            foreach (var group in groupedByNode)
            {
                var node = FindNode(group.Key);

                if (node is null)
                    continue;

                var equipmentsInNode = group.Select(x => x.Equipment).ToList();

                EquipmentClusters.Add(new MapEquipmentClusterMarker
                {
                    NodeId = node.NodeId,
                    Name = node.Name,
                    Description = $"{node.Name} bölgesinde {equipmentsInNode.Count} ekipman bulunuyor.",
                    X = node.X + 34,
                    Y = node.Y + 34,
                    EquipmentCount = equipmentsInNode.Count,
                    EquipmentNames = equipmentsInNode
                        .Select(e =>
                        {
                            var name = string.IsNullOrWhiteSpace(e.EkipmanAdi)
                                ? $"Ekipman-{e.EkipmanId}"
                                : e.EkipmanAdi;

                            return $"{name} - {TextOrDefault(e.EkipmanTuru)}";
                        })
                        .ToList()
                });

                for (var i = 0; i < equipmentsInNode.Count; i++)
                {
                    var equipment = equipmentsInNode[i];
                    var position = GetOffsetPosition(node.X + 18, node.Y + 18, i);

                    Equipments.Add(new MapMarker
                    {
                        Name = string.IsNullOrWhiteSpace(equipment.EkipmanAdi)
                            ? $"Ekipman-{equipment.EkipmanId}"
                            : equipment.EkipmanAdi,
                        Description =
                            $"Tür: {TextOrDefault(equipment.EkipmanTuru)}\n" +
                            $"Durum: {TextOrDefault(equipment.Durum)}",
                        X = position.X,
                        Y = position.Y,
                        IconKind = GetEquipmentIcon(equipment.EkipmanTuru)
                    });
                }
            }

            UpdateEquipmentVisibilityByZoom();
        }
        catch
        {
            LoadEquipments();
            BuildEquipmentClustersFromCurrentMarkers();
            UpdateEquipmentVisibilityByZoom();
        }
    }

    private void LoadEntrances()
    {
        Entrances.Add(new MapMarker
        {
            Name = "Ana Giriş",
            Description = "Maden sahası ana giriş noktası",
            X = 115,
            Y = 135,
            IconKind = PackIconKind.Gate
        });

        Entrances.Add(new MapMarker
        {
            Name = "Acil Çıkış",
            Description = "Acil durum tahliye noktası",
            X = 760,
            Y = 420,
            IconKind = PackIconKind.ExitRun
        });

        Entrances.Add(new MapMarker
        {
            Name = "Batı Girişi",
            Description = "Batı ocağı servis girişi",
            X = 280,
            Y = 520,
            IconKind = PackIconKind.GateArrowRight
        });
    }

    private void LoadEquipments()
    {
        Equipments.Add(new MapMarker
        {
            Name = "Ekskavatör-01",
            Description = "Batı ocağı kazı bölgesinde aktif ekipman",
            X = 410,
            Y = 285,
            IconKind = PackIconKind.Excavator
        });

        Equipments.Add(new MapMarker
        {
            Name = "Kamyon-03",
            Description = "Taşıma hattında görevli kamyon",
            X = 575,
            Y = 350,
            IconKind = PackIconKind.Truck
        });

        Equipments.Add(new MapMarker
        {
            Name = "Jeneratör",
            Description = "Enerji destek ekipmanı",
            X = 690,
            Y = 185,
            IconKind = PackIconKind.Engine
        });
    }

    private void LoadPersonnelMarkers()
    {
        var allPersonnel = new List<MapMarker>
        {
            new()
            {
                PersonelId = 2,
                Name = "Berke",
                Description = "Görev Yeri: Batı Ocağı - Dekapaj Bölgesi",
                X = 445,
                Y = 245,
                IconKind = PackIconKind.AccountHardHat
            },
            new()
            {
                PersonelId = 3,
                Name = "Elif",
                Description = "Görev Yeri: Ana Galeri - Kontrol Noktası",
                X = 610,
                Y = 300,
                IconKind = PackIconKind.AccountHardHat
            },
            new()
            {
                PersonelId = 1,
                Name = "Büşra",
                Description = "Görev Yeri: Yönetim / Merkez Kontrol",
                X = 185,
                Y = 210,
                IconKind = PackIconKind.AccountTieHat
            }
        };

        var visiblePersonnel = _canManage
            ? allPersonnel
            : allPersonnel.Where(x => x.PersonelId == _currentPersonelId).ToList();

        foreach (var person in visiblePersonnel)
            Personnels.Add(person);
    }

    private async Task LoadPersonnelMarkersFromApiAsync()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<PagedResponse<MapPersonelDto>>>(
                "/api/personel?SayfaNumarasi=1&SayfaBoyutu=1000");

            if (response?.Success != true || response.Data is null)
            {
                LoadPersonnelMarkers();
                BuildPersonnelClustersFromCurrentMarkers();
                UpdatePersonnelVisibilityByZoom();
                return;
            }

            var visiblePersonnel = _canManage
                ? response.Data.Items
                : response.Data.Items.Where(x => x.PersonelId == _currentPersonelId).ToList();

            var groupedByNode = visiblePersonnel
                .Select(personel => new
                {
                    Personel = personel,
                    Node = GetNodeForPersonnel(personel)
                })
                .Where(x => x.Node is not null)
                .GroupBy(x => x.Node!.NodeId)
                .ToList();

            foreach (var group in groupedByNode)
            {
                var node = FindNode(group.Key);

                if (node is null)
                    continue;

                var personnelInNode = group.Select(x => x.Personel).ToList();

                PersonnelClusters.Add(new MapClusterMarker
                {
                    NodeId = node.NodeId,
                    Name = node.Name,
                    Description = $"{node.Name} bölgesinde {personnelInNode.Count} personel bulunuyor.",
                    X = node.X,
                    Y = node.Y,
                    PersonnelCount = personnelInNode.Count,
                    PersonnelNames = personnelInNode
                        .Select(p => $"{p.AdSoyad} - {TextOrDefault(p.Uzmanlik)}")
                        .ToList()
                });

                for (var i = 0; i < personnelInNode.Count; i++)
                {
                    var personel = personnelInNode[i];
                    var position = GetOffsetPosition(node.X, node.Y, i);

                    Personnels.Add(new MapMarker
                    {
                        PersonelId = personel.PersonelId,
                        Name = personel.AdSoyad,
                        Description =
                            $"Görev/Konum: {TextOrDefault(personel.CalismaKonumu)}\n" +
                            $"Departman: {TextOrDefault(personel.Departman)}\n" +
                            $"Uzmanlık: {TextOrDefault(personel.Uzmanlik)}\n" +
                            $"Durum: {TextOrDefault(personel.PersonelDurumu)}",
                        X = position.X,
                        Y = position.Y,
                        IconKind = IsAdminRole(personel.KullaniciRolu)
                            ? PackIconKind.AccountTieHat
                            : PackIconKind.AccountHardHat
                    });
                }
            }

            UpdatePersonnelVisibilityByZoom();
        }
        catch
        {
            LoadPersonnelMarkers();
            BuildPersonnelClustersFromCurrentMarkers();
            UpdatePersonnelVisibilityByZoom();
        }
    }

    private void LoadTaskPoints()
    {
        var allTaskPoints = new List<MapMarker>
        {
            new()
            {
                AssignedPersonelId = 2,
                Name = "Görev-01",
                Description = "Batı Ocağı günlük saha kontrol görevi",
                X = 475,
                Y = 220,
                IconKind = PackIconKind.ClipboardCheck
            },
            new()
            {
                AssignedPersonelId = 3,
                Name = "Görev-02",
                Description = "Ana Galeri güvenlik kontrol görevi",
                X = 640,
                Y = 275,
                IconKind = PackIconKind.ClipboardCheck
            },
            new()
            {
                AssignedPersonelId = null,
                Name = "Riskli Bölge",
                Description = "Sensör eşik dışı ölçüm alanı",
                X = 530,
                Y = 430,
                IconKind = PackIconKind.Alert
            }
        };

        var visibleTasks = _canManage
            ? allTaskPoints
            : allTaskPoints
                .Where(x => x.AssignedPersonelId == _currentPersonelId)
                .ToList();

        foreach (var task in visibleTasks)
            TaskPoints.Add(task);
    }

    private void LoadGraph()
    {
        var anaGiris = new MapNode
        {
            NodeId = "ana-giris",
            Name = "Ana Giriş",
            Description = "Maden sahası ana giriş noktası",
            X = 115,
            Y = 135
        };

        var kontrol = new MapNode
        {
            NodeId = "kontrol",
            Name = "Kontrol Noktası",
            Description = "Saha giriş kontrol noktası",
            X = 260,
            Y = 210
        };

        var batiOcagi = new MapNode
        {
            NodeId = "bati-ocagi",
            Name = "Batı Ocağı",
            Description = "Dekapaj ve saha kontrol bölgesi",
            X = 445,
            Y = 245
        };

        var anaGaleri = new MapNode
        {
            NodeId = "ana-galeri",
            Name = "Ana Galeri",
            Description = "Yer altı ana geçiş hattı",
            X = 610,
            Y = 300
        };

        var yukleme = new MapNode
        {
            NodeId = "yukleme",
            Name = "Yükleme Alanı",
            Description = "Malzeme yükleme ve taşıma bölgesi",
            X = 575,
            Y = 350
        };

        var riskliBolge = new MapNode
        {
            NodeId = "riskli-bolge",
            Name = "Riskli Bölge",
            Description = "Sensör eşik dışı ölçüm alanı",
            X = 530,
            Y = 430,
            IsRisky = true
        };

        var acilCikis = new MapNode
        {
            NodeId = "acil-cikis",
            Name = "Acil Çıkış",
            Description = "Acil tahliye çıkış noktası",
            X = 760,
            Y = 420
        };

        Nodes.Add(anaGiris);
        Nodes.Add(kontrol);
        Nodes.Add(batiOcagi);
        Nodes.Add(anaGaleri);
        Nodes.Add(yukleme);
        Nodes.Add(riskliBolge);
        Nodes.Add(acilCikis);

        AddEdge(anaGiris, kontrol);
        AddEdge(kontrol, batiOcagi);
        AddEdge(batiOcagi, anaGaleri);
        AddEdge(anaGaleri, yukleme);
        // Riskli bağlantılar turuncu/kesikli gösterilir ve rota algoritmasında daha yüksek maliyetlidir.

        AddEdge(yukleme, riskliBolge, isRisky: true);
        AddEdge(anaGaleri, acilCikis);
        AddEdge(kontrol, acilCikis);
    }

    private void BuildHighlightedRoute()
    {
        if (Nodes.Count == 0 || Edges.Count == 0)
            return;

        var startNodeId = "ana-giris";
        var targetNodeId = GetTargetNodeForCurrentUser();

        if (string.IsNullOrWhiteSpace(targetNodeId))
            return;

        var path = FindShortestPath(startNodeId, targetNodeId);

        if (path.Count < 2)
            return;

        for (var i = 0; i < path.Count - 1; i++)
        {
            var from = Nodes.FirstOrDefault(x => x.NodeId == path[i]);
            var to = Nodes.FirstOrDefault(x => x.NodeId == path[i + 1]);

            if (from is null || to is null)
                continue;

            RouteEdges.Add(new MapEdge
            {
                FromNodeId = from.NodeId,
                ToNodeId = to.NodeId,
                X1 = from.X,
                Y1 = from.Y,
                X2 = to.X,
                Y2 = to.Y,
                IsRisky = false,
                Weight = 1
            });
        }
    }

    private string GetTargetNodeForCurrentUser()
    {
        if (_canManage)
            return "bati-ocagi";

        return _currentPersonelId switch
        {
            2 => "bati-ocagi",
            3 => "ana-galeri",
            1 => "kontrol",
            _ => "kontrol"
        };
    }

    private List<string> FindShortestPath(string startNodeId, string targetNodeId)
    {
        var distances = Nodes.ToDictionary(x => x.NodeId, _ => double.PositiveInfinity);
        var previous = Nodes.ToDictionary(x => x.NodeId, _ => string.Empty);
        var unvisited = Nodes.Select(x => x.NodeId).ToHashSet();

        distances[startNodeId] = 0;

        while (unvisited.Count > 0)
        {
            var current = unvisited
                .OrderBy(x => distances[x])
                .FirstOrDefault();

            if (string.IsNullOrWhiteSpace(current))
                break;

            if (current == targetNodeId)
                break;

            unvisited.Remove(current);

            var neighbors = Edges
                .Where(x => x.FromNodeId == current || x.ToNodeId == current)
                .ToList();

            foreach (var edge in neighbors)
            {
                var neighbor = edge.FromNodeId == current
                    ? edge.ToNodeId
                    : edge.FromNodeId;

                if (!unvisited.Contains(neighbor))
                    continue;

                var riskPenalty = edge.IsRisky ? 8 : 0;
                var weight = edge.Weight <= 0 ? 1 : edge.Weight;
                var candidateDistance = distances[current] + weight + riskPenalty;

                if (candidateDistance < distances[neighbor])
                {
                    distances[neighbor] = candidateDistance;
                    previous[neighbor] = current;
                }
            }
        }

        var path = new List<string>();
        var step = targetNodeId;

        if (!previous.ContainsKey(step) && step != startNodeId)
            return path;

        while (!string.IsNullOrWhiteSpace(step))
        {
            path.Insert(0, step);

            if (step == startNodeId)
                break;

            step = previous[step];
        }

        return path;
    }

    private void AddEdge(MapNode from, MapNode to, bool isRisky = false)
    {
        Edges.Add(new MapEdge
        {
            FromNodeId = from.NodeId,
            ToNodeId = to.NodeId,
            X1 = from.X,
            Y1 = from.Y,
            X2 = to.X,
            Y2 = to.Y,
            IsRisky = isRisky
        });
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void MapViewport_MouseWheel(object sender, MouseWheelEventArgs e)
    {
        var zoomFactor = e.Delta > 0 ? ZoomStep : 1 / ZoomStep;
        ZoomAtPoint(zoomFactor, e.GetPosition(MapViewport));
    }

    private void ZoomInButton_Click(object sender, RoutedEventArgs e)
    {
        ZoomAtPoint(ZoomStep, new Point(MapViewport.ActualWidth / 2, MapViewport.ActualHeight / 2));
    }

    private void ZoomOutButton_Click(object sender, RoutedEventArgs e)
    {
        ZoomAtPoint(1 / ZoomStep, new Point(MapViewport.ActualWidth / 2, MapViewport.ActualHeight / 2));
    }

    private void ResetZoomButton_Click(object sender, RoutedEventArgs e)
    {
        MapScaleTransform.ScaleX = 1;
        MapScaleTransform.ScaleY = 1;
        MapTranslateTransform.X = 0;
        MapTranslateTransform.Y = 0;

        UpdateMarkerVisualScale();
        UpdatePersonnelVisibilityByZoom();
        UpdateEquipmentVisibilityByZoom();
    }

    private void ZoomAtPoint(double zoomFactor, Point mousePosition)
    {
        var oldScale = MapScaleTransform.ScaleX;
        var newScale = oldScale * zoomFactor;

        if (newScale < MinZoom)
            newScale = MinZoom;

        if (newScale > MaxZoom)
            newScale = MaxZoom;

        zoomFactor = newScale / oldScale;

        MapTranslateTransform.X = mousePosition.X - zoomFactor * (mousePosition.X - MapTranslateTransform.X);
        MapTranslateTransform.Y = mousePosition.Y - zoomFactor * (mousePosition.Y - MapTranslateTransform.Y);

        MapScaleTransform.ScaleX = newScale;
        MapScaleTransform.ScaleY = newScale;

        UpdateMarkerVisualScale();
        UpdatePersonnelVisibilityByZoom();
        UpdateEquipmentVisibilityByZoom();
    }

    private void MapViewport_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        _isDragging = true;
        _lastMousePosition = e.GetPosition(MapViewport);
        MapViewport.CaptureMouse();
        MapViewport.Cursor = Cursors.Hand;
    }

    private void MapViewport_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        _isDragging = false;
        MapViewport.ReleaseMouseCapture();
        MapViewport.Cursor = Cursors.Arrow;
    }

    private void MapViewport_MouseMove(object sender, MouseEventArgs e)
    {
        if (!_isDragging)
            return;

        var currentPosition = e.GetPosition(MapViewport);
        var delta = currentPosition - _lastMousePosition;

        MapTranslateTransform.X += delta.X;
        MapTranslateTransform.Y += delta.Y;

        _lastMousePosition = currentPosition;
    }

    public double MarkerVisualScale
    {
        get => _markerVisualScale;
        set
        {
            _markerVisualScale = value;
            OnPropertyChanged();
        }
    }

    private void UpdateMarkerVisualScale()
    {
        var zoom = MapScaleTransform.ScaleX;

        // 1 / zoom yapsaydık ikon tamamen sabit kalırdı.
        // 0.75 ile ikonlar çok az büyür ama harita kadar büyümez.
        var scale = 1 / Math.Pow(zoom, 0.75);

        if (scale < 0.45)
            scale = 0.45;

        if (scale > 1.35)
            scale = 1.35;

        MarkerVisualScale = scale;
    }

    private MapNode? GetNodeForPersonnel(MapPersonelDto personel)
    {
        var text = $"{personel.CalismaKonumu} {personel.Departman} {personel.Uzmanlik}".ToLowerInvariant();

        if (text.Contains("batı") || text.Contains("bati") || text.Contains("dekapaj"))
            return FindNode("bati-ocagi");

        if (text.Contains("galeri") || text.Contains("yer alt"))
            return FindNode("ana-galeri");

        if (text.Contains("yükleme") || text.Contains("yukleme") || text.Contains("kamyon"))
            return FindNode("yukleme");

        if (text.Contains("risk") || text.Contains("isg") || text.Contains("güvenlik") || text.Contains("guvenlik"))
            return FindNode("riskli-bolge");

        if (text.Contains("acil") || text.Contains("çıkış") || text.Contains("cikis"))
            return FindNode("acil-cikis");

        if (text.Contains("saha") || text.Contains("merkez") || text.Contains("teknik") || text.Contains("üretim") || text.Contains("uretim"))
            return FindNode("kontrol");

        return FindNode("kontrol");
    }

    private MapNode? GetNodeForEquipment(MapEquipmentDto equipment)
    {
        var text = $"{equipment.EkipmanAdi} {equipment.EkipmanTuru} {equipment.Durum}".ToLowerInvariant();

        if (text.Contains("ekskavat"))
            return FindNode("bati-ocagi");

        if (text.Contains("kamyon") || text.Contains("hafriyat"))
            return FindNode("yukleme");

        if (text.Contains("sensor") || text.Contains("sensör") || text.Contains("risk"))
            return FindNode("riskli-bolge");

        if (text.Contains("jeneratör") || text.Contains("jenerator") || text.Contains("enerji"))
            return FindNode("ana-galeri");

        return FindNode("kontrol");
    }

    private static bool IsTrackingDevice(MapEquipmentDto equipment)
    {
        var text = $"{equipment.EkipmanAdi} {equipment.EkipmanTuru}".ToLowerInvariant();

        return text.Contains("takip")
            || text.Contains("cihaz")
            || text.Contains("gps")
            || text.Contains("rfid")
            || text.Contains("konum");
    }

    private static PackIconKind GetEquipmentIcon(string? equipmentType)
    {
        var text = equipmentType?.ToLowerInvariant() ?? string.Empty;

        if (text.Contains("ekskavat"))
            return PackIconKind.Excavator;

        if (text.Contains("kamyon") || text.Contains("hafriyat"))
            return PackIconKind.Truck;

        if (text.Contains("sensor") || text.Contains("sensör"))
            return PackIconKind.AccessPoint;

        if (text.Contains("takip"))
            return PackIconKind.AccessPointCheck;

        if (text.Contains("jeneratör") || text.Contains("jenerator"))
            return PackIconKind.Engine;

        return PackIconKind.Tools;
    }

    private MapNode? FindNode(string nodeId)
    {
        return Nodes.FirstOrDefault(x => x.NodeId == nodeId);
    }

    private static Point GetOffsetPosition(double x, double y, int index)
    {
        var offsets = new[]
        {
        new Point(0, 0),
        new Point(26, 0),
        new Point(-26, 0),
        new Point(0, 26),
        new Point(0, -26),
        new Point(26, 26),
        new Point(-26, 26),
        new Point(26, -26),
        new Point(-26, -26),
        new Point(52, 0),
        new Point(-52, 0),
        new Point(0, 52),
        new Point(0, -52)
    };

        var offset = offsets[index % offsets.Length];
        var ring = index / offsets.Length;

        return new Point(
            x + offset.X + ring * 12,
            y + offset.Y + ring * 12);
    }

    private static string TextOrDefault(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? "-" : value;
    }

    private static bool IsAdminRole(string? role)
    {
        return string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase)
            || string.Equals(role, "Yönetici", StringComparison.OrdinalIgnoreCase)
            || string.Equals(role, "Yonetici", StringComparison.OrdinalIgnoreCase);
    }

    public bool IsPersonnelDetailVisible
    {
        get => _isPersonnelDetailVisible;
        set
        {
            _isPersonnelDetailVisible = value;
            OnPropertyChanged();
        }
    }

    public bool IsPersonnelClusterVisible
    {
        get => _isPersonnelClusterVisible;
        set
        {
            _isPersonnelClusterVisible = value;
            OnPropertyChanged();
        }
    }

    private void UpdatePersonnelVisibilityByZoom()
    {
        var zoom = MapScaleTransform.ScaleX;

        if (!IsPersonnelVisible)
        {
            IsPersonnelClusterVisible = false;
            IsPersonnelMarkersVisible = false;
            return;
        }

        if (zoom < 2.4)
        {
            IsPersonnelClusterVisible = true;
            IsPersonnelMarkersVisible = false;
        }
        else
        {
            IsPersonnelClusterVisible = false;
            IsPersonnelMarkersVisible = true;
        }
    }

    private void BuildPersonnelClustersFromCurrentMarkers()
    {
        PersonnelClusters.Clear();

        if (Personnels.Count == 0)
            return;

        PersonnelClusters.Add(new MapClusterMarker
        {
            NodeId = "fallback-personnel",
            Name = "Personel Yoğunluğu",
            Description = $"Haritada {Personnels.Count} personel gösteriliyor.",
            X = 260,
            Y = 210,
            PersonnelCount = Personnels.Count,
            PersonnelNames = Personnels
        .Select(p => p.Name)
        .ToList()
        });
    }

    public bool IsPersonnelMarkersVisible
    {
        get => _isPersonnelMarkersVisible;
        set
        {
            _isPersonnelMarkersVisible = value;
            OnPropertyChanged();
        }
    }

    public string SelectedClusterTitle
    {
        get => _selectedClusterTitle;
        set
        {
            _selectedClusterTitle = value;
            OnPropertyChanged();
        }
    }

    public string SelectedClusterDescription
    {
        get => _selectedClusterDescription;
        set
        {
            _selectedClusterDescription = value;
            OnPropertyChanged();
        }
    }

    public bool IsClusterDetailPanelVisible
    {
        get => _isClusterDetailPanelVisible;
        set
        {
            _isClusterDetailPanelVisible = value;
            OnPropertyChanged();
        }
    }
    private void PersonnelCluster_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (sender is not FrameworkElement element ||
            element.DataContext is not MapClusterMarker cluster)
            return;

        SelectedClusterTitle = $"{cluster.Name} Personelleri";
        SelectedClusterDescription = cluster.Description;

        SelectedClusterItems.Clear();

        foreach (var personName in cluster.PersonnelNames)
            SelectedClusterItems.Add(personName);

        IsClusterDetailPanelVisible = true;

        e.Handled = true;
    }
    private void EquipmentCluster_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (sender is not FrameworkElement element ||
            element.DataContext is not MapEquipmentClusterMarker cluster)
            return;

        SelectedClusterTitle = $"{cluster.Name} Ekipmanları";
        SelectedClusterDescription = cluster.Description;

        SelectedClusterItems.Clear();

        foreach (var equipmentName in cluster.EquipmentNames)
            SelectedClusterItems.Add(equipmentName);

        IsClusterDetailPanelVisible = true;

        e.Handled = true;
    }

    private void CloseClusterDetailButton_Click(object sender, RoutedEventArgs e)
    {
        IsClusterDetailPanelVisible = false;
    }
    public bool IsEquipmentClusterVisible
    {
        get => _isEquipmentClusterVisible;
        set
        {
            _isEquipmentClusterVisible = value;
            OnPropertyChanged();
        }
    }

    public bool IsEquipmentMarkersVisible
    {
        get => _isEquipmentMarkersVisible;
        set
        {
            _isEquipmentMarkersVisible = value;
            OnPropertyChanged();
        }
    }

    private void UpdateEquipmentVisibilityByZoom()
    {
        var zoom = MapScaleTransform.ScaleX;
        const double DetailZoomThreshold = 2.4;

        if (!IsEquipmentVisible)
        {
            IsEquipmentClusterVisible = false;
            IsEquipmentMarkersVisible = false;
            return;
        }

        if (zoom < DetailZoomThreshold)
        {
            IsEquipmentClusterVisible = true;
            IsEquipmentMarkersVisible = false;
        }
        else
        {
            IsEquipmentClusterVisible = false;
            IsEquipmentMarkersVisible = true;
        }
    }

    private void BuildEquipmentClustersFromCurrentMarkers()
    {
        EquipmentClusters.Clear();

        if (Equipments.Count == 0)
            return;

        EquipmentClusters.Add(new MapEquipmentClusterMarker
        {
            NodeId = "fallback-equipment",
            Name = "Ekipman Yoğunluğu",
            Description = $"Haritada {Equipments.Count} ekipman gösteriliyor.",
            X = 320,
            Y = 245,
            EquipmentCount = Equipments.Count,
            EquipmentNames = Equipments
                .Select(e => e.Name)
                .ToList()
        });
    }
}

public class MapMarker
{
    public int? PersonelId { get; set; }
    public int? AssignedPersonelId { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public double X { get; set; }
    public double Y { get; set; }

    public PackIconKind IconKind { get; set; }
}

public class MapNode
{
    public string NodeId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public double X { get; set; }
    public double Y { get; set; }

    public bool IsRisky { get; set; }
}

public class MapEdge
{
    public string FromNodeId { get; set; } = string.Empty;
    public string ToNodeId { get; set; } = string.Empty;

    public double X1 { get; set; }
    public double Y1 { get; set; }
    public double X2 { get; set; }
    public double Y2 { get; set; }
    public double Weight { get; set; } = 1;

    public bool IsRisky { get; set; }
}

public class MapPersonelDto
{
    public int PersonelId { get; set; }
    public string AdSoyad { get; set; } = string.Empty;
    public string Uzmanlik { get; set; } = string.Empty;
    public string Departman { get; set; } = string.Empty;
    public string KullaniciRolu { get; set; } = string.Empty;
    public string CalismaKonumu { get; set; } = string.Empty;
    public string PersonelDurumu { get; set; } = string.Empty;
    public string Tckn { get; set; } = string.Empty;
    public string TelNo { get; set; } = string.Empty;
    public string KanGrubu { get; set; } = string.Empty;
    public string RfidKartNumarasi { get; set; } = string.Empty;
}

public class MapEquipmentDto
{
    public int EkipmanId { get; set; }
    public string EkipmanAdi { get; set; } = string.Empty;
    public string EkipmanTuru { get; set; } = string.Empty;
    public string Durum { get; set; } = string.Empty;
}

public class MapClusterMarker
{
    public string NodeId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public double X { get; set; }
    public double Y { get; set; }

    public int PersonnelCount { get; set; }

    public List<string> PersonnelNames { get; set; } = new();
}

public class MapEquipmentClusterMarker
{
    public string NodeId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public double X { get; set; }
    public double Y { get; set; }

    public int EquipmentCount { get; set; }

    public List<string> EquipmentNames { get; set; } = new();
}