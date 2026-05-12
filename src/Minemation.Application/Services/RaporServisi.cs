using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Application.Common;
using Minemation.Application.DTOs;
using Minemation.Application.Interfaces;
using Minemation.Domain.Entities;

namespace Minemation.Application.Services;

public class RaporServisi : IRaporServisi
{
    private readonly IRaporRepository _raporRepository;

    public RaporServisi(IRaporRepository raporRepository)
    {
        _raporRepository = raporRepository;
    }

    public async Task<ApiResponse<PagedResult<RaporListeDto>>> TumunuGetirAsync(RaporSorguParametreleri sorgu)
    {
        var raporlar = await _raporRepository.TumunuGetirAsync();

        var filtreli = raporlar.AsQueryable();

        if (!string.IsNullOrWhiteSpace(sorgu.Arama))
        {
            var arama = sorgu.Arama.Trim().ToLower();

            filtreli = filtreli.Where(r =>
                (r.raporAdi ?? "").ToLower().Contains(arama) ||
                (r.raporTuru ?? "").ToLower().Contains(arama) ||
                (r.raporAciklamasi ?? "").ToLower().Contains(arama) ||
                (r.zamanAraligi ?? "").ToLower().Contains(arama));
        }

        if (!string.IsNullOrWhiteSpace(sorgu.RaporTuru))
        {
            var tur = sorgu.RaporTuru.Trim().ToLower();
            filtreli = filtreli.Where(r => (r.raporTuru ?? "").ToLower() == tur);
        }

        if (!string.IsNullOrWhiteSpace(sorgu.ZamanAraligi))
        {
            var zamanAraligi = sorgu.ZamanAraligi.Trim().ToLower();
            filtreli = filtreli.Where(r => (r.zamanAraligi ?? "").ToLower() == zamanAraligi);
        }

        if (sorgu.PersonelId.HasValue)
        {
            filtreli = filtreli.Where(r => r.personelId == sorgu.PersonelId.Value);
        }

        if (sorgu.EkipmanId.HasValue)
        {
            filtreli = filtreli.Where(r => r.ekipmanId == sorgu.EkipmanId.Value);
        }

        filtreli = sorgu.SiralamaAlani?.ToLower() switch
        {
            "raporadi" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(r => r.raporAdi)
                : filtreli.OrderBy(r => r.raporAdi),

            "raporturu" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(r => r.raporTuru)
                : filtreli.OrderBy(r => r.raporTuru),

            _ => sorgu.SiralamaYonu?.ToLower() == "asc"
                ? filtreli.OrderBy(r => r.raporOlusturmaTarihi)
                : filtreli.OrderByDescending(r => r.raporOlusturmaTarihi)
        };

        var toplamKayitSayisi = filtreli.Count();

        var liste = filtreli
            .Skip((sorgu.SayfaNumarasi - 1) * sorgu.SayfaBoyutu)
            .Take(sorgu.SayfaBoyutu)
            .Select(r => new RaporListeDto
            {
                RaporId = r.raporId,
                RaporAdi = r.raporAdi,
                RaporTuru = r.raporTuru,
                RaporOlusturmaTarihi = r.raporOlusturmaTarihi,
                ZamanAraligi = r.zamanAraligi,
                PersonelId = r.personelId,
                PersonelAdSoyad = r.Personel == null
                    ? null
                    : $"{r.Personel.personelAdi} {r.Personel.personelSoyadi}",
                EkipmanId = r.ekipmanId,
                EkipmanAdi = r.Ekipman == null ? null : r.Ekipman.ekipmanAdi
            })
            .ToList();

        var sayfaliSonuc = PagedResult<RaporListeDto>.Create(
            liste,
            sorgu.SayfaNumarasi,
            sorgu.SayfaBoyutu,
            toplamKayitSayisi
        );

        return ApiResponse<PagedResult<RaporListeDto>>.Ok(sayfaliSonuc);
    }

    public async Task<ApiResponse<RaporDetayDto>> IdIleGetirAsync(int id)
    {
        var rapor = await _raporRepository.IdIleGetirAsync(id);

        if (rapor is null)
            return ApiResponse<RaporDetayDto>.Fail("Rapor bulunamadı.");

        return ApiResponse<RaporDetayDto>.Ok(DetayDtoyaDonustur(rapor));
    }

    public async Task<ApiResponse<RaporDetayDto>> OlusturAsync(RaporOlusturDto dto)
    {
        var rapor = new Rapor
        {
            raporAdi = dto.RaporAdi,
            raporTuru = dto.RaporTuru,
            raporOlusturmaTarihi = dto.RaporOlusturmaTarihi,
            raporAciklamasi = dto.RaporAciklamasi,
            raporDosyaYolu = dto.RaporDosyaYolu,
            zamanAraligi = dto.ZamanAraligi,
            personelId = dto.PersonelId,
            ekipmanId = dto.EkipmanId
        };

        await _raporRepository.EkleAsync(rapor);
        await _raporRepository.DegisiklikleriKaydetAsync();

        return ApiResponse<RaporDetayDto>.Ok(
            DetayDtoyaDonustur(rapor),
            "Rapor başarıyla oluşturuldu."
        );
    }

    public async Task<ApiResponse<RaporDetayDto>> GuncelleAsync(int id, RaporGuncelleDto dto)
    {
        var rapor = await _raporRepository.IdIleGetirAsync(id);

        if (rapor is null)
            return ApiResponse<RaporDetayDto>.Fail("Rapor bulunamadı.");

        rapor.raporAdi = dto.RaporAdi;
        rapor.raporTuru = dto.RaporTuru;
        rapor.raporOlusturmaTarihi = dto.RaporOlusturmaTarihi;
        rapor.raporAciklamasi = dto.RaporAciklamasi;
        rapor.raporDosyaYolu = dto.RaporDosyaYolu;
        rapor.zamanAraligi = dto.ZamanAraligi;
        rapor.personelId = dto.PersonelId;
        rapor.ekipmanId = dto.EkipmanId;

        await _raporRepository.DegisiklikleriKaydetAsync();

        return ApiResponse<RaporDetayDto>.Ok(
            DetayDtoyaDonustur(rapor),
            "Rapor başarıyla güncellendi."
        );
    }

    public async Task<ApiResponse<bool>> SilAsync(int id)
    {
        var rapor = await _raporRepository.IdIleGetirAsync(id);

        if (rapor is null)
            return ApiResponse<bool>.Fail("Rapor bulunamadı.");

        _raporRepository.Sil(rapor);
        await _raporRepository.DegisiklikleriKaydetAsync();

        return ApiResponse<bool>.Ok(true, "Rapor başarıyla silindi.");
    }

    private static RaporDetayDto DetayDtoyaDonustur(Rapor rapor)
    {
        return new RaporDetayDto
        {
            RaporId = rapor.raporId,
            RaporAdi = rapor.raporAdi,
            RaporTuru = rapor.raporTuru,
            RaporOlusturmaTarihi = rapor.raporOlusturmaTarihi,
            RaporAciklamasi = rapor.raporAciklamasi,
            RaporDosyaYolu = rapor.raporDosyaYolu,
            ZamanAraligi = rapor.zamanAraligi,
            PersonelId = rapor.personelId,
            PersonelAdSoyad = rapor.Personel == null
                ? null
                : $"{rapor.Personel.personelAdi} {rapor.Personel.personelSoyadi}",
            EkipmanId = rapor.ekipmanId,
            EkipmanAdi = rapor.Ekipman == null ? null : rapor.Ekipman.ekipmanAdi
        };
    }
}