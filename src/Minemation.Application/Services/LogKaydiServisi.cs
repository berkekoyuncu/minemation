using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Application.Common;
using Minemation.Application.DTOs;
using Minemation.Application.Interfaces;
using Minemation.Domain.Entities;

namespace Minemation.Application.Services;

public class LogKaydiServisi : ILogKaydiServisi
{
    private readonly ILogKaydiRepository _logKaydiRepository;

    public LogKaydiServisi(ILogKaydiRepository logKaydiRepository)
    {
        _logKaydiRepository = logKaydiRepository;
    }

    public async Task<ApiResponse<PagedResult<LogKaydiListeDto>>> TumunuGetirAsync(LogKaydiSorguParametreleri sorgu)
    {
        var loglar = await _logKaydiRepository.TumunuGetirAsync();

        var filtreli = loglar.AsQueryable();

        if (!string.IsNullOrWhiteSpace(sorgu.Arama))
        {
            var arama = sorgu.Arama.Trim().ToLower();

            filtreli = filtreli.Where(l =>
                (l.islemTipi ?? "").ToLower().Contains(arama) ||
                (l.logKaydiAciklamasi ?? "").ToLower().Contains(arama) ||
                (l.ipAdresi ?? "").ToLower().Contains(arama) ||
                (l.onemSeviyesi ?? "").ToLower().Contains(arama));
        }

        if (!string.IsNullOrWhiteSpace(sorgu.IslemTipi))
        {
            var islemTipi = sorgu.IslemTipi.Trim().ToLower();
            filtreli = filtreli.Where(l => (l.islemTipi ?? "").ToLower() == islemTipi);
        }

        if (!string.IsNullOrWhiteSpace(sorgu.OnemSeviyesi))
        {
            var onem = sorgu.OnemSeviyesi.Trim().ToLower();
            filtreli = filtreli.Where(l => (l.onemSeviyesi ?? "").ToLower() == onem);
        }

        if (!string.IsNullOrWhiteSpace(sorgu.Durum))
        {
            var durum = sorgu.Durum.Trim().ToLower();
            filtreli = filtreli.Where(l => (l.durum ?? "").ToLower() == durum);
        }

        if (sorgu.PersonelId.HasValue)
        {
            filtreli = filtreli.Where(l => l.personelId == sorgu.PersonelId.Value);
        }

        if (sorgu.EkipmanId.HasValue)
        {
            filtreli = filtreli.Where(l => l.ekipmanId == sorgu.EkipmanId.Value);
        }

        filtreli = sorgu.SiralamaAlani?.ToLower() switch
        {
            "islemtipi" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(l => l.islemTipi)
                : filtreli.OrderBy(l => l.islemTipi),

            "onemseviyesi" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(l => l.onemSeviyesi)
                : filtreli.OrderBy(l => l.onemSeviyesi),

            "durum" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(l => l.durum)
                : filtreli.OrderBy(l => l.durum),

            _ => sorgu.SiralamaYonu?.ToLower() == "asc"
                ? filtreli.OrderBy(l => l.logKaydiTarihi)
                : filtreli.OrderByDescending(l => l.logKaydiTarihi)
        };

        var toplamKayitSayisi = filtreli.Count();

        var liste = filtreli
            .Skip((sorgu.SayfaNumarasi - 1) * sorgu.SayfaBoyutu)
            .Take(sorgu.SayfaBoyutu)
            .Select(l => new LogKaydiListeDto
            {
                LogKaydiID = l.logKaydiID,
                IslemTipi = l.islemTipi,
                LogKaydiAciklamasi = l.logKaydiAciklamasi,
                LogKaydiTarihi = l.logKaydiTarihi,
                IpAdresi = l.ipAdresi,
                OnemSeviyesi = l.onemSeviyesi,
                Durum = l.durum,
                PersonelId = l.personelId,
                PersonelAdSoyad = l.Personel == null
                    ? null
                    : $"{l.Personel.personelAdi} {l.Personel.personelSoyadi}",
                EkipmanId = l.ekipmanId,
                EkipmanAdi = l.Ekipman == null ? null : l.Ekipman.ekipmanAdi
            })
            .ToList();

        var sayfaliSonuc = PagedResult<LogKaydiListeDto>.Create(
            liste,
            sorgu.SayfaNumarasi,
            sorgu.SayfaBoyutu,
            toplamKayitSayisi
        );

        return ApiResponse<PagedResult<LogKaydiListeDto>>.Ok(sayfaliSonuc);
    }

