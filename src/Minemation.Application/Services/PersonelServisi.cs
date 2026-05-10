using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Application.Common;
using Minemation.Application.DTOs;
using Minemation.Application.Interfaces;
using Minemation.Domain.Entities;

namespace Minemation.Application.Services;

public class PersonelServisi : IPersonelServisi
{
    private readonly IPersonelRepository _personelRepository;

    public PersonelServisi(IPersonelRepository personelRepository)
    {
        _personelRepository = personelRepository;
    }

    public async Task<ApiResponse<PagedResult<PersonelListeDto>>> TumunuGetirAsync(PersonelSorguParametreleri sorgu)
    {
        var personeller = await _personelRepository.TumunuGetirAsync();

        var filtreli = personeller.AsQueryable();

        if (!string.IsNullOrWhiteSpace(sorgu.Arama))
        {
            var arama = sorgu.Arama.Trim().ToLower();

            filtreli = filtreli.Where(x =>
                (x.personelAdi ?? "").ToLower().Contains(arama) ||
                (x.personelSoyadi ?? "").ToLower().Contains(arama) ||
                (x.tckn ?? "").ToLower().Contains(arama) ||
                (x.uzmanlik ?? "").ToLower().Contains(arama));
        }

        if (!string.IsNullOrWhiteSpace(sorgu.Departman))
        {
            var departman = sorgu.Departman.Trim().ToLower();
            filtreli = filtreli.Where(x => (x.departman ?? "").ToLower() == departman);
        }

        if (!string.IsNullOrWhiteSpace(sorgu.KullaniciRolu))
        {
            var kullaniciRolu = sorgu.KullaniciRolu.Trim().ToLower();
            filtreli = filtreli.Where(x => (x.kullaniciRolu ?? "").ToLower() == kullaniciRolu);
        }

        if (!string.IsNullOrWhiteSpace(sorgu.Uzmanlik))
        {
            var uzmanlik = sorgu.Uzmanlik.Trim().ToLower();
            filtreli = filtreli.Where(x => (x.uzmanlik ?? "").ToLower() == uzmanlik);
        }

        if (!string.IsNullOrWhiteSpace(sorgu.CalismaKonumu))
        {
            var calismaKonumu = sorgu.CalismaKonumu.Trim().ToLower();
            filtreli = filtreli.Where(x => (x.calismaKonumu ?? "").ToLower() == calismaKonumu);
        }

        if (!string.IsNullOrWhiteSpace(sorgu.PersonelDurumu))
        {
            var personelDurumu = sorgu.PersonelDurumu.Trim().ToLower();
            filtreli = filtreli.Where(x => (x.personelDurumu ?? "").ToLower() == personelDurumu);
        }

        filtreli = sorgu.SiralamaAlani?.ToLower() switch
        {
            "personelsoyadi" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(x => x.personelSoyadi)
                : filtreli.OrderBy(x => x.personelSoyadi),

            "departman" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(x => x.departman)
                : filtreli.OrderBy(x => x.departman),

            "kullanicirolu" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(x => x.kullaniciRolu)
                : filtreli.OrderBy(x => x.kullaniciRolu),

            "uzmanlik" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(x => x.uzmanlik)
                : filtreli.OrderBy(x => x.uzmanlik),

            "calismakonumu" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(x => x.calismaKonumu)
                : filtreli.OrderBy(x => x.calismaKonumu),

            _ => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(x => x.personelAdi)
                : filtreli.OrderBy(x => x.personelAdi)
        };

        var toplamKayitSayisi = filtreli.Count();

        var liste = filtreli
            .Skip((sorgu.SayfaNumarasi - 1) * sorgu.SayfaBoyutu)
            .Take(sorgu.SayfaBoyutu)
            .Select(x => new PersonelListeDto
            {
                PersonelId = x.personelId,
                AdSoyad = $"{x.personelAdi} {x.personelSoyadi}",
                Uzmanlik = x.uzmanlik,
                Departman = x.departman,
                KullaniciRolu = x.kullaniciRolu,
                CalismaKonumu = x.calismaKonumu,
                PersonelDurumu = x.personelDurumu
            })
            .ToList();

        var sayfaliSonuc = PagedResult<PersonelListeDto>.Create(
            liste,
            sorgu.SayfaNumarasi,
            sorgu.SayfaBoyutu,
            toplamKayitSayisi
        );

        return ApiResponse<PagedResult<PersonelListeDto>>.Ok(sayfaliSonuc);
    }

    public async Task<ApiResponse<PersonelDetayDto>> IdIleGetirAsync(int id)
    {
        var personel = await _personelRepository.IdIleGetirAsync(id);

        if (personel is null)
            return ApiResponse<PersonelDetayDto>.Fail("Personel bulunamadı.");

        return ApiResponse<PersonelDetayDto>.Ok(DetayDtoyaDonustur(personel));
    }

