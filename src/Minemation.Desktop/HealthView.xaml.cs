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
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using Minemation.Desktop.Models;

namespace Minemation.Desktop;

public partial class HealthView : UserControl
{
    private readonly HttpClient _httpClient = new()
    {
        BaseAddress = new Uri("http://localhost:5289")
    };

    private readonly int _personelId;
    private readonly bool _canManage;

    public HealthView(int personelId, bool canManage = false)
    {
        _personelId = personelId;
        _canManage = canManage;

        InitializeComponent();

        Loaded += HealthView_Loaded;
    }

    private async void HealthView_Loaded(object sender, RoutedEventArgs e)
    {
        await LoadHealthDataAsync();
    }

    private async void BtnRefresh_Click(object sender, RoutedEventArgs e)
    {
        await LoadHealthDataAsync();
    }

    private async Task LoadHealthDataAsync()
    {
        if (_personelId <= 0)
        {
            MessageBox.Show("Personel bilgisi bulunamadı. Sağlık bilgileri yüklenemedi.");
            return;
        }

        try
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<HealthDetailDto>>(
                $"/api/saglik-bilgileri/{_personelId}");

            if (response?.Success != true || response.Data is null)
            {
                ClearScreen();
                MessageBox.Show("Bu kullanıcı için sağlık bilgisi bulunamadı. Yönetici, Personel Düzenle ekranından sağlık bilgisi eklemelidir.");
                return;
            }

            BindHealthData(response.Data);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Sağlık bilgileri alınırken hata oluştu: {ex.Message}");
        }
    }

    private void BindHealthData(HealthDetailDto health)
    {
        PersonNameText.Text = health.PersonelAdSoyad ?? "-";
        BloodTypeText.Text = string.IsNullOrWhiteSpace(health.KanGrubu) ? "-" : health.KanGrubu;
        HealthStatusText.Text = string.IsNullOrWhiteSpace(health.SaglikDurumu) ? "-" : health.SaglikDurumu;
        LastExamText.Text = health.SonMuayeneTarihi == default
            ? "-"
            : health.SonMuayeneTarihi.ToString("dd.MM.yyyy");

        ChronicDiseasesList.ItemsSource = health.KronikHastaliklar.Count > 0
            ? health.KronikHastaliklar
            : new List<string> { "Kayıtlı kronik hastalık bulunmuyor." };

        AllergiesList.ItemsSource = health.Alerjiler.Count > 0
            ? health.Alerjiler
            : new List<string> { "Kayıtlı alerji bulunmuyor." };

        WorkRestrictionsText.Text = string.IsNullOrWhiteSpace(health.SaglikCalismaKisitlamalari)
            ? "Çalışma kısıtlaması bulunmuyor."
            : health.SaglikCalismaKisitlamalari;

        EmergencyNoteText.Text = string.IsNullOrWhiteSpace(health.AcilDurumNotu)
            ? "Acil durum notu bulunmuyor."
            : health.AcilDurumNotu;
    }

    private void ClearScreen()
    {
        PersonNameText.Text = "-";
        BloodTypeText.Text = "-";
        HealthStatusText.Text = "-";
        LastExamText.Text = "-";
        ChronicDiseasesList.ItemsSource = new List<string> { "Veri bulunamadı." };
        AllergiesList.ItemsSource = new List<string> { "Veri bulunamadı." };
        WorkRestrictionsText.Text = "-";
        EmergencyNoteText.Text = "-";
    }
}

public class HealthDetailDto
{
    public int PersonelId { get; set; }
    public string? PersonelAdSoyad { get; set; }
    public string? PersonelDurumu { get; set; }

    public string KanGrubu { get; set; } = string.Empty;
    public string SaglikDurumu { get; set; } = string.Empty;

    public List<string> KronikHastaliklar { get; set; } = new();
    public List<string> Alerjiler { get; set; } = new();

    public string SaglikCalismaKisitlamalari { get; set; } = string.Empty;
    public string AcilDurumNotu { get; set; } = string.Empty;
    public DateTime SonMuayeneTarihi { get; set; }
}

public class HealthSaveRequest
{
    public int PersonelId { get; set; }
    public string KanGrubu { get; set; } = string.Empty;
    public string SaglikDurumu { get; set; } = string.Empty;
    public List<string> KronikHastaliklar { get; set; } = new();
    public List<string> Alerjiler { get; set; } = new();
    public string SaglikCalismaKisitlamalari { get; set; } = string.Empty;
    public string AcilDurumNotu { get; set; } = string.Empty;
    public DateTime SonMuayeneTarihi { get; set; }
}

public class HealthUpdateRequest
{
    public string KanGrubu { get; set; } = string.Empty;
    public string SaglikDurumu { get; set; } = string.Empty;
    public List<string> KronikHastaliklar { get; set; } = new();
    public List<string> Alerjiler { get; set; } = new();
    public string SaglikCalismaKisitlamalari { get; set; } = string.Empty;
    public string AcilDurumNotu { get; set; } = string.Empty;
    public DateTime SonMuayeneTarihi { get; set; }
}