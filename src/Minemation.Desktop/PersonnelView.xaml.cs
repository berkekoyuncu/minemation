using Minemation.Application.DTOs;
using Minemation.Desktop.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Minemation.Desktop;

public partial class PersonnelView : UserControl
{
    private readonly HttpClient _httpClient = new()
    {
        BaseAddress = new Uri("http://localhost:5289")
    };

    private List<PersonModel> _allPersonnel = new();
    private bool _isShowingAdmin = true;
    private int? _selectedPersonelId;
    private bool _healthRecordExists;

    public PersonnelView()
    {
        InitializeComponent();
        Loaded += PersonnelView_Loaded;
    }

    private async void PersonnelView_Loaded(object sender, RoutedEventArgs e)
    {
        await LoadPersonnelDataAsync();
        RefreshGrid();
    }

    private async Task LoadPersonnelDataAsync()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<PagedResponse<PersonelListItemDto>>>(
                "/api/personel?SayfaNumarasi=1&SayfaBoyutu=1000");

            if (response?.Success != true || response.Data is null)
            {
                MessageBox.Show(response?.Message ?? "Personel listesi alınamadı.");
                return;
            }

            _allPersonnel = response.Data.Items.Select(x => new PersonModel
            {
                Id = x.PersonelId,
                Rfid = x.RfidKartNumarasi,
                Identity = x.Tckn,
                FullName = x.AdSoyad,
                Phone = x.TelNo,
                BloodType = x.KanGrubu,
                Role = x.Uzmanlik,
                Shift = string.Empty,
                Status = x.PersonelDurumu,
                IsAdmin = IsAdminRole(x.KullaniciRolu)
            }).ToList();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Personel verileri alınırken hata oluştu: {ex.Message}");
        }
    }

    private void RefreshGrid()
    {
        if (PersonnelGrid == null)
            return;

        var searchText = SearchBox.Text?.ToLower() ?? "";

        var filteredList = _allPersonnel
            .Where(p => p.IsAdmin == _isShowingAdmin)
            .Where(p =>
                string.IsNullOrWhiteSpace(searchText) ||
                p.FullName.ToLower().Contains(searchText) ||
                p.Identity.ToLower().Contains(searchText) ||
                p.Role.ToLower().Contains(searchText) ||
                p.Rfid.ToLower().Contains(searchText))
            .ToList();

        PersonnelGrid.ItemsSource = filteredList;
    }

    private void BtnAdmin_Click(object sender, RoutedEventArgs e)
    {
        _isShowingAdmin = true;
        UpdateTabStyles();
        RefreshGrid();
    }

    private void BtnField_Click(object sender, RoutedEventArgs e)
    {
        _isShowingAdmin = false;
        UpdateTabStyles();
        RefreshGrid();
    }

    private void UpdateTabStyles()
    {
        if (_isShowingAdmin)
        {
            BtnAdmin.Foreground = (Brush)FindResource("PrimaryBrush");
            BtnAdmin.BorderBrush = (Brush)FindResource("PrimaryBrush");
            BtnAdmin.BorderThickness = new Thickness(0, 0, 0, 2);

            BtnField.Foreground = (Brush)FindResource("TextSecondaryBrush");
            BtnField.BorderThickness = new Thickness(0);
        }
        else
        {
            BtnField.Foreground = (Brush)FindResource("PrimaryBrush");
            BtnField.BorderBrush = (Brush)FindResource("PrimaryBrush");
            BtnField.BorderThickness = new Thickness(0, 0, 0, 2);

            BtnAdmin.Foreground = (Brush)FindResource("TextSecondaryBrush");
            BtnAdmin.BorderThickness = new Thickness(0);
        }
    }

    private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        RefreshGrid();
    }

    private void BtnAddNew_Click(object sender, RoutedEventArgs e)
    {
        _selectedPersonelId = null;
        _healthRecordExists = false;

        FormTitle.Text = "Yeni Personel Kaydı";
        ClearForm();
    }

    private async void BtnEdit_Click(object sender, RoutedEventArgs e)
    {
        if (((Button)sender).DataContext is not PersonModel selectedPerson)
            return;

        try
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<PersonelDetailDto>>(
                $"/api/personel/{selectedPerson.Id}");

            if (response?.Success != true || response.Data is null)
            {
                MessageBox.Show(response?.Message ?? "Personel detayı alınamadı.");
                return;
            }

            _selectedPersonelId = response.Data.PersonelId;

            FormTitle.Text = $"Personel Düzenle: {response.Data.PersonelAdi} {response.Data.PersonelSoyadi}";

            BindPersonnelForm(response.Data);
            await LoadHealthFormAsync(response.Data.PersonelId);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Personel detayı alınırken hata oluştu: {ex.Message}");
        }
    }

    private async void BtnSave_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(FirstNameBox.Text) ||
            string.IsNullOrWhiteSpace(LastNameBox.Text) ||
            string.IsNullOrWhiteSpace(TcknBox.Text))
        {
            MessageBox.Show("Ad, soyad ve T.C. Kimlik No zorunludur.");
            return;
        }

        try
        {
            int personelId;

            if (_selectedPersonelId is null)
            {
                var createRequest = BuildCreateRequest();

                var response = await _httpClient.PostAsJsonAsync("/api/personel", createRequest);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Personel oluşturulamadı.\n\n{error}");
                    return;
                }

                await LoadPersonnelDataAsync();

                var created = _allPersonnel
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefault(x => x.FullName.Contains(FirstNameBox.Text.Trim(), StringComparison.OrdinalIgnoreCase));

                personelId = created?.Id ?? 0;
            }
            else
            {
                personelId = _selectedPersonelId.Value;

                var updateRequest = BuildUpdateRequest();

                var response = await _httpClient.PutAsJsonAsync($"/api/personel/{personelId}", updateRequest);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Personel güncellenemedi.\n\n{error}");
                    return;
                }
            }

            if (personelId > 0)
            {
                var healthSaved = await SaveHealthInfoAsync(personelId);

                if (!healthSaved)
                    return;
            }

            await LoadPersonnelDataAsync();
            RefreshGrid();

            MessageBox.Show("Personel ve sağlık bilgileri başarıyla kaydedildi.");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Kaydetme sırasında hata oluştu: {ex.Message}");
        }
    }

    private async void BtnDelete_Click(object sender, RoutedEventArgs e)
    {
        if (((Button)sender).DataContext is not PersonModel selectedPerson)
            return;

        var confirm = MessageBox.Show(
            $"{selectedPerson.FullName} adlı personeli silmek istiyor musunuz?",
            "Personel Sil",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (confirm != MessageBoxResult.Yes)
            return;

        try
        {
            var response = await _httpClient.DeleteAsync($"/api/personel/{selectedPerson.Id}");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                MessageBox.Show($"Personel silinemedi.\n\n{error}");
                return;
            }

            await LoadPersonnelDataAsync();
            RefreshGrid();

            MessageBox.Show("Personel başarıyla silindi.");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Personel silinirken hata oluştu: {ex.Message}");
        }
    }

    private void BindPersonnelForm(PersonelDetailDto person)
    {
        FirstNameBox.Text = person.PersonelAdi;
        LastNameBox.Text = person.PersonelSoyadi;
        TcknBox.Text = person.Tckn;

        BirthDatePicker.SelectedDate = person.DogumTarihi == default ? null : person.DogumTarihi;
        HireDatePicker.SelectedDate = person.IseGirisTarihi == default ? null : person.IseGirisTarihi;

        SelectComboBoxItem(GenderComboBox, person.Cinsiyet);
        SelectComboBoxItem(StatusComboBox, person.PersonelDurumu);
        SelectComboBoxItem(ExpertiseComboBox, person.Uzmanlik);
        SelectComboBoxItem(EmployeeTypeComboBox, person.CalisanTipi);
        SelectComboBoxItem(UserRoleComboBox, person.KullaniciRolu);

        PhoneBox.Text = person.TelNo;
        SecondPhoneBox.Text = person.IkinciTelNo;
        EmailBox.Text = person.Eposta;
        AddressBox.Text = person.Adres;

        RfidBox.Text = person.RfidKartNumarasi;
        DepartmentBox.Text = person.Departman;
        WorkLocationBox.Text = person.CalismaKonumu;
    }

    private async Task LoadHealthFormAsync(int personelId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/saglik-bilgileri/{personelId}");

            if (!response.IsSuccessStatusCode)
            {
                _healthRecordExists = false;
                ClearHealthForm();
                return;
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<HealthDetailDto>>();

            if (apiResponse?.Success != true || apiResponse.Data is null)
            {
                _healthRecordExists = false;
                ClearHealthForm();
                return;
            }

            _healthRecordExists = true;
            BindHealthForm(apiResponse.Data);
        }
        catch
        {
            _healthRecordExists = false;
            ClearHealthForm();
        }
    }

    private void BindHealthForm(HealthDetailDto health)
    {
        SelectComboBoxItem(HealthBloodTypeComboBox, health.KanGrubu);
        SelectComboBoxItem(HealthStatusComboBox, health.SaglikDurumu);

        ChronicDiseasesBox.Text = string.Join(", ", health.KronikHastaliklar);
        AllergiesBox.Text = string.Join(", ", health.Alerjiler);
        WorkRestrictionsBox.Text = health.SaglikCalismaKisitlamalari;
        EmergencyNoteBox.Text = health.AcilDurumNotu;
        LastExamDatePicker.SelectedDate = health.SonMuayeneTarihi == default ? DateTime.Today : health.SonMuayeneTarihi;
    }

    private async Task<bool> SaveHealthInfoAsync(int personelId)
    {
        var hasAnyHealthData =
            !string.IsNullOrWhiteSpace(GetComboBoxValue(HealthBloodTypeComboBox)) ||
            !string.IsNullOrWhiteSpace(GetComboBoxValue(HealthStatusComboBox)) ||
            !string.IsNullOrWhiteSpace(ChronicDiseasesBox.Text) ||
            !string.IsNullOrWhiteSpace(AllergiesBox.Text) ||
            !string.IsNullOrWhiteSpace(WorkRestrictionsBox.Text) ||
            !string.IsNullOrWhiteSpace(EmergencyNoteBox.Text);

        if (!hasAnyHealthData)
            return true;

        var saveRequest = new HealthSaveRequest
        {
            PersonelId = personelId,
            KanGrubu = GetComboBoxValue(HealthBloodTypeComboBox, "A+"),
            SaglikDurumu = GetComboBoxValue(HealthStatusComboBox, "Çalışabilir"),
            KronikHastaliklar = ParseCsvList(ChronicDiseasesBox.Text),
            Alerjiler = ParseCsvList(AllergiesBox.Text),
            SaglikCalismaKisitlamalari = WorkRestrictionsBox.Text.Trim(),
            AcilDurumNotu = EmergencyNoteBox.Text.Trim(),
            SonMuayeneTarihi = LastExamDatePicker.SelectedDate ?? DateTime.Today
        };

        try
        {
            HttpResponseMessage response;

            if (_healthRecordExists)
            {
                var updateRequest = new HealthUpdateRequest
                {
                    KanGrubu = saveRequest.KanGrubu,
                    SaglikDurumu = saveRequest.SaglikDurumu,
                    KronikHastaliklar = saveRequest.KronikHastaliklar,
                    Alerjiler = saveRequest.Alerjiler,
                    SaglikCalismaKisitlamalari = saveRequest.SaglikCalismaKisitlamalari,
                    AcilDurumNotu = saveRequest.AcilDurumNotu,
                    SonMuayeneTarihi = saveRequest.SonMuayeneTarihi
                };

                response = await _httpClient.PutAsJsonAsync($"/api/saglik-bilgileri/{personelId}", updateRequest);
            }
            else
            {
                response = await _httpClient.PostAsJsonAsync("/api/saglik-bilgileri", saveRequest);
            }

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                MessageBox.Show($"Sağlık bilgileri kaydedilemedi.\n\n{error}");
                return false;
            }

            _healthRecordExists = true;
            return true;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Sağlık bilgileri kaydedilirken hata oluştu: {ex.Message}");
            return false;
        }
    }

    private PersonelCreateRequest BuildCreateRequest()
    {
        return new PersonelCreateRequest
        {
            Tckn = TcknBox.Text.Trim(),
            PersonelAdi = FirstNameBox.Text.Trim(),
            PersonelSoyadi = LastNameBox.Text.Trim(),
            Uzmanlik = GetComboBoxValue(ExpertiseComboBox),
            PersonelDurumu = GetComboBoxValue(StatusComboBox, "Aktif"),
            DogumTarihi = BirthDatePicker.SelectedDate ?? DateTime.Today,
            Cinsiyet = GetComboBoxValue(GenderComboBox),
            TelNo = PhoneBox.Text.Trim(),
            IkinciTelNo = SecondPhoneBox.Text.Trim(),
            Eposta = EmailBox.Text.Trim(),
            Adres = AddressBox.Text.Trim(),
            IseGirisTarihi = HireDatePicker.SelectedDate ?? DateTime.Today,
            CalisanTipi = GetComboBoxValue(EmployeeTypeComboBox),
            RfidKartNumarasi = RfidBox.Text.Trim(),
            KullaniciRolu = GetComboBoxValue(UserRoleComboBox, "Saha Personeli"),
            Departman = DepartmentBox.Text.Trim(),
            CalismaKonumu = WorkLocationBox.Text.Trim(),
            SifreHash = string.Empty
        };
    }

    private PersonelUpdateRequest BuildUpdateRequest()
    {
        return new PersonelUpdateRequest
        {
            Tckn = TcknBox.Text.Trim(),
            PersonelAdi = FirstNameBox.Text.Trim(),
            PersonelSoyadi = LastNameBox.Text.Trim(),
            Uzmanlik = GetComboBoxValue(ExpertiseComboBox),
            PersonelDurumu = GetComboBoxValue(StatusComboBox, "Aktif"),
            DogumTarihi = BirthDatePicker.SelectedDate ?? DateTime.Today,
            Cinsiyet = GetComboBoxValue(GenderComboBox),
            TelNo = PhoneBox.Text.Trim(),
            IkinciTelNo = SecondPhoneBox.Text.Trim(),
            Eposta = EmailBox.Text.Trim(),
            Adres = AddressBox.Text.Trim(),
            IseGirisTarihi = HireDatePicker.SelectedDate ?? DateTime.Today,
            CalisanTipi = GetComboBoxValue(EmployeeTypeComboBox),
            RfidKartNumarasi = RfidBox.Text.Trim(),
            KullaniciRolu = GetComboBoxValue(UserRoleComboBox, "Saha Personeli"),
            Departman = DepartmentBox.Text.Trim(),
            CalismaKonumu = WorkLocationBox.Text.Trim()
        };
    }

    private void ClearForm()
    {
        FirstNameBox.Text = string.Empty;
        LastNameBox.Text = string.Empty;
        TcknBox.Text = string.Empty;
        BirthDatePicker.SelectedDate = DateTime.Today;
        HireDatePicker.SelectedDate = DateTime.Today;

        SelectComboBoxItem(GenderComboBox, "Erkek");
        SelectComboBoxItem(StatusComboBox, "Aktif");
        SelectComboBoxItem(ExpertiseComboBox, "Maden Mühendisi");
        SelectComboBoxItem(EmployeeTypeComboBox, "Tam Zamanlı");
        SelectComboBoxItem(UserRoleComboBox, _isShowingAdmin ? "Admin" : "Saha Personeli");

        PhoneBox.Text = string.Empty;
        SecondPhoneBox.Text = string.Empty;
        EmailBox.Text = string.Empty;
        AddressBox.Text = string.Empty;
        RfidBox.Text = string.Empty;
        DepartmentBox.Text = _isShowingAdmin ? "Yönetim" : "Saha";
        WorkLocationBox.Text = "Merkez Sahası";

        ClearHealthForm();
    }

    private void ClearHealthForm()
    {
        SelectComboBoxItem(HealthBloodTypeComboBox, "A+");
        SelectComboBoxItem(HealthStatusComboBox, "Çalışabilir");
        ChronicDiseasesBox.Text = string.Empty;
        AllergiesBox.Text = string.Empty;
        WorkRestrictionsBox.Text = string.Empty;
        EmergencyNoteBox.Text = string.Empty;
        LastExamDatePicker.SelectedDate = DateTime.Today;
    }

    private static List<string> ParseCsvList(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return new List<string>();

        return value
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToList();
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

    private static bool IsAdminRole(string role)
    {
        return role.Equals("Admin", StringComparison.OrdinalIgnoreCase) ||
               role.Equals("Yönetici", StringComparison.OrdinalIgnoreCase) ||
               role.Equals("Yonetici", StringComparison.OrdinalIgnoreCase) ||
               role.Equals("İdari Personel", StringComparison.OrdinalIgnoreCase);
    }
}

