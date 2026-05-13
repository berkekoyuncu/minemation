using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Application.Common;
using Minemation.Application.DTOs;
using Minemation.Application.Interfaces;
using Minemation.Domain.Entities;

namespace Minemation.Application.Services;

public class KiriciServisi : IKiriciServisi
{
    private readonly IKiriciRepository _kiriciRepository;

    public KiriciServisi(IKiriciRepository kiriciRepository)
    {
        _kiriciRepository = kiriciRepository;
    }

    public async Task<ApiResponse<PagedResult<KiriciListeDto>>> TumunuGetirAsync(KiriciSorguParametreleri sorgu)
    {
        var kiricilar = await _kiriciRepository.TumunuGetirAsync();

        var filtreli = kiricilar.AsQueryable();

        if (!string.IsNullOrWhiteSpace(sorgu.Arama))
        {
            var arama = sorgu.Arama.Trim().ToLower();

            filtreli = filtreli.Where(k =>
                (k.ucTipi ?? "").ToLower().Contains(arama) ||
                (k.gerekenYagDebisi ?? "").ToLower().Contains(arama) ||
                (k.ekipmanAdi ?? "").ToLower().Contains(arama) ||
                (k.ekipmanMarka ?? "").ToLower().Contains(arama) ||
                (k.ekipmanModel ?? "").ToLower().Contains(arama));
        }

        if (!string.IsNullOrWhiteSpace(sorgu.UcTipi))
        {
            var ucTipi = sorgu.UcTipi.Trim().ToLower();
            filtreli = filtreli.Where(k => (k.ucTipi ?? "").ToLower() == ucTipi);
        }

        filtreli = sorgu.SiralamaAlani?.ToLower() switch
        {
            "darbeenerjisi" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(k => k.darbeEnerjisi)
                : filtreli.OrderBy(k => k.darbeEnerjisi),

            "dakikadakidarbeyayisi" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(k => k.dakikadakiDarbeSayisi)
                : filtreli.OrderBy(k => k.dakikadakiDarbeSayisi),

            "calismabasinci" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(k => k.calismaBasinci)
                : filtreli.OrderBy(k => k.calismaBasinci),

            "uctipi" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(k => k.ucTipi)
                : filtreli.OrderBy(k => k.ucTipi),

            _ => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(k => k.ekipmanId)
                : filtreli.OrderBy(k => k.ekipmanId)
        };

        var toplamKayitSayisi = filtreli.Count();

        var liste = filtreli
            .Skip((sorgu.SayfaNumarasi - 1) * sorgu.SayfaBoyutu)
            .Take(sorgu.SayfaBoyutu)
            .Select(k => new KiriciListeDto
            {
                EkipmanId = k.ekipmanId,
                EkipmanAdi = k.ekipmanAdi,
                EkipmanDurumu = k.durum,
                DarbeEnerjisi = k.darbeEnerjisi,
                DakikadakiDarbeSayisi = k.dakikadakiDarbeSayisi,
                CalismaBasinci = k.calismaBasinci,
                UcTipi = k.ucTipi,
                GerekenYagDebisi = k.gerekenYagDebisi
            })
            .ToList();

        var sayfaliSonuc = PagedResult<KiriciListeDto>.Create(
            liste,
            sorgu.SayfaNumarasi,
            sorgu.SayfaBoyutu,
            toplamKayitSayisi
        );

        return ApiResponse<PagedResult<KiriciListeDto>>.Ok(sayfaliSonuc);
    }

    public async Task<ApiResponse<KiriciDetayDto>> EkipmanIdIleGetirAsync(int ekipmanId)
    {
        var kirici = await _kiriciRepository.EkipmanIdIleGetirAsync(ekipmanId);

        if (kirici is null)
            return ApiResponse<KiriciDetayDto>.Fail("Kırıcı kaydı bulunamadı.");

        return ApiResponse<KiriciDetayDto>.Ok(DetayDtoyaDonustur(kirici));
    }

    public async Task<ApiResponse<KiriciDetayDto>> OlusturAsync(KiriciOlusturDto dto)
    {
        var varMi = await _kiriciRepository.VarMiAsync(dto.EkipmanId);

        if (varMi)
            return ApiResponse<KiriciDetayDto>.Fail("Bu ekipman için kırıcı kaydı zaten oluşturulmuş.");

        var kirici = new Kirici
        {
            ekipmanId = dto.EkipmanId,
            darbeEnerjisi = dto.DarbeEnerjisi,
            dakikadakiDarbeSayisi = dto.DakikadakiDarbeSayisi,
            calismaBasinci = dto.CalismaBasinci,
            ucTipi = dto.UcTipi,
            gerekenYagDebisi = dto.GerekenYagDebisi
        };

        await _kiriciRepository.EkleAsync(kirici);
        await _kiriciRepository.DegisiklikleriKaydetAsync();

        return ApiResponse<KiriciDetayDto>.Ok(
            DetayDtoyaDonustur(kirici),
            "Kırıcı kaydı başarıyla oluşturuldu."
        );
    }

    public async Task<ApiResponse<KiriciDetayDto>> GuncelleAsync(int ekipmanId, KiriciGuncelleDto dto)
    {
        var kirici = await _kiriciRepository.EkipmanIdIleGetirAsync(ekipmanId);

        if (kirici is null)
            return ApiResponse<KiriciDetayDto>.Fail("Kırıcı kaydı bulunamadı.");

        kirici.darbeEnerjisi = dto.DarbeEnerjisi;
        kirici.dakikadakiDarbeSayisi = dto.DakikadakiDarbeSayisi;
        kirici.calismaBasinci = dto.CalismaBasinci;
        kirici.ucTipi = dto.UcTipi;
        kirici.gerekenYagDebisi = dto.GerekenYagDebisi;

        await _kiriciRepository.DegisiklikleriKaydetAsync();

        return ApiResponse<KiriciDetayDto>.Ok(
            DetayDtoyaDonustur(kirici),
            "Kırıcı kaydı başarıyla güncellendi."
        );
    }

    public async Task<ApiResponse<bool>> SilAsync(int ekipmanId)
    {
        var kirici = await _kiriciRepository.EkipmanIdIleGetirAsync(ekipmanId);

        if (kirici is null)
            return ApiResponse<bool>.Fail("Kırıcı kaydı bulunamadı.");

            kirici.durum = "Pasif";
            await _kiriciRepository.DegisiklikleriKaydetAsync();
            return ApiResponse<bool>.Ok(true, "Kırıcı ekipmanı pasif hale getirildi.");


        return ApiResponse<bool>.Ok(true, "Kırıcı kaydı bulundu. Bu entity'de durum alanı olmadığı için pasife alma yapılmadı.");
    }

    private static KiriciDetayDto DetayDtoyaDonustur(Kirici kirici)
    {
        return new KiriciDetayDto
        {
            EkipmanId = kirici.ekipmanId,
            EkipmanAdi = kirici.ekipmanAdi,
            EkipmanMarka = kirici.ekipmanMarka,
            EkipmanModel = kirici.ekipmanModel,
            EkipmanDurumu = kirici.durum,
            DarbeEnerjisi = kirici.darbeEnerjisi,
            DakikadakiDarbeSayisi = kirici.dakikadakiDarbeSayisi,
            CalismaBasinci = kirici.calismaBasinci,
            UcTipi = kirici.ucTipi,
            GerekenYagDebisi = kirici.gerekenYagDebisi
        };
    }
}
