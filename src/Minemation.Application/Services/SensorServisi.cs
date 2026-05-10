using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Application.Common;
using Minemation.Application.DTOs;
using Minemation.Application.Interfaces;
using Minemation.Domain.Entities;

namespace Minemation.Application.Services;

public class SensorServisi : ISensorServisi
{
    private readonly ISensorRepository _sensorRepository;

    public SensorServisi(ISensorRepository sensorRepository)
    {
        _sensorRepository = sensorRepository;
    }

    public async Task<ApiResponse<PagedResult<SensorListeDto>>> TumunuGetirAsync(SensorSorguParametreleri sorgu)
    {
        var sensorler = await _sensorRepository.TumunuGetirAsync();

        var filtreli = sensorler.AsQueryable();

        if (!string.IsNullOrWhiteSpace(sorgu.Arama))
        {
            var arama = sorgu.Arama.Trim().ToLower();

            filtreli = filtreli.Where(s =>
                (s.sensorTipi ?? "").ToLower().Contains(arama) ||
                (s.sensorDurumu ?? "").ToLower().Contains(arama) ||
                (s.baglantiProtokolu ?? "").ToLower().Contains(arama) ||
                (s.haberlesmeTipi ?? "").ToLower().Contains(arama) ||
                (s.Ekipman != null && (s.Ekipman.ekipmanAdi ?? "").ToLower().Contains(arama)));
        }

        if (!string.IsNullOrWhiteSpace(sorgu.SensorTipi))
        {
            var sensorTipi = sorgu.SensorTipi.Trim().ToLower();
            filtreli = filtreli.Where(s => (s.sensorTipi ?? "").ToLower() == sensorTipi);
        }

        if (!string.IsNullOrWhiteSpace(sorgu.SensorDurumu))
        {
            var sensorDurumu = sorgu.SensorDurumu.Trim().ToLower();
            filtreli = filtreli.Where(s => (s.sensorDurumu ?? "").ToLower() == sensorDurumu);
        }

        if (!string.IsNullOrWhiteSpace(sorgu.BaglantiProtokolu))
        {
            var protokol = sorgu.BaglantiProtokolu.Trim().ToLower();
            filtreli = filtreli.Where(s => (s.baglantiProtokolu ?? "").ToLower() == protokol);
        }

        if (sorgu.EkipmanId.HasValue)
        {
            filtreli = filtreli.Where(s => s.ekipmanId == sorgu.EkipmanId.Value);
        }

        filtreli = sorgu.SiralamaAlani?.ToLower() switch
        {
            "sensortipi" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(s => s.sensorTipi)
                : filtreli.OrderBy(s => s.sensorTipi),

            "sensordurumu" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(s => s.sensorDurumu)
                : filtreli.OrderBy(s => s.sensorDurumu),

            "baglantiprotokolu" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(s => s.baglantiProtokolu)
                : filtreli.OrderBy(s => s.baglantiProtokolu),

            _ => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(s => s.ekipmanId)
                : filtreli.OrderBy(s => s.ekipmanId)
        };

        var toplamKayitSayisi = filtreli.Count();

        var liste = filtreli
            .Skip((sorgu.SayfaNumarasi - 1) * sorgu.SayfaBoyutu)
            .Take(sorgu.SayfaBoyutu)
            .Select(s => new SensorListeDto
            {
                EkipmanId = s.ekipmanId,
                EkipmanAdi = s.Ekipman == null ? null : s.Ekipman.ekipmanAdi,
                SensorTipi = s.sensorTipi,
                SensorDurumu = s.sensorDurumu,
                MinEsikDeger = s.minEsikDeger,
                MaxEsikDeger = s.maxEsikDeger,
                Hassasiyet = s.hassasiyet,
                BaglantiProtokolu = s.baglantiProtokolu,
                HaberlesmeTipi = s.haberlesmeTipi
            })
            .ToList();

        var sayfaliSonuc = PagedResult<SensorListeDto>.Create(
            liste,
            sorgu.SayfaNumarasi,
            sorgu.SayfaBoyutu,
            toplamKayitSayisi
        );

        return ApiResponse<PagedResult<SensorListeDto>>.Ok(sayfaliSonuc);
    }

