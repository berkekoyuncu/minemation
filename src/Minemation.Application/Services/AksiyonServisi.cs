using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Application.Common;
using Minemation.Application.DTOs;
using Minemation.Application.Interfaces;
using Minemation.Domain.Entities;

namespace Minemation.Application.Services;

public class AksiyonServisi : IAksiyonServisi
{
    private readonly IAksiyonRepository _aksiyonRepository;

    public AksiyonServisi(IAksiyonRepository aksiyonRepository)
    {
        _aksiyonRepository = aksiyonRepository;
    }

    public async Task<ApiResponse<PagedResult<AksiyonListeDto>>> TumunuGetirAsync(AksiyonSorguParametreleri sorgu)
    {
        var aksiyonlar = await _aksiyonRepository.TumunuGetirAsync();

        var filtreli = aksiyonlar.AsQueryable();

        if (!string.IsNullOrWhiteSpace(sorgu.Arama))
        {
            var arama = sorgu.Arama.Trim().ToLower();

            filtreli = filtreli.Where(a =>
                (a.mudahaleTuru ?? "").ToLower().Contains(arama) ||
                (a.uygulananCozum ?? "").ToLower().Contains(arama) ||
                (a.Vaka != null && (a.Vaka.vakaAdi ?? "").ToLower().Contains(arama)));
        }

        if (!string.IsNullOrWhiteSpace(sorgu.MudahaleTuru))
        {
            var mudahaleTuru = sorgu.MudahaleTuru.Trim().ToLower();
            filtreli = filtreli.Where(a => (a.mudahaleTuru ?? "").ToLower() == mudahaleTuru);
        }

        if (sorgu.EkipId.HasValue)
        {
            filtreli = filtreli.Where(a => a.ekipId == sorgu.EkipId.Value);
        }

        if (sorgu.VakaId.HasValue)
        {
            filtreli = filtreli.Where(a => a.vakaId == sorgu.VakaId.Value);
        }

        filtreli = sorgu.SiralamaAlani?.ToLower() switch
        {
            "mudahaleturu" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(a => a.mudahaleTuru)
                : filtreli.OrderBy(a => a.mudahaleTuru),

            "mudahalebitissaati" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(a => a.mudahaleBitisSaati)
                : filtreli.OrderBy(a => a.mudahaleBitisSaati),

            "ekipid" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(a => a.ekipId)
                : filtreli.OrderBy(a => a.ekipId),

            "vakaid" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(a => a.vakaId)
                : filtreli.OrderBy(a => a.vakaId),

            _ => sorgu.SiralamaYonu?.ToLower() == "asc"
                ? filtreli.OrderBy(a => a.mudahaleBaslangicSaati)
                : filtreli.OrderByDescending(a => a.mudahaleBaslangicSaati)
        };

        var toplamKayitSayisi = filtreli.Count();

        var liste = filtreli
            .Skip((sorgu.SayfaNumarasi - 1) * sorgu.SayfaBoyutu)
            .Take(sorgu.SayfaBoyutu)
            .Select(a => new AksiyonListeDto
            {
                MudahaleId = a.mudahaleId,
                MudahaleBaslangicSaati = a.mudahaleBaslangicSaati,
                MudahaleBitisSaati = a.mudahaleBitisSaati,
                MudahaleTuru = a.mudahaleTuru,
                UygulananCozum = a.uygulananCozum,
                EkipId = a.ekipId,
                VakaId = a.vakaId,
                VakaAdi = a.Vaka == null ? null : a.Vaka.vakaAdi
            })
            .ToList();

        var sayfaliSonuc = PagedResult<AksiyonListeDto>.Create(
            liste,
            sorgu.SayfaNumarasi,
            sorgu.SayfaBoyutu,
            toplamKayitSayisi
        );

        return ApiResponse<PagedResult<AksiyonListeDto>>.Ok(sayfaliSonuc);
    }

