using System;
using System.Collections.Generic;
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

public partial class EquipmentView : UserControl
{
    private readonly HttpClient _httpClient = new()
    {
        BaseAddress = new Uri("http://localhost:5289")
    };

    private List<EquipmentModel> _allEquipment = new();
    private bool _showOnlyActive = true;
    private int? _selectedEquipmentId = null;

    public EquipmentView()
    {
        InitializeComponent();
        Loaded += EquipmentView_Loaded;
    }

    private async void EquipmentView_Loaded(object sender, RoutedEventArgs e)
    {
        await LoadEquipmentDataAsync();
        RefreshGrid();
    }

    private async Task LoadEquipmentDataAsync()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<EquipmentPagedData>>("/api/ekipman");

            if (response?.Success != true || response.Data is null)
            {
                MessageBox.Show(response?.Message ?? "Ekipman listesi alınamadı.");
                return;
            }

            _allEquipment = response.Data.Items.Select(x => new EquipmentModel
            {
                Id = x.EkipmanId,
                Name = x.EkipmanAdi,
                Brand = x.EkipmanMarka,
                Model = x.EkipmanModel,
                Status = x.Durum,
                SerialNo = x.SeriNo,
                RfidTag = x.RFIDEtiket,
                OperationType = x.OperasyonTuru
            }).ToList();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ekipman verileri alınırken hata oluştu: {ex.Message}");
        }
    }

    private void RefreshGrid()
    {
        if (EquipmentGrid == null)
            return;

        var searchText = EquipmentSearchBox.Text?.ToLower() ?? "";

        var filteredList = _allEquipment
            .Where(e =>
                !_showOnlyActive ||
                e.Status.Equals("Aktif", StringComparison.OrdinalIgnoreCase))
            .Where(e =>
                string.IsNullOrWhiteSpace(searchText) ||
                e.Name.ToLower().Contains(searchText) ||
                e.Brand.ToLower().Contains(searchText) ||
                e.Model.ToLower().Contains(searchText) ||
                e.SerialNo.ToLower().Contains(searchText) ||
                e.RfidTag.ToLower().Contains(searchText) ||
                e.OperationType.ToLower().Contains(searchText))
            .ToList();

        EquipmentGrid.ItemsSource = filteredList;
        EquipmentTotalText.Text = $"Toplam {filteredList.Count} Ekipman";
    }

    private void BtnActiveEquipment_Click(object sender, RoutedEventArgs e)
    {
        _showOnlyActive = true;
        UpdateTabStyles();
        RefreshGrid();
    }

    private void BtnAllEquipment_Click(object sender, RoutedEventArgs e)
    {
        _showOnlyActive = false;
        UpdateTabStyles();
        RefreshGrid();
    }

    private void UpdateTabStyles()
    {
        if (_showOnlyActive)
        {
            BtnActiveEquipment.Foreground = (Brush)FindResource("PrimaryBrush");
            BtnActiveEquipment.BorderBrush = (Brush)FindResource("PrimaryBrush");
            BtnActiveEquipment.BorderThickness = new Thickness(0, 0, 0, 2);

            BtnAllEquipment.Foreground = (Brush)FindResource("TextSecondaryBrush");
            BtnAllEquipment.BorderThickness = new Thickness(0);
        }
        else
        {
            BtnAllEquipment.Foreground = (Brush)FindResource("PrimaryBrush");
            BtnAllEquipment.BorderBrush = (Brush)FindResource("PrimaryBrush");
            BtnAllEquipment.BorderThickness = new Thickness(0, 0, 0, 2);

            BtnActiveEquipment.Foreground = (Brush)FindResource("TextSecondaryBrush");
            BtnActiveEquipment.BorderThickness = new Thickness(0);
        }
    }

    private void EquipmentSearchBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        RefreshGrid();
    }

    private async void BtnRefresh_Click(object sender, RoutedEventArgs e)
    {
        await LoadEquipmentDataAsync();
        RefreshGrid();
    }

    private void BtnAddNew_Click(object sender, RoutedEventArgs e)
    {
        _selectedEquipmentId = null;

        EquipmentFormTitle.Text = "Yeni Ekipman Kaydı";

        EquipmentNameBox.Text = string.Empty;
        BrandBox.Text = string.Empty;
        ModelBox.Text = string.Empty;
        SelectComboBoxItem(StatusComboBox, "Aktif");

        SerialNoBox.Text = string.Empty;
        RfidTagBox.Text = string.Empty;
        OperationTypeBox.Text = string.Empty;

        LastMaintenanceDatePicker.SelectedDate = DateTime.Today;
        NextMaintenanceDatePicker.SelectedDate = DateTime.Today.AddMonths(1);

        ManufacturerBox.Text = string.Empty;
        SupplierBox.Text = string.Empty;

        ProductionYearPicker.SelectedDate = DateTime.Today;
        PurchaseDatePicker.SelectedDate = DateTime.Today;
        WarrantyStartDatePicker.SelectedDate = DateTime.Today;

        SizeBox.Text = "0";
        WeightBox.Text = "0";

        TechnicalDocumentBox.Text = string.Empty;
        UserManualBox.Text = string.Empty;
        WarrantyDocumentBox.Text = string.Empty;
        MaintenanceFormBox.Text = string.Empty;
    }

    private async void BtnEdit_Click(object sender, RoutedEventArgs e)
    {
        if (((Button)sender).DataContext is not EquipmentModel selectedEquipment)
            return;

        try
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<EquipmentDetailDto>>(
                $"/api/ekipman/{selectedEquipment.Id}");

            if (response?.Success != true || response.Data is null)
            {
                MessageBox.Show(response?.Message ?? "Ekipman detayı alınamadı.");
                return;
            }

            var equipment = response.Data;

            _selectedEquipmentId = equipment.EkipmanId;

            EquipmentFormTitle.Text = $"Ekipman Düzenle: {equipment.EkipmanAdi}";

            EquipmentNameBox.Text = equipment.EkipmanAdi;
            BrandBox.Text = equipment.EkipmanMarka;
            ModelBox.Text = equipment.EkipmanModel;
            SelectComboBoxItem(StatusComboBox, equipment.Durum);

            SerialNoBox.Text = equipment.SeriNo;
            RfidTagBox.Text = equipment.RFIDEtiket;
            OperationTypeBox.Text = equipment.OperasyonTuru;

            LastMaintenanceDatePicker.SelectedDate = equipment.SonBakimTarihi == default ? DateTime.Today : equipment.SonBakimTarihi;
            NextMaintenanceDatePicker.SelectedDate = equipment.GelecekBakimTarihi == default ? DateTime.Today.AddMonths(1) : equipment.GelecekBakimTarihi;

            ManufacturerBox.Text = equipment.UreticiFirma;
            SupplierBox.Text = equipment.TedarikciFirma;

            ProductionYearPicker.SelectedDate = equipment.UretimYili == default ? DateTime.Today : equipment.UretimYili;
            PurchaseDatePicker.SelectedDate = equipment.SatinAlmaTarihi == default ? DateTime.Today : equipment.SatinAlmaTarihi;
            WarrantyStartDatePicker.SelectedDate = equipment.GarantiBaslangicTarihi == default ? DateTime.Today : equipment.GarantiBaslangicTarihi;

            SizeBox.Text = equipment.Boyut.ToString();
            WeightBox.Text = equipment.Agirlik.ToString();

            TechnicalDocumentBox.Text = equipment.TeknikDokuman;
            UserManualBox.Text = equipment.KullanimKilavuzu;
            WarrantyDocumentBox.Text = equipment.GarantiBelgesi;
            MaintenanceFormBox.Text = equipment.BakimFormu;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ekipman detayı alınırken hata oluştu: {ex.Message}");
        }
    }

    private async void BtnDelete_Click(object sender, RoutedEventArgs e)
    {
        if (((Button)sender).DataContext is not EquipmentModel selectedEquipment)
            return;

        var confirm = MessageBox.Show(
            $"{selectedEquipment.Name} adlı ekipmanı silmek istiyor musunuz?",
            "Ekipman Sil",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (confirm != MessageBoxResult.Yes)
            return;

        try
        {
            var response = await _httpClient.DeleteAsync($"/api/ekipman/{selectedEquipment.Id}");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                MessageBox.Show($"Ekipman silinemedi.\n\n{error}");
                return;
            }

            await LoadEquipmentDataAsync();
            RefreshGrid();

            MessageBox.Show("Ekipman başarıyla silindi.");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ekipman silinirken hata oluştu: {ex.Message}");
        }
    }

    private async void BtnSave_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(EquipmentNameBox.Text))
        {
            MessageBox.Show("Ekipman adı zorunludur.");
            return;
        }

        try
        {
            if (_selectedEquipmentId is null)
            {
                var createRequest = BuildCreateRequest();

                var response = await _httpClient.PostAsJsonAsync("/api/ekipman", createRequest);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Ekipman oluşturulamadı.\n\n{error}");
                    return;
                }

                MessageBox.Show("Ekipman başarıyla oluşturuldu.");
            }
            else
            {
                var updateRequest = BuildUpdateRequest();

                var response = await _httpClient.PutAsJsonAsync(
                    $"/api/ekipman/{_selectedEquipmentId.Value}",
                    updateRequest);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Ekipman güncellenemedi.\n\n{error}");
                    return;
                }

                MessageBox.Show("Ekipman başarıyla güncellendi.");
            }

            await LoadEquipmentDataAsync();
            RefreshGrid();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Kaydetme sırasında hata oluştu: {ex.Message}");
        }
    }

    private EquipmentCreateRequest BuildCreateRequest()
    {
        return new EquipmentCreateRequest
        {
            EkipmanAdi = EquipmentNameBox.Text.Trim(),
            EkipmanMarka = BrandBox.Text.Trim(),
            EkipmanModel = ModelBox.Text.Trim(),
            Durum = GetComboBoxValue(StatusComboBox, "Aktif"),
            SonBakimTarihi = LastMaintenanceDatePicker.SelectedDate ?? DateTime.Today,
            GelecekBakimTarihi = NextMaintenanceDatePicker.SelectedDate ?? DateTime.Today.AddMonths(1),
            SeriNo = SerialNoBox.Text.Trim(),
            RFIDEtiket = RfidTagBox.Text.Trim(),
            UreticiFirma = ManufacturerBox.Text.Trim(),
            TedarikciFirma = SupplierBox.Text.Trim(),
            UretimYili = ProductionYearPicker.SelectedDate ?? DateTime.Today,
            Boyut = ParseDecimal(SizeBox.Text),
            Agirlik = ParseDecimal(WeightBox.Text),
            SatinAlmaTarihi = PurchaseDatePicker.SelectedDate ?? DateTime.Today,
            GarantiBaslangicTarihi = WarrantyStartDatePicker.SelectedDate ?? DateTime.Today,
            TeknikDokuman = TechnicalDocumentBox.Text.Trim(),
            KullanimKilavuzu = UserManualBox.Text.Trim(),
            GarantiBelgesi = WarrantyDocumentBox.Text.Trim(),
            BakimFormu = MaintenanceFormBox.Text.Trim(),
            SatinAlmaBelgesi = string.Empty,
            OperasyonTuru = OperationTypeBox.Text.Trim()
        };
    }

    private EquipmentUpdateRequest BuildUpdateRequest()
    {
        return new EquipmentUpdateRequest
        {
            EkipmanAdi = EquipmentNameBox.Text.Trim(),
            EkipmanMarka = BrandBox.Text.Trim(),
            EkipmanModel = ModelBox.Text.Trim(),
            Durum = GetComboBoxValue(StatusComboBox, "Aktif"),
            SonBakimTarihi = LastMaintenanceDatePicker.SelectedDate ?? DateTime.Today,
            GelecekBakimTarihi = NextMaintenanceDatePicker.SelectedDate ?? DateTime.Today.AddMonths(1),
            SeriNo = SerialNoBox.Text.Trim(),
            RFIDEtiket = RfidTagBox.Text.Trim(),
            UreticiFirma = ManufacturerBox.Text.Trim(),
            TedarikciFirma = SupplierBox.Text.Trim(),
            UretimYili = ProductionYearPicker.SelectedDate ?? DateTime.Today,
            Boyut = ParseDecimal(SizeBox.Text),
            Agirlik = ParseDecimal(WeightBox.Text),
            SatinAlmaTarihi = PurchaseDatePicker.SelectedDate ?? DateTime.Today,
            GarantiBaslangicTarihi = WarrantyStartDatePicker.SelectedDate ?? DateTime.Today,
            TeknikDokuman = TechnicalDocumentBox.Text.Trim(),
            KullanimKilavuzu = UserManualBox.Text.Trim(),
            GarantiBelgesi = WarrantyDocumentBox.Text.Trim(),
            BakimFormu = MaintenanceFormBox.Text.Trim(),
            SatinAlmaBelgesi = string.Empty,
            OperasyonTuru = OperationTypeBox.Text.Trim()
        };
    }

    private static decimal ParseDecimal(string? value)
    {
        return decimal.TryParse(value, out var result) ? result : 0;
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
}

