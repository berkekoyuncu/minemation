using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Http;
using System.Net.Http.Json;
using Minemation.Desktop.Models;

namespace Minemation.Desktop;

public partial class FirstLoginPasswordWindow : Window
{
    private readonly HttpClient _httpClient = new()
    {
        BaseAddress = new Uri("http://localhost:5289")
    };

    public FirstLoginPasswordWindow(string tckn = "")
    {
        InitializeComponent();

        TcknBox.Text = tckn;
    }

    private async void CreatePasswordButton_Click(object sender, RoutedEventArgs e)
    {
        ErrorText.Visibility = Visibility.Collapsed;
        ErrorText.Text = string.Empty;

        if (string.IsNullOrWhiteSpace(TcknBox.Text))
        {
            ShowError("T.C. Kimlik No zorunludur.");
            return;
        }

        if (string.IsNullOrWhiteSpace(NewPasswordBox.Password) ||
            string.IsNullOrWhiteSpace(NewPasswordAgainBox.Password))
        {
            ShowError("Yeni şifre ve tekrar alanı zorunludur.");
            return;
        }

        if (NewPasswordBox.Password != NewPasswordAgainBox.Password)
        {
            ShowError("Şifreler eşleşmiyor.");
            return;
        }

        CreatePasswordButton.IsEnabled = false;
        CreatePasswordButton.Content = "Oluşturuluyor...";

        try
        {
            var request = new FirstLoginPasswordCreateRequest
            {
                Tckn = TcknBox.Text.Trim(),
                YeniSifre = NewPasswordBox.Password
            };

            var response = await _httpClient.PostAsJsonAsync(
                "/api/kimlik-dogrulama/ilk-giris-sifre-olustur",
                request);

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();

            if (!response.IsSuccessStatusCode || apiResponse?.Success != true)
            {
                ShowError(apiResponse?.Message ?? "Şifre oluşturulamadı.");
                return;
            }

            MessageBox.Show("Şifreniz başarıyla oluşturuldu. Yeni şifrenizle giriş yapabilirsiniz.");

            DialogResult = true;
            Close();
        }
        catch (Exception ex)
        {
            ShowError($"Şifre oluşturulurken hata oluştu: {ex.Message}");
        }
        finally
        {
            CreatePasswordButton.IsEnabled = true;
            CreatePasswordButton.Content = "Şifre Oluştur";
        }
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }

    private void ShowError(string message)
    {
        ErrorText.Text = message;
        ErrorText.Visibility = Visibility.Visible;
    }
}

public class FirstLoginPasswordCreateRequest
{
    public string Tckn { get; set; } = string.Empty;
    public string YeniSifre { get; set; } = string.Empty;
}
