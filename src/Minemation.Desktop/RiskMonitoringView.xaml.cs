using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace Minemation.Desktop
{
    public partial class RiskMonitoringView : UserControl
    {
        public ObservableCollection<CoData> CoDataList { get; set; }

        public RiskMonitoringView()
        {
            InitializeComponent();

            // Demo data for CO Verileri table to match the sketch
            CoDataList = new ObservableCollection<CoData>
            {
                new CoData { Status = "Up", SensorName = "Sensör A1", Location = "Galeri 3" },
                new CoData { Status = "Down", SensorName = "Sensör B2", Location = "Ana Giriş" },
                new CoData { Status = "Up", SensorName = "Sensör C4", Location = "Şaft 1" },
                new CoData { Status = "Down", SensorName = "Sensör D1", Location = "Kuzey Hat" }
            };

            CoDataGrid.ItemsSource = CoDataList;
        }
    }

    public class CoData
    {
        public string Status { get; set; }
        public string SensorName { get; set; }
        public string Location { get; set; }
    }
}
