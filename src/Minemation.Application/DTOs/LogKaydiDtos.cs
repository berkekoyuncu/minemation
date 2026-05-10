using System;
using System.Collections.Generic;
using System.Text;

namespace Minemation.Application.DTOs;

public class LogKaydiListeDto
{
    public int LogKaydiID { get; set; }

    public string IslemTipi { get; set; } = string.Empty;
    public string LogKaydiAciklamasi { get; set; } = string.Empty;
    public DateTime LogKaydiTarihi { get; set; }

    public string IpAdresi { get; set; } = string.Empty;
    public string OnemSeviyesi { get; set; } = string.Empty;
    public string Durum { get; set; } = string.Empty;

    public int? PersonelId { get; set; }
    public string? PersonelAdSoyad { get; set; }

    public int? EkipmanId { get; set; }
    public string? EkipmanAdi { get; set; }
}

public class LogKaydiDetayDto
{
    public int LogKaydiID { get; set; }

    public string IslemTipi { get; set; } = string.Empty;
    public string LogKaydiAciklamasi { get; set; } = string.Empty;
    public DateTime LogKaydiTarihi { get; set; }

    public string IpAdresi { get; set; } = string.Empty;
    public string OnemSeviyesi { get; set; } = string.Empty;
    public string Durum { get; set; } = string.Empty;

    public int? PersonelId { get; set; }
    public string? PersonelAdSoyad { get; set; }

    public int? EkipmanId { get; set; }
    public string? EkipmanAdi { get; set; }
    public string? EkipmanDurumu { get; set; }
}

public class LogKaydiOlusturDto
{
    public string IslemTipi { get; set; } = string.Empty;
    public string LogKaydiAciklamasi { get; set; } = string.Empty;
    public DateTime LogKaydiTarihi { get; set; }

    public string IpAdresi { get; set; } = string.Empty;
    public string OnemSeviyesi { get; set; } = string.Empty;
    public string Durum { get; set; } = "Aktif";

    public int? PersonelId { get; set; }
    public int? EkipmanId { get; set; }
}

public class LogKaydiGuncelleDto
{
    public string IslemTipi { get; set; } = string.Empty;
    public string LogKaydiAciklamasi { get; set; } = string.Empty;
    public DateTime LogKaydiTarihi { get; set; }

    public string IpAdresi { get; set; } = string.Empty;
    public string OnemSeviyesi { get; set; } = string.Empty;
    public string Durum { get; set; } = string.Empty;

    public int? PersonelId { get; set; }
    public int? EkipmanId { get; set; }
}

public class LogKaydiSorguParametreleri
{
    public string? Arama { get; set; }
    public string? IslemTipi { get; set; }
    public string? OnemSeviyesi { get; set; }
    public string? Durum { get; set; }
    public int? PersonelId { get; set; }
    public int? EkipmanId { get; set; }

    public string? SiralamaAlani { get; set; } = "logKaydiTarihi";
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
