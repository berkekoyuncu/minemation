using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Application.Interfaces;
using Minemation.Domain.Entities;

namespace Minemation.Infrastructure.Repositories;

public class SahtePersonelRepository : IPersonelRepository
{
    private static readonly List<Personel> _personeller = new()
    {
        new Personel
        {
            personelId = 1,
            personelAdi = "Elif",
            personelSoyadi = "Okur",
            uzmanlik = "Backend Developer",
            tckn = "11111111111",
            personelDurumu = "Aktif",
            dogumTarihi = new DateTime(2002, 1, 1),
            cinsiyet = "Kadın",
            telNo = "05551112233",
            ikinciTelNo = "",
            eposta = "elif@example.com",
            adres = "Ankara",
            iseGirisTarihi = DateTime.Today.AddYears(-1),
            calisanTipi = "Tam Zamanlı",
            calisabildigiEkipmaTurleri = new List<string>(),
            rfidKartNumarasi = "RFID-001",
            kullaniciRolu = "Sistem Yöneticisi",
            departman = "Yazılım",
            calismaKonumu = "Merkez",
            sifreHash = "test",
            sonGirisTarihi = DateTime.Now
        },
        new Personel
        {
            personelId = 2,
            personelAdi = "Büşra",
            personelSoyadi = "Arslan",
            uzmanlik = "Veritabanı",
            tckn = "22222222222",
            personelDurumu = "Aktif",
            dogumTarihi = new DateTime(2002, 1, 1),
            cinsiyet = "Kadın",
            telNo = "05554445566",
            ikinciTelNo = "",
            eposta = "busra@example.com",
            adres = "Ankara",
            iseGirisTarihi = DateTime.Today.AddYears(-1),
            calisanTipi = "Tam Zamanlı",
            calisabildigiEkipmaTurleri = new List<string>(),
            rfidKartNumarasi = "RFID-002",
            kullaniciRolu = "Teknik Personel",
            departman = "Veritabanı",
            calismaKonumu = "Maden Sahası",
            sifreHash = "test",
            sonGirisTarihi = DateTime.Now
        }
    };

    public Task<List<Personel>> TumunuGetirAsync()
    {
        return Task.FromResult(_personeller.ToList());
    }

    public Task<Personel?> IdIleGetirAsync(int id)
    {
        var personel = _personeller.FirstOrDefault(p => p.personelId == id);
        return Task.FromResult(personel);
    }

    public Task<bool> TcknVarMiAsync(string tckn, int? haricTutulacakId = null)
    {
        var varMi = _personeller.Any(p =>
            p.tckn == tckn &&
            (!haricTutulacakId.HasValue || p.personelId != haricTutulacakId.Value));

        return Task.FromResult(varMi);
    }

    public Task EkleAsync(Personel personel)
    {
        var yeniId = _personeller.Count == 0
            ? 1
            : _personeller.Max(p => p.personelId) + 1;

        personel.personelId = yeniId;

        if (personel.calisabildigiEkipmaTurleri == null)
            personel.calisabildigiEkipmaTurleri = new List<string>();

        _personeller.Add(personel);

        return Task.CompletedTask;
    }

    public Task DegisiklikleriKaydetAsync()
    {
        return Task.CompletedTask;
    }
}
