using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Application.Common;
using Minemation.Application.DTOs;
using Minemation.Application.Interfaces;
using Minemation.Domain.Entities;

namespace Minemation.Application.Services;

public class HafriyatServisi : IHafriyatServisi
{
    private readonly IHafriyatRepository _hafriyatRepository;

    public HafriyatServisi(IHafriyatRepository hafriyatRepository)
    {
        _hafriyatRepository = hafriyatRepository;
    }

    public async Task<ApiResponse<PagedResult<HafriyatListeDto>>> TumunuGetirAsync(HafriyatSorguParametreleri sorgu)
    {
        var hafriyatlar = await _hafriyatRepository.TumunuGetirAsync();

        var filtreli = hafriyatlar.AsQueryable();

        if (!string.IsNullOrWhiteSpace(sorgu.Arama))
        {
            var arama = sorgu.Arama.Trim().ToLower();

            filtreli = filtreli.Where(h =>
                (h.plaka ?? "").ToLower().Contains(arama) ||
                (h.Ekipman != null && (h.Ekipman.ekipmanAdi ?? "").ToLower().Contains(arama)) ||
                (h.Ekipman != null && (h.Ekipman.ekipmanMarka ?? "").ToLower().Contains(arama)) ||
                (h.Ekipman != null && (h.Ekipman.ekipmanModel ?? "").ToLower().Contains(arama)));
        }

        if (!string.IsNullOrWhiteSpace(sorgu.Plaka))
        {
            var plaka = sorgu.Plaka.Trim().ToLower();
            filtreli = filtreli.Where(h => (h.plaka ?? "").ToLower() == plaka);
        }

        filtreli = sorgu.SiralamaAlani?.ToLower() switch
        {
            "plaka" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(h => h.plaka)
                : filtreli.OrderBy(h => h.plaka),

            "damperhacmi" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(h => h.damperHacmi)
                : filtreli.OrderBy(h => h.damperHacmi),

            "azamiyukagirligi" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(h => h.azamiYukAgirligi)
                : filtreli.OrderBy(h => h.azamiYukAgirligi),

            "dingilsayisi" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(h => h.dingilSayisi)
                : filtreli.OrderBy(h => h.dingilSayisi),

            _ => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(h => h.ekipmanId)
                : filtreli.OrderBy(h => h.ekipmanId)
        };

        var toplamKayitSayisi = filtreli.Count();

        var liste = filtreli
            .Skip((sorgu.SayfaNumarasi - 1) * sorgu.SayfaBoyutu)
            .Take(sorgu.SayfaBoyutu)
            .Select(h => new HafriyatListeDto
            {
                EkipmanId = h.ekipmanId,
                EkipmanAdi = h.Ekipman == null ? null : h.Ekipman.ekipmanAdi,
                EkipmanDurumu = h.Ekipman == null ? null : h.Ekipman.durum,
                Plaka = h.plaka,
                DamperHacmi = h.damperHacmi,
                AzamiYukAgirligi = h.azamiYukAgirligi,
                DingilSayisi = h.dingilSayisi
            })
            .ToList();

        var sayfaliSonuc = PagedResult<HafriyatListeDto>.Create(
            liste,
            sorgu.SayfaNumarasi,
            sorgu.SayfaBoyutu,
            toplamKayitSayisi
        );

        return ApiResponse<PagedResult<HafriyatListeDto>>.Ok(sayfaliSonuc);
    }

    public async Task<ApiResponse<HafriyatDetayDto>> EkipmanIdIleGetirAsync(int ekipmanId)
    {
        var hafriyat = await _hafriyatRepository.EkipmanIdIleGetirAsync(ekipmanId);

        if (hafriyat is null)
            return ApiResponse<HafriyatDetayDto>.Fail("Hafriyat kaydı bulunamadı.");

        return ApiResponse<HafriyatDetayDto>.Ok(DetayDtoyaDonustur(hafriyat));
    }

