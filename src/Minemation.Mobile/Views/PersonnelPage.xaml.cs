using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;

namespace Minemation.Mobile.Views;

public partial class PersonnelPage : ContentPage
{
    public PersonnelPage()
    {
        InitializeComponent();
        
        var personnelList = new ObservableCollection<PersonnelMock>
        {
            new PersonnelMock { FullName = "Ahmet Yılmaz", Role = "Maden Mühendisi", Status = "Aktif" },
            new PersonnelMock { FullName = "Ayşe Demir", Role = "İş Güvenliği", Status = "Aktif" },
            new PersonnelMock { FullName = "Mehmet Kaya", Role = "Operatör", Status = "İstirahat" },
            new PersonnelMock { FullName = "Fatma Çelik", Role = "Teknisyen", Status = "Aktif" }
        };
        
        PersonnelList.ItemsSource = personnelList;
    }
}

public class PersonnelMock
{
    public string FullName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}
