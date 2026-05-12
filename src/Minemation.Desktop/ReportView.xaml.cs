using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Minemation.Desktop.Models;

namespace Minemation.Desktop;

public partial class ReportView : UserControl
{
    private readonly HttpClient _httpClient = new()
    {
        BaseAddress = new Uri("http://localhost:5289")
    };

    private ReportTab _activeTab = ReportTab.General;

    private List<GeneralReportRow> _generalReports = new();
    private List<PersonnelReportRow> _personnelReports = new();
    private List<EquipmentReportRow> _equipmentReports = new();
    private List<IncidentReportRow> _incidentReports = new();

    public ReportView()
    {
        InitializeComponent();

        Loaded += ReportView_Loaded;
    }

    private async void ReportView_Loaded(object sender, RoutedEventArgs e)
    {
        await LoadGeneralReportsAsync();
        RefreshGrid();
    }

    private async Task LoadGeneralReportsAsync()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<ReportPagedData<GeneralReportDto>>>("/api/rapor");

            if (response?.Success != true || response.Data is null)
            {
                MessageBox.Show(response?.Message ?? "Genel rapor listesi alınamadı.");
                return;
            }

            _generalReports = response.Data.Items.Select(x => new GeneralReportRow
            {
                RaporId = x.RaporId,
                RaporAdi = x.RaporAdi,
                RaporTuru = x.RaporTuru,
                OlusturmaTarihi = x.RaporOlusturmaTarihi.ToString("dd.MM.yyyy HH:mm"),
                ZamanAraligi = x.ZamanAraligi,
                Personel = x.PersonelAdSoyad ?? string.Empty,
                Ekipman = x.EkipmanAdi ?? string.Empty
            }).ToList();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Genel raporlar alınırken hata oluştu: {ex.Message}");
        }
    }

    private async Task LoadPersonnelReportsAsync()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<ReportPagedData<PersonnelReportDto>>>("/api/personel-raporu");

            if (response?.Success != true || response.Data is null)
            {
                MessageBox.Show(response?.Message ?? "Personel raporu listesi alınamadı.");
                return;
            }

            _personnelReports = response.Data.Items.Select(x => new PersonnelReportRow
            {
                RaporId = x.RaporId,
                RaporAdi = x.RaporAdi ?? string.Empty,
                RaporTuru = x.RaporTuru ?? string.Empty,
                OlusturmaTarihi = x.RaporOlusturmaTarihi?.ToString("dd.MM.yyyy HH:mm") ?? string.Empty,
                UzmanlikAlani = x.UzmanlikAlani,
                PersonelSayisi = x.PersonelSayisi,
                CalismaSuresi = x.CalismaSuresi
            }).ToList();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Personel raporları alınırken hata oluştu: {ex.Message}");
        }
    }

    private async Task LoadEquipmentReportsAsync()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<ReportPagedData<EquipmentReportDto>>>("/api/ekipman-raporu");

            if (response?.Success != true || response.Data is null)
            {
                MessageBox.Show(response?.Message ?? "Ekipman raporu listesi alınamadı.");
                return;
            }

            _equipmentReports = response.Data.Items.Select(x => new EquipmentReportRow
            {
                RaporId = x.RaporId,
                RaporAdi = x.RaporAdi ?? string.Empty,
                RaporTuru = x.RaporTuru ?? string.Empty,
                OlusturmaTarihi = x.RaporOlusturmaTarihi?.ToString("dd.MM.yyyy HH:mm") ?? string.Empty,
                EkipmanTuru = x.EkipmanTuru,
                ArizaSayisi = x.ArizaSayisi,
                CalismaSuresi = x.CalismaSuresi
            }).ToList();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ekipman raporları alınırken hata oluştu: {ex.Message}");
        }
    }

    private async Task LoadIncidentReportsAsync()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<ReportPagedData<IncidentReportDto>>>("/api/vaka-raporu");

            if (response?.Success != true || response.Data is null)
            {
                MessageBox.Show(response?.Message ?? "Vaka raporu listesi alınamadı.");
                return;
            }

            _incidentReports = response.Data.Items.Select(x => new IncidentReportRow
            {
                RaporId = x.RaporId,
                RaporAdi = x.RaporAdi ?? string.Empty,
                RaporTuru = x.RaporTuru ?? string.Empty,
                OlusturmaTarihi = x.RaporOlusturmaTarihi?.ToString("dd.MM.yyyy HH:mm") ?? string.Empty,
                CiddiyetSeviyesi = x.CiddiyetSeviyesi,
                CozumSuresi = x.CozumSuresi,
                Personel = x.PersonelAdSoyad ?? string.Empty,
                RaporlayanEkipman = x.RaporlayanEkipmanAdi ?? string.Empty
            }).ToList();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Vaka raporları alınırken hata oluştu: {ex.Message}");
        }
    }

    private void RefreshGrid()
    {
        var searchText = ReportSearchBox.Text?.ToLowerInvariant() ?? string.Empty;

        switch (_activeTab)
        {
            case ReportTab.General:
                {
                    var list = _generalReports
                        .Where(x =>
                            string.IsNullOrWhiteSpace(searchText) ||
                            x.RaporAdi.ToLowerInvariant().Contains(searchText) ||
                            x.RaporTuru.ToLowerInvariant().Contains(searchText) ||
                            x.ZamanAraligi.ToLowerInvariant().Contains(searchText) ||
                            x.Personel.ToLowerInvariant().Contains(searchText) ||
                            x.Ekipman.ToLowerInvariant().Contains(searchText))
                        .ToList();

                    ReportGrid.ItemsSource = list;
                    ReportTotalText.Text = $"Toplam {list.Count} Genel Rapor";
                    break;
                }

            case ReportTab.Personnel:
                {
                    var list = _personnelReports
                        .Where(x =>
                            string.IsNullOrWhiteSpace(searchText) ||
                            x.RaporAdi.ToLowerInvariant().Contains(searchText) ||
                            x.RaporTuru.ToLowerInvariant().Contains(searchText) ||
                            x.UzmanlikAlani.ToLowerInvariant().Contains(searchText))
                        .ToList();

                    ReportGrid.ItemsSource = list;
                    ReportTotalText.Text = $"Toplam {list.Count} Personel Raporu";
                    break;
                }

            case ReportTab.Equipment:
                {
                    var list = _equipmentReports
                        .Where(x =>
                            string.IsNullOrWhiteSpace(searchText) ||
                            x.RaporAdi.ToLowerInvariant().Contains(searchText) ||
                            x.RaporTuru.ToLowerInvariant().Contains(searchText) ||
                            x.EkipmanTuru.ToLowerInvariant().Contains(searchText))
                        .ToList();

                    ReportGrid.ItemsSource = list;
                    ReportTotalText.Text = $"Toplam {list.Count} Ekipman Raporu";
                    break;
                }

            case ReportTab.Incident:
                {
                    var list = _incidentReports
                        .Where(x =>
                            string.IsNullOrWhiteSpace(searchText) ||
                            x.RaporAdi.ToLowerInvariant().Contains(searchText) ||
                            x.RaporTuru.ToLowerInvariant().Contains(searchText) ||
                            x.CiddiyetSeviyesi.ToLowerInvariant().Contains(searchText) ||
                            x.Personel.ToLowerInvariant().Contains(searchText) ||
                            x.RaporlayanEkipman.ToLowerInvariant().Contains(searchText))
                        .ToList();

                    ReportGrid.ItemsSource = list;
                    ReportTotalText.Text = $"Toplam {list.Count} Vaka Raporu";
                    break;
                }
        }
    }

    private async void BtnGeneralReports_Click(object sender, RoutedEventArgs e)
    {
        _activeTab = ReportTab.General;
        UpdateTabStyles();

        if (_generalReports.Count == 0)
            await LoadGeneralReportsAsync();

        RefreshGrid();
    }

    private async void BtnPersonnelReports_Click(object sender, RoutedEventArgs e)
    {
        _activeTab = ReportTab.Personnel;
        UpdateTabStyles();

        if (_personnelReports.Count == 0)
            await LoadPersonnelReportsAsync();

        RefreshGrid();
    }

    private async void BtnEquipmentReports_Click(object sender, RoutedEventArgs e)
    {
        _activeTab = ReportTab.Equipment;
        UpdateTabStyles();

        if (_equipmentReports.Count == 0)
            await LoadEquipmentReportsAsync();

        RefreshGrid();
    }

    private async void BtnIncidentReports_Click(object sender, RoutedEventArgs e)
    {
        _activeTab = ReportTab.Incident;
        UpdateTabStyles();

        if (_incidentReports.Count == 0)
            await LoadIncidentReportsAsync();

        RefreshGrid();
    }

    private async void BtnRefresh_Click(object sender, RoutedEventArgs e)
    {
        switch (_activeTab)
        {
            case ReportTab.General:
                await LoadGeneralReportsAsync();
                break;
            case ReportTab.Personnel:
                await LoadPersonnelReportsAsync();
                break;
            case ReportTab.Equipment:
                await LoadEquipmentReportsAsync();
                break;
            case ReportTab.Incident:
                await LoadIncidentReportsAsync();
                break;
        }

        RefreshGrid();
    }

    private void ReportSearchBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        RefreshGrid();
    }

    private void UpdateTabStyles()
    {
        SetTabStyle(BtnGeneralReports, _activeTab == ReportTab.General);
        SetTabStyle(BtnPersonnelReports, _activeTab == ReportTab.Personnel);
        SetTabStyle(BtnEquipmentReports, _activeTab == ReportTab.Equipment);
        SetTabStyle(BtnIncidentReports, _activeTab == ReportTab.Incident);
    }

    private void SetTabStyle(Button button, bool isActive)
    {
        if (isActive)
        {
            button.Foreground = (Brush)FindResource("PrimaryBrush");
            button.BorderBrush = (Brush)FindResource("PrimaryBrush");
            button.BorderThickness = new Thickness(0, 0, 0, 2);
        }
        else
        {
            button.Foreground = (Brush)FindResource("TextSecondaryBrush");
            button.BorderThickness = new Thickness(0);
        }
    }

    private async void BtnCreateReport_Click(object sender, RoutedEventArgs e)
    {
        var window = new CreateReportWindow
        {
            Owner = Window.GetWindow(this)
        };

        window.ShowDialog();

        if (window.ReportCreated)
        {
            await LoadGeneralReportsAsync();
            await LoadPersonnelReportsAsync();
            await LoadEquipmentReportsAsync();
            await LoadIncidentReportsAsync();

            RefreshGrid();
        }
    }
}