public class EquipmentModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string SerialNo { get; set; } = string.Empty;
    public string RfidTag { get; set; } = string.Empty;
    public string OperationType { get; set; } = string.Empty;
}

public class EquipmentPagedData
{
    public List<EquipmentListItemDto> Items { get; set; } = new();
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public bool HasPreviousPage { get; set; }
    public bool HasNextPage { get; set; }
}

public class EquipmentListItemDto
{
    public int EkipmanId { get; set; }
    public string EkipmanAdi { get; set; } = string.Empty;
    public string EkipmanMarka { get; set; } = string.Empty;
    public string EkipmanModel { get; set; } = string.Empty;
    public string Durum { get; set; } = string.Empty;
    public string SeriNo { get; set; } = string.Empty;
    public string RFIDEtiket { get; set; } = string.Empty;
    public string OperasyonTuru { get; set; } = string.Empty;
}

public class EquipmentDetailDto
{
    public int EkipmanId { get; set; }
    public string EkipmanAdi { get; set; } = string.Empty;
    public string EkipmanMarka { get; set; } = string.Empty;
    public string EkipmanModel { get; set; } = string.Empty;
    public string Durum { get; set; } = string.Empty;
    public DateTime SonBakimTarihi { get; set; }
    public DateTime GelecekBakimTarihi { get; set; }
    public string SeriNo { get; set; } = string.Empty;
    public string RFIDEtiket { get; set; } = string.Empty;
    public string UreticiFirma { get; set; } = string.Empty;
    public string TedarikciFirma { get; set; } = string.Empty;
    public DateTime UretimYili { get; set; }
    public decimal Boyut { get; set; }
    public decimal Agirlik { get; set; }
    public DateTime SatinAlmaTarihi { get; set; }
    public DateTime GarantiBaslangicTarihi { get; set; }
    public string TeknikDokuman { get; set; } = string.Empty;
    public string KullanimKilavuzu { get; set; } = string.Empty;
    public string GarantiBelgesi { get; set; } = string.Empty;
    public string BakimFormu { get; set; } = string.Empty;
    public string SatinAlmaBelgesi { get; set; } = string.Empty;
    public string OperasyonTuru { get; set; } = string.Empty;
}

