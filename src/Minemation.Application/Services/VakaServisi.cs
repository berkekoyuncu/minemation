using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Application.Common;
using Minemation.Application.DTOs;
using Minemation.Application.Interfaces;
using Minemation.Domain.Entities;

namespace Minemation.Application.Services;

public class VakaServisi : IVakaServisi
{

    private readonly IVakaRepository _vakaRepository;
    private readonly IPersonelRepository _personelRepository;
    private readonly IEkipmanRepository _ekipmanRepository;


    public VakaServisi(
    IVakaRepository vakaRepository,
    IPersonelRepository personelRepository,
    IEkipmanRepository ekipmanRepository)
    {
        _vakaRepository = vakaRepository;
        _personelRepository = personelRepository;
        _ekipmanRepository = ekipmanRepository;
    }

    public async Task<ApiResponse<PagedResult<VakaListeDto>>> TumunuGetirAsync(VakaSorguParametreleri sorgu)
    {
        var vakalar = await _vakaRepository.TumunuGetirAsync();

        var filtreli = vakalar.AsQueryable();

        if (!string.IsNullOrWhiteSpace(sorgu.Arama))
        {
            var arama = sorgu.Arama.Trim().ToLower();

            filtreli = filtreli.Where(v =>
                (v.vakaAdi ?? "").ToLower().Contains(arama) ||
                (v.vakaTuru ?? "").ToLower().Contains(arama) ||
                (v.vakaAciklamasi ?? "").ToLower().Contains(arama) ||
                (v.olayNedeni ?? "").ToLower().Contains(arama));
        }

        if (!string.IsNullOrWhiteSpace(sorgu.VakaTuru))
        {
            var tur = sorgu.VakaTuru.Trim().ToLower();
            filtreli = filtreli.Where(v => (v.vakaTuru ?? "").ToLower() == tur);
        }

        if (!string.IsNullOrWhiteSpace(sorgu.VakaCiddiyetSeviyesi))
        {
            var ciddiyet = sorgu.VakaCiddiyetSeviyesi.Trim().ToLower();
            filtreli = filtreli.Where(v => (v.vakaCiddiyetSeviyesi ?? "").ToLower() == ciddiyet);
        }

        if (!string.IsNullOrWhiteSpace(sorgu.VakaDurumu))
        {
            var durum = sorgu.VakaDurumu.Trim().ToLower();
            filtreli = filtreli.Where(v => (v.vakaDurumu ?? "").ToLower() == durum);
        }

        if (sorgu.PersonelId.HasValue)
        {
            filtreli = filtreli.Where(v => v.personelId == sorgu.PersonelId.Value);
        }

        if (sorgu.IlgiliEkipmanId.HasValue)
        {
            filtreli = filtreli.Where(v => v.ilgiliEkipmanId == sorgu.IlgiliEkipmanId.Value);
        }

        filtreli = sorgu.SiralamaAlani?.ToLower() switch
        {
            "vakaadi" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(v => v.vakaAdi)
                : filtreli.OrderBy(v => v.vakaAdi),

            "vakaturu" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(v => v.vakaTuru)
                : filtreli.OrderBy(v => v.vakaTuru),

            "vakaciddiyetseviyesi" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(v => v.vakaCiddiyetSeviyesi)
                : filtreli.OrderBy(v => v.vakaCiddiyetSeviyesi),

            "vakadurumu" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(v => v.vakaDurumu)
                : filtreli.OrderBy(v => v.vakaDurumu),

            "vakakapanmatarihi" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(v => v.vakaKapanmaTarihi)
                : filtreli.OrderBy(v => v.vakaKapanmaTarihi),

            _ => sorgu.SiralamaYonu?.ToLower() == "asc"
                ? filtreli.OrderBy(v => v.vakaOlusmaTarihi)
                : filtreli.OrderByDescending(v => v.vakaOlusmaTarihi)
        };

        var toplamKayitSayisi = filtreli.Count();

        var liste = filtreli
            .Skip((sorgu.SayfaNumarasi - 1) * sorgu.SayfaBoyutu)
            .Take(sorgu.SayfaBoyutu)
            .Select(v => new VakaListeDto
            {
                VakaId = v.vakaId,
                VakaTuru = v.vakaTuru,
                VakaAdi = v.vakaAdi,
                VakaCiddiyetSeviyesi = v.vakaCiddiyetSeviyesi,
                VakaDurumu = v.vakaDurumu,
                VakaOlusmaTarihi = v.vakaOlusmaTarihi,
                VakaKapanmaTarihi = v.vakaKapanmaTarihi,
                OlayNedeni = v.olayNedeni,
                PersonelId = v.personelId,
                PersonelAdSoyad = v.Personel == null
                    ? null
                    : $"{v.Personel.personelAdi} {v.Personel.personelSoyadi}",
                RaporlayanEkipmanId = v.raporlayanEkipmanId,
                RaporlayanEkipmanAdi = v.RaporlayanEkipman == null ? null : v.RaporlayanEkipman.ekipmanAdi,
                IlgiliEkipmanId = v.ilgiliEkipmanId,
                IlgiliEkipmanAdi = v.IlgiliEkipman == null ? null : v.IlgiliEkipman.ekipmanAdi
            })
            .ToList();

        var sayfaliSonuc = PagedResult<VakaListeDto>.Create(
            liste,
            sorgu.SayfaNumarasi,
            sorgu.SayfaBoyutu,
            toplamKayitSayisi
        );

        return ApiResponse<PagedResult<VakaListeDto>>.Ok(sayfaliSonuc);
    }

