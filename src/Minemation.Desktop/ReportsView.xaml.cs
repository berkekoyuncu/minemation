using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Minemation.Application.DTOs;
using Minemation.Application.Common;

namespace Minemation.Desktop
{
    public partial class ReportsView : UserControl
    {
        private readonly HttpClient _httpClient = new()
        {
            BaseAddress = new Uri("http://localhost:5289")
        };

        public ReportsView()
        {
            InitializeComponent();
        }

        private async void Tab_Checked(object sender, RoutedEventArgs e)
        {
            if (!this.IsLoaded || ReportsGrid == null)
            {
                this.Loaded += async (s, ev) => 
                {
                    if (sender == TabVaka) await LoadVakaRaporlariAsync();
                    else if (sender == TabEkipman) await LoadEkipmanRaporlariAsync();
                    else if (sender == TabPersonel) await LoadPersonelRaporlariAsync();
                };
                return;
            }

            if (sender == TabVaka) await LoadVakaRaporlariAsync();
            else if (sender == TabEkipman) await LoadEkipmanRaporlariAsync();
            else if (sender == TabPersonel) await LoadPersonelRaporlariAsync();
        }

        private void BtnCreateReport_Click(object sender, RoutedEventArgs e)
        {
            string selectedType = "Vaka Raporu";
            if (TabEkipman.IsChecked == true) selectedType = "Ekipman Raporu";
            else if (TabPersonel.IsChecked == true) selectedType = "Personel Raporu";

            if (Window.GetWindow(this) is MainWindow mainWindow)
            {
                mainWindow.MainContent.Content = new CreateReportView(selectedType);
            }
        }

        private void BtnFilter_Click(object sender, RoutedEventArgs e)
        {
            if (FilterPanel != null)
            {
                FilterPanel.Visibility = FilterPanel.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        private async void BtnApplyFilter_Click(object sender, RoutedEventArgs e)
        {
            if (TabVaka.IsChecked == true) await LoadVakaRaporlariAsync();
            else if (TabEkipman.IsChecked == true) await LoadEkipmanRaporlariAsync();
            else if (TabPersonel.IsChecked == true) await LoadPersonelRaporlariAsync();
        }

        private async void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            if (TxtSearch != null) TxtSearch.Text = string.Empty;
            
            if (TabVaka.IsChecked == true) await LoadVakaRaporlariAsync();
            else if (TabEkipman.IsChecked == true) await LoadEkipmanRaporlariAsync();
            else if (TabPersonel.IsChecked == true) await LoadPersonelRaporlariAsync();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext != null)
            {
                var item = element.DataContext;
                int id = 0;
                string selectedType = "Vaka Raporu";
                
                if (item is VakaReport v) 
                {
                    id = v.Id;
                    selectedType = "Vaka Raporu";
                }
                else if (item is EkipmanReport eq) 
                {
                    id = eq.Id;
                    selectedType = "Ekipman Raporu";
                }
                else if (item is PersonelReport p) 
                {
                    id = p.Id;
                    selectedType = "Personel Raporu";
                }

                if (Window.GetWindow(this) is MainWindow mainWindow)
                {
                    mainWindow.MainContent.Content = new CreateReportView(selectedType, id);
                }
            }
        }

        private async void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext != null)
            {
                var item = element.DataContext;
                var result = MessageBox.Show("Bu raporu silmek istediğinize emin misiniz?", "Silme Onayı", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    int id = 0;
                    if (item is VakaReport v) id = v.Id;
                    else if (item is EkipmanReport eq) id = eq.Id;
                    else if (item is PersonelReport p) id = p.Id;

                    if (id > 0)
                    {
                        try
                        {
                            var response = await _httpClient.DeleteAsync($"/api/rapor/{id}");
                            if (response.IsSuccessStatusCode)
                            {
                                MessageBox.Show("Rapor silindi.");
                                if (item is VakaReport) await LoadVakaRaporlariAsync();
                                else if (item is EkipmanReport) await LoadEkipmanRaporlariAsync();
                                else if (item is PersonelReport) await LoadPersonelRaporlariAsync();
                            }
                            else
                            {
                                MessageBox.Show("Silme işlemi başarısız oldu.");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Hata: {ex.Message}");
                        }
                    }
                }
            }
        }

