using System.Windows;
using Minemation.Desktop.Services;

namespace Minemation.Desktop;

public partial class LoginView : Window
{
    private readonly AuthService _authService = new();

    public LoginView()
    {
        InitializeComponent();
    }

    private async void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        ErrorText.Visibility = Visibility.Collapsed;
        ErrorText.Text = string.Empty;

        if (string.IsNullOrWhiteSpace(IdentityBox.Text) || string.IsNullOrWhiteSpace(PasswordBox.Password))
        {
            ErrorText.Text = "Lütfen kullanıcı adınızı ve şifrenizi giriniz.";
            ErrorText.Visibility = Visibility.Visible;
            return;
        }

        LoginButton.IsEnabled = false;
        LoginButton.Content = "Giriş yapılıyor...";

        var sonuc = await _authService.GirisYapAsync(
            IdentityBox.Text.Trim(),
            PasswordBox.Password);

        LoginButton.IsEnabled = true;
        LoginButton.Content = "Giriş Yap";

        if (!sonuc.Success || sonuc.Kullanici is null)
        {
            if (sonuc.Message == "İLK_GIRIS_SIFRE_OLUSTURULMALI")
            {
                var firstLoginWindow = new FirstLoginPasswordWindow(IdentityBox.Text.Trim())
                {
                    Owner = this
                };

                var result = firstLoginWindow.ShowDialog();

                if (result == true)
                {
                    PasswordBox.Password = string.Empty;
                    ErrorText.Text = "Şifreniz oluşturuldu. Yeni şifrenizle giriş yapabilirsiniz.";
                    ErrorText.Visibility = Visibility.Visible;
                }

                return;
            }

            ErrorText.Text = sonuc.Message;
            ErrorText.Visibility = Visibility.Visible;
            return;
        }

        var role = RolEsle(sonuc.Kullanici.KullaniciRolu);

        var displayName = $"{sonuc.Kullanici.PersonelAdi} {sonuc.Kullanici.PersonelSoyadi}".Trim();

        if (string.IsNullOrWhiteSpace(displayName))
            displayName = sonuc.Kullanici.Eposta;

        MainWindow mainWindow = new MainWindow(
            role,
            displayName,
            sonuc.Kullanici.PersonelId);

        mainWindow.Show();
        Close();
    }

    private static string RolEsle(string kullaniciRolu)
    {
        if (string.IsNullOrWhiteSpace(kullaniciRolu))
            return "Field";

        return kullaniciRolu.Trim().ToLowerInvariant() switch
        {
            "admin" => "Admin",
            "yonetici" => "Admin",
            "yönetici" => "Admin",
            "field" => "Field",
            "saha" => "Field",
            "saha personeli" => "Field",
            _ => kullaniciRolu
        };
    }
}
