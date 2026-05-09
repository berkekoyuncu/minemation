using System;
using System.Collections.Generic;
using System.Text;

namespace Minemation.Application.DTOs;

public class AksiyonListeDto
{
    public int MudahaleId { get; set; }
    public DateTime MudahaleBaslangicSaati { get; set; }
    public DateTime MudahaleBitisSaati { get; set; }
    public string MudahaleTuru { get; set; } = string.Empty;
    public string UygulananCozum { get; set; } = string.Empty;

    public int EkipId { get; set; }
    public int VakaId { get; set; }
    public string? VakaAdi { get; set; }
}

public class AksiyonDetayDto
{
    public int MudahaleId { get; set; }

    public DateTime MudahaleBaslangicSaati { get; set; }
    public DateTime MudahaleBitisSaati { get; set; }

    public string MudahaleTuru { get; set; } = string.Empty;
    public string UygulananCozum { get; set; } = string.Empty;

    public int EkipId { get; set; }

    public int VakaId { get; set; }
    public string? VakaAdi { get; set; }
    public string? VakaDurumu { get; set; }
    public string? VakaCiddiyetSeviyesi { get; set; }
}

public class AksiyonOlusturDto
{
    public DateTime MudahaleBaslangicSaati { get; set; }
    public DateTime MudahaleBitisSaati { get; set; }

    public string MudahaleTuru { get; set; } = string.Empty;
    public string UygulananCozum { get; set; } = string.Empty;

    public int EkipId { get; set; }
    public int VakaId { get; set; }
}

public class AksiyonGuncelleDto
{
    public DateTime MudahaleBaslangicSaati { get; set; }
    public DateTime MudahaleBitisSaati { get; set; }

    public string MudahaleTuru { get; set; } = string.Empty;
    public string UygulananCozum { get; set; } = string.Empty;

    public int EkipId { get; set; }
    public int VakaId { get; set; }
}

public class AksiyonSorguParametreleri
{
    public string? Arama { get; set; }
    public string? MudahaleTuru { get; set; }
    public int? EkipId { get; set; }
    public int? VakaId { get; set; }

    public string? SiralamaAlani { get; set; } = "mudahaleBaslangicSaati";
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
