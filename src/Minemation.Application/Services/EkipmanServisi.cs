using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Application.Common;
using Minemation.Application.DTOs;
using Minemation.Application.Interfaces;
using Minemation.Domain.Entities;

namespace Minemation.Application.Services;

public class EkipmanServisi : IEkipmanServisi
{
    private readonly IEkipmanRepository _ekipmanRepository;

    public EkipmanServisi(IEkipmanRepository ekipmanRepository)
    {
        _ekipmanRepository = ekipmanRepository;
    }

    public async Task<ApiResponse<PagedResult<EkipmanListeDto>>> TumunuGetirAsync(EkipmanSorguParametreleri sorgu)
    {
        var ekipmanlar = await _ekipmanRepository.TumunuGetirAsync();

        var filtreli = ekipmanlar.AsQueryable();

        if (!string.IsNullOrWhiteSpace(sorgu.Arama))
        {
            var arama = sorgu.Arama.Trim().ToLower();

            filtreli = filtreli.Where(x =>
                (x.ekipmanAdi ?? "").ToLower().Contains(arama) ||
                (x.ekipmanMarka ?? "").ToLower().Contains(arama) ||
                (x.ekipmanModel ?? "").ToLower().Contains(arama) ||
                (x.seriNo ?? "").ToLower().Contains(arama) ||
                (x.RFIDEtiket ?? "").ToLower().Contains(arama));
        }

        if (!string.IsNullOrWhiteSpace(sorgu.Durum))
        {
            var durum = sorgu.Durum.Trim().ToLower();
            filtreli = filtreli.Where(x => (x.durum ?? "").ToLower() == durum);
        }

        if (!string.IsNullOrWhiteSpace(sorgu.OperasyonTuru))
        {
            var operasyonTuru = sorgu.OperasyonTuru.Trim().ToLower();
            filtreli = filtreli.Where(x => (x.operasyonTuru ?? "").ToLower() == operasyonTuru);
        }

        if (!string.IsNullOrWhiteSpace(sorgu.EkipmanMarka))
        {
            var ekipmanMarka = sorgu.EkipmanMarka.Trim().ToLower();
            filtreli = filtreli.Where(x => (x.ekipmanMarka ?? "").ToLower() == ekipmanMarka);
        }

        filtreli = sorgu.SiralamaAlani?.ToLower() switch
        {
            "ekipmanmarka" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(x => x.ekipmanMarka)
                : filtreli.OrderBy(x => x.ekipmanMarka),

            "ekipmanmodel" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(x => x.ekipmanModel)
                : filtreli.OrderBy(x => x.ekipmanModel),

            "durum" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(x => x.durum)
                : filtreli.OrderBy(x => x.durum),

            "serino" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(x => x.seriNo)
                : filtreli.OrderBy(x => x.seriNo),

            "operasyonturu" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(x => x.operasyonTuru)
                : filtreli.OrderBy(x => x.operasyonTuru),

            _ => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(x => x.ekipmanAdi)
                : filtreli.OrderBy(x => x.ekipmanAdi)
        };

        var toplamKayitSayisi = filtreli.Count();

        var liste = filtreli
            .Skip((sorgu.SayfaNumarasi - 1) * sorgu.SayfaBoyutu)
            .Take(sorgu.SayfaBoyutu)
            .Select(x => new EkipmanListeDto
            {
                EkipmanId = x.ekipmanId,
                EkipmanAdi = x.ekipmanAdi,
                EkipmanMarka = x.ekipmanMarka,
                EkipmanModel = x.ekipmanModel,
                Durum = x.durum,
                SeriNo = x.seriNo,
                RFIDEtiket = x.RFIDEtiket,
                OperasyonTuru = x.operasyonTuru
            })
            .ToList();

        var sayfaliSonuc = PagedResult<EkipmanListeDto>.Create(
            liste,
            sorgu.SayfaNumarasi,
            sorgu.SayfaBoyutu,
            toplamKayitSayisi
        );

        return ApiResponse<PagedResult<EkipmanListeDto>>.Ok(sayfaliSonuc);
    }

    public async Task<ApiResponse<EkipmanDetayDto>> IdIleGetirAsync(int id)
    {
        var ekipman = await _ekipmanRepository.IdIleGetirAsync(id);

        if (ekipman is null)
            return ApiResponse<EkipmanDetayDto>.Fail("Ekipman bulunamadı.");

        return ApiResponse<EkipmanDetayDto>.Ok(DetayDtoyaDonustur(ekipman));
    }