    public async Task<ApiResponse<PersonelDetayDto>> OlusturAsync(PersonelOlusturDto dto)
    {
        var tcknVarMi = await _personelRepository.TcknVarMiAsync(dto.Tckn);

        if (tcknVarMi)
            return ApiResponse<PersonelDetayDto>.Fail("Bu T.C. kimlik numarası ile kayıtlı personel zaten var.");

        var personel = new Personel
        {
            tckn = dto.Tckn,
            personelAdi = dto.PersonelAdi,
            personelSoyadi = dto.PersonelSoyadi,
            uzmanlik = dto.Uzmanlik,
            personelDurumu = dto.PersonelDurumu,
            dogumTarihi = dto.DogumTarihi,
            cinsiyet = dto.Cinsiyet,
            telNo = dto.TelNo,
            ikinciTelNo = dto.IkinciTelNo,
            eposta = dto.Eposta,
            adres = dto.Adres,
            iseGirisTarihi = dto.IseGirisTarihi,
            calisanTipi = dto.CalisanTipi,
            rfidKartNumarasi = dto.RfidKartNumarasi,
            kullaniciRolu = dto.KullaniciRolu,
            departman = dto.Departman,
            calismaKonumu = dto.CalismaKonumu,
            sifreHash = dto.SifreHash,
            sonGirisTarihi = DateTime.MinValue,
            calisabildigiEkipmaTurleri = new List<string>()
        };

        await _personelRepository.EkleAsync(personel);
        await _personelRepository.DegisiklikleriKaydetAsync();

        return ApiResponse<PersonelDetayDto>.Ok(
            DetayDtoyaDonustur(personel),
            "Personel başarıyla oluşturuldu."
        );
    }

    public async Task<ApiResponse<PersonelDetayDto>> GuncelleAsync(int id, PersonelGuncelleDto dto)
    {
        var personel = await _personelRepository.IdIleGetirAsync(id);

        if (personel is null)
            return ApiResponse<PersonelDetayDto>.Fail("Personel bulunamadı.");

        var tcknVarMi = await _personelRepository.TcknVarMiAsync(dto.Tckn, id);

        if (tcknVarMi)
            return ApiResponse<PersonelDetayDto>.Fail("Bu T.C. kimlik numarası başka bir personele ait.");

        personel.tckn = dto.Tckn;
        personel.personelAdi = dto.PersonelAdi;
        personel.personelSoyadi = dto.PersonelSoyadi;
        personel.uzmanlik = dto.Uzmanlik;
        personel.personelDurumu = dto.PersonelDurumu;
        personel.dogumTarihi = dto.DogumTarihi;
        personel.cinsiyet = dto.Cinsiyet;
        personel.telNo = dto.TelNo;
        personel.ikinciTelNo = dto.IkinciTelNo;
        personel.eposta = dto.Eposta;
        personel.adres = dto.Adres;
        personel.iseGirisTarihi = dto.IseGirisTarihi;
        personel.calisanTipi = dto.CalisanTipi;
        personel.rfidKartNumarasi = dto.RfidKartNumarasi;
        personel.kullaniciRolu = dto.KullaniciRolu;
        personel.departman = dto.Departman;
        personel.calismaKonumu = dto.CalismaKonumu;

        await _personelRepository.DegisiklikleriKaydetAsync();

        return ApiResponse<PersonelDetayDto>.Ok(
            DetayDtoyaDonustur(personel),
            "Personel başarıyla güncellendi."
        );
    }

    public async Task<ApiResponse<bool>> SilAsync(int id)
    {
        var personel = await _personelRepository.IdIleGetirAsync(id);

        if (personel is null)
            return ApiResponse<bool>.Fail("Personel bulunamadı.");

        personel.personelDurumu = "Pasif";

        await _personelRepository.DegisiklikleriKaydetAsync();

        return ApiResponse<bool>.Ok(true, "Personel pasif hale getirildi.");
    }

    private static PersonelDetayDto DetayDtoyaDonustur(Personel personel)
    {
        return new PersonelDetayDto
        {
            PersonelId = personel.personelId,
            Tckn = personel.tckn,
            PersonelAdi = personel.personelAdi,
            PersonelSoyadi = personel.personelSoyadi,
            Uzmanlik = personel.uzmanlik,
            PersonelDurumu = personel.personelDurumu,
            DogumTarihi = personel.dogumTarihi,
            Cinsiyet = personel.cinsiyet,
            TelNo = personel.telNo,
            IkinciTelNo = personel.ikinciTelNo,
            Eposta = personel.eposta,
            Adres = personel.adres,
            IseGirisTarihi = personel.iseGirisTarihi,
            CalisanTipi = personel.calisanTipi,
            RfidKartNumarasi = personel.rfidKartNumarasi,
            KullaniciRolu = personel.kullaniciRolu,
            Departman = personel.departman,
            CalismaKonumu = personel.calismaKonumu,
            SonGirisTarihi = personel.sonGirisTarihi
        };
    }
}