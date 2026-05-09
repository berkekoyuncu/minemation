using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Application.Common;
using Minemation.Application.DTOs;
using Minemation.Application.Interfaces;
using Minemation.Domain.Entities;

namespace Minemation.Application.Services;

public class VardiyaServisi : IVardiyaServisi
{
    private readonly IVardiyaRepository _vardiyaRepository;

    public VardiyaServisi(IVardiyaRepository vardiyaRepository)
    {
        _vardiyaRepository = vardiyaRepository;
    }

    public async Task<ApiResponse<PagedResult<VardiyaListeDto>>> TumunuGetirAsync(VardiyaSorguParametreleri sorgu)
    {
        var vardiyalar = await _vardiyaRepository.TumunuGetirAsync();

        var filtreli = vardiyalar.AsQueryable();

        if (!string.IsNullOrWhiteSpace(sorgu.Arama))
        {
            var arama = sorgu.Arama.Trim().ToLower();

            filtreli = filtreli.Where(v =>
                (v.vardiyaAdi ?? "").ToLower().Contains(arama) ||
                (v.vardiyaTanimi ?? "").ToLower().Contains(arama) ||
                (v.vardiyaSupervizoru ?? "").ToLower().Contains(arama) ||
                (v.calismaBolgesi ?? "").ToLower().Contains(arama) ||
                (v.operasyonTipi ?? "").ToLower().Contains(arama));
        }

        if (!string.IsNullOrWhiteSpace(sorgu.VardiyaDurumu))
        {
            var durum = sorgu.VardiyaDurumu.Trim().ToLower();
            filtreli = filtreli.Where(v => (v.vardiyaDurumu ?? "").ToLower() == durum);
        }

        if (!string.IsNullOrWhiteSpace(sorgu.VardiyaTipi))
        {
            var tip = sorgu.VardiyaTipi.Trim().ToLower();
            filtreli = filtreli.Where(v => (v.vardiyaTipi ?? "").ToLower() == tip);
        }

        if (!string.IsNullOrWhiteSpace(sorgu.CalismaBolgesi))
        {
            var bolge = sorgu.CalismaBolgesi.Trim().ToLower();
            filtreli = filtreli.Where(v => (v.calismaBolgesi ?? "").ToLower() == bolge);
        }

        if (!string.IsNullOrWhiteSpace(sorgu.OperasyonRiskSeviyesi))
        {
            var risk = sorgu.OperasyonRiskSeviyesi.Trim().ToLower();
            filtreli = filtreli.Where(v => (v.operasyonRiskSeviyesi ?? "").ToLower() == risk);
        }

        filtreli = sorgu.SiralamaAlani?.ToLower() switch
        {
            "vardiyabaslangictarihi" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(v => v.vardiyaBaslangicTarihi)
                : filtreli.OrderBy(v => v.vardiyaBaslangicTarihi),

            "vardiyabitistarihi" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(v => v.vardiyaBitisTarihi)
                : filtreli.OrderBy(v => v.vardiyaBitisTarihi),

            "vardiyadurumu" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(v => v.vardiyaDurumu)
                : filtreli.OrderBy(v => v.vardiyaDurumu),

            "vardiyatipi" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(v => v.vardiyaTipi)
                : filtreli.OrderBy(v => v.vardiyaTipi),

            "calismabolgesi" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(v => v.calismaBolgesi)
                : filtreli.OrderBy(v => v.calismaBolgesi),

            "operasyonriskseviyesi" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(v => v.operasyonRiskSeviyesi)
                : filtreli.OrderBy(v => v.operasyonRiskSeviyesi),

            _ => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(v => v.vardiyaAdi)
                : filtreli.OrderBy(v => v.vardiyaAdi)
        };

        var toplamKayitSayisi = filtreli.Count();

        var liste = filtreli
            .Skip((sorgu.SayfaNumarasi - 1) * sorgu.SayfaBoyutu)
            .Take(sorgu.SayfaBoyutu)
            .Select(v => new VardiyaListeDto
            {
                VardiyaId = v.vardiyaId,
                VardiyaAdi = v.vardiyaAdi,
                VardiyaBaslangicTarihi = v.vardiyaBaslangicTarihi,
                VardiyaBitisTarihi = v.vardiyaBitisTarihi,
                VardiyaDurumu = v.vardiyaDurumu,
                VardiyaTipi = v.vardiyaTipi,
                CalismaBolgesi = v.calismaBolgesi,
                OperasyonTipi = v.operasyonTipi,
                OperasyonRiskSeviyesi = v.operasyonRiskSeviyesi,
                PersonelSayisi = v.personelSayisi,
                EkipmanSayisi = v.ekipmanSayisi,
                EkipSayisi = v.ekipSayisi
            })
            .ToList();

        var sayfaliSonuc = PagedResult<VardiyaListeDto>.Create(
            liste,
            sorgu.SayfaNumarasi,
            sorgu.SayfaBoyutu,
            toplamKayitSayisi
        );

        return ApiResponse<PagedResult<VardiyaListeDto>>.Ok(sayfaliSonuc);
    }

