using System.Net.Http;
using System.Net.Http.Json;
using Minemation.Desktop.Models;

namespace Minemation.Desktop.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;

    public GirisSonucDto? AktifKullanici { get; private set; }

    public AuthService()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5289")
        };
    }

    public async Task<(bool Success, string Message, GirisSonucDto? Kullanici)> GirisYapAsync(
        string kullaniciAdi,
        string sifre)
    {
        if (string.IsNullOrWhiteSpace(kullaniciAdi) || string.IsNullOrWhiteSpace(sifre))
            return (false, "Kullanıcı adı ve şifre zorunludur.", null);

        var request = new GirisYapRequest
        {
            KullaniciAdi = kullaniciAdi,
            Sifre = sifre
        };

        try
        {
            var response = await _httpClient.PostAsJsonAsync(
                "/api/kimlik-dogrulama/giris",
                request);

            var sonuc = await response.Content.ReadFromJsonAsync<ApiResponse<GirisSonucDto>>();

            if (!response.IsSuccessStatusCode || sonuc is null || !sonuc.Success)
                return (false, sonuc?.Message ?? "Giriş başarısız.", null);

            AktifKullanici = sonuc.Data;

            return (true, sonuc.Message, sonuc.Data);
        }
        catch (HttpRequestException)
        {
            return (false, "API servisine ulaşılamadı. Minemation.Api çalışıyor mu?", null);
        }
        catch (Exception ex)
        {
            return (false, $"Beklenmeyen hata: {ex.Message}", null);
        }
    }
}
