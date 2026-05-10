using System;
using System.Collections.Generic;
using System.Text;

namespace Minemation.Application.DTOs;

public class KiriciListeDto
{
    public int EkipmanId { get; set; }
    public string? EkipmanAdi { get; set; }
    public string? EkipmanDurumu { get; set; }

    public decimal DarbeEnerjisi { get; set; }
    public decimal DakikadakiDarbeSayisi { get; set; }
    public decimal CalismaBasinci { get; set; }
    public string UcTipi { get; set; } = string.Empty;
    public string GerekenYagDebisi { get; set; } = string.Empty;
}

public class KiriciDetayDto
{
    public int EkipmanId { get; set; }
    public string? EkipmanAdi { get; set; }
    public string? EkipmanMarka { get; set; }
    public string? EkipmanModel { get; set; }
    public string? EkipmanDurumu { get; set; }

    public decimal DarbeEnerjisi { get; set; }
    public decimal DakikadakiDarbeSayisi { get; set; }
    public decimal CalismaBasinci { get; set; }
    public string UcTipi { get; set; } = string.Empty;
    public string GerekenYagDebisi { get; set; } = string.Empty;
}

public class KiriciOlusturDto
{
    public int EkipmanId { get; set; }

    public decimal DarbeEnerjisi { get; set; }
    public decimal DakikadakiDarbeSayisi { get; set; }
    public decimal CalismaBasinci { get; set; }
    public string UcTipi { get; set; } = string.Empty;
    public string GerekenYagDebisi { get; set; } = string.Empty;
}

public class KiriciGuncelleDto
{
    public decimal DarbeEnerjisi { get; set; }
    public decimal DakikadakiDarbeSayisi { get; set; }
    public decimal CalismaBasinci { get; set; }
    public string UcTipi { get; set; } = string.Empty;
    public string GerekenYagDebisi { get; set; } = string.Empty;
}

public class KiriciSorguParametreleri
{
    public string? Arama { get; set; }
    public string? UcTipi { get; set; }

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