    public async Task<ApiResponse<LogKaydiDetayDto>> IdIleGetirAsync(int id)
    {
        var logKaydi = await _logKaydiRepository.IdIleGetirAsync(id);

        if (logKaydi is null)
            return ApiResponse<LogKaydiDetayDto>.Fail("Log kaydı bulunamadı.");

        return ApiResponse<LogKaydiDetayDto>.Ok(DetayDtoyaDonustur(logKaydi));
    }

    public async Task<ApiResponse<LogKaydiDetayDto>> OlusturAsync(LogKaydiOlusturDto dto)
    {
        var logKaydi = new LogKaydi
        {
            islemTipi = dto.IslemTipi,
            logKaydiAciklamasi = dto.LogKaydiAciklamasi,
            logKaydiTarihi = dto.LogKaydiTarihi,
            ipAdresi = dto.IpAdresi,
            onemSeviyesi = dto.OnemSeviyesi,
            durum = dto.Durum,
            personelId = dto.PersonelId,
            ekipmanId = dto.EkipmanId
        };

        await _logKaydiRepository.EkleAsync(logKaydi);
        await _logKaydiRepository.DegisiklikleriKaydetAsync();

        return ApiResponse<LogKaydiDetayDto>.Ok(
            DetayDtoyaDonustur(logKaydi),
            "Log kaydı başarıyla oluşturuldu."
        );
    }

    public async Task<ApiResponse<LogKaydiDetayDto>> GuncelleAsync(int id, LogKaydiGuncelleDto dto)
    {
        var logKaydi = await _logKaydiRepository.IdIleGetirAsync(id);

        if (logKaydi is null)
            return ApiResponse<LogKaydiDetayDto>.Fail("Log kaydı bulunamadı.");

        logKaydi.islemTipi = dto.IslemTipi;
        logKaydi.logKaydiAciklamasi = dto.LogKaydiAciklamasi;
        logKaydi.logKaydiTarihi = dto.LogKaydiTarihi;
        logKaydi.ipAdresi = dto.IpAdresi;
        logKaydi.onemSeviyesi = dto.OnemSeviyesi;
        logKaydi.durum = dto.Durum;
        logKaydi.personelId = dto.PersonelId;
        logKaydi.ekipmanId = dto.EkipmanId;

        await _logKaydiRepository.DegisiklikleriKaydetAsync();

        return ApiResponse<LogKaydiDetayDto>.Ok(
            DetayDtoyaDonustur(logKaydi),
            "Log kaydı başarıyla güncellendi."
        );
    }

    public async Task<ApiResponse<bool>> SilAsync(int id)
    {
        var logKaydi = await _logKaydiRepository.IdIleGetirAsync(id);

        if (logKaydi is null)
            return ApiResponse<bool>.Fail("Log kaydı bulunamadı.");

        logKaydi.durum = "Pasif";

        await _logKaydiRepository.DegisiklikleriKaydetAsync();

        return ApiResponse<bool>.Ok(true, "Log kaydı pasif hale getirildi.");
    }

    private static LogKaydiDetayDto DetayDtoyaDonustur(LogKaydi logKaydi)
    {
        return new LogKaydiDetayDto
        {
            LogKaydiID = logKaydi.logKaydiID,
            IslemTipi = logKaydi.islemTipi,
            LogKaydiAciklamasi = logKaydi.logKaydiAciklamasi,
            LogKaydiTarihi = logKaydi.logKaydiTarihi,
            IpAdresi = logKaydi.ipAdresi,
            OnemSeviyesi = logKaydi.onemSeviyesi,
            Durum = logKaydi.durum,
            PersonelId = logKaydi.personelId,
            PersonelAdSoyad = logKaydi.Personel == null
                ? null
                : $"{logKaydi.Personel.personelAdi} {logKaydi.Personel.personelSoyadi}",
            EkipmanId = logKaydi.ekipmanId,
            EkipmanAdi = logKaydi.Ekipman == null ? null : logKaydi.Ekipman.ekipmanAdi,
            EkipmanDurumu = logKaydi.Ekipman == null ? null : logKaydi.Ekipman.durum
        };
    }
}
