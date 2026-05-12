using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;

namespace Minemation.Desktop;

public partial class MapView : UserControl, INotifyPropertyChanged
{
    private bool _isPersonnelVisible = true;
    private bool _isEquipmentVisible = true;
    private bool _isEntrancesVisible = true;
    private bool _isTaskPointsVisible = true;

    private readonly int _currentPersonelId;
    private readonly bool _canManage;

    public ObservableCollection<MapMarker> Entrances { get; } = new();
    public ObservableCollection<MapMarker> Equipments { get; } = new();
    public ObservableCollection<MapMarker> Personnels { get; } = new();
    public ObservableCollection<MapMarker> TaskPoints { get; } = new();

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
        Entrances.Clear();
        Equipments.Clear();
        Personnels.Clear();
        TaskPoints.Clear();

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

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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