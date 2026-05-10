using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Application.Common;
using Minemation.Application.DTOs;
using Minemation.Application.Interfaces;
using Minemation.Domain.Entities;

namespace Minemation.Application.Services;

public class AcilDurumIletisimServisi : IAcilDurumIletisimServisi
{
    private readonly IAcilDurumIletisimRepository _acilDurumIletisimRepository;

    public AcilDurumIletisimServisi(IAcilDurumIletisimRepository acilDurumIletisimRepository)
    {
        _acilDurumIletisimRepository = acilDurumIletisimRepository;
    }

    public async Task<ApiResponse<PagedResult<AcilDurumIletisimListeDto>>> TumunuGetirAsync(AcilDurumIletisimSorguParametreleri sorgu)
    {
        var kisiler = await _acilDurumIletisimRepository.TumunuGetirAsync();

        var filtreli = kisiler.AsQueryable();

        if (!string.IsNullOrWhiteSpace(sorgu.Arama))
        {
            var arama = sorgu.Arama.Trim().ToLower();

            filtreli = filtreli.Where(a =>
                (a.acilDurumKisileriAd ?? "").ToLower().Contains(arama) ||
                (a.acilDurumKisileriSoyad ?? "").ToLower().Contains(arama) ||
                (a.acilDurumKisileriYakinlik ?? "").ToLower().Contains(arama) ||
                (a.acilDurumKisileriTelNo ?? "").ToLower().Contains(arama) ||
                (a.Personel != null && ((a.Personel.personelAdi ?? "") + " " + (a.Personel.personelSoyadi ?? "")).ToLower().Contains(arama)));
        }

        if (!string.IsNullOrWhiteSpace(sorgu.Yakinlik))
        {
            var yakinlik = sorgu.Yakinlik.Trim().ToLower();
            filtreli = filtreli.Where(a => (a.acilDurumKisileriYakinlik ?? "").ToLower() == yakinlik);
        }

        if (sorgu.PersonelId.HasValue)
        {
            filtreli = filtreli.Where(a => a.personelId == sorgu.PersonelId.Value);
        }

        filtreli = sorgu.SiralamaAlani?.ToLower() switch
        {
            "ad" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(a => a.acilDurumKisileriAd)
                : filtreli.OrderBy(a => a.acilDurumKisileriAd),

            "soyad" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(a => a.acilDurumKisileriSoyad)
                : filtreli.OrderBy(a => a.acilDurumKisileriSoyad),

            "yakinlik" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(a => a.acilDurumKisileriYakinlik)
                : filtreli.OrderBy(a => a.acilDurumKisileriYakinlik),

            "personelid" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(a => a.personelId)
                : filtreli.OrderBy(a => a.personelId),

            _ => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(a => a.acilDurumKisisiId)
                : filtreli.OrderBy(a => a.acilDurumKisisiId)
        };

        var toplamKayitSayisi = filtreli.Count();

        var liste = filtreli
            .Skip((sorgu.SayfaNumarasi - 1) * sorgu.SayfaBoyutu)
            .Take(sorgu.SayfaBoyutu)
            .Select(a => new AcilDurumIletisimListeDto
            {
                AcilDurumKisisiId = a.acilDurumKisisiId,
                AcilDurumKisileriAd = a.acilDurumKisileriAd,
                AcilDurumKisileriSoyad = a.acilDurumKisileriSoyad,
                AcilDurumKisileriYakinlik = a.acilDurumKisileriYakinlik,
                AcilDurumKisileriTelNo = a.acilDurumKisileriTelNo,
                PersonelId = a.personelId,
                PersonelAdSoyad = a.Personel == null
                    ? null
                    : $"{a.Personel.personelAdi} {a.Personel.personelSoyadi}"
            })
            .ToList();

        var sayfaliSonuc = PagedResult<AcilDurumIletisimListeDto>.Create(
            liste,
            sorgu.SayfaNumarasi,
            sorgu.SayfaBoyutu,
            toplamKayitSayisi
        );

        return ApiResponse<PagedResult<AcilDurumIletisimListeDto>>.Ok(sayfaliSonuc);
    }