    public async Task<ApiResponse<VakaDetayDto>> IdIleGetirAsync(int id)
    {
        var vaka = await _vakaRepository.IdIleGetirAsync(id);

        if (vaka is null)
            return ApiResponse<VakaDetayDto>.Fail("Vaka bulunamadı.");

        return ApiResponse<VakaDetayDto>.Ok(DetayDtoyaDonustur(vaka));
    }

    public async Task<ApiResponse<VakaDetayDto>> OlusturAsync(VakaOlusturDto dto)
    {
        

        var dogrulamaSonucu = await VakaIliskileriniDogrulaAsync(
            dto.PersonelId,
            dto.RaporlayanEkipmanId,
            dto.IlgiliEkipmanId);

        if (!dogrulamaSonucu.Success)
            return ApiResponse<VakaDetayDto>.Fail(dogrulamaSonucu.Message);

        var vaka = new Vaka
        {
            vakaTuru = dto.VakaTuru,
            vakaAdi = dto.VakaAdi,
            vakaCiddiyetSeviyesi = dto.VakaCiddiyetSeviyesi,
            vakaDurumu = dto.VakaDurumu,
            vakaAciklamasi = dto.VakaAciklamasi,
            vakaOlusmaTarihi = dto.VakaOlusmaTarihi,
            vakaKapanmaTarihi = dto.VakaKapanmaTarihi,
            olayNedeni = dto.OlayNedeni,
            personelId = dto.PersonelId,
            raporlayanEkipmanId = dto.RaporlayanEkipmanId,
            ilgiliEkipmanId = dto.IlgiliEkipmanId
        };

        await _vakaRepository.EkleAsync(vaka);
        await _vakaRepository.DegisiklikleriKaydetAsync();

        return ApiResponse<VakaDetayDto>.Ok(
            DetayDtoyaDonustur(vaka),
            "Vaka başarıyla oluşturuldu."
        );
    }

