using System.Windows;

namespace Minemation.Desktop;

public partial class LoginView : Window
{
    public LoginView()
    {
        InitializeComponent();
    }

    private void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        // Temel doğrulama
        if (string.IsNullOrWhiteSpace(IdentityBox.Text) || string.IsNullOrWhiteSpace(PasswordBox.Password))
        {
            ErrorText.Text = "Lütfen T.C. Kimlik numaranızı ve şifrenizi giriniz.";
            ErrorText.Visibility = Visibility.Visible;
            return;
        }

        string role = "Admin"; // Varsayılan Yönetici
        if (IdentityBox.Text.StartsWith("2"))
        {
            role = "Field"; // Saha Personeli
        }
        else if (IdentityBox.Text.StartsWith("1"))
        {
            role = "Admin";
        }

        // TODO: Backend entegrasyonu yapılacak.
        MainWindow mainWindow = new MainWindow(role);
        mainWindow.Show();
        this.Close();
    }
}