    public async Task<ApiResponse<SensorDetayDto>> EkipmanIdIleGetirAsync(int ekipmanId)
    {
        var sensor = await _sensorRepository.EkipmanIdIleGetirAsync(ekipmanId);

        if (sensor is null)
            return ApiResponse<SensorDetayDto>.Fail("Sensör bulunamadı.");

        return ApiResponse<SensorDetayDto>.Ok(DetayDtoyaDonustur(sensor));
    }

    public async Task<ApiResponse<SensorDetayDto>> OlusturAsync(SensorOlusturDto dto)
    {
        var sensorVarMi = await _sensorRepository.EkipmandaSensorVarMiAsync(dto.EkipmanId);

        if (sensorVarMi)
            return ApiResponse<SensorDetayDto>.Fail("Bu ekipmana ait sensör zaten kayıtlı.");

        var sensor = new Sensor
        {
            ekipmanId = dto.EkipmanId,
            sensorTipi = dto.SensorTipi,
            sensorDurumu = dto.SensorDurumu,
            minEsikDeger = dto.MinEsikDeger,
            maxEsikDeger = dto.MaxEsikDeger,
            hassasiyet = dto.Hassasiyet,
            baglantiProtokolu = dto.BaglantiProtokolu,
            haberlesmeTipi = dto.HaberlesmeTipi
        };

        await _sensorRepository.EkleAsync(sensor);
        await _sensorRepository.DegisiklikleriKaydetAsync();

        return ApiResponse<SensorDetayDto>.Ok(
            DetayDtoyaDonustur(sensor),
            "Sensör başarıyla oluşturuldu."
        );
    }

    public async Task<ApiResponse<SensorDetayDto>> GuncelleAsync(int ekipmanId, SensorGuncelleDto dto)
    {
        var sensor = await _sensorRepository.EkipmanIdIleGetirAsync(ekipmanId);

        if (sensor is null)
            return ApiResponse<SensorDetayDto>.Fail("Sensör bulunamadı.");

        sensor.sensorTipi = dto.SensorTipi;
        sensor.sensorDurumu = dto.SensorDurumu;
        sensor.minEsikDeger = dto.MinEsikDeger;
        sensor.maxEsikDeger = dto.MaxEsikDeger;
        sensor.hassasiyet = dto.Hassasiyet;
        sensor.baglantiProtokolu = dto.BaglantiProtokolu;
        sensor.haberlesmeTipi = dto.HaberlesmeTipi;

        await _sensorRepository.DegisiklikleriKaydetAsync();

        return ApiResponse<SensorDetayDto>.Ok(
            DetayDtoyaDonustur(sensor),
            "Sensör başarıyla güncellendi."
        );
    }

    public async Task<ApiResponse<bool>> SilAsync(int ekipmanId)
    {
        var sensor = await _sensorRepository.EkipmanIdIleGetirAsync(ekipmanId);

        if (sensor is null)
            return ApiResponse<bool>.Fail("Sensör bulunamadı.");

        sensor.sensorDurumu = "Pasif";

        await _sensorRepository.DegisiklikleriKaydetAsync();

        return ApiResponse<bool>.Ok(true, "Sensör pasif hale getirildi.");
    }

    private static SensorDetayDto DetayDtoyaDonustur(Sensor sensor)
    {
        return new SensorDetayDto
        {
            EkipmanId = sensor.ekipmanId,
            EkipmanAdi = sensor.Ekipman == null ? null : sensor.Ekipman.ekipmanAdi,
            EkipmanDurumu = sensor.Ekipman == null ? null : sensor.Ekipman.durum,
            SensorTipi = sensor.sensorTipi,
            SensorDurumu = sensor.sensorDurumu,
            MinEsikDeger = sensor.minEsikDeger,
            MaxEsikDeger = sensor.maxEsikDeger,
            Hassasiyet = sensor.hassasiyet,
            BaglantiProtokolu = sensor.baglantiProtokolu,
            HaberlesmeTipi = sensor.haberlesmeTipi
        };
    }
}
