using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Minemation.Desktop;

public partial class PersonnelView : UserControl
{
    private List<PersonModel> _allPersonnel = new List<PersonModel>();
    private bool _isShowingAdmin = true;

    public PersonnelView()
    {
        InitializeComponent();
        LoadMockData();
        RefreshGrid();
    }

    private void LoadMockData()
    {
        // İdari Personeller
        _allPersonnel.Add(new PersonModel { Id = 1, Rfid = "RFID-001", Identity = "12345678901", FullName = "Ahmet Yılmaz", Phone = "0532 111 2233", BloodType = "A Rh+", Role = "Maden Mühendisi", Shift = "08:00 - 16:00", Status = "Aktif", IsAdmin = true });
        _allPersonnel.Add(new PersonModel { Id = 2, Rfid = "RFID-002", Identity = "98765432102", FullName = "Ayşe Kaya", Phone = "0544 555 6677", BloodType = "0 Rh-", Role = "İSG Uzmanı", Shift = "08:00 - 16:00", Status = "Aktif", IsAdmin = true });
        _allPersonnel.Add(new PersonModel { Id = 3, Rfid = "RFID-003", Identity = "45612378903", FullName = "Mehmet Demir", Phone = "0505 999 8877", BloodType = "B Rh+", Role = "İdari Amir", Shift = "09:00 - 17:00", Status = "İzinde", IsAdmin = true });

        // Saha Personelleri
        _allPersonnel.Add(new PersonModel { Id = 101, Rfid = "RFID-101", Identity = "11122233344", FullName = "Caner Şahin", Phone = "0555 123 4567", BloodType = "AB Rh+", Role = "Ekskavatör Operatörü", Shift = "16:00 - 00:00", Status = "Aktif", IsAdmin = false });
        _allPersonnel.Add(new PersonModel { Id = 102, Rfid = "RFID-102", Identity = "55566677788", FullName = "Mustafa Yıldız", Phone = "0542 987 6543", BloodType = "A Rh-", Role = "Kırıcı Operatörü", Shift = "16:00 - 00:00", Status = "Aktif", IsAdmin = false });
        _allPersonnel.Add(new PersonModel { Id = 103, Rfid = "RFID-103", Identity = "99900011122", FullName = "Hasan Mert", Phone = "0533 000 1122", BloodType = "0 Rh+", Role = "Saha İşçisi", Shift = "00:00 - 08:00", Status = "Aktif", IsAdmin = false });
    }

    private void RefreshGrid()
    {
        if (PersonnelGrid == null) return;

        var searchText = SearchBox.Text?.ToLower() ?? "";
        var filteredList = _allPersonnel
            .Where(p => p.IsAdmin == _isShowingAdmin)
            .Where(p => string.IsNullOrEmpty(searchText) || 
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
        var title = this.FindName("FormTitle") as TextBlock;
        if (title != null) title.Text = "Yeni Personel Kaydı";
    }

    private void BtnEdit_Click(object sender, RoutedEventArgs e)
    {
        if (((Button)sender).DataContext is PersonModel selectedPerson)
        {
            var title = this.FindName("FormTitle") as TextBlock;
            if (title != null) title.Text = $"Personel Düzenle: {selectedPerson.FullName}";
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
