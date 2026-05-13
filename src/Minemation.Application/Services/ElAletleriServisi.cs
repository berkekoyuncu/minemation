using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Application.Common;
using Minemation.Application.DTOs;
using Minemation.Application.Interfaces;
using Minemation.Domain.Entities;

namespace Minemation.Application.Services;

public class ElAletleriServisi : IElAletleriServisi
{
    private readonly IElAletleriRepository _elAletleriRepository;

    public ElAletleriServisi(IElAletleriRepository elAletleriRepository)
    {
        _elAletleriRepository = elAletleriRepository;
    }

    public async Task<ApiResponse<PagedResult<ElAletleriListeDto>>> TumunuGetirAsync(ElAletleriSorguParametreleri sorgu)
    {
        var elAletleri = await _elAletleriRepository.TumunuGetirAsync();

        var filtreli = elAletleri.AsQueryable();

        if (!string.IsNullOrWhiteSpace(sorgu.Arama))
        {
            var arama = sorgu.Arama.Trim().ToLower();

            filtreli = filtreli.Where(e =>
                (e.gucKaynagiTipi ?? "").ToLower().Contains(arama) ||
                (e.kullanimAmaci ?? "").ToLower().Contains(arama) ||
                (e.ekipmanAdi ?? "").ToLower().Contains(arama) ||
                (e.ekipmanMarka ?? "").ToLower().Contains(arama) ||
                (e.ekipmanModel ?? "").ToLower().Contains(arama));
        }

        if (!string.IsNullOrWhiteSpace(sorgu.GucKaynagiTipi))
        {
            var gucKaynagiTipi = sorgu.GucKaynagiTipi.Trim().ToLower();
            filtreli = filtreli.Where(e => (e.gucKaynagiTipi ?? "").ToLower() == gucKaynagiTipi);
        }

        if (!string.IsNullOrWhiteSpace(sorgu.KullanimAmaci))
        {
            var kullanimAmaci = sorgu.KullanimAmaci.Trim().ToLower();
            filtreli = filtreli.Where(e => (e.kullanimAmaci ?? "").ToLower() == kullanimAmaci);
        }

        filtreli = sorgu.SiralamaAlani?.ToLower() switch
        {
            "guckaynagitipi" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(e => e.gucKaynagiTipi)
                : filtreli.OrderBy(e => e.gucKaynagiTipi),

            "bataryakapasitesi" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(e => e.bataryaKapasitesi)
                : filtreli.OrderBy(e => e.bataryaKapasitesi),

            "kullanimamaci" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(e => e.kullanimAmaci)
                : filtreli.OrderBy(e => e.kullanimAmaci),

            _ => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(e => e.ekipmanId)
                : filtreli.OrderBy(e => e.ekipmanId)
        };

        var toplamKayitSayisi = filtreli.Count();

        var liste = filtreli
            .Skip((sorgu.SayfaNumarasi - 1) * sorgu.SayfaBoyutu)
            .Take(sorgu.SayfaBoyutu)
            .Select(e => new ElAletleriListeDto
            {
                EkipmanId = e.ekipmanId,
                EkipmanAdi = e.ekipmanAdi,
                EkipmanDurumu = e.durum,
                GucKaynagiTipi = e.gucKaynagiTipi,
                BataryaKapasitesi = e.bataryaKapasitesi,
                KullanimAmaci = e.kullanimAmaci
            })
            .ToList();

        var sayfaliSonuc = PagedResult<ElAletleriListeDto>.Create(
            liste,
            sorgu.SayfaNumarasi,
            sorgu.SayfaBoyutu,
            toplamKayitSayisi
        );

        return ApiResponse<PagedResult<ElAletleriListeDto>>.Ok(sayfaliSonuc);
    }

    public async Task<ApiResponse<ElAletleriDetayDto>> EkipmanIdIleGetirAsync(int ekipmanId)
    {
        var elAletleri = await _elAletleriRepository.EkipmanIdIleGetirAsync(ekipmanId);

        if (elAletleri is null)
            return ApiResponse<ElAletleriDetayDto>.Fail("El aletleri kaydı bulunamadı.");

        return ApiResponse<ElAletleriDetayDto>.Ok(DetayDtoyaDonustur(elAletleri));
    }

    public async Task<ApiResponse<ElAletleriDetayDto>> OlusturAsync(ElAletleriOlusturDto dto)
    {
        var varMi = await _elAletleriRepository.VarMiAsync(dto.EkipmanId);

        if (varMi)
            return ApiResponse<ElAletleriDetayDto>.Fail("Bu ekipman için el aletleri kaydı zaten oluşturulmuş.");

        var elAletleri = new ElAletleri
        {
            ekipmanId = dto.EkipmanId,
            gucKaynagiTipi = dto.GucKaynagiTipi,
            bataryaKapasitesi = dto.BataryaKapasitesi,
            kullanimAmaci = dto.KullanimAmaci
        };

        await _elAletleriRepository.EkleAsync(elAletleri);
        await _elAletleriRepository.DegisiklikleriKaydetAsync();

        return ApiResponse<ElAletleriDetayDto>.Ok(
            DetayDtoyaDonustur(elAletleri),
            "El aletleri kaydı başarıyla oluşturuldu."
        );
    }

    public async Task<ApiResponse<ElAletleriDetayDto>> GuncelleAsync(int ekipmanId, ElAletleriGuncelleDto dto)
    {
        var elAletleri = await _elAletleriRepository.EkipmanIdIleGetirAsync(ekipmanId);

        if (elAletleri is null)
            return ApiResponse<ElAletleriDetayDto>.Fail("El aletleri kaydı bulunamadı.");

        elAletleri.gucKaynagiTipi = dto.GucKaynagiTipi;
        elAletleri.bataryaKapasitesi = dto.BataryaKapasitesi;
        elAletleri.kullanimAmaci = dto.KullanimAmaci;

        await _elAletleriRepository.DegisiklikleriKaydetAsync();

        return ApiResponse<ElAletleriDetayDto>.Ok(
            DetayDtoyaDonustur(elAletleri),
            "El aletleri kaydı başarıyla güncellendi."
        );
    }

    public async Task<ApiResponse<bool>> SilAsync(int ekipmanId)
    {
        var elAletleri = await _elAletleriRepository.EkipmanIdIleGetirAsync(ekipmanId);

        if (elAletleri is null)
            return ApiResponse<bool>.Fail("El aletleri kaydı bulunamadı.");


            elAletleri.durum = "Pasif";
            await _elAletleriRepository.DegisiklikleriKaydetAsync();
            return ApiResponse<bool>.Ok(true, "El aletleri ekipmanı pasif hale getirildi.");
        

        return ApiResponse<bool>.Ok(true, "El aletleri kaydı bulundu. Bu entity'de durum alanı olmadığı için pasife alma yapılmadı.");
    }

    private static ElAletleriDetayDto DetayDtoyaDonustur(ElAletleri elAletleri)
    {
        return new ElAletleriDetayDto
        {
            EkipmanId = elAletleri.ekipmanId,
            EkipmanAdi = elAletleri.ekipmanAdi,
            EkipmanMarka = elAletleri.ekipmanMarka,
            EkipmanModel = elAletleri.ekipmanModel,
            EkipmanDurumu = elAletleri.durum,
            GucKaynagiTipi = elAletleri.gucKaynagiTipi,
            BataryaKapasitesi = elAletleri.bataryaKapasitesi,
            KullanimAmaci = elAletleri.kullanimAmaci
        };
    }
}
