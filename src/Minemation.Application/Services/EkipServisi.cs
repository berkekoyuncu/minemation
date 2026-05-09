using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Application.Common;
using Minemation.Application.DTOs;
using Minemation.Application.Interfaces;
using Minemation.Domain.Entities;

namespace Minemation.Application.Services;

public class EkipServisi : IEkipServisi
{
    private readonly IEkipRepository _ekipRepository;

    public EkipServisi(IEkipRepository ekipRepository)
    {
        _ekipRepository = ekipRepository;
    }

    public async Task<ApiResponse<PagedResult<EkipListeDto>>> TumunuGetirAsync(EkipSorguParametreleri sorgu)
    {
        var ekipler = await _ekipRepository.TumunuGetirAsync();

        var filtreli = ekipler.AsQueryable();

        if (!string.IsNullOrWhiteSpace(sorgu.Arama))
        {
            var arama = sorgu.Arama.Trim().ToLower();

            filtreli = filtreli.Where(e =>
                (e.personelGorevi ?? "").ToLower().Contains(arama) ||
                (e.durum ?? "").ToLower().Contains(arama) ||
                (e.Personel != null && ((e.Personel.personelAdi ?? "") + " " + (e.Personel.personelSoyadi ?? "")).ToLower().Contains(arama)) ||
                (e.Vardiya != null && (e.Vardiya.vardiyaAdi ?? "").ToLower().Contains(arama)));
        }

        if (!string.IsNullOrWhiteSpace(sorgu.Durum))
        {
            var durum = sorgu.Durum.Trim().ToLower();
            filtreli = filtreli.Where(e => (e.durum ?? "").ToLower() == durum);
        }

        if (!string.IsNullOrWhiteSpace(sorgu.PersonelGorevi))
        {
            var gorev = sorgu.PersonelGorevi.Trim().ToLower();
            filtreli = filtreli.Where(e => (e.personelGorevi ?? "").ToLower() == gorev);
        }

        if (sorgu.PersonelId.HasValue)
        {
            filtreli = filtreli.Where(e => e.personelId == sorgu.PersonelId.Value);
        }

        if (sorgu.VardiyaId.HasValue)
        {
            filtreli = filtreli.Where(e => e.vardiyaId == sorgu.VardiyaId.Value);
        }

        filtreli = sorgu.SiralamaAlani?.ToLower() switch
        {
            "ekipuyesayisi" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(e => e.ekipUyeSayisi)
                : filtreli.OrderBy(e => e.ekipUyeSayisi),

            "personelgorevi" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(e => e.personelGorevi)
                : filtreli.OrderBy(e => e.personelGorevi),

            "durum" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(e => e.durum)
                : filtreli.OrderBy(e => e.durum),

            "personelid" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(e => e.personelId)
                : filtreli.OrderBy(e => e.personelId),

            "vardiyaid" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(e => e.vardiyaId)
                : filtreli.OrderBy(e => e.vardiyaId),

            _ => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(e => e.ekipId)
                : filtreli.OrderBy(e => e.ekipId)
        };

        var toplamKayitSayisi = filtreli.Count();

        var liste = filtreli
            .Skip((sorgu.SayfaNumarasi - 1) * sorgu.SayfaBoyutu)
            .Take(sorgu.SayfaBoyutu)
            .Select(e => new EkipListeDto
            {
                EkipId = e.ekipId,
                EkipUyeSayisi = e.ekipUyeSayisi,
                PersonelGorevi = e.personelGorevi,
                Durum = e.durum,
                PersonelId = e.personelId,
                PersonelAdSoyad = e.Personel == null
                    ? null
                    : $"{e.Personel.personelAdi} {e.Personel.personelSoyadi}",
                VardiyaId = e.vardiyaId,
                VardiyaAdi = e.Vardiya == null ? null : e.Vardiya.vardiyaAdi
            })
            .ToList();

        var sayfaliSonuc = PagedResult<EkipListeDto>.Create(
            liste,
            sorgu.SayfaNumarasi,
            sorgu.SayfaBoyutu,
            toplamKayitSayisi
        );

        return ApiResponse<PagedResult<EkipListeDto>>.Ok(sayfaliSonuc);
    }

    public async Task<ApiResponse<EkipDetayDto>> IdIleGetirAsync(int id)
    {
        var ekip = await _ekipRepository.IdIleGetirAsync(id);

        if (ekip is null)
            return ApiResponse<EkipDetayDto>.Fail("Ekip bulunamadı.");

        return ApiResponse<EkipDetayDto>.Ok(DetayDtoyaDonustur(ekip));
    }

    public async Task<ApiResponse<EkipDetayDto>> OlusturAsync(EkipOlusturDto dto)
    {
        var ekip = new Ekip
        {
            ekipUyeSayisi = dto.EkipUyeSayisi,
            personelGorevi = dto.PersonelGorevi,
            durum = dto.Durum,
            personelId = dto.PersonelId,
            vardiyaId = dto.VardiyaId
        };

        await _ekipRepository.EkleAsync(ekip);
        await _ekipRepository.DegisiklikleriKaydetAsync();

        return ApiResponse<EkipDetayDto>.Ok(
            DetayDtoyaDonustur(ekip),
            "Ekip başarıyla oluşturuldu."
        );
    }

    public async Task<ApiResponse<EkipDetayDto>> GuncelleAsync(int id, EkipGuncelleDto dto)
    {
        var ekip = await _ekipRepository.IdIleGetirAsync(id);

        if (ekip is null)
            return ApiResponse<EkipDetayDto>.Fail("Ekip bulunamadı.");

        ekip.ekipUyeSayisi = dto.EkipUyeSayisi;
        ekip.personelGorevi = dto.PersonelGorevi;
        ekip.durum = dto.Durum;
        ekip.personelId = dto.PersonelId;
        ekip.vardiyaId = dto.VardiyaId;

        await _ekipRepository.DegisiklikleriKaydetAsync();

        return ApiResponse<EkipDetayDto>.Ok(
            DetayDtoyaDonustur(ekip),
            "Ekip başarıyla güncellendi."
        );
    }

    public async Task<ApiResponse<bool>> SilAsync(int id)
    {
        var ekip = await _ekipRepository.IdIleGetirAsync(id);

        if (ekip is null)
            return ApiResponse<bool>.Fail("Ekip bulunamadı.");

        ekip.durum = "Pasif";

        await _ekipRepository.DegisiklikleriKaydetAsync();

        return ApiResponse<bool>.Ok(true, "Ekip pasif hale getirildi.");
    }

    private static EkipDetayDto DetayDtoyaDonustur(Ekip ekip)
    {
        return new EkipDetayDto
        {
            EkipId = ekip.ekipId,
            EkipUyeSayisi = ekip.ekipUyeSayisi,
            PersonelGorevi = ekip.personelGorevi,
            Durum = ekip.durum,
            PersonelId = ekip.personelId,
            PersonelAdSoyad = ekip.Personel == null
                ? null
                : $"{ekip.Personel.personelAdi} {ekip.Personel.personelSoyadi}",
            VardiyaId = ekip.vardiyaId,
            VardiyaAdi = ekip.Vardiya == null ? null : ekip.Vardiya.vardiyaAdi,
            VardiyaDurumu = ekip.Vardiya == null ? null : ekip.Vardiya.vardiyaDurumu
        };
    }
}