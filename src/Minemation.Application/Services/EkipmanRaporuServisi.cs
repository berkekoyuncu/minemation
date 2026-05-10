using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Application.Common;
using Minemation.Application.DTOs;
using Minemation.Application.Interfaces;
using Minemation.Domain.Entities;

namespace Minemation.Application.Services;

public class EkipmanRaporuServisi : IEkipmanRaporuServisi
{
    private readonly IEkipmanRaporuRepository _ekipmanRaporuRepository;

    public EkipmanRaporuServisi(IEkipmanRaporuRepository ekipmanRaporuRepository)
    {
        _ekipmanRaporuRepository = ekipmanRaporuRepository;
    }

    public async Task<ApiResponse<PagedResult<EkipmanRaporuListeDto>>> TumunuGetirAsync(EkipmanRaporuSorguParametreleri sorgu)
    {
        var raporlar = await _ekipmanRaporuRepository.TumunuGetirAsync();

        var filtreli = raporlar.AsQueryable();

        if (!string.IsNullOrWhiteSpace(sorgu.Arama))
        {
            var arama = sorgu.Arama.Trim().ToLower();

            filtreli = filtreli.Where(er =>
                (er.ekipmanTuru ?? "").ToLower().Contains(arama) ||
                (er.Rapor != null && (er.Rapor.raporAdi ?? "").ToLower().Contains(arama)) ||
                (er.Rapor != null && (er.Rapor.raporTuru ?? "").ToLower().Contains(arama)));
        }

        if (!string.IsNullOrWhiteSpace(sorgu.EkipmanTuru))
        {
            var tur = sorgu.EkipmanTuru.Trim().ToLower();
            filtreli = filtreli.Where(er => (er.ekipmanTuru ?? "").ToLower() == tur);
        }

        filtreli = sorgu.SiralamaAlani?.ToLower() switch
        {
            "arizasayisi" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(er => er.arizaSayisi)
                : filtreli.OrderBy(er => er.arizaSayisi),

            "calismasuresi" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(er => er.calismaSuresi)
                : filtreli.OrderBy(er => er.calismaSuresi),

            "ekipmanturu" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(er => er.ekipmanTuru)
                : filtreli.OrderBy(er => er.ekipmanTuru),

            _ => sorgu.SiralamaYonu?.ToLower() == "asc"
                ? filtreli.OrderBy(er => er.raporId)
                : filtreli.OrderByDescending(er => er.raporId)
        };

        var toplamKayitSayisi = filtreli.Count();

        var liste = filtreli
            .Skip((sorgu.SayfaNumarasi - 1) * sorgu.SayfaBoyutu)
            .Take(sorgu.SayfaBoyutu)
            .Select(er => new EkipmanRaporuListeDto
            {
                RaporId = er.raporId,
                RaporAdi = er.Rapor == null ? null : er.Rapor.raporAdi,
                RaporTuru = er.Rapor == null ? null : er.Rapor.raporTuru,
                RaporOlusturmaTarihi = er.Rapor == null ? null : er.Rapor.raporOlusturmaTarihi,
                EkipmanTuru = er.ekipmanTuru,
                ArizaSayisi = er.arizaSayisi,
                CalismaSuresi = er.calismaSuresi
            })
            .ToList();

        var sayfaliSonuc = PagedResult<EkipmanRaporuListeDto>.Create(
            liste,
            sorgu.SayfaNumarasi,
            sorgu.SayfaBoyutu,
            toplamKayitSayisi
        );

        return ApiResponse<PagedResult<EkipmanRaporuListeDto>>.Ok(sayfaliSonuc);
    }

    public async Task<ApiResponse<EkipmanRaporuDetayDto>> RaporIdIleGetirAsync(int raporId)
    {
        var ekipmanRaporu = await _ekipmanRaporuRepository.RaporIdIleGetirAsync(raporId);

        if (ekipmanRaporu is null)
            return ApiResponse<EkipmanRaporuDetayDto>.Fail("Ekipman raporu bulunamadı.");

        return ApiResponse<EkipmanRaporuDetayDto>.Ok(DetayDtoyaDonustur(ekipmanRaporu));
    }

