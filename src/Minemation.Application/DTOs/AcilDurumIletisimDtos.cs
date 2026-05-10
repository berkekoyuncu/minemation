using System;
using System.Collections.Generic;
using System.Text;

namespace Minemation.Application.DTOs;

public class AcilDurumIletisimListeDto
{
    public int AcilDurumKisisiId { get; set; }
    public string AcilDurumKisileriAd { get; set; } = string.Empty;
    public string AcilDurumKisileriSoyad { get; set; } = string.Empty;
    public string AcilDurumKisileriYakinlik { get; set; } = string.Empty;
    public string AcilDurumKisileriTelNo { get; set; } = string.Empty;

    public int PersonelId { get; set; }
    public string? PersonelAdSoyad { get; set; }
}

public class AcilDurumIletisimDetayDto
{
    public int AcilDurumKisisiId { get; set; }
    public string AcilDurumKisileriAd { get; set; } = string.Empty;
    public string AcilDurumKisileriSoyad { get; set; } = string.Empty;
    public string AcilDurumKisileriYakinlik { get; set; } = string.Empty;
    public string AcilDurumKisileriTelNo { get; set; } = string.Empty;

    public int PersonelId { get; set; }
    public string? PersonelAdSoyad { get; set; }
    public string? PersonelTelNo { get; set; }
    public string? PersonelDurumu { get; set; }
}

public class AcilDurumIletisimOlusturDto
{
    public string AcilDurumKisileriAd { get; set; } = string.Empty;
    public string AcilDurumKisileriSoyad { get; set; } = string.Empty;
    public string AcilDurumKisileriYakinlik { get; set; } = string.Empty;
    public string AcilDurumKisileriTelNo { get; set; } = string.Empty;

    public int PersonelId { get; set; }
}

public class AcilDurumIletisimGuncelleDto
{
    public string AcilDurumKisileriAd { get; set; } = string.Empty;
    public string AcilDurumKisileriSoyad { get; set; } = string.Empty;
    public string AcilDurumKisileriYakinlik { get; set; } = string.Empty;
    public string AcilDurumKisileriTelNo { get; set; } = string.Empty;

    public int PersonelId { get; set; }
}

public class AcilDurumIletisimSorguParametreleri
{
    public string? Arama { get; set; }
    public string? Yakinlik { get; set; }
    public int? PersonelId { get; set; }

    public string? SiralamaAlani { get; set; } = "acilDurumKisisiId";
    public string? SiralamaYonu { get; set; } = "asc";

    private int _sayfaNumarasi = 1;
    public int SayfaNumarasi
    {
        get => _sayfaNumarasi;
        set => _sayfaNumarasi = value <= 0 ? 1 : value;
    }

    private int _sayfaBoyutu = 10;
    public int SayfaBoyutu
    {
        get => _sayfaBoyutu;
        set => _sayfaBoyutu = value <= 0 ? 10 : value > 100 ? 100 : value;
    }
}