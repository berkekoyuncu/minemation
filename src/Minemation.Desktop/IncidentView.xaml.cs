using System;
using System.Text;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Minemation.Desktop.Models;

namespace Minemation.Desktop;

public partial class IncidentView : UserControl
{

    private readonly bool _canManage;
    private readonly int _currentPersonelId;

    private readonly HttpClient _httpClient = new()
    {
        BaseAddress = new Uri("http://localhost:5289")
    };

    private List<IncidentModel> _allIncidents = new();
    private bool _showOnlyOpen = true;
    private int? _selectedIncidentId = null;

    public IncidentView(bool canManage = true, int currentPersonelId = 0)
    {
        _canManage = canManage;
        _currentPersonelId = currentPersonelId;

        InitializeComponent();
        Loaded += IncidentView_Loaded;
    }

    private async void IncidentView_Loaded(object sender, RoutedEventArgs e)
    {
        ApplyPermissions();

        await LoadIncidentDataAsync();
        RefreshGrid();
    }

    private async Task LoadIncidentDataAsync()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<VakaPagedData>>("/api/vaka");

            if (response?.Success != true || response.Data is null)
            {
                MessageBox.Show(response?.Message ?? "Vaka listesi alınamadı.");
                return;
            }

            _allIncidents = response.Data.Items.Select(x => new IncidentModel
            {
                Id = x.VakaId,
                Type = x.VakaTuru,
                Name = x.VakaAdi,
                Severity = x.VakaCiddiyetSeviyesi,
                Status = x.VakaDurumu,
                CreatedDate = x.VakaOlusmaTarihi,
                CreatedText = x.VakaOlusmaTarihi.ToString("dd.MM.yyyy HH:mm"),
                ClosedDate = x.VakaKapanmaTarihi,
                Cause = x.OlayNedeni,
                PersonelId = x.PersonelId,
                PersonnelName = x.PersonelAdSoyad ?? string.Empty,
                ReporterEquipmentId = x.RaporlayanEkipmanId,
                ReporterEquipmentName = x.RaporlayanEkipmanAdi ?? string.Empty,
                RelatedEquipmentId = x.IlgiliEkipmanId,
                RelatedEquipmentName = x.IlgiliEkipmanAdi ?? string.Empty
            }).ToList();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Vaka verileri alınırken hata oluştu: {ex.Message}");
        }
    }

    private void RefreshGrid()
    {
        if (IncidentGrid == null)
            return;

        var searchText = IncidentSearchBox.Text?.ToLower() ?? "";

        var filteredList = _allIncidents
    .Where(i =>
        _canManage ||
        _currentPersonelId <= 0 ||
        i.PersonelId == _currentPersonelId)
    .Where(i =>
        !_showOnlyOpen ||
        i.Status.Equals("Açık", StringComparison.OrdinalIgnoreCase) ||
        i.Status.Equals("İnceleniyor", StringComparison.OrdinalIgnoreCase))
    .Where(i =>
        string.IsNullOrWhiteSpace(searchText) ||
        i.Name.ToLower().Contains(searchText) ||
        i.Type.ToLower().Contains(searchText) ||
        i.Severity.ToLower().Contains(searchText) ||
        i.Status.ToLower().Contains(searchText) ||
        i.Cause.ToLower().Contains(searchText) ||
        i.PersonnelName.ToLower().Contains(searchText))
    .ToList();

        IncidentGrid.ItemsSource = filteredList;
        IncidentTotalText.Text = $"Toplam {filteredList.Count} Vaka";
    }

    private void BtnOpenIncidents_Click(object sender, RoutedEventArgs e)
    {
        _showOnlyOpen = true;
        UpdateTabStyles();
        RefreshGrid();
    }

    private void BtnAllIncidents_Click(object sender, RoutedEventArgs e)
    {
        _showOnlyOpen = false;
        UpdateTabStyles();
        RefreshGrid();
    }

    private void UpdateTabStyles()
    {
        if (_showOnlyOpen)
        {
            BtnOpenIncidents.Foreground = (Brush)FindResource("PrimaryBrush");
            BtnOpenIncidents.BorderBrush = (Brush)FindResource("PrimaryBrush");
            BtnOpenIncidents.BorderThickness = new Thickness(0, 0, 0, 2);

            BtnAllIncidents.Foreground = (Brush)FindResource("TextSecondaryBrush");
            BtnAllIncidents.BorderThickness = new Thickness(0);
        }
        else
        {
            BtnAllIncidents.Foreground = (Brush)FindResource("PrimaryBrush");
            BtnAllIncidents.BorderBrush = (Brush)FindResource("PrimaryBrush");
            BtnAllIncidents.BorderThickness = new Thickness(0, 0, 0, 2);

            BtnOpenIncidents.Foreground = (Brush)FindResource("TextSecondaryBrush");
            BtnOpenIncidents.BorderThickness = new Thickness(0);
        }
    }

    private void IncidentSearchBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        RefreshGrid();
    }

    private async void BtnRefresh_Click(object sender, RoutedEventArgs e)
    {
        await LoadIncidentDataAsync();
        RefreshGrid();
    }

    private void BtnAddNew_Click(object sender, RoutedEventArgs e)
    {

        if (!_canManage)
            return;
        _selectedIncidentId = null;

        IncidentFormTitle.Text = "Yeni Vaka Kaydı";

        IncidentNameBox.Text = string.Empty;
        SelectComboBoxItem(IncidentTypeComboBox, "Sensör Uyarısı");
        SelectComboBoxItem(SeverityComboBox, "Orta");
        SelectComboBoxItem(IncidentStatusComboBox, "Açık");

        IncidentDatePicker.SelectedDate = DateTime.Today;
        IncidentTimeBox.Text = DateTime.Now.ToString("HH:mm");

        CauseBox.Text = string.Empty;
        DescriptionBox.Text = string.Empty;

        PersonnelIdBox.Text = string.Empty;
        ReporterEquipmentIdBox.Text = string.Empty;
        RelatedEquipmentIdBox.Text = string.Empty;
        CloseDatePicker.SelectedDate = null;
    }

    private async void BtnEdit_Click(object sender, RoutedEventArgs e)
    {
        if (!_canManage)
            return;

        if (((Button)sender).DataContext is not IncidentModel selectedIncident)
            return;

        try
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<VakaDetailDto>>(
                $"/api/vaka/{selectedIncident.Id}");

            if (response?.Success != true || response.Data is null)
            {
                MessageBox.Show(response?.Message ?? "Vaka detayı alınamadı.");
                return;
            }

            var vaka = response.Data;

            _selectedIncidentId = vaka.VakaId;

            IncidentFormTitle.Text = $"Vaka Düzenle: {vaka.VakaAdi}";

            IncidentNameBox.Text = vaka.VakaAdi;
            SelectComboBoxItem(IncidentTypeComboBox, vaka.VakaTuru);
            SelectComboBoxItem(SeverityComboBox, vaka.VakaCiddiyetSeviyesi);
            SelectComboBoxItem(IncidentStatusComboBox, vaka.VakaDurumu);

            IncidentDatePicker.SelectedDate = vaka.VakaOlusmaTarihi.Date;
            IncidentTimeBox.Text = vaka.VakaOlusmaTarihi.ToString("HH:mm");

            CauseBox.Text = vaka.OlayNedeni;
            DescriptionBox.Text = vaka.VakaAciklamasi;

            PersonnelIdBox.Text = vaka.PersonelId?.ToString() ?? string.Empty;
            ReporterEquipmentIdBox.Text = vaka.RaporlayanEkipmanId?.ToString() ?? string.Empty;
            RelatedEquipmentIdBox.Text = vaka.IlgiliEkipmanId?.ToString() ?? string.Empty;
            CloseDatePicker.SelectedDate = vaka.VakaKapanmaTarihi?.Date;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Vaka detayı alınırken hata oluştu: {ex.Message}");
        }
    }

    private async void BtnDelete_Click(object sender, RoutedEventArgs e)
    {
        if (!_canManage)
            return;

        if (((Button)sender).DataContext is not IncidentModel selectedIncident)
            return;

        var confirm = MessageBox.Show(
            $"{selectedIncident.Name} adlı vakayı silmek istiyor musunuz?",
            "Vaka Sil",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (confirm != MessageBoxResult.Yes)
            return;

        try
        {
            var response = await _httpClient.DeleteAsync($"/api/vaka/{selectedIncident.Id}");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                MessageBox.Show($"Vaka silinemedi.\n\n{error}");
                return;
            }

            await LoadIncidentDataAsync();
            RefreshGrid();

            MessageBox.Show("Vaka başarıyla silindi.");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Vaka silinirken hata oluştu: {ex.Message}");
        }
    }

    private async void BtnSave_Click(object sender, RoutedEventArgs e)
    {
        if (!_canManage)
            return;

        if (string.IsNullOrWhiteSpace(IncidentNameBox.Text))
        {
            MessageBox.Show("Vaka adı zorunludur.");
            return;
        }

        if (!TryBuildDateTime(IncidentDatePicker.SelectedDate, IncidentTimeBox.Text, out var incidentDateTime))
        {
            MessageBox.Show("Oluşma tarihi veya saati geçersiz. Saat örneği: 14:30");
            return;
        }

        try
        {
            if (_selectedIncidentId is null)
            {
                var createRequest = BuildCreateRequest(incidentDateTime);

                var response = await _httpClient.PostAsJsonAsync("/api/vaka", createRequest);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();

                    if (error.Contains("FOREIGN KEY", StringComparison.OrdinalIgnoreCase) ||
                        error.Contains("FK_Vaka_Ekipman", StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show("Vaka kaydedilemedi. Girilen ekipman ID veritabanında bulunmuyor. Raporlayan/İlgili Ekipman ID alanlarını boş bırakın veya var olan bir ekipman ID girin.");
                        return;
                    }

                    MessageBox.Show($"Vaka oluşturulamadı.\n\n{error}");
                    return;
                }

                MessageBox.Show("Vaka başarıyla oluşturuldu.");
            }
            else
            {
                var updateRequest = BuildUpdateRequest(incidentDateTime);

                var response = await _httpClient.PutAsJsonAsync(
                    $"/api/vaka/{_selectedIncidentId.Value}",
                    updateRequest);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Vaka güncellenemedi.\n\n{error}");
                    return;
                }

                MessageBox.Show("Vaka başarıyla güncellendi.");
            }

            await LoadIncidentDataAsync();
            RefreshGrid();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Kaydetme sırasında hata oluştu: {ex.Message}");
        }
    }

    private VakaCreateRequest BuildCreateRequest(DateTime incidentDateTime)
    {
        return new VakaCreateRequest
        {
            VakaTuru = GetComboBoxValue(IncidentTypeComboBox, "Sensör Uyarısı"),
            VakaAdi = IncidentNameBox.Text.Trim(),
            VakaCiddiyetSeviyesi = GetComboBoxValue(SeverityComboBox, "Orta"),
            VakaDurumu = GetComboBoxValue(IncidentStatusComboBox, "Açık"),
            VakaAciklamasi = DescriptionBox.Text.Trim(),
            VakaOlusmaTarihi = incidentDateTime,
            VakaKapanmaTarihi = CloseDatePicker.SelectedDate,
            OlayNedeni = CauseBox.Text.Trim(),
            PersonelId = ParseNullableInt(PersonnelIdBox.Text),
            RaporlayanEkipmanId = ParseNullableInt(ReporterEquipmentIdBox.Text),
            IlgiliEkipmanId = ParseNullableInt(RelatedEquipmentIdBox.Text)
        };
    }

    private VakaUpdateRequest BuildUpdateRequest(DateTime incidentDateTime)
    {
        return new VakaUpdateRequest
        {
            VakaTuru = GetComboBoxValue(IncidentTypeComboBox, "Sensör Uyarısı"),
            VakaAdi = IncidentNameBox.Text.Trim(),
            VakaCiddiyetSeviyesi = GetComboBoxValue(SeverityComboBox, "Orta"),
            VakaDurumu = GetComboBoxValue(IncidentStatusComboBox, "Açık"),
            VakaAciklamasi = DescriptionBox.Text.Trim(),
            VakaOlusmaTarihi = incidentDateTime,
            VakaKapanmaTarihi = CloseDatePicker.SelectedDate,
            OlayNedeni = CauseBox.Text.Trim(),
            PersonelId = ParseNullableInt(PersonnelIdBox.Text),
            RaporlayanEkipmanId = ParseNullableInt(ReporterEquipmentIdBox.Text),
            IlgiliEkipmanId = ParseNullableInt(RelatedEquipmentIdBox.Text)
        };
    }

    private static bool TryBuildDateTime(DateTime? date, string timeText, out DateTime result)
    {
        result = default;

        if (date is null)
            return false;

        if (!TimeSpan.TryParse(timeText, out var time))
            return false;

        result = date.Value.Date.Add(time);
        return true;
    }

    private static int? ParseNullableInt(string? value)
    {
        if (!int.TryParse(value, out var result))
            return null;

        return result <= 0 ? null : result;
    }

    private static string GetComboBoxValue(ComboBox comboBox, string defaultValue = "")
    {
        if (comboBox.SelectedItem is ComboBoxItem item)
            return item.Content?.ToString() ?? defaultValue;

        return string.IsNullOrWhiteSpace(comboBox.Text) ? defaultValue : comboBox.Text;
    }

    private static void SelectComboBoxItem(ComboBox comboBox, string value)
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

    private void ApplyPermissions()
    {
        BtnAddNewIncident.Visibility = _canManage ? Visibility.Visible : Visibility.Collapsed;
        IncidentActionsColumn.Visibility = _canManage ? Visibility.Visible : Visibility.Collapsed;
    }
}

