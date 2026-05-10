using System;
using System.Collections.Generic;
using System.Text;

namespace Minemation.Application.DTOs;

public class EkskavatorListeDto
{
    public int EkipmanId { get; set; }
    public string? EkipmanAdi { get; set; }
    public string? EkipmanDurumu { get; set; }

    public string Plaka { get; set; } = string.Empty;
    public decimal KovaKapasitesi { get; set; }
    public decimal MotorGucu { get; set; }
    public decimal MaksimumKaziDerinligi { get; set; }
    public string PaletTipi { get; set; } = string.Empty;
    public decimal BomUzunlugu { get; set; }
}

public class EkskavatorDetayDto
{
    public int EkipmanId { get; set; }
    public string? EkipmanAdi { get; set; }
    public string? EkipmanMarka { get; set; }
    public string? EkipmanModel { get; set; }
    public string? EkipmanDurumu { get; set; }

    public string Plaka { get; set; } = string.Empty;
    public decimal KovaKapasitesi { get; set; }
    public decimal MotorGucu { get; set; }
    public decimal MaksimumKaziDerinligi { get; set; }
    public string PaletTipi { get; set; } = string.Empty;
    public decimal BomUzunlugu { get; set; }
}

public class EkskavatorOlusturDto
{
    public int EkipmanId { get; set; }

    public string Plaka { get; set; } = string.Empty;
    public decimal KovaKapasitesi { get; set; }
    public decimal MotorGucu { get; set; }
    public decimal MaksimumKaziDerinligi { get; set; }
    public string PaletTipi { get; set; } = string.Empty;
    public decimal BomUzunlugu { get; set; }
}

public class EkskavatorGuncelleDto
{
    public string Plaka { get; set; } = string.Empty;
    public decimal KovaKapasitesi { get; set; }
    public decimal MotorGucu { get; set; }
    public decimal MaksimumKaziDerinligi { get; set; }
    public string PaletTipi { get; set; } = string.Empty;
    public decimal BomUzunlugu { get; set; }
}

public class EkskavatorSorguParametreleri
{
    public string? Arama { get; set; }
    public string? Plaka { get; set; }
    public string? PaletTipi { get; set; }

    public string? SiralamaAlani { get; set; } = "ekipmanId";
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