    public async Task<ApiResponse<VardiyaDetayDto>> IdIleGetirAsync(int id)
    {
        var vardiya = await _vardiyaRepository.IdIleGetirAsync(id);

        if (vardiya is null)
            return ApiResponse<VardiyaDetayDto>.Fail("Vardiya bulunamadı.");

        return ApiResponse<VardiyaDetayDto>.Ok(DetayDtoyaDonustur(vardiya));
    }

    public async Task<ApiResponse<VardiyaDetayDto>> OlusturAsync(VardiyaOlusturDto dto)
    {
        var vardiya = new Vardiya
        {
            vardiyaAdi = dto.VardiyaAdi,
            vardiyaTanimi = dto.VardiyaTanimi,
            vardiyaBaslangicTarihi = dto.VardiyaBaslangicTarihi,
            vardiyaBitisTarihi = dto.VardiyaBitisTarihi,
            vardiyaOlusturmaTarihi = DateTime.Now,
            vardiyaSupervizoru = dto.VardiyaSupervizoru,
            personelSayisi = dto.PersonelSayisi,
            ekipmanSayisi = dto.EkipmanSayisi,
            ekipSayisi = dto.EkipSayisi,
            vardiyaDurumu = dto.VardiyaDurumu,
            vardiyaTipi = dto.VardiyaTipi,
            toplaVardiyaSuresi = dto.ToplaVardiyaSuresi,
            calismaBolgesi = dto.CalismaBolgesi,
            operasyonTipi = dto.OperasyonTipi,
            operasyonRiskSeviyesi = dto.OperasyonRiskSeviyesi,
            vardiyaNotlari = dto.VardiyaNotlari,
            ekipmanOperatoru = dto.EkipmanOperatoru,
            ekipmanId = dto.EkipmanId,
            vardiyaSorumlusu = dto.VardiyaSorumlusu,
            vardiyaIsgSorumlusu = dto.VardiyaIsgSorumlusu,
            vardiyaTeknikSorumlusu = dto.VardiyaTeknikSorumlusu
        };

        await _vardiyaRepository.EkleAsync(vardiya);
        await _vardiyaRepository.DegisiklikleriKaydetAsync();

        return ApiResponse<VardiyaDetayDto>.Ok(
            DetayDtoyaDonustur(vardiya),
            "Vardiya başarıyla oluşturuldu."
        );
    }