    public async Task<ApiResponse<HafriyatDetayDto>> OlusturAsync(HafriyatOlusturDto dto)
    {
        var varMi = await _hafriyatRepository.VarMiAsync(dto.EkipmanId);

        if (varMi)
            return ApiResponse<HafriyatDetayDto>.Fail("Bu ekipman için hafriyat kaydı zaten oluşturulmuş.");

        var plakaVarMi = await _hafriyatRepository.PlakaVarMiAsync(dto.Plaka);

        if (plakaVarMi)
            return ApiResponse<HafriyatDetayDto>.Fail("Bu plaka ile kayıtlı hafriyat aracı zaten var.");

        var hafriyat = new Hafriyat
        {
            ekipmanId = dto.EkipmanId,
            plaka = dto.Plaka,
            damperHacmi = dto.DamperHacmi,
            azamiYukAgirligi = dto.AzamiYukAgirligi,
            dingilSayisi = dto.DingilSayisi
        };

        await _hafriyatRepository.EkleAsync(hafriyat);
        await _hafriyatRepository.DegisiklikleriKaydetAsync();

        return ApiResponse<HafriyatDetayDto>.Ok(
            DetayDtoyaDonustur(hafriyat),
            "Hafriyat kaydı başarıyla oluşturuldu."
        );
    }

    public async Task<ApiResponse<HafriyatDetayDto>> GuncelleAsync(int ekipmanId, HafriyatGuncelleDto dto)
    {
        var hafriyat = await _hafriyatRepository.EkipmanIdIleGetirAsync(ekipmanId);

        if (hafriyat is null)
            return ApiResponse<HafriyatDetayDto>.Fail("Hafriyat kaydı bulunamadı.");

        var plakaVarMi = await _hafriyatRepository.PlakaVarMiAsync(dto.Plaka, ekipmanId);

        if (plakaVarMi)
            return ApiResponse<HafriyatDetayDto>.Fail("Bu plaka başka bir hafriyat aracına ait.");

        hafriyat.plaka = dto.Plaka;
        hafriyat.damperHacmi = dto.DamperHacmi;
        hafriyat.azamiYukAgirligi = dto.AzamiYukAgirligi;
        hafriyat.dingilSayisi = dto.DingilSayisi;

        await _hafriyatRepository.DegisiklikleriKaydetAsync();

        return ApiResponse<HafriyatDetayDto>.Ok(
            DetayDtoyaDonustur(hafriyat),
            "Hafriyat kaydı başarıyla güncellendi."
        );
    }

    public async Task<ApiResponse<bool>> SilAsync(int ekipmanId)
    {
        var hafriyat = await _hafriyatRepository.EkipmanIdIleGetirAsync(ekipmanId);

        if (hafriyat is null)
            return ApiResponse<bool>.Fail("Hafriyat kaydı bulunamadı.");

        if (hafriyat.Ekipman != null)
        {
            hafriyat.Ekipman.durum = "Pasif";
            await _hafriyatRepository.DegisiklikleriKaydetAsync();
            return ApiResponse<bool>.Ok(true, "Hafriyat ekipmanı pasif hale getirildi.");
        }

        return ApiResponse<bool>.Ok(true, "Hafriyat kaydı bulundu. Bu entity'de durum alanı olmadığı için pasife alma yapılmadı.");
    }

    private static HafriyatDetayDto DetayDtoyaDonustur(Hafriyat hafriyat)
    {
        return new HafriyatDetayDto
        {
            EkipmanId = hafriyat.ekipmanId,
            EkipmanAdi = hafriyat.Ekipman == null ? null : hafriyat.Ekipman.ekipmanAdi,
            EkipmanMarka = hafriyat.Ekipman == null ? null : hafriyat.Ekipman.ekipmanMarka,
            EkipmanModel = hafriyat.Ekipman == null ? null : hafriyat.Ekipman.ekipmanModel,
            EkipmanDurumu = hafriyat.Ekipman == null ? null : hafriyat.Ekipman.durum,
            Plaka = hafriyat.plaka,
            DamperHacmi = hafriyat.damperHacmi,
            AzamiYukAgirligi = hafriyat.azamiYukAgirligi,
            DingilSayisi = hafriyat.dingilSayisi
        };
    }
}