using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;
using System.Windows;
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

    private const double MinZoom = 0.6;
    private const double MaxZoom = 3.5;
    private const double ZoomStep = 1.15;
    private double _markerVisualScale = 1;

    
    private Point _lastMousePosition;

    private readonly int _currentPersonelId;
    private readonly bool _canManage;

    public ObservableCollection<MapMarker> Entrances { get; } = new();
    public ObservableCollection<MapMarker> Equipments { get; } = new();
    public ObservableCollection<MapMarker> Personnels { get; } = new();
    public ObservableCollection<MapMarker> TaskPoints { get; } = new();
    public ObservableCollection<MapNode> Nodes { get; } = new();
    public ObservableCollection<MapEdge> Edges { get; } = new();
    public ObservableCollection<MapEdge> RouteEdges { get; } = new();

    public bool IsPersonnelVisible
    {
        get => _isPersonnelVisible;
        set
        {
            _isPersonnelVisible = value;
            OnPropertyChanged();
        }
    }

    public bool IsEquipmentVisible
    {
        get => _isEquipmentVisible;
        set
        {
            _isEquipmentVisible = value;
            OnPropertyChanged();
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
        LoadMapData();
    }

    private void BtnRefresh_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        LoadMapData();
    }

    private void LoadMapData()
    {
        Nodes.Clear();
        Edges.Clear();
        RouteEdges.Clear();
        Entrances.Clear();
        Equipments.Clear();
        Personnels.Clear();
        TaskPoints.Clear();

        LoadGraph();
        BuildHighlightedRoute();

        LoadEntrances();
        LoadEquipments();
        LoadPersonnelMarkers();
        LoadTaskPoints();
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