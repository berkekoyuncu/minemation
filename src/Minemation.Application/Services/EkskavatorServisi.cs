using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Application.Common;
using Minemation.Application.DTOs;
using Minemation.Application.Interfaces;
using Minemation.Domain.Entities;

namespace Minemation.Application.Services;

public class EkskavatorServisi : IEkskavatorServisi
{
    private readonly IEkskavatorRepository _ekskavatorRepository;

    public EkskavatorServisi(IEkskavatorRepository ekskavatorRepository)
    {
        _ekskavatorRepository = ekskavatorRepository;
    }

    public async Task<ApiResponse<PagedResult<EkskavatorListeDto>>> TumunuGetirAsync(EkskavatorSorguParametreleri sorgu)
    {
        var ekskavatorler = await _ekskavatorRepository.TumunuGetirAsync();

        var filtreli = ekskavatorler.AsQueryable();

        if (!string.IsNullOrWhiteSpace(sorgu.Arama))
        {
            var arama = sorgu.Arama.Trim().ToLower();

            filtreli = filtreli.Where(e =>
                (e.plaka ?? "").ToLower().Contains(arama) ||
                (e.paletTipi ?? "").ToLower().Contains(arama) ||
                (e.Ekipman != null && (e.Ekipman.ekipmanAdi ?? "").ToLower().Contains(arama)) ||
                (e.Ekipman != null && (e.Ekipman.ekipmanMarka ?? "").ToLower().Contains(arama)) ||
                (e.Ekipman != null && (e.Ekipman.ekipmanModel ?? "").ToLower().Contains(arama)));
        }

        if (!string.IsNullOrWhiteSpace(sorgu.Plaka))
        {
            var plaka = sorgu.Plaka.Trim().ToLower();
            filtreli = filtreli.Where(e => (e.plaka ?? "").ToLower() == plaka);
        }

        if (!string.IsNullOrWhiteSpace(sorgu.PaletTipi))
        {
            var paletTipi = sorgu.PaletTipi.Trim().ToLower();
            filtreli = filtreli.Where(e => (e.paletTipi ?? "").ToLower() == paletTipi);
        }

        filtreli = sorgu.SiralamaAlani?.ToLower() switch
        {
            "plaka" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(e => e.plaka)
                : filtreli.OrderBy(e => e.plaka),

            "kovakapasitesi" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(e => e.kovaKapasitesi)
                : filtreli.OrderBy(e => e.kovaKapasitesi),

            "motorgucu" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(e => e.motorGucu)
                : filtreli.OrderBy(e => e.motorGucu),

            "maksimumkaziderinligi" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(e => e.maksimumKaziDerinligi)
                : filtreli.OrderBy(e => e.maksimumKaziDerinligi),

            "bomuzunlugu" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(e => e.bomUzunlugu)
                : filtreli.OrderBy(e => e.bomUzunlugu),

            _ => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(e => e.ekipmanId)
                : filtreli.OrderBy(e => e.ekipmanId)
        };

        var toplamKayitSayisi = filtreli.Count();

        var liste = filtreli
            .Skip((sorgu.SayfaNumarasi - 1) * sorgu.SayfaBoyutu)
            .Take(sorgu.SayfaBoyutu)
            .Select(e => new EkskavatorListeDto
            {
                EkipmanId = e.ekipmanId,
                EkipmanAdi = e.Ekipman == null ? null : e.Ekipman.ekipmanAdi,
                EkipmanDurumu = e.Ekipman == null ? null : e.Ekipman.durum,
                Plaka = e.plaka,
                KovaKapasitesi = e.kovaKapasitesi,
                MotorGucu = e.motorGucu,
                MaksimumKaziDerinligi = e.maksimumKaziDerinligi,
                PaletTipi = e.paletTipi,
                BomUzunlugu = e.bomUzunlugu
            })
            .ToList();

        var sayfaliSonuc = PagedResult<EkskavatorListeDto>.Create(
            liste,
            sorgu.SayfaNumarasi,
            sorgu.SayfaBoyutu,
            toplamKayitSayisi
        );

        return ApiResponse<PagedResult<EkskavatorListeDto>>.Ok(sayfaliSonuc);
    }

    public async Task<ApiResponse<EkskavatorDetayDto>> EkipmanIdIleGetirAsync(int ekipmanId)
    {
        var ekskavator = await _ekskavatorRepository.EkipmanIdIleGetirAsync(ekipmanId);

        if (ekskavator is null)
            return ApiResponse<EkskavatorDetayDto>.Fail("Ekskavatör kaydı bulunamadı.");

        return ApiResponse<EkskavatorDetayDto>.Ok(DetayDtoyaDonustur(ekskavator));
    }