public class IncidentModel
{
    public int Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public string CreatedText { get; set; } = string.Empty;
    public DateTime? ClosedDate { get; set; }
    public string Cause { get; set; } = string.Empty;
    public int? PersonelId { get; set; }
    public string PersonnelName { get; set; } = string.Empty;
    public int? ReporterEquipmentId { get; set; }
    public string ReporterEquipmentName { get; set; } = string.Empty;
    public int? RelatedEquipmentId { get; set; }
    public string RelatedEquipmentName { get; set; } = string.Empty;
}

public class VakaPagedData
{
    public List<VakaListItemDto> Items { get; set; } = new();
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public bool HasPreviousPage { get; set; }
    public bool HasNextPage { get; set; }
}

public class VakaListItemDto
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

public class VakaDetailDto
{
    public int VakaId { get; set; }
    public string VakaTuru { get; set; } = string.Empty;
    public string VakaAdi { get; set; } = string.Empty;
    public string VakaCiddiyetSeviyesi { get; set; } = string.Empty;
    public string VakaDurumu { get; set; } = string.Empty;
    public string VakaAciklamasi { get; set; } = string.Empty;
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

public class VakaCreateRequest
{
    public string VakaTuru { get; set; } = string.Empty;
    public string VakaAdi { get; set; } = string.Empty;
    public string VakaCiddiyetSeviyesi { get; set; } = string.Empty;
    public string VakaDurumu { get; set; } = "Açık";
    public string VakaAciklamasi { get; set; } = string.Empty;
    public DateTime VakaOlusmaTarihi { get; set; }
    public DateTime? VakaKapanmaTarihi { get; set; }
    public string OlayNedeni { get; set; } = string.Empty;
    public int? PersonelId { get; set; }
    public int? RaporlayanEkipmanId { get; set; }
    public int? IlgiliEkipmanId { get; set; }
}

public class VakaUpdateRequest
{
    public string VakaTuru { get; set; } = string.Empty;
    public string VakaAdi { get; set; } = string.Empty;
    public string VakaCiddiyetSeviyesi { get; set; } = string.Empty;
    public string VakaDurumu { get; set; } = string.Empty;
    public string VakaAciklamasi { get; set; } = string.Empty;
    public DateTime VakaOlusmaTarihi { get; set; }
    public DateTime? VakaKapanmaTarihi { get; set; }
    public string OlayNedeni { get; set; } = string.Empty;
    public int? PersonelId { get; set; }
    public int? RaporlayanEkipmanId { get; set; }
    public int? IlgiliEkipmanId { get; set; }
}