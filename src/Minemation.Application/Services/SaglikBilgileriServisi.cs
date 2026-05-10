using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Application.Common;
using Minemation.Application.DTOs;
using Minemation.Application.Interfaces;
using Minemation.Domain.Entities;

namespace Minemation.Application.Services;

public class SaglikBilgileriServisi : ISaglikBilgileriServisi
{
    private readonly ISaglikBilgileriRepository _saglikBilgileriRepository;

    public SaglikBilgileriServisi(ISaglikBilgileriRepository saglikBilgileriRepository)
    {
        _saglikBilgileriRepository = saglikBilgileriRepository;
    }

    public async Task<ApiResponse<PagedResult<SaglikBilgileriListeDto>>> TumunuGetirAsync(SaglikBilgileriSorguParametreleri sorgu)
    {
        var saglikBilgileri = await _saglikBilgileriRepository.TumunuGetirAsync();

        var filtreli = saglikBilgileri.AsQueryable();

        if (!string.IsNullOrWhiteSpace(sorgu.Arama))
        {
            var arama = sorgu.Arama.Trim().ToLower();

            filtreli = filtreli.Where(s =>
                (s.kanGrubu ?? "").ToLower().Contains(arama) ||
                (s.saglikDurumu ?? "").ToLower().Contains(arama) ||
                (s.saglikCalismaKisitlamalari ?? "").ToLower().Contains(arama) ||
                (s.acilDurumNotu ?? "").ToLower().Contains(arama) ||
                s.kronikHastaliklar.Any(k => (k ?? "").ToLower().Contains(arama)) ||
                s.alerjiler.Any(a => (a ?? "").ToLower().Contains(arama)) ||
                (s.Personel != null && ((s.Personel.personelAdi ?? "") + " " + (s.Personel.personelSoyadi ?? "")).ToLower().Contains(arama)));
        }

        if (!string.IsNullOrWhiteSpace(sorgu.KanGrubu))
        {
            var kanGrubu = sorgu.KanGrubu.Trim().ToLower();
            filtreli = filtreli.Where(s => (s.kanGrubu ?? "").ToLower() == kanGrubu);
        }

        if (!string.IsNullOrWhiteSpace(sorgu.SaglikDurumu))
        {
            var saglikDurumu = sorgu.SaglikDurumu.Trim().ToLower();
            filtreli = filtreli.Where(s => (s.saglikDurumu ?? "").ToLower() == saglikDurumu);
        }

        filtreli = sorgu.SiralamaAlani?.ToLower() switch
        {
            "kangrubu" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(s => s.kanGrubu)
                : filtreli.OrderBy(s => s.kanGrubu),

            "saglikdurumu" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(s => s.saglikDurumu)
                : filtreli.OrderBy(s => s.saglikDurumu),

            "sonmuayenetarihi" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(s => s.sonMuayeneTarihi)
                : filtreli.OrderBy(s => s.sonMuayeneTarihi),

            _ => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(s => s.personelId)
                : filtreli.OrderBy(s => s.personelId)
        };

        var toplamKayitSayisi = filtreli.Count();

        var liste = filtreli
            .Skip((sorgu.SayfaNumarasi - 1) * sorgu.SayfaBoyutu)
            .Take(sorgu.SayfaBoyutu)
            .Select(s => new SaglikBilgileriListeDto
            {
                PersonelId = s.personelId,
                PersonelAdSoyad = s.Personel == null
                    ? null
                    : $"{s.Personel.personelAdi} {s.Personel.personelSoyadi}",
                KanGrubu = s.kanGrubu,
                SaglikDurumu = s.saglikDurumu,
                KronikHastaliklar = s.kronikHastaliklar,
                Alerjiler = s.alerjiler,
                SaglikCalismaKisitlamalari = s.saglikCalismaKisitlamalari,
                SonMuayeneTarihi = s.sonMuayeneTarihi
            })
            .ToList();

        var sayfaliSonuc = PagedResult<SaglikBilgileriListeDto>.Create(
            liste,
            sorgu.SayfaNumarasi,
            sorgu.SayfaBoyutu,
            toplamKayitSayisi
        );

        return ApiResponse<PagedResult<SaglikBilgileriListeDto>>.Ok(sayfaliSonuc);
    }

    public async Task<ApiResponse<SaglikBilgileriDetayDto>> PersonelIdIleGetirAsync(int personelId)
    {
        var saglikBilgileri = await _saglikBilgileriRepository.PersonelIdIleGetirAsync(personelId);

        if (saglikBilgileri is null)
            return ApiResponse<SaglikBilgileriDetayDto>.Fail("Sağlık bilgileri bulunamadı.");

        return ApiResponse<SaglikBilgileriDetayDto>.Ok(DetayDtoyaDonustur(saglikBilgileri));
    }

