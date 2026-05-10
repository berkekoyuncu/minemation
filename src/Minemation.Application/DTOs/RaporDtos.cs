using System;
using System.Collections.Generic;
using System.Text;

namespace Minemation.Application.DTOs;

public class RaporListeDto
{
    public int RaporId { get; set; }
    public string RaporAdi { get; set; } = string.Empty;
    public string RaporTuru { get; set; } = string.Empty;
    public DateTime RaporOlusturmaTarihi { get; set; }
    public string ZamanAraligi { get; set; } = string.Empty;

    public int? PersonelId { get; set; }
    public string? PersonelAdSoyad { get; set; }

    public int? EkipmanId { get; set; }
    public string? EkipmanAdi { get; set; }
}

public class RaporDetayDto
{
    public int RaporId { get; set; }
    public string RaporAdi { get; set; } = string.Empty;
    public string RaporTuru { get; set; } = string.Empty;
    public DateTime RaporOlusturmaTarihi { get; set; }

    public string RaporAciklamasi { get; set; } = string.Empty;
    public string RaporDosyaYolu { get; set; } = string.Empty;
    public string ZamanAraligi { get; set; } = string.Empty;

    public int? PersonelId { get; set; }
    public string? PersonelAdSoyad { get; set; }

    public int? EkipmanId { get; set; }
    public string? EkipmanAdi { get; set; }
}

public class RaporOlusturDto
{
    public string RaporAdi { get; set; } = string.Empty;
    public string RaporTuru { get; set; } = string.Empty;
    public DateTime RaporOlusturmaTarihi { get; set; }

    public string RaporAciklamasi { get; set; } = string.Empty;
    public string RaporDosyaYolu { get; set; } = string.Empty;
    public string ZamanAraligi { get; set; } = string.Empty;

    public int? PersonelId { get; set; }
    public int? EkipmanId { get; set; }
}

public class RaporGuncelleDto
{
    public string RaporAdi { get; set; } = string.Empty;
    public string RaporTuru { get; set; } = string.Empty;
    public DateTime RaporOlusturmaTarihi { get; set; }

    public string RaporAciklamasi { get; set; } = string.Empty;
    public string RaporDosyaYolu { get; set; } = string.Empty;
    public string ZamanAraligi { get; set; } = string.Empty;

    public int? PersonelId { get; set; }
    public int? EkipmanId { get; set; }
}

public class RaporSorguParametreleri
{
    public string? Arama { get; set; }
    public string? RaporTuru { get; set; }
    public string? ZamanAraligi { get; set; }
    public int? PersonelId { get; set; }
    public int? EkipmanId { get; set; }

    public string? SiralamaAlani { get; set; } = "raporOlusturmaTarihi";
    public string? SiralamaYonu { get; set; } = "desc";

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
