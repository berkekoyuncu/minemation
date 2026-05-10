using System;
using System.Collections.Generic;
using System.Text;

namespace Minemation.Application.DTOs;

public class SensorListeDto
{
    public int EkipmanId { get; set; }
    public string? EkipmanAdi { get; set; }

    public string SensorTipi { get; set; } = string.Empty;
    public string SensorDurumu { get; set; } = string.Empty;

    public double MinEsikDeger { get; set; }
    public double MaxEsikDeger { get; set; }
    public double Hassasiyet { get; set; }

    public string BaglantiProtokolu { get; set; } = string.Empty;
    public string HaberlesmeTipi { get; set; } = string.Empty;
}

public class SensorDetayDto
{
    public int EkipmanId { get; set; }
    public string? EkipmanAdi { get; set; }
    public string? EkipmanDurumu { get; set; }

    public string SensorTipi { get; set; } = string.Empty;
    public string SensorDurumu { get; set; } = string.Empty;

    public double MinEsikDeger { get; set; }
    public double MaxEsikDeger { get; set; }
    public double Hassasiyet { get; set; }

    public string BaglantiProtokolu { get; set; } = string.Empty;
    public string HaberlesmeTipi { get; set; } = string.Empty;
}

public class SensorOlusturDto
{
    public int EkipmanId { get; set; }

    public string SensorTipi { get; set; } = string.Empty;
    public string SensorDurumu { get; set; } = "Aktif";

    public double MinEsikDeger { get; set; }
    public double MaxEsikDeger { get; set; }
    public double Hassasiyet { get; set; }

    public string BaglantiProtokolu { get; set; } = string.Empty;
    public string HaberlesmeTipi { get; set; } = string.Empty;
}

public class SensorGuncelleDto
{
    public string SensorTipi { get; set; } = string.Empty;
    public string SensorDurumu { get; set; } = string.Empty;

    public double MinEsikDeger { get; set; }
    public double MaxEsikDeger { get; set; }
    public double Hassasiyet { get; set; }

    public string BaglantiProtokolu { get; set; } = string.Empty;
    public string HaberlesmeTipi { get; set; } = string.Empty;
}

public class SensorSorguParametreleri
{
    public string? Arama { get; set; }
    public string? SensorTipi { get; set; }
    public string? SensorDurumu { get; set; }
    public string? BaglantiProtokolu { get; set; }
    public int? EkipmanId { get; set; }

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