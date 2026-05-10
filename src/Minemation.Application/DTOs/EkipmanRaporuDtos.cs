using System;
using System.Collections.Generic;
using System.Text;

namespace Minemation.Application.DTOs;

public class EkipmanRaporuListeDto
{
    public int RaporId { get; set; }
    public string? RaporAdi { get; set; }
    public string? RaporTuru { get; set; }
    public DateTime? RaporOlusturmaTarihi { get; set; }

    public string EkipmanTuru { get; set; } = string.Empty;
    public int ArizaSayisi { get; set; }
    public int CalismaSuresi { get; set; }
}

public class EkipmanRaporuDetayDto
{
    public int RaporId { get; set; }
    public string? RaporAdi { get; set; }
    public string? RaporTuru { get; set; }
    public string? RaporAciklamasi { get; set; }
    public DateTime? RaporOlusturmaTarihi { get; set; }
    public string? RaporDosyaYolu { get; set; }
    public string? ZamanAraligi { get; set; }

    public string EkipmanTuru { get; set; } = string.Empty;
    public int ArizaSayisi { get; set; }
    public int CalismaSuresi { get; set; }
}

public class EkipmanRaporuOlusturDto
{
    public int RaporId { get; set; }
    public string EkipmanTuru { get; set; } = string.Empty;
    public int ArizaSayisi { get; set; }
    public int CalismaSuresi { get; set; }
}

public class EkipmanRaporuGuncelleDto
{
    public string EkipmanTuru { get; set; } = string.Empty;
    public int ArizaSayisi { get; set; }
    public int CalismaSuresi { get; set; }
}

public class EkipmanRaporuSorguParametreleri
{
    public string? Arama { get; set; }
    public string? EkipmanTuru { get; set; }

    public string? SiralamaAlani { get; set; } = "raporId";
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
