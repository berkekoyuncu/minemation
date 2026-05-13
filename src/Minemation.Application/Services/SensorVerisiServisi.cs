using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Application.Common;
using Minemation.Application.DTOs;
using Minemation.Application.Interfaces;
using Minemation.Domain.Entities;

namespace Minemation.Application.Services;

public class SensorVerisiServisi : ISensorVerisiServisi
{
    private readonly ISensorVerisiRepository _sensorVerisiRepository;

    public SensorVerisiServisi(ISensorVerisiRepository sensorVerisiRepository)
    {
        _sensorVerisiRepository = sensorVerisiRepository;
    }

    public async Task<ApiResponse<PagedResult<SensorVerisiListeDto>>> TumunuGetirAsync(SensorVerisiSorguParametreleri sorgu)
    {
        var sensorVerileri = await _sensorVerisiRepository.TumunuGetirAsync();

        var filtreli = sensorVerileri.AsQueryable();

        if (!string.IsNullOrWhiteSpace(sorgu.Arama))
        {
            var arama = sorgu.Arama.Trim().ToLower();

            filtreli = filtreli.Where(sv =>
                (sv.birim ?? "").ToLower().Contains(arama) ||
                (sv.Sensor != null && (sv.Sensor.sensorTipi ?? "").ToLower().Contains(arama)) ||
                (sv.Sensor != null && (sv.Sensor.ekipmanAdi ?? "").ToLower().Contains(arama)) ||
                (sv.Vardiya != null && (sv.Vardiya.vardiyaAdi ?? "").ToLower().Contains(arama)));
        }

        if (sorgu.EkipmanId.HasValue)
        {
            filtreli = filtreli.Where(sv => sv.ekipmanId == sorgu.EkipmanId.Value);
        }

        if (sorgu.VardiyaId.HasValue)
        {
            filtreli = filtreli.Where(sv => sv.vardiyaId == sorgu.VardiyaId.Value);
        }

        if (!string.IsNullOrWhiteSpace(sorgu.Birim))
        {
            var birim = sorgu.Birim.Trim().ToLower();
            filtreli = filtreli.Where(sv => (sv.birim ?? "").ToLower() == birim);
        }

        if (sorgu.SadeceEsikDisi.HasValue)
        {
            filtreli = filtreli.Where(sv => EsikDisiMi(sv) == sorgu.SadeceEsikDisi.Value);
        }

        filtreli = sorgu.SiralamaAlani?.ToLower() switch
        {
            "deger" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(sv => sv.deger)
                : filtreli.OrderBy(sv => sv.deger),

            "ekipmanid" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(sv => sv.ekipmanId)
                : filtreli.OrderBy(sv => sv.ekipmanId),

            "vardiyaid" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(sv => sv.vardiyaId)
                : filtreli.OrderBy(sv => sv.vardiyaId),

            _ => sorgu.SiralamaYonu?.ToLower() == "asc"
                ? filtreli.OrderBy(sv => sv.olcumTarihi)
                : filtreli.OrderByDescending(sv => sv.olcumTarihi)
        };

        var toplamKayitSayisi = filtreli.Count();

        var liste = filtreli
            .Skip((sorgu.SayfaNumarasi - 1) * sorgu.SayfaBoyutu)
            .Take(sorgu.SayfaBoyutu)
            .Select(sv => new SensorVerisiListeDto
            {
                SensorVerisiId = sv.sensorVerisiId,
                Deger = sv.deger,
                Birim = sv.birim,
                OlcumTarihi = sv.olcumTarihi,
                EkipmanId = sv.ekipmanId,
                SensorTipi = sv.Sensor == null ? null : sv.Sensor.sensorTipi,
                EkipmanAdi = sv.Sensor == null ? null : sv.Sensor.ekipmanAdi,
                VardiyaId = sv.vardiyaId,
                VardiyaAdi = sv.Vardiya == null ? null : sv.Vardiya.vardiyaAdi,
                EsikDisiMi = EsikDisiMi(sv)
            })
            .ToList();

        var sayfaliSonuc = PagedResult<SensorVerisiListeDto>.Create(
            liste,
            sorgu.SayfaNumarasi,
            sorgu.SayfaBoyutu,
            toplamKayitSayisi
        );

        return ApiResponse<PagedResult<SensorVerisiListeDto>>.Ok(sayfaliSonuc);
    }