public class EquipmentCreateRequest
{
    public string EkipmanAdi { get; set; } = string.Empty;
    public string EkipmanMarka { get; set; } = string.Empty;
    public string EkipmanModel { get; set; } = string.Empty;
    public string Durum { get; set; } = "Aktif";
    public DateTime SonBakimTarihi { get; set; }
    public DateTime GelecekBakimTarihi { get; set; }
    public string SeriNo { get; set; } = string.Empty;
    public string RFIDEtiket { get; set; } = string.Empty;
    public string UreticiFirma { get; set; } = string.Empty;
    public string TedarikciFirma { get; set; } = string.Empty;
    public DateTime UretimYili { get; set; }
    public decimal Boyut { get; set; }
    public decimal Agirlik { get; set; }
    public DateTime SatinAlmaTarihi { get; set; }
    public DateTime GarantiBaslangicTarihi { get; set; }
    public string TeknikDokuman { get; set; } = string.Empty;
    public string KullanimKilavuzu { get; set; } = string.Empty;
    public string GarantiBelgesi { get; set; } = string.Empty;
    public string BakimFormu { get; set; } = string.Empty;
    public string SatinAlmaBelgesi { get; set; } = string.Empty;
    public string OperasyonTuru { get; set; } = string.Empty;
}

public class EquipmentUpdateRequest
{
    public string EkipmanAdi { get; set; } = string.Empty;
    public string EkipmanMarka { get; set; } = string.Empty;
    public string EkipmanModel { get; set; } = string.Empty;
    public string Durum { get; set; } = string.Empty;
    public DateTime SonBakimTarihi { get; set; }
    public DateTime GelecekBakimTarihi { get; set; }
    public string SeriNo { get; set; } = string.Empty;
    public string RFIDEtiket { get; set; } = string.Empty;
    public string UreticiFirma { get; set; } = string.Empty;
    public string TedarikciFirma { get; set; } = string.Empty;
    public DateTime UretimYili { get; set; }
    public decimal Boyut { get; set; }
    public decimal Agirlik { get; set; }
    public DateTime SatinAlmaTarihi { get; set; }
    public DateTime GarantiBaslangicTarihi { get; set; }
    public string TeknikDokuman { get; set; } = string.Empty;
    public string KullanimKilavuzu { get; set; } = string.Empty;
    public string GarantiBelgesi { get; set; } = string.Empty;
    public string BakimFormu { get; set; } = string.Empty;
    public string SatinAlmaBelgesi { get; set; } = string.Empty;
    public string OperasyonTuru { get; set; } = string.Empty;
}
