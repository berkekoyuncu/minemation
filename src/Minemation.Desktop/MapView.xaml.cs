using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace Minemation.Desktop
{
    public partial class MapView : UserControl, INotifyPropertyChanged
    {
        private bool _isPersonnelVisible = true;
        private bool _isEquipmentVisible = true;
        private bool _isEntrancesVisible = true;

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
            DataContext = this; // Bind to self for checkboxes
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