    public async Task<ApiResponse<SensorVerisiDetayDto>> IdIleGetirAsync(int id)
    {
        var sensorVerisi = await _sensorVerisiRepository.IdIleGetirAsync(id);

        if (sensorVerisi is null)
            return ApiResponse<SensorVerisiDetayDto>.Fail("Sensör verisi bulunamadı.");

        return ApiResponse<SensorVerisiDetayDto>.Ok(DetayDtoyaDonustur(sensorVerisi));
    }

    public async Task<ApiResponse<SensorVerisiDetayDto>> OlusturAsync(SensorVerisiOlusturDto dto)
    {
        var sensorVerisi = new SensorVerisi
        {
            deger = dto.Deger,
            birim = dto.Birim,
            olcumTarihi = dto.OlcumTarihi,
            ekipmanId = dto.EkipmanId,
            vardiyaId = dto.VardiyaId
        };

        await _sensorVerisiRepository.EkleAsync(sensorVerisi);
        await _sensorVerisiRepository.DegisiklikleriKaydetAsync();

        return ApiResponse<SensorVerisiDetayDto>.Ok(
            DetayDtoyaDonustur(sensorVerisi),
            "Sensör verisi başarıyla oluşturuldu."
        );
    }

    public async Task<ApiResponse<SensorVerisiDetayDto>> GuncelleAsync(int id, SensorVerisiGuncelleDto dto)
    {
        var sensorVerisi = await _sensorVerisiRepository.IdIleGetirAsync(id);

        if (sensorVerisi is null)
            return ApiResponse<SensorVerisiDetayDto>.Fail("Sensör verisi bulunamadı.");

        sensorVerisi.deger = dto.Deger;
        sensorVerisi.birim = dto.Birim;
        sensorVerisi.olcumTarihi = dto.OlcumTarihi;
        sensorVerisi.ekipmanId = dto.EkipmanId;
        sensorVerisi.vardiyaId = dto.VardiyaId;

        await _sensorVerisiRepository.DegisiklikleriKaydetAsync();

        return ApiResponse<SensorVerisiDetayDto>.Ok(
            DetayDtoyaDonustur(sensorVerisi),
            "Sensör verisi başarıyla güncellendi."
        );
    }

    public async Task<ApiResponse<bool>> SilAsync(int id)
    {
        var sensorVerisi = await _sensorVerisiRepository.IdIleGetirAsync(id);

        if (sensorVerisi is null)
            return ApiResponse<bool>.Fail("Sensör verisi bulunamadı.");

        // SensorVerisi entity'sinde durum alanı olmadığı için fiziksel silme yapmıyoruz.
        // Teslim için endpoint bulunabilir/veri var kontrolü şeklinde bırakıldı.
        return ApiResponse<bool>.Ok(true, "Sensör verisi bulundu. Bu entity'de durum alanı olmadığı için pasife alma yapılmadı.");
    }

    private static SensorVerisiDetayDto DetayDtoyaDonustur(SensorVerisi sensorVerisi)
    {
        return new SensorVerisiDetayDto
        {
            SensorVerisiId = sensorVerisi.sensorVerisiId,
            Deger = sensorVerisi.deger,
            Birim = sensorVerisi.birim,
            OlcumTarihi = sensorVerisi.olcumTarihi,
            EkipmanId = sensorVerisi.ekipmanId,
            SensorTipi = sensorVerisi.Sensor == null ? null : sensorVerisi.Sensor.sensorTipi,
            SensorDurumu = sensorVerisi.Sensor == null ? null : sensorVerisi.Sensor.sensorDurumu,
            EkipmanAdi = sensorVerisi.Sensor == null ? null : sensorVerisi.Sensor.ekipmanAdi,
            MinEsikDeger = sensorVerisi.Sensor == null ? 0 : sensorVerisi.Sensor.minEsikDeger,
            MaxEsikDeger = sensorVerisi.Sensor == null ? 0 : sensorVerisi.Sensor.maxEsikDeger,
            VardiyaId = sensorVerisi.vardiyaId,
            VardiyaAdi = sensorVerisi.Vardiya == null ? null : sensorVerisi.Vardiya.vardiyaAdi,
            EsikDisiMi = EsikDisiMi(sensorVerisi)
        };
    }

    private static bool EsikDisiMi(SensorVerisi sensorVerisi)
    {
        if (sensorVerisi.Sensor is null)
            return false;

        var deger = (double)sensorVerisi.deger;

        return deger < sensorVerisi.Sensor.minEsikDeger ||
               deger > sensorVerisi.Sensor.maxEsikDeger;
    }
}
