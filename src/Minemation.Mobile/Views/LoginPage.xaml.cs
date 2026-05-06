using Microsoft.Maui.Controls;

namespace Minemation.Mobile.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    private void OnLoginClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(IdentityEntry.Text) || string.IsNullOrWhiteSpace(PasswordEntry.Text))
        {
            ErrorLabel.Text = "Lütfen T.C. Kimlik numaranızı ve şifrenizi giriniz.";
            ErrorLabel.IsVisible = true;
            return;
        }

        // Navigate to AppShell
        Microsoft.Maui.Controls.Application.Current.MainPage = new AppShell();
    }
}
