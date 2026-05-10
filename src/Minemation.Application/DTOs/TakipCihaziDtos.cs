using System;
using System.Collections.Generic;
using System.Text;

namespace Minemation.Application.DTOs;

public class TakipCihaziListeDto
{
    public int TakipCihaziId { get; set; }
    public string TakipCihaziSeriNo { get; set; } = string.Empty;
    public string TakipCihaziTuru { get; set; } = string.Empty;
    public string TakipCihaziModeli { get; set; } = string.Empty;
    public string TakipCihaziDurumu { get; set; } = string.Empty;
    public DateTime TakipCihaziSonBaglantiZamani { get; set; }
    public string TakipCihaziHaberlesmeProtokolu { get; set; } = string.Empty;
    public decimal PilSeviyesi { get; set; }

    public int? PersonelId { get; set; }
    public string? PersonelAdSoyad { get; set; }

    public int? EkipmanId { get; set; }
    public string? EkipmanAdi { get; set; }
}

public class TakipCihaziDetayDto
{
    public int TakipCihaziId { get; set; }
    public string TakipCihaziSeriNo { get; set; } = string.Empty;
    public string TakipCihaziTuru { get; set; } = string.Empty;
    public string TakipCihaziModeli { get; set; } = string.Empty;
    public string TakipCihaziDurumu { get; set; } = string.Empty;
    public DateTime TakipCihaziSonBaglantiZamani { get; set; }
    public string TakipCihaziHaberlesmeProtokolu { get; set; } = string.Empty;
    public decimal PilSeviyesi { get; set; }

    public int? PersonelId { get; set; }
    public string? PersonelAdSoyad { get; set; }

    public int? EkipmanId { get; set; }
    public string? EkipmanAdi { get; set; }
    public string? EkipmanDurumu { get; set; }
}

public class TakipCihaziOlusturDto
{
    public string TakipCihaziSeriNo { get; set; } = string.Empty;
    public string TakipCihaziTuru { get; set; } = string.Empty;
    public string TakipCihaziModeli { get; set; } = string.Empty;
    public string TakipCihaziDurumu { get; set; } = "Aktif";
    public DateTime TakipCihaziSonBaglantiZamani { get; set; }
    public string TakipCihaziHaberlesmeProtokolu { get; set; } = string.Empty;
    public decimal PilSeviyesi { get; set; }

    public int? PersonelId { get; set; }
    public int? EkipmanId { get; set; }
}

public class TakipCihaziGuncelleDto
{
    public string TakipCihaziSeriNo { get; set; } = string.Empty;
    public string TakipCihaziTuru { get; set; } = string.Empty;
    public string TakipCihaziModeli { get; set; } = string.Empty;
    public string TakipCihaziDurumu { get; set; } = string.Empty;
    public DateTime TakipCihaziSonBaglantiZamani { get; set; }
    public string TakipCihaziHaberlesmeProtokolu { get; set; } = string.Empty;
    public decimal PilSeviyesi { get; set; }

    public int? PersonelId { get; set; }
    public int? EkipmanId { get; set; }
}

public class TakipCihaziSorguParametreleri
{
    public string? Arama { get; set; }
    public string? TakipCihaziTuru { get; set; }
    public string? TakipCihaziDurumu { get; set; }
    public string? TakipCihaziHaberlesmeProtokolu { get; set; }
    public int? PersonelId { get; set; }
    public int? EkipmanId { get; set; }

    public string? SiralamaAlani { get; set; } = "takipCihaziId";
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
