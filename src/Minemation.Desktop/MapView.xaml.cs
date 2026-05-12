using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using MapControl;

namespace Minemation.Desktop
{
    public class MapMarker
    {
        public string Name { get; set; }
        public Location Location { get; set; }
        public string Type { get; set; } // Personnel, Equipment, Entrance
        public string IconKind { get; set; } 
    }

    public partial class MapView : UserControl, INotifyPropertyChanged
    {
        private bool _isPersonnelVisible = true;
        private bool _isEquipmentVisible = true;
        private bool _isEntrancesVisible = true;

        public ObservableCollection<MapMarker> Personnels { get; set; }
        public ObservableCollection<MapMarker> Equipments { get; set; }
        public ObservableCollection<MapMarker> Entrances { get; set; }

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
            
            // OpenStreetMap kuralları gereği User-Agent ayarlanması gerekiyor
            if (!MapControl.ImageLoader.HttpClient.DefaultRequestHeaders.Contains("User-Agent"))
            {
                MapControl.ImageLoader.HttpClient.DefaultRequestHeaders.Add("User-Agent", "Minemation/1.0 (contact@minemation.com)");
            }
            
            // Dummy Data (Zonguldak Region)
            Personnels = new ObservableCollection<MapMarker>
            {
                new MapMarker { Name = "Ahmet Y.", Location = new Location(41.455, 31.792), IconKind = "AccountHardHat" },
                new MapMarker { Name = "Mehmet K.", Location = new Location(41.451, 31.785), IconKind = "AccountHardHat" },
                new MapMarker { Name = "Ali V.", Location = new Location(41.458, 31.795), IconKind = "AccountHardHat" }
            };

            Equipments = new ObservableCollection<MapMarker>
            {
                new MapMarker { Name = "E-01", Location = new Location(41.453, 31.788), IconKind = "Excavator" },
                new MapMarker { Name = "K-02", Location = new Location(41.456, 31.790), IconKind = "Truck" }
            };

            Entrances = new ObservableCollection<MapMarker>
            {
                new MapMarker { Name = "Kuzey Girişi", Location = new Location(41.460, 31.791), IconKind = "MapMarkerRadius" },
                new MapMarker { Name = "Doğu Tüneli", Location = new Location(41.450, 31.800), IconKind = "MapMarkerRadius" }
            };

            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
