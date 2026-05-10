using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Application.Common;
using Minemation.Application.DTOs;
using Minemation.Application.Interfaces;
using Minemation.Domain.Entities;

namespace Minemation.Application.Services;

public class TakipCihaziServisi : ITakipCihaziServisi
{
    private readonly ITakipCihaziRepository _takipCihaziRepository;

    public TakipCihaziServisi(ITakipCihaziRepository takipCihaziRepository)
    {
        _takipCihaziRepository = takipCihaziRepository;
    }

    public async Task<ApiResponse<PagedResult<TakipCihaziListeDto>>> TumunuGetirAsync(TakipCihaziSorguParametreleri sorgu)
    {
        var cihazlar = await _takipCihaziRepository.TumunuGetirAsync();

        var filtreli = cihazlar.AsQueryable();

        if (!string.IsNullOrWhiteSpace(sorgu.Arama))
        {
            var arama = sorgu.Arama.Trim().ToLower();

            filtreli = filtreli.Where(t =>
                (t.takipCihaziSeriNo ?? "").ToLower().Contains(arama) ||
                (t.takipCihaziTuru ?? "").ToLower().Contains(arama) ||
                (t.takipCihaziModeli ?? "").ToLower().Contains(arama) ||
                (t.takipCihaziHaberlesmeProtokolu ?? "").ToLower().Contains(arama) ||
                (t.Personel != null && ((t.Personel.personelAdi ?? "") + " " + (t.Personel.personelSoyadi ?? "")).ToLower().Contains(arama)) ||
                (t.Ekipman != null && (t.Ekipman.ekipmanAdi ?? "").ToLower().Contains(arama)));
        }

        if (!string.IsNullOrWhiteSpace(sorgu.TakipCihaziTuru))
        {
            var tur = sorgu.TakipCihaziTuru.Trim().ToLower();
            filtreli = filtreli.Where(t => (t.takipCihaziTuru ?? "").ToLower() == tur);
        }

        if (!string.IsNullOrWhiteSpace(sorgu.TakipCihaziDurumu))
        {
            var durum = sorgu.TakipCihaziDurumu.Trim().ToLower();
            filtreli = filtreli.Where(t => (t.takipCihaziDurumu ?? "").ToLower() == durum);
        }

        if (!string.IsNullOrWhiteSpace(sorgu.TakipCihaziHaberlesmeProtokolu))
        {
            var protokol = sorgu.TakipCihaziHaberlesmeProtokolu.Trim().ToLower();
            filtreli = filtreli.Where(t => (t.takipCihaziHaberlesmeProtokolu ?? "").ToLower() == protokol);
        }

        if (sorgu.PersonelId.HasValue)
        {
            filtreli = filtreli.Where(t => t.personelId == sorgu.PersonelId.Value);
        }

        if (sorgu.EkipmanId.HasValue)
        {
            filtreli = filtreli.Where(t => t.ekipmanId == sorgu.EkipmanId.Value);
        }

        filtreli = sorgu.SiralamaAlani?.ToLower() switch
        {
            "serino" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(t => t.takipCihaziSeriNo)
                : filtreli.OrderBy(t => t.takipCihaziSeriNo),

            "tur" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(t => t.takipCihaziTuru)
                : filtreli.OrderBy(t => t.takipCihaziTuru),

            "durum" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(t => t.takipCihaziDurumu)
                : filtreli.OrderBy(t => t.takipCihaziDurumu),

            "pilseviyesi" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(t => t.pilSeviyesi)
                : filtreli.OrderBy(t => t.pilSeviyesi),

            "sonbaglantizamani" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(t => t.takipCihaziSonBaglantiZamani)
                : filtreli.OrderBy(t => t.takipCihaziSonBaglantiZamani),

            _ => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(t => t.takipCihaziId)
                : filtreli.OrderBy(t => t.takipCihaziId)
        };

        var toplamKayitSayisi = filtreli.Count();

        var liste = filtreli
            .Skip((sorgu.SayfaNumarasi - 1) * sorgu.SayfaBoyutu)
            .Take(sorgu.SayfaBoyutu)
            .Select(t => new TakipCihaziListeDto
            {
                TakipCihaziId = t.takipCihaziId,
                TakipCihaziSeriNo = t.takipCihaziSeriNo,
                TakipCihaziTuru = t.takipCihaziTuru,
                TakipCihaziModeli = t.takipCihaziModeli,
                TakipCihaziDurumu = t.takipCihaziDurumu,
                TakipCihaziSonBaglantiZamani = t.takipCihaziSonBaglantiZamani,
                TakipCihaziHaberlesmeProtokolu = t.takipCihaziHaberlesmeProtokolu,
                PilSeviyesi = t.pilSeviyesi,
                PersonelId = t.personelId,
                PersonelAdSoyad = t.Personel == null
                    ? null
                    : $"{t.Personel.personelAdi} {t.Personel.personelSoyadi}",
                EkipmanId = t.ekipmanId,
                EkipmanAdi = t.Ekipman == null ? null : t.Ekipman.ekipmanAdi
            })
            .ToList();

        var sayfaliSonuc = PagedResult<TakipCihaziListeDto>.Create(
            liste,
            sorgu.SayfaNumarasi,
            sorgu.SayfaBoyutu,
            toplamKayitSayisi
        );

        return ApiResponse<PagedResult<TakipCihaziListeDto>>.Ok(sayfaliSonuc);
    }

