using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Controls;
using Minemation.Application.DTOs;
using Minemation.Desktop.Models;

namespace Minemation.Desktop
{
    public partial class CreateReportView : UserControl
    {
        private readonly HttpClient _httpClient = new()
        {
            BaseAddress = new Uri("http://localhost:5289")
        };

        public int? ReportId { get; private set; }

        public CreateReportView(string reportType, int? reportId = null)
        {
            InitializeComponent();
            TxtReportType.Text = reportType;
            ReportId = reportId;

            if (ReportId.HasValue)
            {
                ReportTitleText.Text = $"Rapor Düzenle (ID: {ReportId})";
                BtnSave.Content = "Güncelle";
                TxtReportNo.Text = ReportId.Value.ToString();
                TxtReportNo.IsReadOnly = true;
                
                Loaded += CreateReportView_Loaded;
            }
        }

        private async void CreateReportView_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<ApiResponse<RaporDetayDto>>($"/api/rapor/{ReportId}");
                if (response?.Success == true && response.Data != null)
                {
                    TxtReportName.Text = response.Data.RaporAdi;
                    SelectComboBoxItem(CmbZamanAraligi, response.Data.ZamanAraligi);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Rapor bilgileri alınırken hata oluştu: {ex.Message}");
            }
        }

        private void SelectComboBoxItem(ComboBox comboBox, string value)
        {
            foreach (var item in comboBox.Items)
            {
                if (item is ComboBoxItem comboBoxItem &&
                    string.Equals(comboBoxItem.Content?.ToString(), value, StringComparison.OrdinalIgnoreCase))
                {
                    comboBox.SelectedItem = comboBoxItem;
                    return;
                }
            }
            comboBox.SelectedIndex = -1;
            comboBox.Text = value;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(this) is MainWindow mainWindow)
            {
                mainWindow.MainContent.Content = new ReportsView();
            }
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtReportName.Text))
            {
                MessageBox.Show("Rapor adı zorunludur.");
                return;
            }

            string zamanAraligi = "Günlük";
            if (CmbZamanAraligi.SelectedItem is ComboBoxItem item)
            {
                zamanAraligi = item.Content?.ToString() ?? "Günlük";
            }
            else if (!string.IsNullOrWhiteSpace(CmbZamanAraligi.Text))
            {
                zamanAraligi = CmbZamanAraligi.Text;
            }

            BtnSave.IsEnabled = false;

            try
            {
                if (ReportId.HasValue)
                {
                    var dto = new RaporGuncelleDto
                    {
                        RaporAdi = TxtReportName.Text.Trim(),
                        RaporTuru = TxtReportType.Text.Trim(),
                        ZamanAraligi = zamanAraligi,
                        RaporOlusturmaTarihi = DateTime.Now
                    };

                    var response = await _httpClient.PutAsJsonAsync($"/api/rapor/{ReportId.Value}", dto);

                    if (!response.IsSuccessStatusCode)
                    {
                        var error = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Rapor güncellenemedi.\n\n{error}");
                        return;
                    }

                    // Not: Alt tabloların güncellenmesi UI'da o alanlar olmadığı için atlandı

                    MessageBox.Show("Rapor başarıyla güncellendi.");
                }
                else
                {
                    var dto = new RaporOlusturDto
                    {
                        RaporAdi = TxtReportName.Text.Trim(),
                        RaporTuru = TxtReportType.Text.Trim(),
                        ZamanAraligi = zamanAraligi,
                        RaporOlusturmaTarihi = DateTime.Now
                    };

                    var response = await _httpClient.PostAsJsonAsync("/api/rapor", dto);

                    if (!response.IsSuccessStatusCode)
                    {
                        var error = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Rapor oluşturulamadı.\n\n{error}");
                        return;
                    }

                    var resultData = await response.Content.ReadFromJsonAsync<ApiResponse<RaporDetayDto>>();
                    if (resultData?.Data != null)
                    {
                        int newRaporId = resultData.Data.RaporId;
                        string type = TxtReportType.Text.Trim();

                        if (type == "Vaka Raporu")
                        {
                            var vakaDto = new VakaRaporuOlusturDto { RaporId = newRaporId, CiddiyetSeviyesi = "Düşük", CozumSuresi = 0 };
                            await _httpClient.PostAsJsonAsync("/api/vaka-raporu", vakaDto);
                        }
                        else if (type == "Ekipman Raporu")
                        {
                            var eqDto = new EkipmanRaporuOlusturDto { RaporId = newRaporId, EkipmanTuru = "Genel", ArizaSayisi = 0, CalismaSuresi = 0 };
                            await _httpClient.PostAsJsonAsync("/api/ekipman-raporu", eqDto);
                        }
                        else if (type == "Personel Raporu")
                        {
                            var pDto = new PersonelRaporuOlusturDto { RaporId = newRaporId, UzmanlikAlani = "Genel", PersonelSayisi = 0, CalismaSuresi = 0 };
                            await _httpClient.PostAsJsonAsync("/api/personel-raporu", pDto);
                        }
                    }

                    MessageBox.Show("Rapor başarıyla oluşturuldu.");
                }

                if (Window.GetWindow(this) is MainWindow mainWindow)
                {
                    mainWindow.MainContent.Content = new ReportsView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kaydetme sırasında hata oluştu: {ex.Message}");
            }
            finally
            {
                BtnSave.IsEnabled = true;
            }
        }
    }
}
