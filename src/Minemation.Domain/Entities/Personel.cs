using System;
using System.Collections.Generic;
using System.Text;

namespace Minemation.Domain.Entities;


public class Personel
{
    public int personelId { get; set; }
    public string personelAdi { get; set; }
    public string personelSoyadi { get; set; }
    public string uzmanlik { get; set; }
    public string tckn { get; set; }
    public string personelDurumu { get; set; }
    public DateTime dogumTarihi { get; set; }
    public string cinsiyet { get; set; }
    public string telNo { get; set; }
    public string ikinciTelNo { get; set; }
    public string eposta { get; set; }
    public string adres { get; set; }
    public DateTime iseGirisTarihi { get; set; }
    public string calisanTipi { get; set; }
    public List<string> calisabildigiEkipmaTurleri { get; set; }
    public string rfidKartNumarasi { get; set; }
    public string kullaniciRolu { get; set; }
    public string departman { get; set; }
    public string calismaKonumu { get; set; }  // er diyagramda yok
    public string sifreHash { get; set; }
    public DateTime sonGirisTarihi { get; set; }    // er diyagramda yok

    // SAĞLIK BİLGİSİ: Bire-Bir (Zayıf Varlık)
    public virtual SaglikBilgileri SaglikBilgileri { get; set; }

    // ACİL DURUM KİŞİLERİ: Bire-Çok (Zayıf Varlık)
    // Tekil olan "AcilDurumIletisim" özelliğini sildik, sadece liste kaldı.
    public virtual ICollection<AcilDurumIletisim> AcilDurumKisileri { get; set; }

    public Personel()
    {
        AcilDurumKisileri = new HashSet<AcilDurumIletisim>();
    }
}