public enum ReportTab
{
    General,
    Personnel,
    Equipment,
    Incident
}

public class ReportPagedData<T>
{
    public List<T> Items { get; set; } = new();
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public bool HasPreviousPage { get; set; }
    public bool HasNextPage { get; set; }
}

public class GeneralReportDto
{
    public int RaporId { get; set; }
    public string RaporAdi { get; set; } = string.Empty;
    public string RaporTuru { get; set; } = string.Empty;
    public DateTime RaporOlusturmaTarihi { get; set; }
    public string ZamanAraligi { get; set; } = string.Empty;
    public int? PersonelId { get; set; }
    public string? PersonelAdSoyad { get; set; }
    public int? EkipmanId { get; set; }
    public string? EkipmanAdi { get; set; }
}

public class PersonnelReportDto
{
    public int RaporId { get; set; }
    public string? RaporAdi { get; set; }
    public string? RaporTuru { get; set; }
    public DateTime? RaporOlusturmaTarihi { get; set; }
    public string UzmanlikAlani { get; set; } = string.Empty;
    public int PersonelSayisi { get; set; }
    public decimal CalismaSuresi { get; set; }
}

public class EquipmentReportDto
{
    public int RaporId { get; set; }
    public string? RaporAdi { get; set; }
    public string? RaporTuru { get; set; }
    public DateTime? RaporOlusturmaTarihi { get; set; }
    public string EkipmanTuru { get; set; } = string.Empty;
    public int ArizaSayisi { get; set; }
    public int CalismaSuresi { get; set; }
}

