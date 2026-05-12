using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using Minemation.Desktop.Models;

namespace Minemation.Desktop;

public partial class AdminHomeView : UserControl
{
    private readonly HttpClient _httpClient = new()
    {
        BaseAddress = new Uri("http://localhost:5289")
    };

    public AdminHomeView()
    {
        InitializeComponent();
        Loaded += AdminHomeView_Loaded;
    }

    private async void AdminHomeView_Loaded(object sender, RoutedEventArgs e)
    {
        await LoadDashboardAsync();
    }

    private async void BtnRefresh_Click(object sender, RoutedEventArgs e)
    {
        await LoadDashboardAsync();
    }

    private async Task LoadDashboardAsync()
    {
        try
        {
            await LoadPersonnelSummaryAsync();
            await LoadShiftSummaryAsync();
            await LoadIncidentSummaryAsync();
            await LoadRiskSummaryAsync();

            LastUpdatedText.Text = $"Son güncelleme: {DateTime.Now:dd.MM.yyyy HH:mm:ss}";
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Yönetici ana ekranı yüklenirken hata oluştu: {ex.Message}");
        }
    }

    private async Task LoadPersonnelSummaryAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<ApiResponse<PagedResponse<AdminPersonelListItemDto>>>("/api/personel");

        if (response?.Success != true || response.Data is null)
        {
            PersonnelCountText.Text = "0";
            PersonnelSummaryText.Text = "-";
            return;
        }

        var total = response.Data.TotalCount;
        var active = response.Data.Items.Count(x => x.PersonelDurumu.Equals("Aktif", StringComparison.OrdinalIgnoreCase));

        PersonnelCountText.Text = total.ToString();
        PersonnelSummaryText.Text = $"{active} aktif / {total} toplam";
    }

    private async Task LoadShiftSummaryAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<ApiResponse<PagedResponse<AdminShiftListItemDto>>>("/api/vardiya");

        if (response?.Success != true || response.Data is null)
        {
            ActiveShiftCountText.Text = "0";
            ShiftSummaryText.Text = "-";
            return;
        }

        var active = response.Data.Items.Count(x =>
            x.VardiyaDurumu.Equals("Aktif", StringComparison.OrdinalIgnoreCase) ||
            x.VardiyaDurumu.Equals("Planlandı", StringComparison.OrdinalIgnoreCase));

        ActiveShiftCountText.Text = active.ToString();
        ShiftSummaryText.Text = $"{active} aktif/planlı vardiya";
    }

    private async Task LoadIncidentSummaryAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<ApiResponse<PagedResponse<AdminIncidentListItemDto>>>("/api/vaka");

        if (response?.Success != true || response.Data is null)
        {
            OpenIncidentCountText.Text = "0";
            RecentIncidentsGrid.ItemsSource = new List<AdminIncidentRow>();
            return;
        }

        var open = response.Data.Items.Count(x =>
            x.VakaDurumu.Equals("Açık", StringComparison.OrdinalIgnoreCase) ||
            x.VakaDurumu.Equals("İnceleniyor", StringComparison.OrdinalIgnoreCase));

        OpenIncidentCountText.Text = open.ToString();

        var recent = response.Data.Items
            .OrderByDescending(x => x.VakaOlusmaTarihi)
            .Take(5)
            .Select(x => new AdminIncidentRow
            {
                VakaId = x.VakaId,
                VakaAdi = x.VakaAdi,
                VakaTuru = x.VakaTuru,
                VakaCiddiyetSeviyesi = x.VakaCiddiyetSeviyesi,
                VakaDurumu = x.VakaDurumu,
                VakaOlusmaTarihi = x.VakaOlusmaTarihi
            })
            .ToList();

        RecentIncidentsGrid.ItemsSource = recent;
    }

    private async Task LoadRiskSummaryAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<ApiResponse<PagedResponse<AdminSensorDataDto>>>("/api/sensor-verisi");

        if (response?.Success != true || response.Data is null)
        {
            RiskSensorCountText.Text = "0";
            RiskStatusText.Text = "-";
            return;
        }

        var riskCount = response.Data.Items.Count(x => x.EsikDisiMi);

        RiskSensorCountText.Text = riskCount.ToString();
        RiskStatusText.Text = riskCount > 0 ? "Dikkat Gerekiyor" : "Normal";

        EquipmentSummaryText.Text = $"{response.Data.Items.Select(x => x.EkipmanId).Distinct().Count()} ekipmandan sensör verisi";
    }

    private void OpenMapButton_Click(object sender, RoutedEventArgs e)
    {
        if (Window.GetWindow(this) is MainWindow mainWindow)
            mainWindow.OpenMapPage();
    }
}

public class AdminPersonelListItemDto
{
    public int PersonelId { get; set; }
    public string AdSoyad { get; set; } = string.Empty;
    public string Uzmanlik { get; set; } = string.Empty;
    public string Departman { get; set; } = string.Empty;
    public string KullaniciRolu { get; set; } = string.Empty;
    public string CalismaKonumu { get; set; } = string.Empty;
    public string PersonelDurumu { get; set; } = string.Empty;
}

public class AdminShiftListItemDto
{
    public int VardiyaId { get; set; }
    public string VardiyaAdi { get; set; } = string.Empty;
    public DateTime VardiyaBaslangicTarihi { get; set; }
    public DateTime VardiyaBitisTarihi { get; set; }
    public string VardiyaDurumu { get; set; } = string.Empty;
    public string VardiyaTipi { get; set; } = string.Empty;
    public string CalismaBolgesi { get; set; } = string.Empty;
    public string OperasyonTipi { get; set; } = string.Empty;
    public string OperasyonRiskSeviyesi { get; set; } = string.Empty;
}

public class AdminIncidentListItemDto
{
    public int VakaId { get; set; }
    public string VakaTuru { get; set; } = string.Empty;
    public string VakaAdi { get; set; } = string.Empty;
    public string VakaCiddiyetSeviyesi { get; set; } = string.Empty;
    public string VakaDurumu { get; set; } = string.Empty;
    public DateTime VakaOlusmaTarihi { get; set; }
}

public class AdminSensorDataDto
{
    public int SensorVerisiId { get; set; }
    public decimal Deger { get; set; }
    public string Birim { get; set; } = string.Empty;
    public DateTime OlcumTarihi { get; set; }
    public int EkipmanId { get; set; }
    public string? SensorTipi { get; set; }
    public string? EkipmanAdi { get; set; }
    public bool EsikDisiMi { get; set; }
}

public class AdminIncidentRow
{
    public int VakaId { get; set; }
    public string VakaAdi { get; set; } = string.Empty;
    public string VakaTuru { get; set; } = string.Empty;
    public string VakaCiddiyetSeviyesi { get; set; } = string.Empty;
    public string VakaDurumu { get; set; } = string.Empty;
    public DateTime VakaOlusmaTarihi { get; set; }

    public string TarihText => VakaOlusmaTarihi.ToString("dd.MM.yyyy HH:mm");
}
