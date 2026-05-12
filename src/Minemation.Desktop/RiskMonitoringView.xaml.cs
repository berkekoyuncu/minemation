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

public partial class RiskMonitoringView : UserControl
{
    private readonly HttpClient _httpClient = new()
    {
        BaseAddress = new Uri("http://localhost:5289")
    };

    private List<SensorRiskRow> _allSensorRows = new();

    public RiskMonitoringView()
    {
        InitializeComponent();
        Loaded += RiskMonitoringView_Loaded;
    }

    private async void RiskMonitoringView_Loaded(object sender, RoutedEventArgs e)
    {
        await LoadRiskDataAsync();
    }

    private async void BtnRefresh_Click(object sender, RoutedEventArgs e)
    {
        await LoadRiskDataAsync();
    }

    private async Task LoadRiskDataAsync()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<PagedData<SensorVerisiListItemDto>>>("/api/sensor-verisi");

            if (response?.Success != true || response.Data is null)
            {
                MessageBox.Show(response?.Message ?? "Sensör verileri alınamadı.");
                return;
            }

            _allSensorRows = response.Data.Items
                .OrderByDescending(x => x.OlcumTarihi)
                .Select(x => new SensorRiskRow
                {
                    SensorVerisiId = x.SensorVerisiId,
                    SensorTipi = x.SensorTipi ?? "Bilinmeyen",
                    EkipmanAdi = x.EkipmanAdi ?? "Bilinmeyen",
                    VardiyaAdi = x.VardiyaAdi ?? string.Empty,
                    Deger = x.Deger,
                    Birim = x.Birim,
                    OlcumTarihi = x.OlcumTarihi,
                    EsikDisiMi = x.EsikDisiMi,
                    RiskLevel = GetRiskLevel(x)
                })
                .ToList();

            BindSections();

            LastUpdatedText.Text = $"Son güncelleme: {DateTime.Now:dd.MM.yyyy HH:mm:ss}";
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Risk verileri alınırken hata oluştu: {ex.Message}");
        }
    }

    private void BindSections()
    {
        var priority = _allSensorRows
            .Where(x => x.EsikDisiMi || x.RiskLevel is "Kritik" or "Yüksek")
            .OrderByDescending(x => GetRiskSortValue(x.RiskLevel))
            .ThenByDescending(x => x.OlcumTarihi)
            .Take(10)
            .ToList();

        PriorityGrid.ItemsSource = priority;

        CoGrid.ItemsSource = _allSensorRows
            .Where(x => IsCoSensor(x.SensorTipi))
            .Take(8)
            .ToList();

        PressureGrid.ItemsSource = _allSensorRows
            .Where(x => IsPressureSensor(x.SensorTipi))
            .Take(8)
            .ToList();

        TemperatureGrid.ItemsSource = _allSensorRows
            .Where(x => IsTemperatureSensor(x.SensorTipi))
            .Take(8)
            .ToList();

        EquipmentGrid.ItemsSource = _allSensorRows
            .Where(x =>
                !IsCoSensor(x.SensorTipi) &&
                !IsPressureSensor(x.SensorTipi) &&
                !IsTemperatureSensor(x.SensorTipi))
            .Take(8)
            .ToList();
    }

    private static string GetRiskLevel(SensorVerisiListItemDto item)
    {
        if (item.EsikDisiMi)
        {
            if (IsCoSensor(item.SensorTipi ?? string.Empty))
                return "Kritik";

            if (IsTemperatureSensor(item.SensorTipi ?? string.Empty))
                return "Yüksek";

            return "Yüksek";
        }

        return "Normal";
    }

    private static int GetRiskSortValue(string riskLevel)
    {
        return riskLevel switch
        {
            "Kritik" => 4,
            "Yüksek" => 3,
            "Orta" => 2,
            "Normal" => 1,
            _ => 0
        };
    }

    private static bool IsCoSensor(string sensorType)
    {
        var value = sensorType.ToLowerInvariant();

        return value.Contains("co") ||
               value.Contains("karbon") ||
               value.Contains("gaz");
    }

    private static bool IsPressureSensor(string sensorType)
    {
        var value = sensorType.ToLowerInvariant();

        return value.Contains("basınç") ||
               value.Contains("basinc") ||
               value.Contains("pressure");
    }

    private static bool IsTemperatureSensor(string sensorType)
    {
        var value = sensorType.ToLowerInvariant();

        return value.Contains("sıcak") ||
               value.Contains("sicak") ||
               value.Contains("temperature") ||
               value.Contains("ısı") ||
               value.Contains("isi");
    }
}

public class SensorRiskRow
{
    public int SensorVerisiId { get; set; }
    public string SensorTipi { get; set; } = string.Empty;
    public string EkipmanAdi { get; set; } = string.Empty;
    public string VardiyaAdi { get; set; } = string.Empty;
    public decimal Deger { get; set; }
    public string Birim { get; set; } = string.Empty;
    public DateTime OlcumTarihi { get; set; }
    public bool EsikDisiMi { get; set; }
    public string RiskLevel { get; set; } = string.Empty;

    public string ValueText => $"{Deger} {Birim}";
    public string OlcumText => OlcumTarihi.ToString("dd.MM.yyyy HH:mm");
}

public class SensorVerisiListItemDto
{
    public int SensorVerisiId { get; set; }
    public decimal Deger { get; set; }
    public string Birim { get; set; } = string.Empty;
    public DateTime OlcumTarihi { get; set; }

    public int EkipmanId { get; set; }
    public string? SensorTipi { get; set; }
    public string? EkipmanAdi { get; set; }

    public int VardiyaId { get; set; }
    public string? VardiyaAdi { get; set; }

    public bool EsikDisiMi { get; set; }
}

public class PagedData<T>
{
    public List<T> Items { get; set; } = new();
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public bool HasPreviousPage { get; set; }
    public bool HasNextPage { get; set; }
}