public class PersonModel
{
    public int Id { get; set; }
    public string Rfid { get; set; } = string.Empty;
    public string Identity { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string BloodType { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Shift { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public bool IsAdmin { get; set; }
}

public class PersonelListItemDto
{
    public int PersonelId { get; set; }
    public string AdSoyad { get; set; } = string.Empty;
    public string Uzmanlik { get; set; } = string.Empty;
    public string Departman { get; set; } = string.Empty;
    public string KullaniciRolu { get; set; } = string.Empty;
    public string CalismaKonumu { get; set; } = string.Empty;
    public string PersonelDurumu { get; set; } = string.Empty;

    public string Tckn { get; set; } = string.Empty;
    public string TelNo { get; set; } = string.Empty;
    public string KanGrubu { get; set; } = string.Empty;
    public string RfidKartNumarasi { get; set; } = string.Empty;
}

public class PersonelDetailDto
{
    public int PersonelId { get; set; }
    public string Tckn { get; set; } = string.Empty;
    public string PersonelAdi { get; set; } = string.Empty;
    public string PersonelSoyadi { get; set; } = string.Empty;
    public string Uzmanlik { get; set; } = string.Empty;
    public string PersonelDurumu { get; set; } = string.Empty;
    public DateTime DogumTarihi { get; set; }
    public string Cinsiyet { get; set; } = string.Empty;
    public string TelNo { get; set; } = string.Empty;
    public string IkinciTelNo { get; set; } = string.Empty;
    public string Eposta { get; set; } = string.Empty;
    public string Adres { get; set; } = string.Empty;
    public DateTime IseGirisTarihi { get; set; }
    public string CalisanTipi { get; set; } = string.Empty;
    public string RfidKartNumarasi { get; set; } = string.Empty;
    public string KullaniciRolu { get; set; } = string.Empty;
    public string Departman { get; set; } = string.Empty;
    public string CalismaKonumu { get; set; } = string.Empty;
}

public class PersonelCreateRequest
{
    public string Tckn { get; set; } = string.Empty;
    public string PersonelAdi { get; set; } = string.Empty;
    public string PersonelSoyadi { get; set; } = string.Empty;
    public string Uzmanlik { get; set; } = string.Empty;
    public string PersonelDurumu { get; set; } = "Aktif";
    public DateTime DogumTarihi { get; set; }
    public string Cinsiyet { get; set; } = string.Empty;
    public string TelNo { get; set; } = string.Empty;
    public string IkinciTelNo { get; set; } = string.Empty;
    public string Eposta { get; set; } = string.Empty;
    public string Adres { get; set; } = string.Empty;
    public DateTime IseGirisTarihi { get; set; }
    public string CalisanTipi { get; set; } = string.Empty;
    public string RfidKartNumarasi { get; set; } = string.Empty;
    public string KullaniciRolu { get; set; } = string.Empty;
    public string Departman { get; set; } = string.Empty;
    public string CalismaKonumu { get; set; } = string.Empty;
    public string SifreHash { get; set; } = string.Empty;
}

public class PersonelUpdateRequest
{
    public string Tckn { get; set; } = string.Empty;
    public string PersonelAdi { get; set; } = string.Empty;
    public string PersonelSoyadi { get; set; } = string.Empty;
    public string Uzmanlik { get; set; } = string.Empty;
    public string PersonelDurumu { get; set; } = string.Empty;
    public DateTime DogumTarihi { get; set; }
    public string Cinsiyet { get; set; } = string.Empty;
    public string TelNo { get; set; } = string.Empty;
    public string IkinciTelNo { get; set; } = string.Empty;
    public string Eposta { get; set; } = string.Empty;
    public string Adres { get; set; } = string.Empty;
    public DateTime IseGirisTarihi { get; set; }
    public string CalisanTipi { get; set; } = string.Empty;
    public string RfidKartNumarasi { get; set; } = string.Empty;
    public string KullaniciRolu { get; set; } = string.Empty;
    public string Departman { get; set; } = string.Empty;
    public string CalismaKonumu { get; set; } = string.Empty;
}

