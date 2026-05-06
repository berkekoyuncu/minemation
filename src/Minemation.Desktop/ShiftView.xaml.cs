using System.Collections.Generic;
using System.Windows.Controls;

namespace Minemation.Desktop;

public partial class ShiftView : UserControl
{
    public ShiftView()
    {
        InitializeComponent();
        LoadShiftData();
    }

    private void LoadShiftData()
    {
        var shifts = new List<ShiftModel>
        {
            new ShiftModel { Time = "08:00 - 16:00", PersonName = "Ahmet Yılmaz", Location = "A Galerisi", Task = "Gözetim" },
            new ShiftModel { Time = "08:00 - 16:00", PersonName = "Mehmet Demir", Location = "Kırma Tesisi", Task = "Bakım" },
            new ShiftModel { Time = "16:00 - 00:00", PersonName = "Caner Şahin", Location = "B Galerisi", Task = "Operatör" },
            new ShiftModel { Time = "16:00 - 00:00", PersonName = "Mustafa Yıldız", Location = "B Galerisi", Task = "Operatör" },
            new ShiftModel { Time = "00:00 - 08:00", PersonName = "Hasan Mert", Location = "A Galerisi", Task = "Saha Kontrol" }
        };

        ShiftGrid.ItemsSource = shifts;
    }
}

public class ShiftModel
{
    public string Time { get; set; } = string.Empty;
    public string PersonName { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Task { get; set; } = string.Empty;
}
