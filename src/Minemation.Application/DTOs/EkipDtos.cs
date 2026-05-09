using System;
using System.Collections.Generic;
using System.Text;

namespace Minemation.Application.DTOs;

public class EkipListeDto
{
    public int EkipId { get; set; }
    public int EkipUyeSayisi { get; set; }
    public string PersonelGorevi { get; set; } = string.Empty;
    public string Durum { get; set; } = string.Empty;

    public int PersonelId { get; set; }
    public string? PersonelAdSoyad { get; set; }

    public int VardiyaId { get; set; }
    public string? VardiyaAdi { get; set; }
}

public class EkipDetayDto
{
    public int EkipId { get; set; }
    public int EkipUyeSayisi { get; set; }
    public string PersonelGorevi { get; set; } = string.Empty;
    public string Durum { get; set; } = string.Empty;

    public int PersonelId { get; set; }
    public string? PersonelAdSoyad { get; set; }

    public int VardiyaId { get; set; }
    public string? VardiyaAdi { get; set; }
    public string? VardiyaDurumu { get; set; }
}

public class EkipOlusturDto
{
    public int EkipUyeSayisi { get; set; }
    public string PersonelGorevi { get; set; } = string.Empty;
    public string Durum { get; set; } = "Aktif";

    public int PersonelId { get; set; }
    public int VardiyaId { get; set; }
}

public class EkipGuncelleDto
{
    public int EkipUyeSayisi { get; set; }
    public string PersonelGorevi { get; set; } = string.Empty;
    public string Durum { get; set; } = string.Empty;

    public int PersonelId { get; set; }
    public int VardiyaId { get; set; }
}

public class EkipSorguParametreleri
{
    public string? Arama { get; set; }
    public string? Durum { get; set; }
    public string? PersonelGorevi { get; set; }
    public int? PersonelId { get; set; }
    public int? VardiyaId { get; set; }

    public string? SiralamaAlani { get; set; } = "ekipId";
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
