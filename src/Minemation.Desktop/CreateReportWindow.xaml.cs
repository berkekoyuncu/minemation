using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Http;
using System.Text.Json;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Controls;
using Minemation.Desktop.Models;
using Minemation.Desktop.Pdf;

namespace Minemation.Desktop;

public partial class CreateReportWindow : Window
{
    private readonly HttpClient _httpClient = new()
    {
        BaseAddress = new Uri("http://localhost:5289")
    };

    public bool ReportCreated { get; private set; }

    public CreateReportWindow()
    {
        InitializeComponent();

        StartDatePicker.SelectedDate = DateTime.Today;
        EndDatePicker.SelectedDate = DateTime.Today;
        ReportTemplateComboBox.SelectedIndex = 0;

        ApplyTemplate();
    }

    private void ReportTemplateComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ApplyTemplate();
    }

    private void ApplyTemplate()
    {
        var templateName = GetSelectedTemplate();

        ReportNameBox.Text = $"{templateName} - {DateTime.Today:dd.MM.yyyy}";
        ReportDescriptionBox.Text = BuildTemplateDescription(templateName);
    }

    private string GetSelectedTemplate()
    {
        if (ReportTemplateComboBox.SelectedItem is ComboBoxItem item)
            return item.Content?.ToString() ?? "Genel Rapor";

        return "Genel Rapor";
    }

    private static string BuildTemplateDescription(string templateName)
    {
        return templateName switch
        {
            "Günlük Ekipman Raporu" =>
                "GÜNLÜK EKİPMAN RAPORU\n\n" +
                "1. Ekipman genel durumu:\n" +
                "- Aktif ekipmanlar:\n" +
                "- Bakımda olan ekipmanlar:\n" +
                "- Arızalı ekipmanlar:\n\n" +
                "2. Gün içinde yapılan işlemler:\n" +
                "- Bakım çalışmaları:\n" +
                "- Operasyon kullanımı:\n" +
                "- RFID / takip kontrolü:\n\n" +
                "3. Değerlendirme:\n" +
                "- Riskli ekipmanlar:\n" +
                "- Önerilen aksiyonlar:",

            "Günlük Arıza Raporu" =>
                "GÜNLÜK ARIZA RAPORU\n\n" +
                "1. Arıza özeti:\n" +
                "- Arıza tespit zamanı:\n" +
                "- Etkilenen ekipman:\n" +
                "- Arıza kategorisi:\n\n" +
                "2. Etki analizi:\n" +
                "- Operasyona etkisi:\n" +
                "- Güvenlik riski:\n" +
                "- Duruş süresi:\n\n" +
                "3. Çözüm / aksiyon:\n" +
                "- Alınan aksiyon:\n" +
                "- Sorumlu ekip:\n" +
                "- Takip durumu:",

            "Günlük Vaka Raporu" =>
                "GÜNLÜK VAKA RAPORU\n\n" +
                "1. Vaka özeti:\n" +
                "- Vaka türü:\n" +
                "- Ciddiyet seviyesi:\n" +
                "- Olay nedeni:\n\n" +
                "2. İlgili kişi / ekipman:\n" +
                "- Personel:\n" +
                "- Raporlayan ekipman:\n" +
                "- İlgili ekipman:\n\n" +
                "3. Durum ve takip:\n" +
                "- Vaka durumu:\n" +
                "- Alınan önlem:\n" +
                "- Kapatma notu:",

            "Personel Çalışma Raporu" =>
                "PERSONEL ÇALIŞMA RAPORU\n\n" +
                "1. Personel özeti:\n" +
                "- Çalışan personel sayısı:\n" +
                "- Vardiya bilgisi:\n" +
                "- Uzmanlık dağılımı:\n\n" +
                "2. Çalışma performansı:\n" +
                "- Toplam çalışma süresi:\n" +
                "- Görev bölgeleri:\n" +
                "- Riskli görevler:\n\n" +
                "3. Değerlendirme:\n" +
                "- Eksik personel:\n" +
                "- Önerilen vardiya düzeni:",

            "Risk İzleme Raporu" =>
                "RİSK İZLEME RAPORU\n\n" +
                "1. Risk özeti:\n" +
                "- Kritik sensör uyarıları:\n" +
                "- Eşik dışı değerler:\n" +
                "- Risk seviyesi:\n\n" +
                "2. Bölgesel durum:\n" +
                "- Riskli çalışma bölgeleri:\n" +
                "- Etkilenen ekipman/personel:\n\n" +
                "3. Önerilen aksiyonlar:\n" +
                "- Acil aksiyon:\n" +
                "- Önleyici bakım:\n" +
                "- İzleme notu:",

            _ =>
                "GENEL RAPOR\n\n1. Özet:\n\n2. Detaylar:\n\n3. Değerlendirme:\n"
        };
    }

    private async void CreateButton_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(ReportNameBox.Text))
        {
            MessageBox.Show("Rapor adı zorunludur.");
            return;
        }

        var start = StartDatePicker.SelectedDate ?? DateTime.Today;
        var end = EndDatePicker.SelectedDate ?? start;

        if (end < start)
        {
            MessageBox.Show("Bitiş tarihi başlangıç tarihinden önce olamaz.");
            return;
        }

        var reportName = ReportNameBox.Text.Trim();
        var reportType = GetSelectedTemplate();
        var dateRange = $"{start:dd.MM.yyyy} - {end:dd.MM.yyyy}";
        var description = ReportDescriptionBox.Text.Trim();
        var personnelId = ParseNullableInt(PersonnelIdBox.Text);
        var equipmentId = ParseNullableInt(EquipmentIdBox.Text);

        try
        {
            var pdfPath = ReportPdfGenerator.GenerateReportPdf(new ReportPdfModel
            {
                ReportName = reportName,
                ReportType = reportType,
                DateRange = dateRange,
                Description = description,
                PersonnelId = personnelId,
                EquipmentId = equipmentId
            });

            var generalRequest = new ReportCreateRequest
            {
                RaporAdi = reportName,
                RaporTuru = reportType,
                RaporOlusturmaTarihi = DateTime.Now,
                RaporAciklamasi = description,
                RaporDosyaYolu = pdfPath,
                ZamanAraligi = dateRange,
                PersonelId = personnelId,
                EkipmanId = equipmentId
            };

            var generalResponse = await _httpClient.PostAsJsonAsync("/api/rapor", generalRequest);

            if (!generalResponse.IsSuccessStatusCode)
            {
                var error = await generalResponse.Content.ReadAsStringAsync();
                MessageBox.Show($"Genel rapor oluşturulamadı.\n\n{error}");
                return;
            }

            var generalResponseJson = await generalResponse.Content.ReadAsStringAsync();
            var createdReportId = ExtractRaporId(generalResponseJson);

            if (createdReportId <= 0)
            {
                MessageBox.Show("Genel rapor oluşturuldu ancak oluşan RaporId okunamadı. İlgili rapor sekmesine kayıt atılamadı.");
                return;
            }

            var detailCreated = await CreateDetailReportAsync(reportType, createdReportId, personnelId, equipmentId);

            if (!detailCreated)
                return;

            ReportCreated = true;

            MessageBox.Show($"Rapor başarıyla oluşturuldu.\n\nPDF dosyası:\n{pdfPath}");
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Rapor oluşturulurken hata oluştu: {ex.Message}");
        }
    }

    private async Task<bool> CreateDetailReportAsync(string reportType, int raporId, int? personnelId, int? equipmentId)
    {
        if (reportType.Contains("Ekipman", StringComparison.OrdinalIgnoreCase) ||
            reportType.Contains("Arıza", StringComparison.OrdinalIgnoreCase))
        {
            var request = new EquipmentReportCreateRequest
            {
                RaporId = raporId,
                EkipmanTuru = reportType.Contains("Arıza", StringComparison.OrdinalIgnoreCase)
                    ? "Arıza"
                    : "Genel Ekipman",
                ArizaSayisi = reportType.Contains("Arıza", StringComparison.OrdinalIgnoreCase) ? 1 : 0,
                CalismaSuresi = 8
            };

            return await PostDetailReportAsync("/api/ekipman-raporu", request, "Ekipman raporu");
        }

        if (reportType.Contains("Vaka", StringComparison.OrdinalIgnoreCase))
        {
            var request = new IncidentReportCreateRequest
            {
                RaporId = raporId,
                CiddiyetSeviyesi = "Orta",
                CozumSuresi = 0,
                PersonelId = personnelId,
                RaporlayanEkipmanId = equipmentId
            };

            return await PostDetailReportAsync("/api/vaka-raporu", request, "Vaka raporu");
        }

        if (reportType.Contains("Personel", StringComparison.OrdinalIgnoreCase))
        {
            var request = new PersonnelReportCreateRequest
            {
                RaporId = raporId,
                UzmanlikAlani = "Genel",
                PersonelSayisi = 1,
                CalismaSuresi = 8
            };

            return await PostDetailReportAsync("/api/personel-raporu", request, "Personel raporu");
        }

        // Risk İzleme için şimdilik sadece genel rapor oluşturuyoruz.
        // Risk modülünü yaptığımızda burada /api/risk-raporu gibi ayrı endpoint varsa bağlarız.
        return true;
    }

    private async Task<bool> PostDetailReportAsync<TRequest>(string endpoint, TRequest request, string reportName)
    {
        var response = await _httpClient.PostAsJsonAsync(endpoint, request);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            MessageBox.Show($"{reportName} detay kaydı oluşturulamadı.\n\n{error}");
            return false;
        }

        return true;
    }

    private static int ExtractRaporId(string json)
    {
        using var document = JsonDocument.Parse(json);
        return FindRaporId(document.RootElement);
    }

    private static int FindRaporId(JsonElement element)
    {
        if (element.ValueKind == JsonValueKind.Object)
        {
            foreach (var property in element.EnumerateObject())
            {
                if (string.Equals(property.Name, "raporId", StringComparison.OrdinalIgnoreCase) &&
                    property.Value.ValueKind == JsonValueKind.Number &&
                    property.Value.TryGetInt32(out var id))
                {
                    return id;
                }

                var nestedResult = FindRaporId(property.Value);
                if (nestedResult > 0)
                    return nestedResult;
            }
        }

        if (element.ValueKind == JsonValueKind.Array)
        {
            foreach (var item in element.EnumerateArray())
            {
                var nestedResult = FindRaporId(item);
                if (nestedResult > 0)
                    return nestedResult;
            }
        }

        return 0;
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private static int? ParseNullableInt(string? value)
    {
        if (!int.TryParse(value, out var result))
            return null;

        return result <= 0 ? null : result;
    }
}

public class ReportCreateRequest
{
    public string RaporAdi { get; set; } = string.Empty;
    public string RaporTuru { get; set; } = string.Empty;
    public DateTime RaporOlusturmaTarihi { get; set; }
    public string RaporAciklamasi { get; set; } = string.Empty;
    public string RaporDosyaYolu { get; set; } = string.Empty;
    public string ZamanAraligi { get; set; } = string.Empty;
    public int? PersonelId { get; set; }
    public int? EkipmanId { get; set; }
}

public class PersonnelReportCreateRequest
{
    public int RaporId { get; set; }
    public string UzmanlikAlani { get; set; } = string.Empty;
    public int PersonelSayisi { get; set; }
    public decimal CalismaSuresi { get; set; }
}

public class EquipmentReportCreateRequest
{
    public int RaporId { get; set; }
    public string EkipmanTuru { get; set; } = string.Empty;
    public int ArizaSayisi { get; set; }
    public int CalismaSuresi { get; set; }
}

public class IncidentReportCreateRequest
{
    public int RaporId { get; set; }
    public string CiddiyetSeviyesi { get; set; } = string.Empty;
    public decimal CozumSuresi { get; set; }
    public int? PersonelId { get; set; }
    public int? RaporlayanEkipmanId { get; set; }
}