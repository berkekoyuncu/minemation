using System;
using System.Collections.Generic;
using System.Text;

namespace Minemation.Application.DTOs;

public class PersonelSorguParametreleri
{
    public string? Arama { get; set; }

    public string? Departman { get; set; }
    public string? KullaniciRolu { get; set; }
    public string? Uzmanlik { get; set; }
    public string? CalismaKonumu { get; set; }
    public string? PersonelDurumu { get; set; }

    public string? SiralamaAlani { get; set; } = "personelAdi";
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
