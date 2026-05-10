using System;
using System.Collections.Generic;
using System.Text;

namespace Minemation.Application.DTOs;

public class SaglikBilgileriListeDto
{
    public int PersonelId { get; set; }
    public string? PersonelAdSoyad { get; set; }

    public string KanGrubu { get; set; } = string.Empty;
    public string SaglikDurumu { get; set; } = string.Empty;

    public List<string> KronikHastaliklar { get; set; } = new();
    public List<string> Alerjiler { get; set; } = new();

    public string SaglikCalismaKisitlamalari { get; set; } = string.Empty;
    public DateTime SonMuayeneTarihi { get; set; }
}

public class SaglikBilgileriDetayDto
{
    public int PersonelId { get; set; }
    public string? PersonelAdSoyad { get; set; }
    public string? PersonelDurumu { get; set; }

    public string KanGrubu { get; set; } = string.Empty;
    public string SaglikDurumu { get; set; } = string.Empty;

    public List<string> KronikHastaliklar { get; set; } = new();
    public List<string> Alerjiler { get; set; } = new();

    public string SaglikCalismaKisitlamalari { get; set; } = string.Empty;
    public string AcilDurumNotu { get; set; } = string.Empty;
    public DateTime SonMuayeneTarihi { get; set; }
}

public class SaglikBilgileriOlusturDto
{
    public int PersonelId { get; set; }

    public string KanGrubu { get; set; } = string.Empty;
    public string SaglikDurumu { get; set; } = string.Empty;

    public List<string> KronikHastaliklar { get; set; } = new();
    public List<string> Alerjiler { get; set; } = new();

    public string SaglikCalismaKisitlamalari { get; set; } = string.Empty;
    public string AcilDurumNotu { get; set; } = string.Empty;
    public DateTime SonMuayeneTarihi { get; set; }
}

public class SaglikBilgileriGuncelleDto
{
    public string KanGrubu { get; set; } = string.Empty;
    public string SaglikDurumu { get; set; } = string.Empty;

    public List<string> KronikHastaliklar { get; set; } = new();
    public List<string> Alerjiler { get; set; } = new();

    public string SaglikCalismaKisitlamalari { get; set; } = string.Empty;
    public string AcilDurumNotu { get; set; } = string.Empty;
    public DateTime SonMuayeneTarihi { get; set; }
}

public class SaglikBilgileriSorguParametreleri
{
    public string? Arama { get; set; }
    public string? KanGrubu { get; set; }
    public string? SaglikDurumu { get; set; }

    public string? SiralamaAlani { get; set; } = "personelId";
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