public class IncidentReportDto
{
    public int RaporId { get; set; }
    public string? RaporAdi { get; set; }
    public string? RaporTuru { get; set; }
    public DateTime? RaporOlusturmaTarihi { get; set; }
    public string CiddiyetSeviyesi { get; set; } = string.Empty;
    public decimal CozumSuresi { get; set; }
    public int? PersonelId { get; set; }
    public string? PersonelAdSoyad { get; set; }
    public int? RaporlayanEkipmanId { get; set; }
    public string? RaporlayanEkipmanAdi { get; set; }
}

public class GeneralReportRow
{
    public int RaporId { get; set; }
    public string RaporAdi { get; set; } = string.Empty;
    public string RaporTuru { get; set; } = string.Empty;
    public string OlusturmaTarihi { get; set; } = string.Empty;
    public string ZamanAraligi { get; set; } = string.Empty;
    public string Personel { get; set; } = string.Empty;
    public string Ekipman { get; set; } = string.Empty;
}

public class PersonnelReportRow
{
    public int RaporId { get; set; }
    public string RaporAdi { get; set; } = string.Empty;
    public string RaporTuru { get; set; } = string.Empty;
    public string OlusturmaTarihi { get; set; } = string.Empty;
    public string UzmanlikAlani { get; set; } = string.Empty;
    public int PersonelSayisi { get; set; }
    public decimal CalismaSuresi { get; set; }
}

public class EquipmentReportRow
{
    public int RaporId { get; set; }
    public string RaporAdi { get; set; } = string.Empty;
    public string RaporTuru { get; set; } = string.Empty;
    public string OlusturmaTarihi { get; set; } = string.Empty;
    public string EkipmanTuru { get; set; } = string.Empty;
    public int ArizaSayisi { get; set; }
    public int CalismaSuresi { get; set; }
}

public class IncidentReportRow
{
    public int RaporId { get; set; }
    public string RaporAdi { get; set; } = string.Empty;
    public string RaporTuru { get; set; } = string.Empty;
    public string OlusturmaTarihi { get; set; } = string.Empty;
    public string CiddiyetSeviyesi { get; set; } = string.Empty;
    public decimal CozumSuresi { get; set; }
    public string Personel { get; set; } = string.Empty;
    public string RaporlayanEkipman { get; set; } = string.Empty;
}

