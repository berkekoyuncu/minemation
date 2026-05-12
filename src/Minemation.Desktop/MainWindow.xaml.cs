using System.Windows;
using MaterialDesignThemes.Wpf;
namespace Minemation.Desktop;

public partial class MainWindow : Window
{
    public MainWindow(string role = "Admin")
    {
        InitializeComponent();
        
        SetupRole(role);
    }

    private void NavPersonnel_Click(object sender, RoutedEventArgs e)
    {
        MainContent.Content = new PersonnelView();
        UpdateNavStyles(NavPersonnel);
    }

    private void NavShift_Click(object sender, RoutedEventArgs e)
    {
        MainContent.Content = new ShiftView();
        UpdateNavStyles(NavShift);
    }

    private void NavHome_Click(object sender, RoutedEventArgs e)
    {
        MainContent.Content = new ModulePlaceholderView(
            "Genel Bakış",
            "Dashboard ekranında personel, vardiya, vaka, sensör ve ekipman özetleri gösterilecek.",
            PackIconKind.Home);
    
        UpdateNavStyles(NavHome);
    }

    private void NavEquipment_Click(object sender, RoutedEventArgs e)
    {
        MainContent.Content = new EquipmentView();
        UpdateNavStyles(NavEquipment);
    }

    private void NavIncidents_Click(object sender, RoutedEventArgs e)
    {
        MainContent.Content = new IncidentView();
        UpdateNavStyles(NavIncidents);
    }

    private void NavReports_Click(object sender, RoutedEventArgs e)
    {
        MainContent.Content = new ReportView();
        UpdateNavStyles(NavReports);
    }

    private void NavRisk_Click(object sender, RoutedEventArgs e)
    {
        MainContent.Content = new RiskMonitoringView();
        UpdateNavStyles(NavRisk);
    }

    private void NavMap_Click(object sender, RoutedEventArgs e)
    {
        MainContent.Content = new ModulePlaceholderView(
            "Harita",
            "Personel, ekipman ve takip cihazı konumları bu ekranda görüntülenecek.",
            PackIconKind.MapMarkerRadius);
    
        UpdateNavStyles(NavMap);
    }
    
    private void UpdateNavStyles(System.Windows.Controls.Button activeBtn)
    {
        var btns = new[] { NavHome, NavPersonnel, NavShift, NavEquipment, NavIncidents, NavReports, NavRisk, NavMap };
        foreach(var btn in btns)
        {
            if(btn == null) continue;
            
            if(btn == activeBtn)
            {
                btn.Foreground = (System.Windows.Media.Brush)FindResource("PrimaryBrush");
                btn.BorderThickness = new Thickness(0,0,0,3);
                btn.BorderBrush = (System.Windows.Media.Brush)FindResource("PrimaryBrush");
            }
            else
            {
                btn.Foreground = (System.Windows.Media.Brush)FindResource("TextSecondaryBrush");
                btn.BorderThickness = new Thickness(0);
            }
        }
    }

    private void SetupRole(string role)
    {
        if (role == "Admin") // Yönetici
        {
            NavPersonnel.Visibility = Visibility.Visible;
            NavEquipment.Visibility = Visibility.Visible;
            NavReports.Visibility = Visibility.Visible;
            NavRisk.Visibility = Visibility.Visible;
            
            NavShift.Content = "Vardiya";
            NavIncidents.Content = "Vakalar";
            UserNameText.Text = "Ahmet Yılmaz";
            
            // Default view for Admin
            MainContent.Content = new PersonnelView();
            UpdateNavStyles(NavPersonnel);
        }
        else // Saha Personeli
        {
            NavPersonnel.Visibility = Visibility.Collapsed;
            NavEquipment.Visibility = Visibility.Collapsed;
            NavReports.Visibility = Visibility.Collapsed;
            NavRisk.Visibility = Visibility.Collapsed;
            
            NavShift.Content = "Vardiyalarım";
            NavIncidents.Content = "Vakalarım";
            UserNameText.Text = "Mehmet Kaya";
            
            // Saha Personeli Personel ekranını göremez, o yüzden Vardiyalarım ekranına at
            MainContent.Content = new ShiftView();
            UpdateNavStyles(NavShift);
        }
    }

    private bool _isDarkTheme = true;

    private void ThemeToggleButton_Click(object sender, RoutedEventArgs e)
    {
        _isDarkTheme = !_isDarkTheme;
        
        // 1. BaseTheme'i Güncelle
        var paletteHelper = new MaterialDesignThemes.Wpf.PaletteHelper();
        var theme = paletteHelper.GetTheme();
        theme.SetBaseTheme(_isDarkTheme ? MaterialDesignThemes.Wpf.BaseTheme.Dark : MaterialDesignThemes.Wpf.BaseTheme.Light);
        paletteHelper.SetTheme(theme);

        // 2. Custom Colors sözlüğünü değiştir
        var appResources = System.Windows.Application.Current.Resources;
        var oldDict = System.Linq.Enumerable.FirstOrDefault(appResources.MergedDictionaries, d => d.Source != null && d.Source.OriginalString.Contains("Colors"));
        if (oldDict != null)
        {
            appResources.MergedDictionaries.Remove(oldDict);
        }
        
        string newDictName = _isDarkTheme ? "ColorsDark.xaml" : "ColorsLight.xaml";
        appResources.MergedDictionaries.Add(new System.Windows.ResourceDictionary 
        { 
            Source = new System.Uri($"pack://application:,,,/Resources/{newDictName}") 
        });

        // 3. İkonu Güncelle
        var themeIcon = this.FindName("ThemeIcon") as MaterialDesignThemes.Wpf.PackIcon;
        if (themeIcon != null)
        {
            themeIcon.Kind = _isDarkTheme ? MaterialDesignThemes.Wpf.PackIconKind.WeatherSunny : MaterialDesignThemes.Wpf.PackIconKind.WeatherNight;
        }
    }
}
