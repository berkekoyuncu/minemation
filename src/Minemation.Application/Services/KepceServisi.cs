using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Application.Common;
using Minemation.Application.DTOs;
using Minemation.Application.Interfaces;
using Minemation.Domain.Entities;

namespace Minemation.Application.Services;

public class KepceServisi : IKepceServisi
{
    private readonly IKepceRepository _kepceRepository;

    public KepceServisi(IKepceRepository kepceRepository)
    {
        _kepceRepository = kepceRepository;
    }

    public async Task<ApiResponse<PagedResult<KepceListeDto>>> TumunuGetirAsync(KepceSorguParametreleri sorgu)
    {
        var kepceler = await _kepceRepository.TumunuGetirAsync();

        var filtreli = kepceler.AsQueryable();

        if (!string.IsNullOrWhiteSpace(sorgu.Arama))
        {
            var arama = sorgu.Arama.Trim().ToLower();

            filtreli = filtreli.Where(k =>
                (k.plaka ?? "").ToLower().Contains(arama) ||
                (k.Ekipman != null && (k.Ekipman.ekipmanAdi ?? "").ToLower().Contains(arama)) ||
                (k.Ekipman != null && (k.Ekipman.ekipmanMarka ?? "").ToLower().Contains(arama)) ||
                (k.Ekipman != null && (k.Ekipman.ekipmanModel ?? "").ToLower().Contains(arama)));
        }

        if (!string.IsNullOrWhiteSpace(sorgu.Plaka))
        {
            var plaka = sorgu.Plaka.Trim().ToLower();
            filtreli = filtreli.Where(k => (k.plaka ?? "").ToLower() == plaka);
        }

        filtreli = sorgu.SiralamaAlani?.ToLower() switch
        {
            "plaka" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(k => k.plaka)
                : filtreli.OrderBy(k => k.plaka),

            "yuklemekapasitesi" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(k => k.yuklemeKapasitesi)
                : filtreli.OrderBy(k => k.yuklemeKapasitesi),

            "kovakapasitesi" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(k => k.kovaKapasitesi)
                : filtreli.OrderBy(k => k.kovaKapasitesi),

            "bosaltmayuksekligi" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(k => k.bosaltmaYuksekligi)
                : filtreli.OrderBy(k => k.bosaltmaYuksekligi),

            "devrilmeyuku" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(k => k.devrilmeYuku)
                : filtreli.OrderBy(k => k.devrilmeYuku),

            _ => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(k => k.ekipmanId)
                : filtreli.OrderBy(k => k.ekipmanId)
        };

        var toplamKayitSayisi = filtreli.Count();

        var liste = filtreli
            .Skip((sorgu.SayfaNumarasi - 1) * sorgu.SayfaBoyutu)
            .Take(sorgu.SayfaBoyutu)
            .Select(k => new KepceListeDto
            {
                EkipmanId = k.ekipmanId,
                EkipmanAdi = k.Ekipman == null ? null : k.Ekipman.ekipmanAdi,
                EkipmanDurumu = k.Ekipman == null ? null : k.Ekipman.durum,
                Plaka = k.plaka,
                YuklemeKapasitesi = k.yuklemeKapasitesi,
                KovaKapasitesi = k.kovaKapasitesi,
                BosaltmaYuksekligi = k.bosaltmaYuksekligi,
                DevrilmeYuku = k.devrilmeYuku
            })
            .ToList();

        var sayfaliSonuc = PagedResult<KepceListeDto>.Create(
            liste,
            sorgu.SayfaNumarasi,
            sorgu.SayfaBoyutu,
            toplamKayitSayisi
        );

        return ApiResponse<PagedResult<KepceListeDto>>.Ok(sayfaliSonuc);
    }

    public async Task<ApiResponse<KepceDetayDto>> EkipmanIdIleGetirAsync(int ekipmanId)
    {
        var kepce = await _kepceRepository.EkipmanIdIleGetirAsync(ekipmanId);

        if (kepce is null)
            return ApiResponse<KepceDetayDto>.Fail("Kepçe kaydı bulunamadı.");

        return ApiResponse<KepceDetayDto>.Ok(DetayDtoyaDonustur(kepce));
    }

