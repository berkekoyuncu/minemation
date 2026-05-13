using System;
using System.Collections.Generic;
using System.Text;

namespace Minemation.Application.DTOs;

public class EkipmanListeDto
{
    public int EkipmanId { get; set; }
    public string EkipmanAdi { get; set; } = string.Empty;
    public string EkipmanMarka { get; set; } = string.Empty;
    public string EkipmanModel { get; set; } = string.Empty;
    public string Durum { get; set; } = string.Empty;
    public string SeriNo { get; set; } = string.Empty;
    public string RFIDEtiket { get; set; } = string.Empty;
    public string OperasyonTuru { get; set; } = string.Empty;
    public string EkipmanTuru { get; set; } = string.Empty;
}

public class EkipmanDetayDto
{
    public int EkipmanId { get; set; }

    public string EkipmanAdi { get; set; } = string.Empty;
    public string EkipmanMarka { get; set; } = string.Empty;
    public string EkipmanModel { get; set; } = string.Empty;
    public string Durum { get; set; } = string.Empty;

    public DateTime SonBakimTarihi { get; set; }
    public DateTime GelecekBakimTarihi { get; set; }

    public string SeriNo { get; set; } = string.Empty;
    public string RFIDEtiket { get; set; } = string.Empty;

    public string UreticiFirma { get; set; } = string.Empty;
    public string TedarikciFirma { get; set; } = string.Empty;

    public DateTime UretimYili { get; set; }

    public decimal Boyut { get; set; }
    public decimal Agirlik { get; set; }

    public DateTime SatinAlmaTarihi { get; set; }
    public DateTime GarantiBaslangicTarihi { get; set; }

    public string TeknikDokuman { get; set; } = string.Empty;
    public string KullanimKilavuzu { get; set; } = string.Empty;
    public string GarantiBelgesi { get; set; } = string.Empty;
    public string BakimFormu { get; set; } = string.Empty;
    public string SatinAlmaBelgesi { get; set; } = string.Empty;

    public string OperasyonTuru { get; set; } = string.Empty;
    public string EkipmanTuru { get; set; } = string.Empty;
}

public class EkipmanOlusturDto
{
    public string EkipmanAdi { get; set; } = string.Empty;
    public string EkipmanMarka { get; set; } = string.Empty;
    public string EkipmanModel { get; set; } = string.Empty;
    public string Durum { get; set; } = "Aktif";

    public DateTime SonBakimTarihi { get; set; }
    public DateTime GelecekBakimTarihi { get; set; }

    public string SeriNo { get; set; } = string.Empty;
    public string RFIDEtiket { get; set; } = string.Empty;

    public string UreticiFirma { get; set; } = string.Empty;
    public string TedarikciFirma { get; set; } = string.Empty;

    public DateTime UretimYili { get; set; }

    public decimal Boyut { get; set; }
    public decimal Agirlik { get; set; }

    public DateTime SatinAlmaTarihi { get; set; }
    public DateTime GarantiBaslangicTarihi { get; set; }

    public string TeknikDokuman { get; set; } = string.Empty;
    public string KullanimKilavuzu { get; set; } = string.Empty;
    public string GarantiBelgesi { get; set; } = string.Empty;
    public string BakimFormu { get; set; } = string.Empty;
    public string SatinAlmaBelgesi { get; set; } = string.Empty;

    public string OperasyonTuru { get; set; } = string.Empty;
    public string EkipmanTuru { get; set; } = string.Empty;
}

public class EkipmanGuncelleDto
{
    public string EkipmanAdi { get; set; } = string.Empty;
    public string EkipmanMarka { get; set; } = string.Empty;
    public string EkipmanModel { get; set; } = string.Empty;
    public string Durum { get; set; } = string.Empty;

    public DateTime SonBakimTarihi { get; set; }
    public DateTime GelecekBakimTarihi { get; set; }

    public string SeriNo { get; set; } = string.Empty;
    public string RFIDEtiket { get; set; } = string.Empty;

    public string UreticiFirma { get; set; } = string.Empty;
    public string TedarikciFirma { get; set; } = string.Empty;

    public DateTime UretimYili { get; set; }

    public decimal Boyut { get; set; }
    public decimal Agirlik { get; set; }

    public DateTime SatinAlmaTarihi { get; set; }
    public DateTime GarantiBaslangicTarihi { get; set; }

    public string TeknikDokuman { get; set; } = string.Empty;
    public string KullanimKilavuzu { get; set; } = string.Empty;
    public string GarantiBelgesi { get; set; } = string.Empty;
    public string BakimFormu { get; set; } = string.Empty;
    public string SatinAlmaBelgesi { get; set; } = string.Empty;

    public string OperasyonTuru { get; set; } = string.Empty;
    public string EkipmanTuru { get; set; } = string.Empty;
}

public class EkipmanSorguParametreleri
{
    public string? Arama { get; set; }
    public string? Durum { get; set; }
    public string? OperasyonTuru { get; set; }
    public string? EkipmanMarka { get; set; }

    public string? SiralamaAlani { get; set; } = "ekipmanAdi";
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
