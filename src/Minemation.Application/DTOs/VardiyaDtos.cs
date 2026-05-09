using System;
using System.Collections.Generic;
using System.Text;

namespace Minemation.Application.DTOs;

public class VardiyaListeDto
{
    public int VardiyaId { get; set; }
    public string VardiyaAdi { get; set; } = string.Empty;
    public DateTime VardiyaBaslangicTarihi { get; set; }
    public DateTime VardiyaBitisTarihi { get; set; }
    public string VardiyaDurumu { get; set; } = string.Empty;
    public string VardiyaTipi { get; set; } = string.Empty;
    public string CalismaBolgesi { get; set; } = string.Empty;
    public string OperasyonTipi { get; set; } = string.Empty;
    public string OperasyonRiskSeviyesi { get; set; } = string.Empty;
    public int PersonelSayisi { get; set; }
    public int EkipmanSayisi { get; set; }
    public int EkipSayisi { get; set; }
}

public class VardiyaDetayDto
{
    public int VardiyaId { get; set; }
    public string VardiyaAdi { get; set; } = string.Empty;
    public string VardiyaTanimi { get; set; } = string.Empty;

    public DateTime VardiyaBaslangicTarihi { get; set; }
    public DateTime VardiyaBitisTarihi { get; set; }
    public DateTime VardiyaOlusturmaTarihi { get; set; }

    public string VardiyaSupervizoru { get; set; } = string.Empty;

    public int PersonelSayisi { get; set; }
    public int EkipmanSayisi { get; set; }
    public int EkipSayisi { get; set; }

    public string VardiyaDurumu { get; set; } = string.Empty;
    public string VardiyaTipi { get; set; } = string.Empty;
    public int ToplaVardiyaSuresi { get; set; }

    public string CalismaBolgesi { get; set; } = string.Empty;
    public string OperasyonTipi { get; set; } = string.Empty;
    public string OperasyonRiskSeviyesi { get; set; } = string.Empty;
    public string VardiyaNotlari { get; set; } = string.Empty;
    public string EkipmanOperatoru { get; set; } = string.Empty;

    public int? EkipmanId { get; set; }
    public string? EkipmanAdi { get; set; }

    public int VardiyaSorumlusu { get; set; }
    public string? VardiyaSorumlusuAdSoyad { get; set; }

    public int VardiyaIsgSorumlusu { get; set; }
    public string? IsgSorumlusuAdSoyad { get; set; }

    public int VardiyaTeknikSorumlusu { get; set; }
    public string? TeknikSorumlusuAdSoyad { get; set; }
}

public class VardiyaOlusturDto
{
    public string VardiyaAdi { get; set; } = string.Empty;
    public string VardiyaTanimi { get; set; } = string.Empty;

    public DateTime VardiyaBaslangicTarihi { get; set; }
    public DateTime VardiyaBitisTarihi { get; set; }

    public string VardiyaSupervizoru { get; set; } = string.Empty;

    public int PersonelSayisi { get; set; }
    public int EkipmanSayisi { get; set; }
    public int EkipSayisi { get; set; }

    public string VardiyaDurumu { get; set; } = "Aktif";
    public string VardiyaTipi { get; set; } = string.Empty;
    public int ToplaVardiyaSuresi { get; set; }

    public string CalismaBolgesi { get; set; } = string.Empty;
    public string OperasyonTipi { get; set; } = string.Empty;
    public string OperasyonRiskSeviyesi { get; set; } = string.Empty;
    public string VardiyaNotlari { get; set; } = string.Empty;
    public string EkipmanOperatoru { get; set; } = string.Empty;

    public int? EkipmanId { get; set; }

    public int VardiyaSorumlusu { get; set; }
    public int VardiyaIsgSorumlusu { get; set; }
    public int VardiyaTeknikSorumlusu { get; set; }
}

public class VardiyaGuncelleDto
{
    public string VardiyaAdi { get; set; } = string.Empty;
    public string VardiyaTanimi { get; set; } = string.Empty;

    public DateTime VardiyaBaslangicTarihi { get; set; }
    public DateTime VardiyaBitisTarihi { get; set; }

    public string VardiyaSupervizoru { get; set; } = string.Empty;

    public int PersonelSayisi { get; set; }
    public int EkipmanSayisi { get; set; }
    public int EkipSayisi { get; set; }

    public string VardiyaDurumu { get; set; } = string.Empty;
    public string VardiyaTipi { get; set; } = string.Empty;
    public int ToplaVardiyaSuresi { get; set; }

    public string CalismaBolgesi { get; set; } = string.Empty;
    public string OperasyonTipi { get; set; } = string.Empty;
    public string OperasyonRiskSeviyesi { get; set; } = string.Empty;
    public string VardiyaNotlari { get; set; } = string.Empty;
    public string EkipmanOperatoru { get; set; } = string.Empty;

    public int? EkipmanId { get; set; }

    public int VardiyaSorumlusu { get; set; }
    public int VardiyaIsgSorumlusu { get; set; }
    public int VardiyaTeknikSorumlusu { get; set; }
}

public class VardiyaSorguParametreleri
{
    public string? Arama { get; set; }
    public string? VardiyaDurumu { get; set; }
    public string? VardiyaTipi { get; set; }
    public string? CalismaBolgesi { get; set; }
    public string? OperasyonRiskSeviyesi { get; set; }

    public string? SiralamaAlani { get; set; } = "vardiyaAdi";
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