    public async Task<ApiResponse<KepceDetayDto>> OlusturAsync(KepceOlusturDto dto)
    {
        var varMi = await _kepceRepository.VarMiAsync(dto.EkipmanId);

        if (varMi)
            return ApiResponse<KepceDetayDto>.Fail("Bu ekipman için kepçe kaydı zaten oluşturulmuş.");

        var plakaVarMi = await _kepceRepository.PlakaVarMiAsync(dto.Plaka);

        if (plakaVarMi)
            return ApiResponse<KepceDetayDto>.Fail("Bu plaka ile kayıtlı kepçe zaten var.");

        var kepce = new Kepce
        {
            ekipmanId = dto.EkipmanId,
            plaka = dto.Plaka,
            yuklemeKapasitesi = dto.YuklemeKapasitesi,
            kovaKapasitesi = dto.KovaKapasitesi,
            bosaltmaYuksekligi = dto.BosaltmaYuksekligi,
            devrilmeYuku = dto.DevrilmeYuku
        };

        await _kepceRepository.EkleAsync(kepce);
        await _kepceRepository.DegisiklikleriKaydetAsync();

        return ApiResponse<KepceDetayDto>.Ok(
            DetayDtoyaDonustur(kepce),
            "Kepçe kaydı başarıyla oluşturuldu."
        );
    }

    public async Task<ApiResponse<KepceDetayDto>> GuncelleAsync(int ekipmanId, KepceGuncelleDto dto)
    {
        var kepce = await _kepceRepository.EkipmanIdIleGetirAsync(ekipmanId);

        if (kepce is null)
            return ApiResponse<KepceDetayDto>.Fail("Kepçe kaydı bulunamadı.");

        var plakaVarMi = await _kepceRepository.PlakaVarMiAsync(dto.Plaka, ekipmanId);

        if (plakaVarMi)
            return ApiResponse<KepceDetayDto>.Fail("Bu plaka başka bir kepçeye ait.");

        kepce.plaka = dto.Plaka;
        kepce.yuklemeKapasitesi = dto.YuklemeKapasitesi;
        kepce.kovaKapasitesi = dto.KovaKapasitesi;
        kepce.bosaltmaYuksekligi = dto.BosaltmaYuksekligi;
        kepce.devrilmeYuku = dto.DevrilmeYuku;

        await _kepceRepository.DegisiklikleriKaydetAsync();

        return ApiResponse<KepceDetayDto>.Ok(
            DetayDtoyaDonustur(kepce),
            "Kepçe kaydı başarıyla güncellendi."
        );
    }

    public async Task<ApiResponse<bool>> SilAsync(int ekipmanId)
    {
        var kepce = await _kepceRepository.EkipmanIdIleGetirAsync(ekipmanId);

        if (kepce is null)
            return ApiResponse<bool>.Fail("Kepçe kaydı bulunamadı.");

        if (kepce.Ekipman != null)
        {
            kepce.Ekipman.durum = "Pasif";
            await _kepceRepository.DegisiklikleriKaydetAsync();
            return ApiResponse<bool>.Ok(true, "Kepçe ekipmanı pasif hale getirildi.");
        }

        return ApiResponse<bool>.Ok(true, "Kepçe kaydı bulundu. Bu entity'de durum alanı olmadığı için pasife alma yapılmadı.");
    }

    private static KepceDetayDto DetayDtoyaDonustur(Kepce kepce)
    {
        return new KepceDetayDto
        {
            EkipmanId = kepce.ekipmanId,
            EkipmanAdi = kepce.Ekipman == null ? null : kepce.Ekipman.ekipmanAdi,
            EkipmanMarka = kepce.Ekipman == null ? null : kepce.Ekipman.ekipmanMarka,
            EkipmanModel = kepce.Ekipman == null ? null : kepce.Ekipman.ekipmanModel,
            EkipmanDurumu = kepce.Ekipman == null ? null : kepce.Ekipman.durum,
            Plaka = kepce.plaka,
            YuklemeKapasitesi = kepce.yuklemeKapasitesi,
            KovaKapasitesi = kepce.kovaKapasitesi,
            BosaltmaYuksekligi = kepce.bosaltmaYuksekligi,
            DevrilmeYuku = kepce.devrilmeYuku
        };
    }
}