    public async Task<ApiResponse<EkipmanDetayDto>> OlusturAsync(EkipmanOlusturDto dto)
    {
        var seriNoVarMi = await _ekipmanRepository.SeriNoVarMiAsync(dto.SeriNo);

        if (seriNoVarMi)
            return ApiResponse<EkipmanDetayDto>.Fail("Bu seri numarası ile kayıtlı ekipman zaten var.");

        var ekipman = new Ekipman
        {
            ekipmanAdi = dto.EkipmanAdi,
            ekipmanMarka = dto.EkipmanMarka,
            ekipmanModel = dto.EkipmanModel,
            durum = dto.Durum,
            sonBakimTarihi = dto.SonBakimTarihi,
            gelecekBakimTarihi = dto.GelecekBakimTarihi,
            seriNo = dto.SeriNo,
            RFIDEtiket = dto.RFIDEtiket,
            ureticiFirma = dto.UreticiFirma,
            tedarikciFirma = dto.TedarikciFirma,
            uretimYili = dto.UretimYili,
            boyut = dto.Boyut,
            agirlik = dto.Agirlik,
            satinAlmaTarihi = dto.SatinAlmaTarihi,
            garantiBaslangicTarihi = dto.GarantiBaslangicTarihi,
            teknikDokuman = dto.TeknikDokuman,
            kullanimKilavuzu = dto.KullanimKilavuzu,
            garantiBelgesi = dto.GarantiBelgesi,
            bakimFormu = dto.BakimFormu,
            satinAlmaBelgesi = dto.SatinAlmaBelgesi,
            operasyonTuru = dto.OperasyonTuru
        };

        await _ekipmanRepository.EkleAsync(ekipman);
        await _ekipmanRepository.DegisiklikleriKaydetAsync();

        return ApiResponse<EkipmanDetayDto>.Ok(
            DetayDtoyaDonustur(ekipman),
            "Ekipman başarıyla oluşturuldu."
        );
    }

    public async Task<ApiResponse<EkipmanDetayDto>> GuncelleAsync(int id, EkipmanGuncelleDto dto)
    {
        var ekipman = await _ekipmanRepository.IdIleGetirAsync(id);

        if (ekipman is null)
            return ApiResponse<EkipmanDetayDto>.Fail("Ekipman bulunamadı.");

        var seriNoVarMi = await _ekipmanRepository.SeriNoVarMiAsync(dto.SeriNo, id);

        if (seriNoVarMi)
            return ApiResponse<EkipmanDetayDto>.Fail("Bu seri numarası başka bir ekipmana ait.");

        ekipman.ekipmanAdi = dto.EkipmanAdi;
        ekipman.ekipmanMarka = dto.EkipmanMarka;
        ekipman.ekipmanModel = dto.EkipmanModel;
        ekipman.durum = dto.Durum;
        ekipman.sonBakimTarihi = dto.SonBakimTarihi;
        ekipman.gelecekBakimTarihi = dto.GelecekBakimTarihi;
        ekipman.seriNo = dto.SeriNo;
        ekipman.RFIDEtiket = dto.RFIDEtiket;
        ekipman.ureticiFirma = dto.UreticiFirma;
        ekipman.tedarikciFirma = dto.TedarikciFirma;
        ekipman.uretimYili = dto.UretimYili;
        ekipman.boyut = dto.Boyut;
        ekipman.agirlik = dto.Agirlik;
        ekipman.satinAlmaTarihi = dto.SatinAlmaTarihi;
        ekipman.garantiBaslangicTarihi = dto.GarantiBaslangicTarihi;
        ekipman.teknikDokuman = dto.TeknikDokuman;
        ekipman.kullanimKilavuzu = dto.KullanimKilavuzu;
        ekipman.garantiBelgesi = dto.GarantiBelgesi;
        ekipman.bakimFormu = dto.BakimFormu;
        ekipman.satinAlmaBelgesi = dto.SatinAlmaBelgesi;
        ekipman.operasyonTuru = dto.OperasyonTuru;

        await _ekipmanRepository.DegisiklikleriKaydetAsync();

        return ApiResponse<EkipmanDetayDto>.Ok(
            DetayDtoyaDonustur(ekipman),
            "Ekipman başarıyla güncellendi."
        );
    }

    public async Task<ApiResponse<bool>> SilAsync(int id)
    {
        var ekipman = await _ekipmanRepository.IdIleGetirAsync(id);

        if (ekipman is null)
            return ApiResponse<bool>.Fail("Ekipman bulunamadı.");

        ekipman.durum = "Pasif";

        await _ekipmanRepository.DegisiklikleriKaydetAsync();

        return ApiResponse<bool>.Ok(true, "Ekipman pasif hale getirildi.");
    }

    private static EkipmanDetayDto DetayDtoyaDonustur(Ekipman ekipman)
    {
        return new EkipmanDetayDto
        {
            EkipmanId = ekipman.ekipmanId,
            EkipmanAdi = ekipman.ekipmanAdi,
            EkipmanMarka = ekipman.ekipmanMarka,
            EkipmanModel = ekipman.ekipmanModel,
            Durum = ekipman.durum,
            SonBakimTarihi = ekipman.sonBakimTarihi,
            GelecekBakimTarihi = ekipman.gelecekBakimTarihi,
            SeriNo = ekipman.seriNo,
            RFIDEtiket = ekipman.RFIDEtiket,
            UreticiFirma = ekipman.ureticiFirma,
            TedarikciFirma = ekipman.tedarikciFirma,
            UretimYili = ekipman.uretimYili,
            Boyut = ekipman.boyut,
            Agirlik = ekipman.agirlik,
            SatinAlmaTarihi = ekipman.satinAlmaTarihi,
            GarantiBaslangicTarihi = ekipman.garantiBaslangicTarihi,
            TeknikDokuman = ekipman.teknikDokuman,
            KullanimKilavuzu = ekipman.kullanimKilavuzu,
            GarantiBelgesi = ekipman.garantiBelgesi,
            BakimFormu = ekipman.bakimFormu,
            SatinAlmaBelgesi = ekipman.satinAlmaBelgesi,
            OperasyonTuru = ekipman.operasyonTuru
        };
    }
}
