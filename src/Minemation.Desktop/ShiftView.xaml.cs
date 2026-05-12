using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Minemation.Desktop.Models;

namespace Minemation.Desktop;

public partial class ShiftView : UserControl
{
    private readonly bool _canManage;

    private readonly HttpClient _httpClient = new()
    {
        BaseAddress = new Uri("http://localhost:5289")
    };

    private List<ShiftModel> _allShifts = new();
    private bool _showOnlyActive = true;
    private int? _selectedShiftId = null;

    public ShiftView(bool canManage = true)
    {
        _canManage = canManage;

        InitializeComponent();
        Loaded += ShiftView_Loaded;
    }

    private async void ShiftView_Loaded(object sender, RoutedEventArgs e)
    {
        ApplyPermissions();

        await LoadShiftDataAsync();
        RefreshGrid();
    }

    private async Task LoadShiftDataAsync()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<VardiyaPagedData>>("/api/vardiya");

            if (response?.Success != true || response.Data is null)
            {
                MessageBox.Show(response?.Message ?? "Vardiya listesi alınamadı.");
                return;
            }

            _allShifts = response.Data.Items.Select(x => new ShiftModel
            {
                Id = x.VardiyaId,
                Name = x.VardiyaAdi,
                StartDate = x.VardiyaBaslangicTarihi,
                EndDate = x.VardiyaBitisTarihi,
                StartText = x.VardiyaBaslangicTarihi.ToString("dd.MM.yyyy HH:mm"),
                EndText = x.VardiyaBitisTarihi.ToString("dd.MM.yyyy HH:mm"),
                Status = x.VardiyaDurumu,
                Type = x.VardiyaTipi,
                WorkArea = x.CalismaBolgesi,
                OperationType = x.OperasyonTipi,
                RiskLevel = x.OperasyonRiskSeviyesi,
                PersonnelCount = x.PersonelSayisi,
                EquipmentCount = x.EkipmanSayisi,
                TeamCount = x.EkipSayisi
            }).ToList();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Vardiya verileri alınırken hata oluştu: {ex.Message}");
        }
    }

    private void RefreshGrid()
    {
        if (ShiftGrid == null)
            return;

        var searchText = ShiftSearchBox.Text?.ToLower() ?? "";

        var filteredList = _allShifts
            .Where(s =>
                !_showOnlyActive ||
                s.Status.Equals("Aktif", StringComparison.OrdinalIgnoreCase) ||
                s.Status.Equals("Planlandı", StringComparison.OrdinalIgnoreCase))
            .Where(s =>
                string.IsNullOrWhiteSpace(searchText) ||
                s.Name.ToLower().Contains(searchText) ||
                s.WorkArea.ToLower().Contains(searchText) ||
                s.OperationType.ToLower().Contains(searchText) ||
                s.RiskLevel.ToLower().Contains(searchText) ||
                s.Type.ToLower().Contains(searchText))
            .ToList();

        ShiftGrid.ItemsSource = filteredList;
        ShiftTotalText.Text = $"Toplam {filteredList.Count} Vardiya";
    }

    private void BtnActiveShifts_Click(object sender, RoutedEventArgs e)
    {
        _showOnlyActive = true;
        UpdateTabStyles();
        RefreshGrid();
    }

    private void BtnShiftHistory_Click(object sender, RoutedEventArgs e)
    {
        _showOnlyActive = false;
        UpdateTabStyles();
        RefreshGrid();
    }

    private void UpdateTabStyles()
    {
        if (_showOnlyActive)
        {
            BtnActiveShifts.Foreground = (Brush)FindResource("PrimaryBrush");
            BtnActiveShifts.BorderBrush = (Brush)FindResource("PrimaryBrush");
            BtnActiveShifts.BorderThickness = new Thickness(0, 0, 0, 2);

            BtnShiftHistory.Foreground = (Brush)FindResource("TextSecondaryBrush");
            BtnShiftHistory.BorderThickness = new Thickness(0);
        }
        else
        {
            BtnShiftHistory.Foreground = (Brush)FindResource("PrimaryBrush");
            BtnShiftHistory.BorderBrush = (Brush)FindResource("PrimaryBrush");
            BtnShiftHistory.BorderThickness = new Thickness(0, 0, 0, 2);

            BtnActiveShifts.Foreground = (Brush)FindResource("TextSecondaryBrush");
            BtnActiveShifts.BorderThickness = new Thickness(0);
        }
    }

    private void ShiftSearchBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        RefreshGrid();
    }

    private async void BtnRefresh_Click(object sender, RoutedEventArgs e)
    {
        await LoadShiftDataAsync();
        RefreshGrid();
    }

    private void BtnAddNew_Click(object sender, RoutedEventArgs e)
    {
        if (!_canManage)
            return;

        _selectedShiftId = null;

        ShiftFormTitle.Text = "Yeni Vardiya Kaydı";

        ShiftNameBox.Text = string.Empty;
        ShiftDescriptionBox.Text = string.Empty;

        StartDatePicker.SelectedDate = DateTime.Today;
        EndDatePicker.SelectedDate = DateTime.Today;

        StartTimeBox.Text = "08:00";
        EndTimeBox.Text = "16:00";

        SupervisorBox.Text = string.Empty;

        SelectComboBoxItem(ShiftStatusComboBox, "Aktif");
        ShiftTypeComboBox.SelectedIndex = -1;
        WorkAreaBox.Text = string.Empty;
        OperationTypeBox.Text = string.Empty;
        RiskLevelComboBox.SelectedIndex = -1;
        ShiftNotesBox.Text = string.Empty;
        EquipmentOperatorBox.Text = string.Empty;

        PersonnelCountBox.Text = "0";
        EquipmentCountBox.Text = "0";
        TeamCountBox.Text = "0";

        ResponsiblePersonBox.Text = "1";
        IsgResponsibleBox.Text = "1";
        TechnicalResponsibleBox.Text = "1";
    }

    private async void BtnEdit_Click(object sender, RoutedEventArgs e)
    {
        if (!_canManage)
            return;

        if (((Button)sender).DataContext is not ShiftModel selectedShift)
            return;

        try
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<VardiyaDetailDto>>(
                $"/api/vardiya/{selectedShift.Id}");

            if (response?.Success != true || response.Data is null)
            {
                MessageBox.Show(response?.Message ?? "Vardiya detayı alınamadı.");
                return;
            }

            var vardiya = response.Data;

            _selectedShiftId = vardiya.VardiyaId;

            ShiftFormTitle.Text = $"Vardiya Düzenle: {vardiya.VardiyaAdi}";

            ShiftNameBox.Text = vardiya.VardiyaAdi;
            ShiftDescriptionBox.Text = vardiya.VardiyaTanimi;

            StartDatePicker.SelectedDate = vardiya.VardiyaBaslangicTarihi.Date;
            EndDatePicker.SelectedDate = vardiya.VardiyaBitisTarihi.Date;

            StartTimeBox.Text = vardiya.VardiyaBaslangicTarihi.ToString("HH:mm");
            EndTimeBox.Text = vardiya.VardiyaBitisTarihi.ToString("HH:mm");

            SupervisorBox.Text = vardiya.VardiyaSupervizoru;

            SelectComboBoxItem(ShiftStatusComboBox, vardiya.VardiyaDurumu);
            SelectComboBoxItem(ShiftTypeComboBox, vardiya.VardiyaTipi);

            WorkAreaBox.Text = vardiya.CalismaBolgesi;
            OperationTypeBox.Text = vardiya.OperasyonTipi;

            SelectComboBoxItem(RiskLevelComboBox, vardiya.OperasyonRiskSeviyesi);

            ShiftNotesBox.Text = vardiya.VardiyaNotlari;
            EquipmentOperatorBox.Text = vardiya.EkipmanOperatoru;

            PersonnelCountBox.Text = vardiya.PersonelSayisi.ToString();
            EquipmentCountBox.Text = vardiya.EkipmanSayisi.ToString();
            TeamCountBox.Text = vardiya.EkipSayisi.ToString();

            ResponsiblePersonBox.Text = vardiya.VardiyaSorumlusu.ToString();
            IsgResponsibleBox.Text = vardiya.VardiyaIsgSorumlusu.ToString();
            TechnicalResponsibleBox.Text = vardiya.VardiyaTeknikSorumlusu.ToString();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Vardiya detayı alınırken hata oluştu: {ex.Message}");
        }
    }

    private async void BtnDelete_Click(object sender, RoutedEventArgs e)
    {
        if (!_canManage)
            return;

        if (((Button)sender).DataContext is not ShiftModel selectedShift)
            return;

        var confirm = MessageBox.Show(
            $"{selectedShift.Name} adlı vardiyayı silmek istiyor musunuz?",
            "Vardiya Sil",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (confirm != MessageBoxResult.Yes)
            return;

        try
        {
            var response = await _httpClient.DeleteAsync($"/api/vardiya/{selectedShift.Id}");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                MessageBox.Show($"Vardiya silinemedi.\n\n{error}");
                return;
            }

            await LoadShiftDataAsync();
            RefreshGrid();

            MessageBox.Show("Vardiya başarıyla silindi.");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Vardiya silinirken hata oluştu: {ex.Message}");
        }
    }

    private async void BtnSave_Click(object sender, RoutedEventArgs e)
    {
        if (!_canManage)
            return;

        if (string.IsNullOrWhiteSpace(ShiftNameBox.Text))
        {
            MessageBox.Show("Vardiya adı zorunludur.");
            return;
        }

        if (!TryBuildDateTime(StartDatePicker.SelectedDate, StartTimeBox.Text, out var startDateTime))
        {
            MessageBox.Show("Başlangıç tarihi veya saati geçersiz. Saat örneği: 08:00");
            return;
        }

        if (!TryBuildDateTime(EndDatePicker.SelectedDate, EndTimeBox.Text, out var endDateTime))
        {
            MessageBox.Show("Bitiş tarihi veya saati geçersiz. Saat örneği: 16:00");
            return;
        }

        if (endDateTime <= startDateTime)
        {
            MessageBox.Show("Bitiş zamanı başlangıç zamanından sonra olmalıdır.");
            return;
        }

        try
        {
            if (_selectedShiftId is null)
            {
                var createRequest = BuildCreateRequest(startDateTime, endDateTime);

                var response = await _httpClient.PostAsJsonAsync("/api/vardiya", createRequest);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Vardiya oluşturulamadı.\n\n{error}");
                    return;
                }

                MessageBox.Show("Vardiya başarıyla oluşturuldu.");
            }
            else
            {
                var updateRequest = BuildUpdateRequest(startDateTime, endDateTime);

                var response = await _httpClient.PutAsJsonAsync(
                    $"/api/vardiya/{_selectedShiftId.Value}",
                    updateRequest);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Vardiya güncellenemedi.\n\n{error}");
                    return;
                }

                MessageBox.Show("Vardiya başarıyla güncellendi.");
            }

            await LoadShiftDataAsync();
            RefreshGrid();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Kaydetme sırasında hata oluştu: {ex.Message}");
        }
    }

    private VardiyaCreateRequest BuildCreateRequest(DateTime startDateTime, DateTime endDateTime)
    {
        return new VardiyaCreateRequest
        {
            VardiyaAdi = ShiftNameBox.Text.Trim(),
            VardiyaTanimi = ShiftDescriptionBox.Text.Trim(),
            VardiyaBaslangicTarihi = startDateTime,
            VardiyaBitisTarihi = endDateTime,
            VardiyaSupervizoru = SupervisorBox.Text.Trim(),
            PersonelSayisi = ParseInt(PersonnelCountBox.Text),
            EkipmanSayisi = ParseInt(EquipmentCountBox.Text),
            EkipSayisi = ParseInt(TeamCountBox.Text),
            VardiyaDurumu = GetComboBoxValue(ShiftStatusComboBox, "Aktif"),
            VardiyaTipi = GetComboBoxValue(ShiftTypeComboBox, "Gündüz"),
            ToplaVardiyaSuresi = CalculateShiftHours(startDateTime, endDateTime),
            CalismaBolgesi = WorkAreaBox.Text.Trim(),
            OperasyonTipi = OperationTypeBox.Text.Trim(),
            OperasyonRiskSeviyesi = GetComboBoxValue(RiskLevelComboBox, "Düşük"),
            VardiyaNotlari = ShiftNotesBox.Text.Trim(),
            EkipmanOperatoru = EquipmentOperatorBox.Text.Trim(),
            EkipmanId = null,
            VardiyaSorumlusu = ParseInt(ResponsiblePersonBox.Text, 1),
            VardiyaIsgSorumlusu = ParseInt(IsgResponsibleBox.Text, 1),
            VardiyaTeknikSorumlusu = ParseInt(TechnicalResponsibleBox.Text, 1)
        };
    }

    private VardiyaUpdateRequest BuildUpdateRequest(DateTime startDateTime, DateTime endDateTime)
    {
        return new VardiyaUpdateRequest
        {
            VardiyaAdi = ShiftNameBox.Text.Trim(),
            VardiyaTanimi = ShiftDescriptionBox.Text.Trim(),
            VardiyaBaslangicTarihi = startDateTime,
            VardiyaBitisTarihi = endDateTime,
            VardiyaSupervizoru = SupervisorBox.Text.Trim(),
            PersonelSayisi = ParseInt(PersonnelCountBox.Text),
            EkipmanSayisi = ParseInt(EquipmentCountBox.Text),
            EkipSayisi = ParseInt(TeamCountBox.Text),
            VardiyaDurumu = GetComboBoxValue(ShiftStatusComboBox, "Aktif"),
            VardiyaTipi = GetComboBoxValue(ShiftTypeComboBox, "Gündüz"),
            ToplaVardiyaSuresi = CalculateShiftHours(startDateTime, endDateTime),
            CalismaBolgesi = WorkAreaBox.Text.Trim(),
            OperasyonTipi = OperationTypeBox.Text.Trim(),
            OperasyonRiskSeviyesi = GetComboBoxValue(RiskLevelComboBox, "Düşük"),
            VardiyaNotlari = ShiftNotesBox.Text.Trim(),
            EkipmanOperatoru = EquipmentOperatorBox.Text.Trim(),
            EkipmanId = null,
            VardiyaSorumlusu = ParseInt(ResponsiblePersonBox.Text, 1),
            VardiyaIsgSorumlusu = ParseInt(IsgResponsibleBox.Text, 1),
            VardiyaTeknikSorumlusu = ParseInt(TechnicalResponsibleBox.Text, 1)
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

    private static int ParseInt(string? value, int defaultValue = 0)
    {
        return int.TryParse(value, out var result) ? result : defaultValue;
    }

    private static int CalculateShiftHours(DateTime startDateTime, DateTime endDateTime)
    {
        return Math.Max(1, (int)Math.Ceiling((endDateTime - startDateTime).TotalHours));
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
        BtnAddNewShift.Visibility = _canManage ? Visibility.Visible : Visibility.Collapsed;
        ShiftActionsColumn.Visibility = _canManage ? Visibility.Visible : Visibility.Collapsed;
    }
}

public class ShiftModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string StartText { get; set; } = string.Empty;
    public string EndText { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string WorkArea { get; set; } = string.Empty;
    public string OperationType { get; set; } = string.Empty;
    public string RiskLevel { get; set; } = string.Empty;
    public int PersonnelCount { get; set; }
    public int EquipmentCount { get; set; }
    public int TeamCount { get; set; }
}

public class VardiyaPagedData
{
    public List<VardiyaListItemDto> Items { get; set; } = new();
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public bool HasPreviousPage { get; set; }
    public bool HasNextPage { get; set; }
}

public class VardiyaListItemDto
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

public class VardiyaDetailDto
{
    public int VardiyaId { get; set; }
    public string VardiyaAdi { get; set; } = string.Empty;
    public string VardiyaTanimi { get; set; } = string.Empty;
    public DateTime VardiyaBaslangicTarihi { get; set; }
    public DateTime VardiyaBitisTarihi { get; set; }
    public DateTime VardiyaOlusturmaTarihi { get; set; }
    public string VardiyaSupervizoru { get; set; } = string.Empty;
    public int PersonelSayisi { get; set; }
    public int EkipmanSayisi { get; set; }
    public int EkipSayisi { get; set; }
    public string VardiyaDurumu { get; set; } = string.Empty;
    public string VardiyaTipi { get; set; } = string.Empty;
    public int ToplaVardiyaSuresi { get; set; }
    public string CalismaBolgesi { get; set; } = string.Empty;
    public string OperasyonTipi { get; set; } = string.Empty;
    public string OperasyonRiskSeviyesi { get; set; } = string.Empty;
    public string VardiyaNotlari { get; set; } = string.Empty;
    public string EkipmanOperatoru { get; set; } = string.Empty;
    public int? EkipmanId { get; set; }
    public string? EkipmanAdi { get; set; }
    public int VardiyaSorumlusu { get; set; }
    public string? VardiyaSorumlusuAdSoyad { get; set; }
    public int VardiyaIsgSorumlusu { get; set; }
    public string? IsgSorumlusuAdSoyad { get; set; }
    public int VardiyaTeknikSorumlusu { get; set; }
    public string? TeknikSorumlusuAdSoyad { get; set; }
}

public class VardiyaCreateRequest
{
    public string VardiyaAdi { get; set; } = string.Empty;
    public string VardiyaTanimi { get; set; } = string.Empty;
    public DateTime VardiyaBaslangicTarihi { get; set; }
    public DateTime VardiyaBitisTarihi { get; set; }
    public string VardiyaSupervizoru { get; set; } = string.Empty;
    public int PersonelSayisi { get; set; }
    public int EkipmanSayisi { get; set; }
    public int EkipSayisi { get; set; }
    public string VardiyaDurumu { get; set; } = "Aktif";
    public string VardiyaTipi { get; set; } = string.Empty;
    public int ToplaVardiyaSuresi { get; set; }
    public string CalismaBolgesi { get; set; } = string.Empty;
    public string OperasyonTipi { get; set; } = string.Empty;
    public string OperasyonRiskSeviyesi { get; set; } = string.Empty;
    public string VardiyaNotlari { get; set; } = string.Empty;
    public string EkipmanOperatoru { get; set; } = string.Empty;
    public int? EkipmanId { get; set; }
    public int VardiyaSorumlusu { get; set; }
    public int VardiyaIsgSorumlusu { get; set; }
    public int VardiyaTeknikSorumlusu { get; set; }
}

public class VardiyaUpdateRequest
{
    public string VardiyaAdi { get; set; } = string.Empty;
    public string VardiyaTanimi { get; set; } = string.Empty;
    public DateTime VardiyaBaslangicTarihi { get; set; }
    public DateTime VardiyaBitisTarihi { get; set; }
    public string VardiyaSupervizoru { get; set; } = string.Empty;
    public int PersonelSayisi { get; set; }
    public int EkipmanSayisi { get; set; }
    public int EkipSayisi { get; set; }
    public string VardiyaDurumu { get; set; } = string.Empty;
    public string VardiyaTipi { get; set; } = string.Empty;
    public int ToplaVardiyaSuresi { get; set; }
    public string CalismaBolgesi { get; set; } = string.Empty;
    public string OperasyonTipi { get; set; } = string.Empty;
    public string OperasyonRiskSeviyesi { get; set; } = string.Empty;
    public string VardiyaNotlari { get; set; } = string.Empty;
    public string EkipmanOperatoru { get; set; } = string.Empty;
    public int? EkipmanId { get; set; }
    public int VardiyaSorumlusu { get; set; }
    public int VardiyaIsgSorumlusu { get; set; }
    public int VardiyaTeknikSorumlusu { get; set; }
}