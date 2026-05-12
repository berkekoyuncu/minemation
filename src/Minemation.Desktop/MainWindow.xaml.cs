using System.Windows;
using MaterialDesignThemes.Wpf;
namespace Minemation.Desktop;

public partial class MainWindow : Window
{
    private readonly string _currentRole;
    private readonly string _currentUserName;
    private readonly int _currentPersonelId;

    private bool CanManage => _currentRole == "Admin";

    public MainWindow(string role = "Admin", string userName = "", int personelId = 0)
    {
        InitializeComponent();

        _currentRole = NormalizeRole(role);
        _currentUserName = string.IsNullOrWhiteSpace(userName) ? "Kullanıcı" : userName;
        _currentPersonelId = personelId;

        SetupRole();
    }

    private void NavPersonnel_Click(object sender, RoutedEventArgs e)
    {
        MainContent.Content = new PersonnelView();
        UpdateNavStyles(NavPersonnel);
    }

    private void NavShift_Click(object sender, RoutedEventArgs e)
    {
        MainContent.Content = new ShiftView(CanManage);
        UpdateNavStyles(NavShift);
    }

    private void NavHome_Click(object sender, RoutedEventArgs e)
    {
        if (CanManage)
        {
            MainContent.Content = new AdminHomeView();
        }
        else
        {
            MainContent.Content = new FieldHomeView(_currentPersonelId);
        }

        UpdateNavStyles(NavHome);
    }

    private void NavEquipment_Click(object sender, RoutedEventArgs e)
    {
        MainContent.Content = new EquipmentView();
        UpdateNavStyles(NavEquipment);
    }

    private void NavIncidents_Click(object sender, RoutedEventArgs e)
    {
        MainContent.Content = new IncidentView(CanManage, _currentPersonelId);
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

    private void NavHealth_Click(object sender, RoutedEventArgs e)
    {
        MainContent.Content = new HealthView(_currentPersonelId, CanManage);
        UpdateNavStyles(NavHealth);
    }

    private void UpdateNavStyles(System.Windows.Controls.Button activeBtn)
    {
        var btns = new[] { NavHome, NavPersonnel, NavShift, NavEquipment, NavIncidents, NavReports, NavRisk, NavMap, NavHealth };
        foreach (var btn in btns)
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

    private void SetupRole()
    {
        UserNameText.Text = _currentUserName;

        if (_currentRole == "Admin")
        {
            ApplyAdminMenu();
            MainContent.Content = new AdminHomeView();
            UpdateNavStyles(NavHome);
        }
        else
        {
            ApplyFieldMenu();
            MainContent.Content = new FieldHomeView(_currentPersonelId);
            UpdateNavStyles(NavHome);
        }
    }

    private void ApplyAdminMenu()
    {
        NavHome.Visibility = Visibility.Visible;
        NavPersonnel.Visibility = Visibility.Visible;
        NavShift.Visibility = Visibility.Visible;
        NavEquipment.Visibility = Visibility.Visible;
        NavIncidents.Visibility = Visibility.Visible;
        NavReports.Visibility = Visibility.Visible;
        NavRisk.Visibility = Visibility.Visible;
        NavMap.Visibility = Visibility.Visible;

        NavHealth.Visibility = Visibility.Collapsed;

        NavShift.Content = "Vardiya";
        NavIncidents.Content = "Vakalar";
    }

    private void ApplyFieldMenu()
    {
        NavHome.Visibility = Visibility.Visible;
        NavPersonnel.Visibility = Visibility.Collapsed;
        NavEquipment.Visibility = Visibility.Collapsed;
        NavReports.Visibility = Visibility.Collapsed;
        NavRisk.Visibility = Visibility.Collapsed;

        NavShift.Visibility = Visibility.Visible;
        NavIncidents.Visibility = Visibility.Visible;
        NavMap.Visibility = Visibility.Visible;
        NavHealth.Visibility = Visibility.Visible;

        NavHealth.Content = "Sağlık";
        NavShift.Content = "Vardiyalarım";
        NavIncidents.Content = "Vakalarım";
    }

    private static string NormalizeRole(string role)
    {
        if (string.IsNullOrWhiteSpace(role))
            return "Field";

        return role.Trim().ToLowerInvariant() switch
        {
            "admin" => "Admin",
            "yonetici" => "Admin",
            "yönetici" => "Admin",
            "idari personel" => "Admin",
            "field" => "Field",
            "saha" => "Field",
            "saha personeli" => "Field",
            _ => role
        };
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
