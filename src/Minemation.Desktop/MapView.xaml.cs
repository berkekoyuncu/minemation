using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using MapControl;

namespace Minemation.Desktop
{
    public class MapMarker
    {
        public string Name { get; set; } = string.Empty;
        public Location Location { get; set; } = new Location(0,0);
        public string Type { get; set; } = string.Empty;
        public string IconKind { get; set; } = string.Empty;
    }

    public partial class MapView : UserControl, INotifyPropertyChanged
    {
        private bool _isPersonnelVisible = true;
        private bool _isEquipmentVisible = true;
        private bool _isEntrancesVisible = true;

        public ObservableCollection<MapMarker> Personnels { get; set; }
        public ObservableCollection<MapMarker> Equipments { get; set; }
        public ObservableCollection<MapMarker> Entrances { get; set; }

        public BoundingBox MapImageBounds { get; set; } = new BoundingBox(41.440, 31.770, 41.470, 31.810);

        public bool IsPersonnelVisible
        {
            get => _isPersonnelVisible;
            set
            {
                if (_isPersonnelVisible != value)
                {
                    _isPersonnelVisible = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsEquipmentVisible
        {
            get => _isEquipmentVisible;
            set
            {
                if (_isEquipmentVisible != value)
                {
                    _isEquipmentVisible = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsEntrancesVisible
        {
            get => _isEntrancesVisible;
            set
            {
                if (_isEntrancesVisible != value)
                {
                    _isEntrancesVisible = value;
                    OnPropertyChanged();
                }
            }
        }

        public MapView()
        {
            InitializeComponent();
            
            // Dummy Data (Zonguldak Region)
            // Coordinates set specifically to fall nicely within the new BoundingBox (Lat: 41.440 to 41.470, Lon: 31.770 to 31.810)
            Personnels = new ObservableCollection<MapMarker>
            {
                new MapMarker { Name = "Ahmet Y.", Location = new Location(41.455, 31.792), IconKind = "AccountHardHat", Type="Personnel" },
                new MapMarker { Name = "Mehmet K.", Location = new Location(41.451, 31.785), IconKind = "AccountHardHat", Type="Personnel" },
                new MapMarker { Name = "Ali V.", Location = new Location(41.458, 31.795), IconKind = "AccountHardHat", Type="Personnel" },
                new MapMarker { Name = "Veli C.", Location = new Location(41.462, 31.782), IconKind = "AccountHardHat", Type="Personnel" },
                new MapMarker { Name = "Hasan S.", Location = new Location(41.448, 31.798), IconKind = "AccountHardHat", Type="Personnel" }
            };

            Equipments = new ObservableCollection<MapMarker>
            {
                new MapMarker { Name = "E-01 (Ekskavatör)", Location = new Location(41.453, 31.788), IconKind = "Excavator", Type="Equipment" },
                new MapMarker { Name = "K-02 (Kamyon)", Location = new Location(41.456, 31.790), IconKind = "Truck", Type="Equipment" },
                new MapMarker { Name = "D-01 (Delici)", Location = new Location(41.459, 31.780), IconKind = "Drill", Type="Equipment" }
            };

            Entrances = new ObservableCollection<MapMarker>
            {
                new MapMarker { Name = "Kuzey Tünel Girişi", Location = new Location(41.465, 31.791), IconKind = "MapMarkerRadius", Type="Entrance" },
                new MapMarker { Name = "Doğu Tünel Girişi", Location = new Location(41.450, 31.805), IconKind = "MapMarkerRadius", Type="Entrance" },
                new MapMarker { Name = "Güney Açık Ocak", Location = new Location(41.442, 31.785), IconKind = "MapMarkerRadius", Type="Entrance" }
            };

            DataContext = this;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