    public async Task<ApiResponse<EkskavatorDetayDto>> OlusturAsync(EkskavatorOlusturDto dto)
    {
        var varMi = await _ekskavatorRepository.VarMiAsync(dto.EkipmanId);

        if (varMi)
            return ApiResponse<EkskavatorDetayDto>.Fail("Bu ekipman için ekskavatör kaydı zaten oluşturulmuş.");

        var plakaVarMi = await _ekskavatorRepository.PlakaVarMiAsync(dto.Plaka);

        if (plakaVarMi)
            return ApiResponse<EkskavatorDetayDto>.Fail("Bu plaka ile kayıtlı ekskavatör zaten var.");

        var ekskavator = new Ekskavator
        {
            ekipmanId = dto.EkipmanId,
            plaka = dto.Plaka,
            kovaKapasitesi = dto.KovaKapasitesi,
            motorGucu = dto.MotorGucu,
            maksimumKaziDerinligi = dto.MaksimumKaziDerinligi,
            paletTipi = dto.PaletTipi,
            bomUzunlugu = dto.BomUzunlugu
        };

        await _ekskavatorRepository.EkleAsync(ekskavator);
        await _ekskavatorRepository.DegisiklikleriKaydetAsync();

        return ApiResponse<EkskavatorDetayDto>.Ok(
            DetayDtoyaDonustur(ekskavator),
            "Ekskavatör kaydı başarıyla oluşturuldu."
        );
    }

    public async Task<ApiResponse<EkskavatorDetayDto>> GuncelleAsync(int ekipmanId, EkskavatorGuncelleDto dto)
    {
        var ekskavator = await _ekskavatorRepository.EkipmanIdIleGetirAsync(ekipmanId);

        if (ekskavator is null)
            return ApiResponse<EkskavatorDetayDto>.Fail("Ekskavatör kaydı bulunamadı.");

        var plakaVarMi = await _ekskavatorRepository.PlakaVarMiAsync(dto.Plaka, ekipmanId);

        if (plakaVarMi)
            return ApiResponse<EkskavatorDetayDto>.Fail("Bu plaka başka bir ekskavatöre ait.");

        ekskavator.plaka = dto.Plaka;
        ekskavator.kovaKapasitesi = dto.KovaKapasitesi;
        ekskavator.motorGucu = dto.MotorGucu;
        ekskavator.maksimumKaziDerinligi = dto.MaksimumKaziDerinligi;
        ekskavator.paletTipi = dto.PaletTipi;
        ekskavator.bomUzunlugu = dto.BomUzunlugu;

        await _ekskavatorRepository.DegisiklikleriKaydetAsync();

        return ApiResponse<EkskavatorDetayDto>.Ok(
            DetayDtoyaDonustur(ekskavator),
            "Ekskavatör kaydı başarıyla güncellendi."
        );
    }

    public async Task<ApiResponse<bool>> SilAsync(int ekipmanId)
    {
        var ekskavator = await _ekskavatorRepository.EkipmanIdIleGetirAsync(ekipmanId);

        if (ekskavator is null)
            return ApiResponse<bool>.Fail("Ekskavatör kaydı bulunamadı.");

        if (ekskavator.Ekipman != null)
        {
            ekskavator.Ekipman.durum = "Pasif";
            await _ekskavatorRepository.DegisiklikleriKaydetAsync();
            return ApiResponse<bool>.Ok(true, "Ekskavatör ekipmanı pasif hale getirildi.");
        }

        return ApiResponse<bool>.Ok(true, "Ekskavatör kaydı bulundu. Bu entity'de durum alanı olmadığı için pasife alma yapılmadı.");
    }

    private static EkskavatorDetayDto DetayDtoyaDonustur(Ekskavator ekskavator)
    {
        return new EkskavatorDetayDto
        {
            EkipmanId = ekskavator.ekipmanId,
            EkipmanAdi = ekskavator.Ekipman == null ? null : ekskavator.Ekipman.ekipmanAdi,
            EkipmanMarka = ekskavator.Ekipman == null ? null : ekskavator.Ekipman.ekipmanMarka,
            EkipmanModel = ekskavator.Ekipman == null ? null : ekskavator.Ekipman.ekipmanModel,
            EkipmanDurumu = ekskavator.Ekipman == null ? null : ekskavator.Ekipman.durum,
            Plaka = ekskavator.plaka,
            KovaKapasitesi = ekskavator.kovaKapasitesi,
            MotorGucu = ekskavator.motorGucu,
            MaksimumKaziDerinligi = ekskavator.maksimumKaziDerinligi,
            PaletTipi = ekskavator.paletTipi,
            BomUzunlugu = ekskavator.bomUzunlugu
        };
    }
}
