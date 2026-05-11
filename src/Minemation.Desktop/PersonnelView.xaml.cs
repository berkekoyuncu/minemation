using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Minemation.Desktop.Models;

namespace Minemation.Desktop;

public partial class PersonnelView : UserControl
{
    private readonly HttpClient _httpClient = new()
    {
        BaseAddress = new Uri("http://localhost:5289")
    };

    private List<PersonModel> _allPersonnel = new();
    private bool _isShowingAdmin = true;

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
                "/api/personel");

            if (response?.Success != true || response.Data is null)
            {
                MessageBox.Show(response?.Message ?? "Personel listesi alınamadı.");
                return;
            }

            _allPersonnel = response.Data.Items.Select(x => new PersonModel
            {
                Id = x.PersonelId,
                Rfid = string.Empty,
                Identity = string.Empty,
                FullName = x.AdSoyad,
                Phone = string.Empty,
                BloodType = string.Empty,
                Role = x.Uzmanlik,
                Shift = string.Empty,
                Status = x.PersonelDurumu,
                IsAdmin = x.KullaniciRolu.Equals("Admin", StringComparison.OrdinalIgnoreCase)
            }).ToList();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Personel verileri alınırken hata oluştu: {ex.Message}");
        }
    }

    private void RefreshGrid()
    {
        if (PersonnelGrid == null) return;

        var searchText = SearchBox.Text?.ToLower() ?? "";

        var filteredList = _allPersonnel
            .Where(p => p.IsAdmin == _isShowingAdmin)
            .Where(p =>
                string.IsNullOrEmpty(searchText) ||
                p.FullName.ToLower().Contains(searchText) ||
                p.Identity.Contains(searchText) ||
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
        var title = FindName("FormTitle") as TextBlock;
        if (title != null)
            title.Text = "Yeni Personel Kaydı";
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

            var title = FindName("FormTitle") as TextBlock;
            if (title != null)
                title.Text = $"Personel Düzenle: {response.Data.PersonelAdi} {response.Data.PersonelSoyadi}";

            MessageBox.Show(
                $"Personel Detayı\n\n" +
                $"Ad Soyad: {response.Data.PersonelAdi} {response.Data.PersonelSoyadi}\n" +
                $"TCKN: {response.Data.Tckn}\n" +
                $"Telefon: {response.Data.TelNo}\n" +
                $"E-posta: {response.Data.Eposta}\n" +
                $"Rol: {response.Data.KullaniciRolu}\n" +
                $"Durum: {response.Data.PersonelDurumu}");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Personel detayı alınırken hata oluştu: {ex.Message}");
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
                MessageBox.Show("Personel silinemedi.");
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
}

public class PersonelDetailDto
{
    public int PersonelId { get; set; }
    public string Tckn { get; set; } = string.Empty;
    public string PersonelAdi { get; set; } = string.Empty;
    public string PersonelSoyadi { get; set; } = string.Empty;
    public string Uzmanlik { get; set; } = string.Empty;
    public string PersonelDurumu { get; set; } = string.Empty;
    public string Cinsiyet { get; set; } = string.Empty;
    public string TelNo { get; set; } = string.Empty;
    public string IkinciTelNo { get; set; } = string.Empty;
    public string Eposta { get; set; } = string.Empty;
    public string Adres { get; set; } = string.Empty;
    public string CalisanTipi { get; set; } = string.Empty;
    public string RfidKartNumarasi { get; set; } = string.Empty;
    public string KullaniciRolu { get; set; } = string.Empty;
    public string Departman { get; set; } = string.Empty;
    public string CalismaKonumu { get; set; } = string.Empty;
}
