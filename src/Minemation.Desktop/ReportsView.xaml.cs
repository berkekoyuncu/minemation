using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Minemation.Desktop
{
    public partial class ReportsView : UserControl
    {
        public ReportsView()
        {
            InitializeComponent();
        }

        private void Tab_Checked(object sender, RoutedEventArgs e)
        {
            if (!this.IsLoaded || ReportsGrid == null)
            {
                this.Loaded += (s, ev) => 
                {
                    if (sender == TabVaka) LoadVakaRaporlari();
                    else if (sender == TabEkipman) LoadEkipmanRaporlari();
                    else if (sender == TabPersonel) LoadPersonelRaporlari();
                };
                return;
            }

            if (sender == TabVaka) LoadVakaRaporlari();
            else if (sender == TabEkipman) LoadEkipmanRaporlari();
            else if (sender == TabPersonel) LoadPersonelRaporlari();
        }

        private void BtnCreateReport_Click(object sender, RoutedEventArgs e)
        {
            string selectedType = "Vaka Raporu";
            if (TabEkipman.IsChecked == true) selectedType = "Ekipman Raporu";
            else if (TabPersonel.IsChecked == true) selectedType = "Personel Raporu";

            var mainWindow = (MainWindow)System.Windows.Application.Current.MainWindow;
            mainWindow.MainContent.Content = new CreateReportView(selectedType);
        }

        private void LoadVakaRaporlari()
        {
            ReportsGrid.ItemsSource = null;
            ReportsGrid.Columns.Clear();

            var durumTemplate = (DataTemplate)FindResource("DurumCellTemplate");
            var islemlerTemplate = (DataTemplate)FindResource("IslemlerCellTemplate");

            ReportsGrid.Columns.Add(new DataGridTextColumn { Header = "No.", Binding = new Binding("Id"), Width = new DataGridLength(50) });
            ReportsGrid.Columns.Add(new DataGridTextColumn { Header = "Tarih", Binding = new Binding("Date") { StringFormat = "dd.MM.yyyy" }, Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            ReportsGrid.Columns.Add(new DataGridTextColumn { Header = "Vardiya", Binding = new Binding("Shift"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            ReportsGrid.Columns.Add(new DataGridTextColumn { Header = "Konum", Binding = new Binding("Location"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            ReportsGrid.Columns.Add(new DataGridTextColumn { Header = "Vaka Tipi", Binding = new Binding("IncidentType"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            ReportsGrid.Columns.Add(new DataGridTemplateColumn { Header = "Durum", Width = new DataGridLength(100), CellTemplate = durumTemplate });
            ReportsGrid.Columns.Add(new DataGridTextColumn { Header = "Oluşturan*", Binding = new Binding("CreatedBy"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            ReportsGrid.Columns.Add(new DataGridTemplateColumn { Header = "İşlemler", Width = new DataGridLength(100), CellTemplate = islemlerTemplate });

            ReportsGrid.ItemsSource = new ObservableCollection<VakaReport>
            {
                new VakaReport { Id = 1, Date = new DateTime(2026, 5, 10), Shift = "08:00-16:00", Location = "Galeri 3", IncidentType = "Sensör Alarmı", Status = "Çözüldü", CreatedBy = "Sistem" },
                new VakaReport { Id = 2, Date = new DateTime(2026, 5, 11), Shift = "16:00-00:00", Location = "Ana Şaft", IncidentType = "Ekipman Arızası", Status = "Açık", CreatedBy = "Ahmet Y." },
                new VakaReport { Id = 3, Date = new DateTime(2026, 5, 12), Shift = "00:00-08:00", Location = "Kuzey Hat", IncidentType = "Gaz Sızıntısı Şüphesi", Status = "Açık", CreatedBy = "Mehmet K." }
            };
        }

        private void LoadEkipmanRaporlari()
        {
            ReportsGrid.ItemsSource = null;
            ReportsGrid.Columns.Clear();

            var islemlerTemplate = (DataTemplate)FindResource("IslemlerCellTemplate");

            ReportsGrid.Columns.Add(new DataGridTextColumn { Header = "No.", Binding = new Binding("Id"), Width = new DataGridLength(50) });
            ReportsGrid.Columns.Add(new DataGridTextColumn { Header = "Tarih", Binding = new Binding("Date") { StringFormat = "dd.MM.yyyy" }, Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            ReportsGrid.Columns.Add(new DataGridTextColumn { Header = "Ekipman", Binding = new Binding("Equipment"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            ReportsGrid.Columns.Add(new DataGridTextColumn { Header = "Bakım Türü", Binding = new Binding("MaintenanceType"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            ReportsGrid.Columns.Add(new DataGridTextColumn { Header = "Çalışma Saati", Binding = new Binding("WorkingHours"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            ReportsGrid.Columns.Add(new DataGridTextColumn { Header = "Oluşturan*", Binding = new Binding("CreatedBy"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            ReportsGrid.Columns.Add(new DataGridTemplateColumn { Header = "İşlemler", Width = new DataGridLength(100), CellTemplate = islemlerTemplate });

            ReportsGrid.ItemsSource = new ObservableCollection<EkipmanReport>
            {
                new EkipmanReport { Id = 1, Date = new DateTime(2026, 5, 9), Equipment = "Ekskavatör E-01", MaintenanceType = "Periyodik", WorkingHours = "1200", CreatedBy = "Sistem" },
                new EkipmanReport { Id = 2, Date = new DateTime(2026, 5, 11), Equipment = "Kırıcı K-03", MaintenanceType = "Arıza", WorkingHours = "850", CreatedBy = "Ali V." }
            };
        }

        private void LoadPersonelRaporlari()
        {
            ReportsGrid.ItemsSource = null;
            ReportsGrid.Columns.Clear();

            var durumTemplate = (DataTemplate)FindResource("DurumCellTemplate");
            var islemlerTemplate = (DataTemplate)FindResource("IslemlerCellTemplate");

            ReportsGrid.Columns.Add(new DataGridTextColumn { Header = "No.", Binding = new Binding("Id"), Width = new DataGridLength(50) });
            ReportsGrid.Columns.Add(new DataGridTextColumn { Header = "Tarih", Binding = new Binding("Date") { StringFormat = "dd.MM.yyyy" }, Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            ReportsGrid.Columns.Add(new DataGridTextColumn { Header = "Personel", Binding = new Binding("Personnel"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            ReportsGrid.Columns.Add(new DataGridTextColumn { Header = "Vardiya", Binding = new Binding("Shift"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            ReportsGrid.Columns.Add(new DataGridTemplateColumn { Header = "Durum", Width = new DataGridLength(100), CellTemplate = durumTemplate });
            ReportsGrid.Columns.Add(new DataGridTextColumn { Header = "Oluşturan*", Binding = new Binding("CreatedBy"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            ReportsGrid.Columns.Add(new DataGridTemplateColumn { Header = "İşlemler", Width = new DataGridLength(100), CellTemplate = islemlerTemplate });

            ReportsGrid.ItemsSource = new ObservableCollection<PersonelReport>
            {
                new PersonelReport { Id = 1, Date = new DateTime(2026, 5, 10), Personnel = "Mehmet Kaya", Shift = "08:00-16:00", Status = "Görevde", CreatedBy = "Sistem" },
                new PersonelReport { Id = 2, Date = new DateTime(2026, 5, 10), Personnel = "Ayşe Yılmaz", Shift = "08:00-16:00", Status = "İzinli", CreatedBy = "Ahmet Y." },
                new PersonelReport { Id = 3, Date = new DateTime(2026, 5, 11), Personnel = "Ali Veli", Shift = "16:00-00:00", Status = "Görevde", CreatedBy = "Sistem" }
            };
        }
    }

    public class VakaReport
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Shift { get; set; }
        public string Location { get; set; }
        public string IncidentType { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
    }

    public class EkipmanReport
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Equipment { get; set; }
        public string MaintenanceType { get; set; }
        public string WorkingHours { get; set; }
        public string CreatedBy { get; set; }
    }

    public class PersonelReport
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Personnel { get; set; }
        public string Shift { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
    }
}