    public async Task<ApiResponse<AcilDurumIletisimDetayDto>> IdIleGetirAsync(int id)
    {
        var kisi = await _acilDurumIletisimRepository.IdIleGetirAsync(id);

        if (kisi is null)
            return ApiResponse<AcilDurumIletisimDetayDto>.Fail("Acil durum iletişim kişisi bulunamadı.");

        return ApiResponse<AcilDurumIletisimDetayDto>.Ok(DetayDtoyaDonustur(kisi));
    }

    public async Task<ApiResponse<AcilDurumIletisimDetayDto>> OlusturAsync(AcilDurumIletisimOlusturDto dto)
    {
        var kisi = new AcilDurumIletisim
        {
            acilDurumKisileriAd = dto.AcilDurumKisileriAd,
            acilDurumKisileriSoyad = dto.AcilDurumKisileriSoyad,
            acilDurumKisileriYakinlik = dto.AcilDurumKisileriYakinlik,
            acilDurumKisileriTelNo = dto.AcilDurumKisileriTelNo,
            personelId = dto.PersonelId
        };

        await _acilDurumIletisimRepository.EkleAsync(kisi);
        await _acilDurumIletisimRepository.DegisiklikleriKaydetAsync();

        return ApiResponse<AcilDurumIletisimDetayDto>.Ok(
            DetayDtoyaDonustur(kisi),
            "Acil durum iletişim kişisi başarıyla oluşturuldu."
        );
    }

    public async Task<ApiResponse<AcilDurumIletisimDetayDto>> GuncelleAsync(int id, AcilDurumIletisimGuncelleDto dto)
    {
        var kisi = await _acilDurumIletisimRepository.IdIleGetirAsync(id);

        if (kisi is null)
            return ApiResponse<AcilDurumIletisimDetayDto>.Fail("Acil durum iletişim kişisi bulunamadı.");

        kisi.acilDurumKisileriAd = dto.AcilDurumKisileriAd;
        kisi.acilDurumKisileriSoyad = dto.AcilDurumKisileriSoyad;
        kisi.acilDurumKisileriYakinlik = dto.AcilDurumKisileriYakinlik;
        kisi.acilDurumKisileriTelNo = dto.AcilDurumKisileriTelNo;
        kisi.personelId = dto.PersonelId;

        await _acilDurumIletisimRepository.DegisiklikleriKaydetAsync();

        return ApiResponse<AcilDurumIletisimDetayDto>.Ok(
            DetayDtoyaDonustur(kisi),
            "Acil durum iletişim kişisi başarıyla güncellendi."
        );
    }

    public async Task<ApiResponse<bool>> SilAsync(int id)
    {
        var kisi = await _acilDurumIletisimRepository.IdIleGetirAsync(id);

        if (kisi is null)
            return ApiResponse<bool>.Fail("Acil durum iletişim kişisi bulunamadı.");

        return ApiResponse<bool>.Ok(true, "Acil durum iletişim kişisi bulundu. Bu entity'de durum alanı olmadığı için pasife alma yapılmadı.");
    }

    private static AcilDurumIletisimDetayDto DetayDtoyaDonustur(AcilDurumIletisim kisi)
    {
        return new AcilDurumIletisimDetayDto
        {
            AcilDurumKisisiId = kisi.acilDurumKisisiId,
            AcilDurumKisileriAd = kisi.acilDurumKisileriAd,
            AcilDurumKisileriSoyad = kisi.acilDurumKisileriSoyad,
            AcilDurumKisileriYakinlik = kisi.acilDurumKisileriYakinlik,
            AcilDurumKisileriTelNo = kisi.acilDurumKisileriTelNo,
            PersonelId = kisi.personelId,
            PersonelAdSoyad = kisi.Personel == null
                ? null
                : $"{kisi.Personel.personelAdi} {kisi.Personel.personelSoyadi}",
            PersonelTelNo = kisi.Personel == null ? null : kisi.Personel.telNo,
            PersonelDurumu = kisi.Personel == null ? null : kisi.Personel.personelDurumu
        };
    }
}