    public async Task<ApiResponse<SaglikBilgileriDetayDto>> OlusturAsync(SaglikBilgileriOlusturDto dto)
    {
        var varMi = await _saglikBilgileriRepository.VarMiAsync(dto.PersonelId);

        if (varMi)
            return ApiResponse<SaglikBilgileriDetayDto>.Fail("Bu personel için sağlık bilgileri zaten oluşturulmuş.");

        var saglikBilgileri = new SaglikBilgileri
        {
            personelId = dto.PersonelId,
            kanGrubu = dto.KanGrubu,
            saglikDurumu = dto.SaglikDurumu,
            kronikHastaliklar = dto.KronikHastaliklar,
            alerjiler = dto.Alerjiler,
            saglikCalismaKisitlamalari = dto.SaglikCalismaKisitlamalari,
            acilDurumNotu = dto.AcilDurumNotu,
            sonMuayeneTarihi = dto.SonMuayeneTarihi
        };

        await _saglikBilgileriRepository.EkleAsync(saglikBilgileri);
        await _saglikBilgileriRepository.DegisiklikleriKaydetAsync();

        return ApiResponse<SaglikBilgileriDetayDto>.Ok(
            DetayDtoyaDonustur(saglikBilgileri),
            "Sağlık bilgileri başarıyla oluşturuldu."
        );
    }

    public async Task<ApiResponse<SaglikBilgileriDetayDto>> GuncelleAsync(int personelId, SaglikBilgileriGuncelleDto dto)
    {
        var saglikBilgileri = await _saglikBilgileriRepository.PersonelIdIleGetirAsync(personelId);

        if (saglikBilgileri is null)
            return ApiResponse<SaglikBilgileriDetayDto>.Fail("Sağlık bilgileri bulunamadı.");

        saglikBilgileri.kanGrubu = dto.KanGrubu;
        saglikBilgileri.saglikDurumu = dto.SaglikDurumu;
        saglikBilgileri.kronikHastaliklar = dto.KronikHastaliklar;
        saglikBilgileri.alerjiler = dto.Alerjiler;
        saglikBilgileri.saglikCalismaKisitlamalari = dto.SaglikCalismaKisitlamalari;
        saglikBilgileri.acilDurumNotu = dto.AcilDurumNotu;
        saglikBilgileri.sonMuayeneTarihi = dto.SonMuayeneTarihi;

        await _saglikBilgileriRepository.DegisiklikleriKaydetAsync();

        return ApiResponse<SaglikBilgileriDetayDto>.Ok(
            DetayDtoyaDonustur(saglikBilgileri),
            "Sağlık bilgileri başarıyla güncellendi."
        );
    }

    public async Task<ApiResponse<bool>> SilAsync(int personelId)
    {
        var saglikBilgileri = await _saglikBilgileriRepository.PersonelIdIleGetirAsync(personelId);

        if (saglikBilgileri is null)
            return ApiResponse<bool>.Fail("Sağlık bilgileri bulunamadı.");

        saglikBilgileri.saglikDurumu = "Pasif";

        await _saglikBilgileriRepository.DegisiklikleriKaydetAsync();

        return ApiResponse<bool>.Ok(true, "Sağlık bilgileri pasif hale getirildi.");
    }

    private static SaglikBilgileriDetayDto DetayDtoyaDonustur(SaglikBilgileri saglikBilgileri)
    {
        return new SaglikBilgileriDetayDto
        {
            PersonelId = saglikBilgileri.personelId,
            PersonelAdSoyad = saglikBilgileri.Personel == null
                ? null
                : $"{saglikBilgileri.Personel.personelAdi} {saglikBilgileri.Personel.personelSoyadi}",
            PersonelDurumu = saglikBilgileri.Personel == null ? null : saglikBilgileri.Personel.personelDurumu,
            KanGrubu = saglikBilgileri.kanGrubu,
            SaglikDurumu = saglikBilgileri.saglikDurumu,
            KronikHastaliklar = saglikBilgileri.kronikHastaliklar,
            Alerjiler = saglikBilgileri.alerjiler,
            SaglikCalismaKisitlamalari = saglikBilgileri.saglikCalismaKisitlamalari,
            AcilDurumNotu = saglikBilgileri.acilDurumNotu,
            SonMuayeneTarihi = saglikBilgileri.sonMuayeneTarihi
        };
    }
}