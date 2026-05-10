using System;
using System.Collections.Generic;
using System.Text;
namespace Minemation.Application.DTOs;

public class SensorVerisiListeDto
{
    public int SensorVerisiId { get; set; }
    public decimal Deger { get; set; }
    public string Birim { get; set; } = string.Empty;
    public DateTime OlcumTarihi { get; set; }

    public int EkipmanId { get; set; }
    public string? SensorTipi { get; set; }
    public string? EkipmanAdi { get; set; }

    public int VardiyaId { get; set; }
    public string? VardiyaAdi { get; set; }

    public bool EsikDisiMi { get; set; }
}

public class SensorVerisiDetayDto
{
    public int SensorVerisiId { get; set; }
    public decimal Deger { get; set; }
    public string Birim { get; set; } = string.Empty;
    public DateTime OlcumTarihi { get; set; }

    public int EkipmanId { get; set; }
    public string? SensorTipi { get; set; }
    public string? SensorDurumu { get; set; }
    public string? EkipmanAdi { get; set; }

    public double MinEsikDeger { get; set; }
    public double MaxEsikDeger { get; set; }

    public int VardiyaId { get; set; }
    public string? VardiyaAdi { get; set; }

    public bool EsikDisiMi { get; set; }
}

public class SensorVerisiOlusturDto
{
    public decimal Deger { get; set; }
    public string Birim { get; set; } = string.Empty;
    public DateTime OlcumTarihi { get; set; }

    public int EkipmanId { get; set; }
    public int VardiyaId { get; set; }
}

public class SensorVerisiGuncelleDto
{
    public decimal Deger { get; set; }
    public string Birim { get; set; } = string.Empty;
    public DateTime OlcumTarihi { get; set; }

    public int EkipmanId { get; set; }
    public int VardiyaId { get; set; }
}

public class SensorVerisiSorguParametreleri
{
    public string? Arama { get; set; }
    public int? EkipmanId { get; set; }
    public int? VardiyaId { get; set; }
    public string? Birim { get; set; }
    public bool? SadeceEsikDisi { get; set; }

    public string? SiralamaAlani { get; set; } = "olcumTarihi";
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