    public async Task<ApiResponse<EkipmanRaporuDetayDto>> OlusturAsync(EkipmanRaporuOlusturDto dto)
    {
        var varMi = await _ekipmanRaporuRepository.VarMiAsync(dto.RaporId);

        if (varMi)
            return ApiResponse<EkipmanRaporuDetayDto>.Fail("Bu rapor için ekipman raporu zaten oluşturulmuş.");

        var ekipmanRaporu = new EkipmanRaporu
        {
            raporId = dto.RaporId,
            ekipmanTuru = dto.EkipmanTuru,
            arizaSayisi = dto.ArizaSayisi,
            calismaSuresi = dto.CalismaSuresi
        };

        await _ekipmanRaporuRepository.EkleAsync(ekipmanRaporu);
        await _ekipmanRaporuRepository.DegisiklikleriKaydetAsync();

        return ApiResponse<EkipmanRaporuDetayDto>.Ok(
            DetayDtoyaDonustur(ekipmanRaporu),
            "Ekipman raporu başarıyla oluşturuldu."
        );
    }

    public async Task<ApiResponse<EkipmanRaporuDetayDto>> GuncelleAsync(int raporId, EkipmanRaporuGuncelleDto dto)
    {
        var ekipmanRaporu = await _ekipmanRaporuRepository.RaporIdIleGetirAsync(raporId);

        if (ekipmanRaporu is null)
            return ApiResponse<EkipmanRaporuDetayDto>.Fail("Ekipman raporu bulunamadı.");

        ekipmanRaporu.ekipmanTuru = dto.EkipmanTuru;
        ekipmanRaporu.arizaSayisi = dto.ArizaSayisi;
        ekipmanRaporu.calismaSuresi = dto.CalismaSuresi;

        await _ekipmanRaporuRepository.DegisiklikleriKaydetAsync();

        return ApiResponse<EkipmanRaporuDetayDto>.Ok(
            DetayDtoyaDonustur(ekipmanRaporu),
            "Ekipman raporu başarıyla güncellendi."
        );
    }

    public async Task<ApiResponse<bool>> SilAsync(int raporId)
    {
        var ekipmanRaporu = await _ekipmanRaporuRepository.RaporIdIleGetirAsync(raporId);

        if (ekipmanRaporu is null)
            return ApiResponse<bool>.Fail("Ekipman raporu bulunamadı.");

        return ApiResponse<bool>.Ok(true, "Ekipman raporu bulundu. Bu entity'de durum alanı olmadığı için pasife alma yapılmadı.");
    }

    private static EkipmanRaporuDetayDto DetayDtoyaDonustur(EkipmanRaporu ekipmanRaporu)
    {
        return new EkipmanRaporuDetayDto
        {
            RaporId = ekipmanRaporu.raporId,
            RaporAdi = ekipmanRaporu.Rapor == null ? null : ekipmanRaporu.Rapor.raporAdi,
            RaporTuru = ekipmanRaporu.Rapor == null ? null : ekipmanRaporu.Rapor.raporTuru,
            RaporAciklamasi = ekipmanRaporu.Rapor == null ? null : ekipmanRaporu.Rapor.raporAciklamasi,
            RaporOlusturmaTarihi = ekipmanRaporu.Rapor == null ? null : ekipmanRaporu.Rapor.raporOlusturmaTarihi,
            RaporDosyaYolu = ekipmanRaporu.Rapor == null ? null : ekipmanRaporu.Rapor.raporDosyaYolu,
            ZamanAraligi = ekipmanRaporu.Rapor == null ? null : ekipmanRaporu.Rapor.zamanAraligi,
            EkipmanTuru = ekipmanRaporu.ekipmanTuru,
            ArizaSayisi = ekipmanRaporu.arizaSayisi,
            CalismaSuresi = ekipmanRaporu.calismaSuresi
        };
    }
}
