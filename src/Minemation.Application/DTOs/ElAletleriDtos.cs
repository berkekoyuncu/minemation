using System;
using System.Collections.Generic;
using System.Text;

namespace Minemation.Application.DTOs;

public class ElAletleriListeDto
{
    public int EkipmanId { get; set; }
    public string? EkipmanAdi { get; set; }
    public string? EkipmanDurumu { get; set; }

    public string GucKaynagiTipi { get; set; } = string.Empty;
    public decimal BataryaKapasitesi { get; set; }
    public string KullanimAmaci { get; set; } = string.Empty;
}

public class ElAletleriDetayDto
{
    public int EkipmanId { get; set; }
    public string? EkipmanAdi { get; set; }
    public string? EkipmanMarka { get; set; }
    public string? EkipmanModel { get; set; }
    public string? EkipmanDurumu { get; set; }

    public string GucKaynagiTipi { get; set; } = string.Empty;
    public decimal BataryaKapasitesi { get; set; }
    public string KullanimAmaci { get; set; } = string.Empty;
}

public class ElAletleriOlusturDto
{
    public int EkipmanId { get; set; }

    public string GucKaynagiTipi { get; set; } = string.Empty;
    public decimal BataryaKapasitesi { get; set; }
    public string KullanimAmaci { get; set; } = string.Empty;
}

public class ElAletleriGuncelleDto
{
    public string GucKaynagiTipi { get; set; } = string.Empty;
    public decimal BataryaKapasitesi { get; set; }
    public string KullanimAmaci { get; set; } = string.Empty;
}

public class ElAletleriSorguParametreleri
{
    public string? Arama { get; set; }
    public string? GucKaynagiTipi { get; set; }
    public string? KullanimAmaci { get; set; }

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
