using System;
using System.Collections.Generic;
using System.Text;


namespace Minemation.Application.DTOs;

public class PersonelListeDto
{
    public int PersonelId { get; set; }
    public string AdSoyad { get; set; } = string.Empty;
    public string Uzmanlik { get; set; } = string.Empty;
    public string Departman { get; set; } = string.Empty;
    public string KullaniciRolu { get; set; } = string.Empty;
    public string CalismaKonumu { get; set; } = string.Empty;
    public string PersonelDurumu { get; set; } = string.Empty;
}

public class PersonelDetayDto
{
    public int PersonelId { get; set; }

    public string Tckn { get; set; } = string.Empty;
    public string PersonelAdi { get; set; } = string.Empty;
    public string PersonelSoyadi { get; set; } = string.Empty;

    public string Uzmanlik { get; set; } = string.Empty;
    public string PersonelDurumu { get; set; } = string.Empty;

    public DateTime DogumTarihi { get; set; }
    public string Cinsiyet { get; set; } = string.Empty;

    public string TelNo { get; set; } = string.Empty;
    public string IkinciTelNo { get; set; } = string.Empty;
    public string Eposta { get; set; } = string.Empty;
    public string Adres { get; set; } = string.Empty;

    public DateTime IseGirisTarihi { get; set; }
    public string CalisanTipi { get; set; } = string.Empty;

    public string RfidKartNumarasi { get; set; } = string.Empty;
    public string KullaniciRolu { get; set; } = string.Empty;
    public string Departman { get; set; } = string.Empty;
    public string CalismaKonumu { get; set; } = string.Empty;

    public DateTime SonGirisTarihi { get; set; }
}

public class PersonelOlusturDto
{
    public string Tckn { get; set; } = string.Empty;
    public string PersonelAdi { get; set; } = string.Empty;
    public string PersonelSoyadi { get; set; } = string.Empty;

    public string Uzmanlik { get; set; } = string.Empty;
    public string PersonelDurumu { get; set; } = "Aktif";

    public DateTime DogumTarihi { get; set; }
    public string Cinsiyet { get; set; } = string.Empty;

    public string TelNo { get; set; } = string.Empty;
    public string IkinciTelNo { get; set; } = string.Empty;
    public string Eposta { get; set; } = string.Empty;
    public string Adres { get; set; } = string.Empty;

    public DateTime IseGirisTarihi { get; set; }
    public string CalisanTipi { get; set; } = string.Empty;

    public string RfidKartNumarasi { get; set; } = string.Empty;
    public string KullaniciRolu { get; set; } = string.Empty;
    public string Departman { get; set; } = string.Empty;
    public string CalismaKonumu { get; set; } = string.Empty;

    public string SifreHash { get; set; } = string.Empty;
}

public class PersonelGuncelleDto
{
    public string Tckn { get; set; } = string.Empty;
    public string PersonelAdi { get; set; } = string.Empty;
    public string PersonelSoyadi { get; set; } = string.Empty;

    public string Uzmanlik { get; set; } = string.Empty;
    public string PersonelDurumu { get; set; } = string.Empty;

    public DateTime DogumTarihi { get; set; }
    public string Cinsiyet { get; set; } = string.Empty;

    public string TelNo { get; set; } = string.Empty;
    public string IkinciTelNo { get; set; } = string.Empty;
    public string Eposta { get; set; } = string.Empty;
    public string Adres { get; set; } = string.Empty;

    public DateTime IseGirisTarihi { get; set; }
    public string CalisanTipi { get; set; } = string.Empty;

    public string RfidKartNumarasi { get; set; } = string.Empty;
    public string KullaniciRolu { get; set; } = string.Empty;
    public string Departman { get; set; } = string.Empty;
    public string CalismaKonumu { get; set; } = string.Empty;
}
