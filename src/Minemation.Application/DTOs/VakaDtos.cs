using System;
using System.Collections.Generic;
using System.Text;

namespace Minemation.Application.DTOs;

public class VakaListeDto
{
    public int VakaId { get; set; }
    public string VakaTuru { get; set; } = string.Empty;
    public string VakaAdi { get; set; } = string.Empty;
    public string VakaCiddiyetSeviyesi { get; set; } = string.Empty;
    public string VakaDurumu { get; set; } = string.Empty;
    public DateTime VakaOlusmaTarihi { get; set; }
    public DateTime? VakaKapanmaTarihi { get; set; }
    public string OlayNedeni { get; set; } = string.Empty;

    public int? PersonelId { get; set; }
    public string? PersonelAdSoyad { get; set; }

    public int? RaporlayanEkipmanId { get; set; }
    public string? RaporlayanEkipmanAdi { get; set; }

    public int? IlgiliEkipmanId { get; set; }
    public string? IlgiliEkipmanAdi { get; set; }
}

public class VakaDetayDto
{
    public int VakaId { get; set; }

    public string VakaTuru { get; set; } = string.Empty;
    public string VakaAdi { get; set; } = string.Empty;
    public string VakaCiddiyetSeviyesi { get; set; } = string.Empty;
    public string VakaDurumu { get; set; } = string.Empty;
    public string VakaAciklamasi { get; set; } = string.Empty;

    public DateTime VakaOlusmaTarihi { get; set; }
    public DateTime? VakaKapanmaTarihi { get; set; }

    public string OlayNedeni { get; set; } = string.Empty;

    public int? PersonelId { get; set; }
    public string? PersonelAdSoyad { get; set; }

    public int? RaporlayanEkipmanId { get; set; }
    public string? RaporlayanEkipmanAdi { get; set; }

    public int? IlgiliEkipmanId { get; set; }
    public string? IlgiliEkipmanAdi { get; set; }
}

public class VakaOlusturDto
{
    public string VakaTuru { get; set; } = string.Empty;
    public string VakaAdi { get; set; } = string.Empty;
    public string VakaCiddiyetSeviyesi { get; set; } = string.Empty;
    public string VakaDurumu { get; set; } = "Açık";
    public string VakaAciklamasi { get; set; } = string.Empty;

    public DateTime VakaOlusmaTarihi { get; set; }
    public DateTime? VakaKapanmaTarihi { get; set; }

    public string OlayNedeni { get; set; } = string.Empty;

    public int? PersonelId { get; set; }
    public int? RaporlayanEkipmanId { get; set; }
    public int? IlgiliEkipmanId { get; set; }
}

public class VakaGuncelleDto
{
    public string VakaTuru { get; set; } = string.Empty;
    public string VakaAdi { get; set; } = string.Empty;
    public string VakaCiddiyetSeviyesi { get; set; } = string.Empty;
    public string VakaDurumu { get; set; } = string.Empty;
    public string VakaAciklamasi { get; set; } = string.Empty;

    public DateTime VakaOlusmaTarihi { get; set; }
    public DateTime? VakaKapanmaTarihi { get; set; }

    public string OlayNedeni { get; set; } = string.Empty;

    public int? PersonelId { get; set; }
    public int? RaporlayanEkipmanId { get; set; }
    public int? IlgiliEkipmanId { get; set; }
}

public class VakaSorguParametreleri
{
    public string? Arama { get; set; }
    public string? VakaTuru { get; set; }
    public string? VakaCiddiyetSeviyesi { get; set; }
    public string? VakaDurumu { get; set; }
    public int? PersonelId { get; set; }
    public int? IlgiliEkipmanId { get; set; }

    public string? SiralamaAlani { get; set; } = "vakaOlusmaTarihi";
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