    public async Task<ApiResponse<TakipCihaziDetayDto>> IdIleGetirAsync(int id)
    {
        var cihaz = await _takipCihaziRepository.IdIleGetirAsync(id);

        if (cihaz is null)
            return ApiResponse<TakipCihaziDetayDto>.Fail("Takip cihazı bulunamadı.");

        return ApiResponse<TakipCihaziDetayDto>.Ok(DetayDtoyaDonustur(cihaz));
    }

    public async Task<ApiResponse<TakipCihaziDetayDto>> OlusturAsync(TakipCihaziOlusturDto dto)
    {
        var seriNoVarMi = await _takipCihaziRepository.SeriNoVarMiAsync(dto.TakipCihaziSeriNo);

        if (seriNoVarMi)
            return ApiResponse<TakipCihaziDetayDto>.Fail("Bu seri numarası ile kayıtlı takip cihazı zaten var.");

        var cihaz = new TakipCihazi
        {
            takipCihaziSeriNo = dto.TakipCihaziSeriNo,
            takipCihaziTuru = dto.TakipCihaziTuru,
            takipCihaziModeli = dto.TakipCihaziModeli,
            takipCihaziDurumu = dto.TakipCihaziDurumu,
            takipCihaziSonBaglantiZamani = dto.TakipCihaziSonBaglantiZamani,
            takipCihaziHaberlesmeProtokolu = dto.TakipCihaziHaberlesmeProtokolu,
            pilSeviyesi = dto.PilSeviyesi,
            personelId = dto.PersonelId,
            ekipmanId = dto.EkipmanId
        };

        await _takipCihaziRepository.EkleAsync(cihaz);
        await _takipCihaziRepository.DegisiklikleriKaydetAsync();

        return ApiResponse<TakipCihaziDetayDto>.Ok(
            DetayDtoyaDonustur(cihaz),
            "Takip cihazı başarıyla oluşturuldu."
        );
    }

    public async Task<ApiResponse<TakipCihaziDetayDto>> GuncelleAsync(int id, TakipCihaziGuncelleDto dto)
    {
        var cihaz = await _takipCihaziRepository.IdIleGetirAsync(id);

        if (cihaz is null)
            return ApiResponse<TakipCihaziDetayDto>.Fail("Takip cihazı bulunamadı.");

        var seriNoVarMi = await _takipCihaziRepository.SeriNoVarMiAsync(dto.TakipCihaziSeriNo, id);

        if (seriNoVarMi)
            return ApiResponse<TakipCihaziDetayDto>.Fail("Bu seri numarası başka bir takip cihazına ait.");

        cihaz.takipCihaziSeriNo = dto.TakipCihaziSeriNo;
        cihaz.takipCihaziTuru = dto.TakipCihaziTuru;
        cihaz.takipCihaziModeli = dto.TakipCihaziModeli;
        cihaz.takipCihaziDurumu = dto.TakipCihaziDurumu;
        cihaz.takipCihaziSonBaglantiZamani = dto.TakipCihaziSonBaglantiZamani;
        cihaz.takipCihaziHaberlesmeProtokolu = dto.TakipCihaziHaberlesmeProtokolu;
        cihaz.pilSeviyesi = dto.PilSeviyesi;
        cihaz.personelId = dto.PersonelId;
        cihaz.ekipmanId = dto.EkipmanId;

        await _takipCihaziRepository.DegisiklikleriKaydetAsync();

        return ApiResponse<TakipCihaziDetayDto>.Ok(
            DetayDtoyaDonustur(cihaz),
            "Takip cihazı başarıyla güncellendi."
        );
    }

    public async Task<ApiResponse<bool>> SilAsync(int id)
    {
        var cihaz = await _takipCihaziRepository.IdIleGetirAsync(id);

        if (cihaz is null)
            return ApiResponse<bool>.Fail("Takip cihazı bulunamadı.");

        cihaz.takipCihaziDurumu = "Pasif";

        await _takipCihaziRepository.DegisiklikleriKaydetAsync();

        return ApiResponse<bool>.Ok(true, "Takip cihazı pasif hale getirildi.");
    }

    private static TakipCihaziDetayDto DetayDtoyaDonustur(TakipCihazi cihaz)
    {
        return new TakipCihaziDetayDto
        {
            TakipCihaziId = cihaz.takipCihaziId,
            TakipCihaziSeriNo = cihaz.takipCihaziSeriNo,
            TakipCihaziTuru = cihaz.takipCihaziTuru,
            TakipCihaziModeli = cihaz.takipCihaziModeli,
            TakipCihaziDurumu = cihaz.takipCihaziDurumu,
            TakipCihaziSonBaglantiZamani = cihaz.takipCihaziSonBaglantiZamani,
            TakipCihaziHaberlesmeProtokolu = cihaz.takipCihaziHaberlesmeProtokolu,
            PilSeviyesi = cihaz.pilSeviyesi,
            PersonelId = cihaz.personelId,
            PersonelAdSoyad = cihaz.Personel == null
                ? null
                : $"{cihaz.Personel.personelAdi} {cihaz.Personel.personelSoyadi}",
            EkipmanId = cihaz.ekipmanId,
            EkipmanAdi = cihaz.Ekipman == null ? null : cihaz.Ekipman.ekipmanAdi,
            EkipmanDurumu = cihaz.Ekipman == null ? null : cihaz.Ekipman.durum
        };
    }
}
