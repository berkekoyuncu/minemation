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

public partial class FieldHomeView : UserControl
{
    private readonly HttpClient _httpClient = new()
    {
        BaseAddress = new Uri("http://localhost:5289")
    };

    private readonly int _personelId;

    public FieldHomeView(int personelId)
    {
        _personelId = personelId;

        InitializeComponent();

        Loaded += FieldHomeView_Loaded;
    }

    private async void FieldHomeView_Loaded(object sender, RoutedEventArgs e)
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
            await LoadActiveShiftAsync();
            await LoadMyIncidentsAsync();

            LastUpdatedText.Text = $"Son güncelleme: {DateTime.Now:dd.MM.yyyy HH:mm:ss}";
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ana ekran bilgileri yüklenirken hata oluştu: {ex.Message}");
        }
    }

    private async Task LoadActiveShiftAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<ApiResponse<PagedResponse<FieldShiftDto>>>("/api/vardiya");

        if (response?.Success != true || response.Data is null || response.Data.Items.Count == 0)
        {
            ClearShiftInfo();
            return;
        }

        var items = response.Data.Items.ToList();

        var relatedItems = new List<FieldShiftDto>();

        foreach (var item in items)
        {
            if (await IsShiftRelatedToCurrentPersonnelAsync(item.VardiyaId))
                relatedItems.Add(item);
        }

        items = relatedItems;

        if (items.Count == 0)
        {
            ClearShiftInfo();
            return;
        }

        var activeShift = items
            .Where(x =>
                x.VardiyaDurumu.Equals("Aktif", StringComparison.OrdinalIgnoreCase) ||
                x.VardiyaDurumu.Equals("Planlandı", StringComparison.OrdinalIgnoreCase))
            .OrderBy(x => x.VardiyaBaslangicTarihi)
            .FirstOrDefault();

        activeShift ??= items
            .OrderByDescending(x => x.VardiyaBaslangicTarihi)
            .FirstOrDefault();

        if (activeShift is null)
        {
            ClearShiftInfo();
            return;
        }

        ShiftDateText.Text = activeShift.VardiyaBaslangicTarihi.ToString("dd.MM.yyyy");
        ShiftTimeText.Text = $"{activeShift.VardiyaBaslangicTarihi:HH:mm} - {activeShift.VardiyaBitisTarihi:HH:mm}";
        EquipmentText.Text = string.IsNullOrWhiteSpace(activeShift.OperasyonTipi) ? "-" : activeShift.OperasyonTipi;
        LocationText.Text = string.IsNullOrWhiteSpace(activeShift.CalismaBolgesi) ? "-" : activeShift.CalismaBolgesi;
        ShiftStatusText.Text = string.IsNullOrWhiteSpace(activeShift.VardiyaDurumu) ? "-" : activeShift.VardiyaDurumu;
    }

    private async Task LoadMyIncidentsAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<ApiResponse<PagedResponse<FieldIncidentDto>>>("/api/vaka");

        if (response?.Success != true || response.Data is null)
        {
            MyIncidentsGrid.ItemsSource = new List<FieldIncidentRow>();
            return;
        }

        var incidents = response.Data.Items
            .Where(x => _personelId <= 0 || x.PersonelId == _personelId)
            .OrderByDescending(x => x.VakaOlusmaTarihi)
            .Take(5)
            .Select(x => new FieldIncidentRow
            {
                VakaId = x.VakaId,
                VakaAdi = x.VakaAdi,
                VakaTuru = x.VakaTuru,
                VakaCiddiyetSeviyesi = x.VakaCiddiyetSeviyesi,
                VakaDurumu = x.VakaDurumu,
                VakaOlusmaTarihi = x.VakaOlusmaTarihi
            })
            .ToList();

        MyIncidentsGrid.ItemsSource = incidents;
    }

    private void ClearShiftInfo()
    {
        ShiftDateText.Text = "-";
        ShiftTimeText.Text = "-";
        EquipmentText.Text = "-";
        LocationText.Text = "-";
        ShiftStatusText.Text = "-";
    }

    private async Task<bool> IsShiftRelatedToCurrentPersonnelAsync(int vardiyaId)
    {
        if (_personelId <= 0)
            return false;

        try
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<ShiftDetailDto>>(
                $"/api/vardiya/{vardiyaId}");

            if (response?.Success != true || response.Data is null)
                return false;

            var detail = response.Data;

            return detail.VardiyaSorumlusu == _personelId ||
                   detail.VardiyaIsgSorumlusu == _personelId ||
                   detail.VardiyaTeknikSorumlusu == _personelId;
        }
        catch
        {
            return false;
        }
    }
}

public class FieldShiftDto
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
    public int PersonelSayisi { get; set; }
    public int EkipmanSayisi { get; set; }
    public int EkipSayisi { get; set; }
}

public class FieldIncidentDto
{
    public int VakaId { get; set; }
    public string VakaTuru { get; set; } = string.Empty;
    public string VakaAdi { get; set; } = string.Empty;
    public string VakaCiddiyetSeviyesi { get; set; } = string.Empty;
    public string VakaDurumu { get; set; } = string.Empty;
    public DateTime VakaOlusmaTarihi { get; set; }
    public DateTime? VakaKapanmaTarihi { get; set; }
    public string OlayNedeni { get; set; } = string.Empty;
    public int? PersonelId { get; set; }
    public string? PersonelAdSoyad { get; set; }
    public int? RaporlayanEkipmanId { get; set; }
    public string? RaporlayanEkipmanAdi { get; set; }
    public int? IlgiliEkipmanId { get; set; }
    public string? IlgiliEkipmanAdi { get; set; }
}

public class FieldIncidentRow
{
    public int VakaId { get; set; }
    public string VakaAdi { get; set; } = string.Empty;
    public string VakaTuru { get; set; } = string.Empty;
    public string VakaCiddiyetSeviyesi { get; set; } = string.Empty;
    public string VakaDurumu { get; set; } = string.Empty;
    public DateTime VakaOlusmaTarihi { get; set; }

    public string TarihText => VakaOlusmaTarihi.ToString("dd.MM.yyyy HH:mm");
}