        private async Task LoadVakaRaporlariAsync()
        {
            ReportsGrid.ItemsSource = null;
            ReportsGrid.Columns.Clear();

            var durumTemplate = (DataTemplate)FindResource("DurumCellTemplate");
            var islemlerTemplate = (DataTemplate)FindResource("IslemlerCellTemplate");

            ReportsGrid.Columns.Add(new DataGridTextColumn { Header = "No.", Binding = new Binding("Id"), Width = new DataGridLength(50) });
            ReportsGrid.Columns.Add(new DataGridTextColumn { Header = "Tarih", Binding = new Binding("Date") { StringFormat = "dd.MM.yyyy" }, Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            ReportsGrid.Columns.Add(new DataGridTextColumn { Header = "Rapor Adı", Binding = new Binding("IncidentType"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            ReportsGrid.Columns.Add(new DataGridTextColumn { Header = "Ekipman", Binding = new Binding("Location"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            ReportsGrid.Columns.Add(new DataGridTextColumn { Header = "Çözüm Süresi", Binding = new Binding("Shift"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            ReportsGrid.Columns.Add(new DataGridTemplateColumn { Header = "Ciddiyet", Width = new DataGridLength(100), CellTemplate = durumTemplate });
            ReportsGrid.Columns.Add(new DataGridTextColumn { Header = "Oluşturan*", Binding = new Binding("CreatedBy"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            ReportsGrid.Columns.Add(new DataGridTemplateColumn { Header = "İşlemler", Width = new DataGridLength(100), CellTemplate = islemlerTemplate });

            try
            {
                string endpoint = "/api/vaka-raporu";
                if (!string.IsNullOrWhiteSpace(TxtSearch.Text))
                {
                    endpoint += $"?Arama={Uri.EscapeDataString(TxtSearch.Text.Trim())}";
                }
                
                var response = await _httpClient.GetFromJsonAsync<ApiResponse<PagedResult<VakaRaporuListeDto>>>(endpoint);
                if (response?.Success == true && response.Data != null)
                {
                    var list = new ObservableCollection<VakaReport>();
                    foreach (var item in response.Data.Items)
                    {
                        list.Add(new VakaReport { 
                            Id = item.RaporId, 
                            Date = item.RaporOlusturmaTarihi ?? DateTime.MinValue,
                            Shift = item.CozumSuresi.ToString("0.0"),
                            Location = item.RaporlayanEkipmanAdi ?? "-",
                            IncidentType = item.RaporAdi ?? "-",
                            Status = item.CiddiyetSeviyesi,
                            CreatedBy = item.PersonelAdSoyad ?? "-"
                        });
                    }
                    ReportsGrid.ItemsSource = list;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Veriler alınamadı: {ex.Message}");
            }
        }

        private async Task LoadEkipmanRaporlariAsync()
        {
            ReportsGrid.ItemsSource = null;
            ReportsGrid.Columns.Clear();

            var islemlerTemplate = (DataTemplate)FindResource("IslemlerCellTemplate");

            ReportsGrid.Columns.Add(new DataGridTextColumn { Header = "No.", Binding = new Binding("Id"), Width = new DataGridLength(50) });
            ReportsGrid.Columns.Add(new DataGridTextColumn { Header = "Tarih", Binding = new Binding("Date") { StringFormat = "dd.MM.yyyy" }, Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            ReportsGrid.Columns.Add(new DataGridTextColumn { Header = "Rapor Adı", Binding = new Binding("MaintenanceType"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            ReportsGrid.Columns.Add(new DataGridTextColumn { Header = "Ekipman Türü", Binding = new Binding("Equipment"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            ReportsGrid.Columns.Add(new DataGridTextColumn { Header = "Arıza Sayısı", Binding = new Binding("WorkingHours"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            ReportsGrid.Columns.Add(new DataGridTemplateColumn { Header = "İşlemler", Width = new DataGridLength(100), CellTemplate = islemlerTemplate });

            try
            {
                string endpoint = "/api/ekipman-raporu";
                if (!string.IsNullOrWhiteSpace(TxtSearch.Text))
                {
                    endpoint += $"?Arama={Uri.EscapeDataString(TxtSearch.Text.Trim())}";
                }

                var response = await _httpClient.GetFromJsonAsync<ApiResponse<PagedResult<EkipmanRaporuListeDto>>>(endpoint);
                if (response?.Success == true && response.Data != null)
                {
                    var list = new ObservableCollection<EkipmanReport>();
                    foreach (var item in response.Data.Items)
                    {
                        list.Add(new EkipmanReport { 
                            Id = item.RaporId, 
                            Date = item.RaporOlusturmaTarihi ?? DateTime.MinValue,
                            MaintenanceType = item.RaporAdi ?? "-",
                            Equipment = item.EkipmanTuru,
                            WorkingHours = item.ArizaSayisi.ToString()
                        });
                    }
                    ReportsGrid.ItemsSource = list;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Veriler alınamadı: {ex.Message}");
            }
        }

        private async Task LoadPersonelRaporlariAsync()
        {
            ReportsGrid.ItemsSource = null;
            ReportsGrid.Columns.Clear();

            var islemlerTemplate = (DataTemplate)FindResource("IslemlerCellTemplate");

            ReportsGrid.Columns.Add(new DataGridTextColumn { Header = "No.", Binding = new Binding("Id"), Width = new DataGridLength(50) });
            ReportsGrid.Columns.Add(new DataGridTextColumn { Header = "Tarih", Binding = new Binding("Date") { StringFormat = "dd.MM.yyyy" }, Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            ReportsGrid.Columns.Add(new DataGridTextColumn { Header = "Rapor Adı", Binding = new Binding("Personnel"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            ReportsGrid.Columns.Add(new DataGridTextColumn { Header = "Uzmanlık", Binding = new Binding("Shift"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            ReportsGrid.Columns.Add(new DataGridTextColumn { Header = "Personel Sayısı", Binding = new Binding("Status"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            ReportsGrid.Columns.Add(new DataGridTemplateColumn { Header = "İşlemler", Width = new DataGridLength(100), CellTemplate = islemlerTemplate });

            try
            {
                string endpoint = "/api/personel-raporu";
                if (!string.IsNullOrWhiteSpace(TxtSearch.Text))
                {
                    endpoint += $"?Arama={Uri.EscapeDataString(TxtSearch.Text.Trim())}";
                }

                var response = await _httpClient.GetFromJsonAsync<ApiResponse<PagedResult<PersonelRaporuListeDto>>>(endpoint);
                if (response?.Success == true && response.Data != null)
                {
                    var list = new ObservableCollection<PersonelReport>();
                    foreach (var item in response.Data.Items)
                    {
                        list.Add(new PersonelReport { 
                            Id = item.RaporId, 
                            Date = item.RaporOlusturmaTarihi ?? DateTime.MinValue,
                            Personnel = item.RaporAdi ?? "-",
                            Shift = item.UzmanlikAlani,
                            Status = item.PersonelSayisi.ToString()
                        });
                    }
                    ReportsGrid.ItemsSource = list;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Veriler alınamadı: {ex.Message}");
            }
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
