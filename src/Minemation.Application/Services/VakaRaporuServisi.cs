using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Application.Common;
using Minemation.Application.DTOs;
using Minemation.Application.Interfaces;
using Minemation.Domain.Entities;

namespace Minemation.Application.Services;

public class VakaRaporuServisi : IVakaRaporuServisi
{
    private readonly IVakaRaporuRepository _vakaRaporuRepository;

    public VakaRaporuServisi(IVakaRaporuRepository vakaRaporuRepository)
    {
        _vakaRaporuRepository = vakaRaporuRepository;
    }

    public async Task<ApiResponse<PagedResult<VakaRaporuListeDto>>> TumunuGetirAsync(VakaRaporuSorguParametreleri sorgu)
    {
        var raporlar = await _vakaRaporuRepository.TumunuGetirAsync();

        var filtreli = raporlar.AsQueryable();

        if (!string.IsNullOrWhiteSpace(sorgu.Arama))
        {
            var arama = sorgu.Arama.Trim().ToLower();

            filtreli = filtreli.Where(vr =>
                (vr.ciddiyetSeviyesi ?? "").ToLower().Contains(arama) ||
                (vr.Rapor != null && (vr.Rapor.raporAdi ?? "").ToLower().Contains(arama)) ||
                (vr.Rapor != null && (vr.Rapor.raporTuru ?? "").ToLower().Contains(arama)));
        }

        if (!string.IsNullOrWhiteSpace(sorgu.CiddiyetSeviyesi))
        {
            var ciddiyet = sorgu.CiddiyetSeviyesi.Trim().ToLower();
            filtreli = filtreli.Where(vr => (vr.ciddiyetSeviyesi ?? "").ToLower() == ciddiyet);
        }

        if (sorgu.PersonelId.HasValue)
        {
            filtreli = filtreli.Where(vr => vr.personelId == sorgu.PersonelId.Value);
        }

        if (sorgu.RaporlayanEkipmanId.HasValue)
        {
            filtreli = filtreli.Where(vr => vr.raporlayanEkipmanId == sorgu.RaporlayanEkipmanId.Value);
        }

        filtreli = sorgu.SiralamaAlani?.ToLower() switch
        {
            "cozumsuresi" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(vr => vr.cozumSuresi)
                : filtreli.OrderBy(vr => vr.cozumSuresi),

            "ciddiyetseviyesi" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(vr => vr.ciddiyetSeviyesi)
                : filtreli.OrderBy(vr => vr.ciddiyetSeviyesi),

            _ => sorgu.SiralamaYonu?.ToLower() == "asc"
                ? filtreli.OrderBy(vr => vr.raporId)
                : filtreli.OrderByDescending(vr => vr.raporId)
        };

        var toplamKayitSayisi = filtreli.Count();

        var liste = filtreli
            .Skip((sorgu.SayfaNumarasi - 1) * sorgu.SayfaBoyutu)
            .Take(sorgu.SayfaBoyutu)
            .Select(vr => new VakaRaporuListeDto
            {
                RaporId = vr.raporId,
                RaporAdi = vr.Rapor == null ? null : vr.Rapor.raporAdi,
                RaporTuru = vr.Rapor == null ? null : vr.Rapor.raporTuru,
                RaporOlusturmaTarihi = vr.Rapor == null ? null : vr.Rapor.raporOlusturmaTarihi,
                CiddiyetSeviyesi = vr.ciddiyetSeviyesi,
                CozumSuresi = vr.cozumSuresi,
                PersonelId = vr.personelId,
                PersonelAdSoyad = vr.Personel == null
                    ? null
                    : $"{vr.Personel.personelAdi} {vr.Personel.personelSoyadi}",
                RaporlayanEkipmanId = vr.raporlayanEkipmanId,
                RaporlayanEkipmanAdi = vr.RaporlayanEkipman == null ? null : vr.RaporlayanEkipman.ekipmanAdi
            })
            .ToList();

        var sayfaliSonuc = PagedResult<VakaRaporuListeDto>.Create(
            liste,
            sorgu.SayfaNumarasi,
            sorgu.SayfaBoyutu,
            toplamKayitSayisi
        );

        return ApiResponse<PagedResult<VakaRaporuListeDto>>.Ok(sayfaliSonuc);
    }