    public async Task<ApiResponse<VardiyaDetayDto>> GuncelleAsync(int id, VardiyaGuncelleDto dto)
    {
        var vardiya = await _vardiyaRepository.IdIleGetirAsync(id);

        if (vardiya is null)
            return ApiResponse<VardiyaDetayDto>.Fail("Vardiya bulunamadı.");

        vardiya.vardiyaAdi = dto.VardiyaAdi;
        vardiya.vardiyaTanimi = dto.VardiyaTanimi;
        vardiya.vardiyaBaslangicTarihi = dto.VardiyaBaslangicTarihi;
        vardiya.vardiyaBitisTarihi = dto.VardiyaBitisTarihi;
        vardiya.vardiyaSupervizoru = dto.VardiyaSupervizoru;
        vardiya.personelSayisi = dto.PersonelSayisi;
        vardiya.ekipmanSayisi = dto.EkipmanSayisi;
        vardiya.ekipSayisi = dto.EkipSayisi;
        vardiya.vardiyaDurumu = dto.VardiyaDurumu;
        vardiya.vardiyaTipi = dto.VardiyaTipi;
        vardiya.toplaVardiyaSuresi = dto.ToplaVardiyaSuresi;
        vardiya.calismaBolgesi = dto.CalismaBolgesi;
        vardiya.operasyonTipi = dto.OperasyonTipi;
        vardiya.operasyonRiskSeviyesi = dto.OperasyonRiskSeviyesi;
        vardiya.vardiyaNotlari = dto.VardiyaNotlari;
        vardiya.ekipmanOperatoru = dto.EkipmanOperatoru;
        vardiya.ekipmanId = dto.EkipmanId;
        vardiya.vardiyaSorumlusu = dto.VardiyaSorumlusu;
        vardiya.vardiyaIsgSorumlusu = dto.VardiyaIsgSorumlusu;
        vardiya.vardiyaTeknikSorumlusu = dto.VardiyaTeknikSorumlusu;

        await _vardiyaRepository.DegisiklikleriKaydetAsync();

        return ApiResponse<VardiyaDetayDto>.Ok(
            DetayDtoyaDonustur(vardiya),
            "Vardiya başarıyla güncellendi."
        );
    }

    public async Task<ApiResponse<bool>> SilAsync(int id)
    {
        var vardiya = await _vardiyaRepository.IdIleGetirAsync(id);

        if (vardiya is null)
            return ApiResponse<bool>.Fail("Vardiya bulunamadı.");

        vardiya.vardiyaDurumu = "Pasif";

        await _vardiyaRepository.DegisiklikleriKaydetAsync();

        return ApiResponse<bool>.Ok(true, "Vardiya pasif hale getirildi.");
    }

    private static VardiyaDetayDto DetayDtoyaDonustur(Vardiya vardiya)
    {
        return new VardiyaDetayDto
        {
            VardiyaId = vardiya.vardiyaId,
            VardiyaAdi = vardiya.vardiyaAdi,
            VardiyaTanimi = vardiya.vardiyaTanimi,
            VardiyaBaslangicTarihi = vardiya.vardiyaBaslangicTarihi,
            VardiyaBitisTarihi = vardiya.vardiyaBitisTarihi,
            VardiyaOlusturmaTarihi = vardiya.vardiyaOlusturmaTarihi,
            VardiyaSupervizoru = vardiya.vardiyaSupervizoru,
            PersonelSayisi = vardiya.personelSayisi,
            EkipmanSayisi = vardiya.ekipmanSayisi,
            EkipSayisi = vardiya.ekipSayisi,
            VardiyaDurumu = vardiya.vardiyaDurumu,
            VardiyaTipi = vardiya.vardiyaTipi,
            ToplaVardiyaSuresi = vardiya.toplaVardiyaSuresi,
            CalismaBolgesi = vardiya.calismaBolgesi,
            OperasyonTipi = vardiya.operasyonTipi,
            OperasyonRiskSeviyesi = vardiya.operasyonRiskSeviyesi,
            VardiyaNotlari = vardiya.vardiyaNotlari,
            EkipmanOperatoru = vardiya.ekipmanOperatoru,
            EkipmanId = vardiya.ekipmanId,
            EkipmanAdi = vardiya.Ekipman?.ekipmanAdi,
            VardiyaSorumlusu = vardiya.vardiyaSorumlusu,
            VardiyaSorumlusuAdSoyad = vardiya.VardiyaSorumlusuPersonel == null
                ? null
                : $"{vardiya.VardiyaSorumlusuPersonel.personelAdi} {vardiya.VardiyaSorumlusuPersonel.personelSoyadi}",
            VardiyaIsgSorumlusu = vardiya.vardiyaIsgSorumlusu,
            IsgSorumlusuAdSoyad = vardiya.IsgSorumlusuPersonel == null
                ? null
                : $"{vardiya.IsgSorumlusuPersonel.personelAdi} {vardiya.IsgSorumlusuPersonel.personelSoyadi}",
            VardiyaTeknikSorumlusu = vardiya.vardiyaTeknikSorumlusu,
            TeknikSorumlusuAdSoyad = vardiya.TeknikSorumlusuPersonel == null
                ? null
                : $"{vardiya.TeknikSorumlusuPersonel.personelAdi} {vardiya.TeknikSorumlusuPersonel.personelSoyadi}"
        };
    }
}