    public async Task<ApiResponse<VakaDetayDto>> GuncelleAsync(int id, VakaGuncelleDto dto)
    {
        var vaka = await _vakaRepository.IdIleGetirAsync(id);

        if (vaka is null)
            return ApiResponse<VakaDetayDto>.Fail("Vaka bulunamadı.");

        var dogrulamaSonucu = await VakaIliskileriniDogrulaAsync(
            dto.PersonelId,
            dto.RaporlayanEkipmanId,
            dto.IlgiliEkipmanId);

        if (!dogrulamaSonucu.Success)
            return ApiResponse<VakaDetayDto>.Fail(dogrulamaSonucu.Message);

        vaka.vakaTuru = dto.VakaTuru;
        vaka.vakaAdi = dto.VakaAdi;
        vaka.vakaCiddiyetSeviyesi = dto.VakaCiddiyetSeviyesi;
        vaka.vakaDurumu = dto.VakaDurumu;
        vaka.vakaAciklamasi = dto.VakaAciklamasi;
        vaka.vakaOlusmaTarihi = dto.VakaOlusmaTarihi;
        vaka.vakaKapanmaTarihi = dto.VakaKapanmaTarihi;
        vaka.olayNedeni = dto.OlayNedeni;
        vaka.personelId = dto.PersonelId;
        vaka.raporlayanEkipmanId = dto.RaporlayanEkipmanId;
        vaka.ilgiliEkipmanId = dto.IlgiliEkipmanId;

        await _vakaRepository.DegisiklikleriKaydetAsync();

        return ApiResponse<VakaDetayDto>.Ok(
            DetayDtoyaDonustur(vaka),
            "Vaka başarıyla güncellendi."
        );
    }

    public async Task<ApiResponse<bool>> SilAsync(int id)
    {
        var vaka = await _vakaRepository.IdIleGetirAsync(id);

        if (vaka is null)
            return ApiResponse<bool>.Fail("Vaka bulunamadı.");

        vaka.vakaDurumu = "Kapalı";
        vaka.vakaKapanmaTarihi ??= DateTime.Now;

        await _vakaRepository.DegisiklikleriKaydetAsync();

        return ApiResponse<bool>.Ok(true, "Vaka kapalı hale getirildi.");
    }

    private static VakaDetayDto DetayDtoyaDonustur(Vaka vaka)
    {
        return new VakaDetayDto
        {
            VakaId = vaka.vakaId,
            VakaTuru = vaka.vakaTuru,
            VakaAdi = vaka.vakaAdi,
            VakaCiddiyetSeviyesi = vaka.vakaCiddiyetSeviyesi,
            VakaDurumu = vaka.vakaDurumu,
            VakaAciklamasi = vaka.vakaAciklamasi,
            VakaOlusmaTarihi = vaka.vakaOlusmaTarihi,
            VakaKapanmaTarihi = vaka.vakaKapanmaTarihi,
            OlayNedeni = vaka.olayNedeni,
            PersonelId = vaka.personelId,
            PersonelAdSoyad = vaka.Personel == null
                ? null
                : $"{vaka.Personel.personelAdi} {vaka.Personel.personelSoyadi}",
            RaporlayanEkipmanId = vaka.raporlayanEkipmanId,
            RaporlayanEkipmanAdi = vaka.RaporlayanEkipman?.ekipmanAdi,
            IlgiliEkipmanId = vaka.ilgiliEkipmanId,
            IlgiliEkipmanAdi = vaka.IlgiliEkipman?.ekipmanAdi
        };
    }

    private async Task<ApiResponse<bool>> VakaIliskileriniDogrulaAsync(
    int? personelId,
    int? raporlayanEkipmanId,
    int? ilgiliEkipmanId)
    {
        if (personelId.HasValue && personelId.Value > 0)
        {
            var personel = await _personelRepository.IdIleGetirAsync(personelId.Value);

            if (personel is null)
                return ApiResponse<bool>.Fail("Seçilen personel sistemde bulunamadı. Lütfen geçerli bir Personel ID girin.");
        }

        if (raporlayanEkipmanId.HasValue && raporlayanEkipmanId.Value > 0)
        {
            var raporlayanEkipman = await _ekipmanRepository.IdIleGetirAsync(raporlayanEkipmanId.Value);

            if (raporlayanEkipman is null)
                return ApiResponse<bool>.Fail("Raporlayan ekipman sistemde bulunamadı. Lütfen geçerli bir Raporlayan Ekipman ID girin.");
        }

        if (ilgiliEkipmanId.HasValue && ilgiliEkipmanId.Value > 0)
        {
            var ilgiliEkipman = await _ekipmanRepository.IdIleGetirAsync(ilgiliEkipmanId.Value);

            if (ilgiliEkipman is null)
                return ApiResponse<bool>.Fail("İlgili ekipman sistemde bulunamadı. Lütfen geçerli bir İlgili Ekipman ID girin.");
        }

        return ApiResponse<bool>.Ok(true);
    }

}