    public async Task<ApiResponse<VakaRaporuDetayDto>> RaporIdIleGetirAsync(int raporId)
    {
        var vakaRaporu = await _vakaRaporuRepository.RaporIdIleGetirAsync(raporId);

        if (vakaRaporu is null)
            return ApiResponse<VakaRaporuDetayDto>.Fail("Vaka raporu bulunamadı.");

        return ApiResponse<VakaRaporuDetayDto>.Ok(DetayDtoyaDonustur(vakaRaporu));
    }

    public async Task<ApiResponse<VakaRaporuDetayDto>> OlusturAsync(VakaRaporuOlusturDto dto)
    {
        var varMi = await _vakaRaporuRepository.VarMiAsync(dto.RaporId);

        if (varMi)
            return ApiResponse<VakaRaporuDetayDto>.Fail("Bu rapor için vaka raporu zaten oluşturulmuş.");

        var vakaRaporu = new VakaRaporu
        {
            raporId = dto.RaporId,
            ciddiyetSeviyesi = dto.CiddiyetSeviyesi,
            cozumSuresi = dto.CozumSuresi,
            personelId = dto.PersonelId,
            raporlayanEkipmanId = dto.RaporlayanEkipmanId
        };

        await _vakaRaporuRepository.EkleAsync(vakaRaporu);
        await _vakaRaporuRepository.DegisiklikleriKaydetAsync();

        return ApiResponse<VakaRaporuDetayDto>.Ok(
            DetayDtoyaDonustur(vakaRaporu),
            "Vaka raporu başarıyla oluşturuldu."
        );
    }

    public async Task<ApiResponse<VakaRaporuDetayDto>> GuncelleAsync(int raporId, VakaRaporuGuncelleDto dto)
    {
        var vakaRaporu = await _vakaRaporuRepository.RaporIdIleGetirAsync(raporId);

        if (vakaRaporu is null)
            return ApiResponse<VakaRaporuDetayDto>.Fail("Vaka raporu bulunamadı.");

        vakaRaporu.ciddiyetSeviyesi = dto.CiddiyetSeviyesi;
        vakaRaporu.cozumSuresi = dto.CozumSuresi;
        vakaRaporu.personelId = dto.PersonelId;
        vakaRaporu.raporlayanEkipmanId = dto.RaporlayanEkipmanId;

        await _vakaRaporuRepository.DegisiklikleriKaydetAsync();

        return ApiResponse<VakaRaporuDetayDto>.Ok(
            DetayDtoyaDonustur(vakaRaporu),
            "Vaka raporu başarıyla güncellendi."
        );
    }

    public async Task<ApiResponse<bool>> SilAsync(int raporId)
    {
        var vakaRaporu = await _vakaRaporuRepository.RaporIdIleGetirAsync(raporId);

        if (vakaRaporu is null)
            return ApiResponse<bool>.Fail("Vaka raporu bulunamadı.");

        return ApiResponse<bool>.Ok(true, "Vaka raporu bulundu. Bu entity'de durum alanı olmadığı için pasife alma yapılmadı.");
    }

    private static VakaRaporuDetayDto DetayDtoyaDonustur(VakaRaporu vakaRaporu)
    {
        return new VakaRaporuDetayDto
        {
            RaporId = vakaRaporu.raporId,
            RaporAdi = vakaRaporu.Rapor == null ? null : vakaRaporu.Rapor.raporAdi,
            RaporTuru = vakaRaporu.Rapor == null ? null : vakaRaporu.Rapor.raporTuru,
            RaporAciklamasi = vakaRaporu.Rapor == null ? null : vakaRaporu.Rapor.raporAciklamasi,
            RaporOlusturmaTarihi = vakaRaporu.Rapor == null ? null : vakaRaporu.Rapor.raporOlusturmaTarihi,
            RaporDosyaYolu = vakaRaporu.Rapor == null ? null : vakaRaporu.Rapor.raporDosyaYolu,
            ZamanAraligi = vakaRaporu.Rapor == null ? null : vakaRaporu.Rapor.zamanAraligi,
            CiddiyetSeviyesi = vakaRaporu.ciddiyetSeviyesi,
            CozumSuresi = vakaRaporu.cozumSuresi,
            PersonelId = vakaRaporu.personelId,
            PersonelAdSoyad = vakaRaporu.Personel == null
                ? null
                : $"{vakaRaporu.Personel.personelAdi} {vakaRaporu.Personel.personelSoyadi}",
            RaporlayanEkipmanId = vakaRaporu.raporlayanEkipmanId,
            RaporlayanEkipmanAdi = vakaRaporu.RaporlayanEkipman == null ? null : vakaRaporu.RaporlayanEkipman.ekipmanAdi
        };
    }
}