    public async Task<ApiResponse<AksiyonDetayDto>> IdIleGetirAsync(int id)
    {
        var aksiyon = await _aksiyonRepository.IdIleGetirAsync(id);

        if (aksiyon is null)
            return ApiResponse<AksiyonDetayDto>.Fail("Aksiyon bulunamadı.");

        return ApiResponse<AksiyonDetayDto>.Ok(DetayDtoyaDonustur(aksiyon));
    }

    public async Task<ApiResponse<AksiyonDetayDto>> OlusturAsync(AksiyonOlusturDto dto)
    {
        var aksiyon = new Aksiyon
        {
            mudahaleBaslangicSaati = dto.MudahaleBaslangicSaati,
            mudahaleBitisSaati = dto.MudahaleBitisSaati,
            mudahaleTuru = dto.MudahaleTuru,
            uygulananCozum = dto.UygulananCozum,
            ekipId = dto.EkipId,
            vakaId = dto.VakaId
        };

        await _aksiyonRepository.EkleAsync(aksiyon);
        await _aksiyonRepository.DegisiklikleriKaydetAsync();

        return ApiResponse<AksiyonDetayDto>.Ok(
            DetayDtoyaDonustur(aksiyon),
            "Aksiyon başarıyla oluşturuldu."
        );
    }

    public async Task<ApiResponse<AksiyonDetayDto>> GuncelleAsync(int id, AksiyonGuncelleDto dto)
    {
        var aksiyon = await _aksiyonRepository.IdIleGetirAsync(id);

        if (aksiyon is null)
            return ApiResponse<AksiyonDetayDto>.Fail("Aksiyon bulunamadı.");

        aksiyon.mudahaleBaslangicSaati = dto.MudahaleBaslangicSaati;
        aksiyon.mudahaleBitisSaati = dto.MudahaleBitisSaati;
        aksiyon.mudahaleTuru = dto.MudahaleTuru;
        aksiyon.uygulananCozum = dto.UygulananCozum;
        aksiyon.ekipId = dto.EkipId;
        aksiyon.vakaId = dto.VakaId;

        await _aksiyonRepository.DegisiklikleriKaydetAsync();

        return ApiResponse<AksiyonDetayDto>.Ok(
            DetayDtoyaDonustur(aksiyon),
            "Aksiyon başarıyla güncellendi."
        );
    }

    public async Task<ApiResponse<bool>> SilAsync(int id)
    {
        var aksiyon = await _aksiyonRepository.IdIleGetirAsync(id);

        if (aksiyon is null)
            return ApiResponse<bool>.Fail("Aksiyon bulunamadı.");

        // Aksiyon entity'sinde durum alanı olmadığı için fiziksel silme yapmıyoruz.
        // Teslim için silme endpointi "bulundu" cevabı döndürüyor.
        // Gerçek silme istenirse repository'ye Remove metodu eklenebilir.
        return ApiResponse<bool>.Ok(true, "Aksiyon bulundu. Bu entity'de durum alanı olmadığı için pasife alma yapılmadı.");
    }

    private static AksiyonDetayDto DetayDtoyaDonustur(Aksiyon aksiyon)
    {
        return new AksiyonDetayDto
        {
            MudahaleId = aksiyon.mudahaleId,
            MudahaleBaslangicSaati = aksiyon.mudahaleBaslangicSaati,
            MudahaleBitisSaati = aksiyon.mudahaleBitisSaati,
            MudahaleTuru = aksiyon.mudahaleTuru,
            UygulananCozum = aksiyon.uygulananCozum,
            EkipId = aksiyon.ekipId,
            VakaId = aksiyon.vakaId,
            VakaAdi = aksiyon.Vaka == null ? null : aksiyon.Vaka.vakaAdi,
            VakaDurumu = aksiyon.Vaka == null ? null : aksiyon.Vaka.vakaDurumu,
            VakaCiddiyetSeviyesi = aksiyon.Vaka == null ? null : aksiyon.Vaka.vakaCiddiyetSeviyesi
        };
    }
}
