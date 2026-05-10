using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Application.Common;
using Minemation.Application.DTOs;
using Minemation.Application.Interfaces;
using Minemation.Domain.Entities;

namespace Minemation.Application.Services;

public class PersonelRaporuServisi : IPersonelRaporuServisi
{
    private readonly IPersonelRaporuRepository _personelRaporuRepository;

    public PersonelRaporuServisi(IPersonelRaporuRepository personelRaporuRepository)
    {
        _personelRaporuRepository = personelRaporuRepository;
    }

    public async Task<ApiResponse<PagedResult<PersonelRaporuListeDto>>> TumunuGetirAsync(PersonelRaporuSorguParametreleri sorgu)
    {
        var raporlar = await _personelRaporuRepository.TumunuGetirAsync();

        var filtreli = raporlar.AsQueryable();

        if (!string.IsNullOrWhiteSpace(sorgu.Arama))
        {
            var arama = sorgu.Arama.Trim().ToLower();

            filtreli = filtreli.Where(pr =>
                (pr.uzmanlikAlani ?? "").ToLower().Contains(arama) ||
                (pr.Rapor != null && (pr.Rapor.raporAdi ?? "").ToLower().Contains(arama)) ||
                (pr.Rapor != null && (pr.Rapor.raporTuru ?? "").ToLower().Contains(arama)));
        }

        if (!string.IsNullOrWhiteSpace(sorgu.UzmanlikAlani))
        {
            var uzmanlik = sorgu.UzmanlikAlani.Trim().ToLower();
            filtreli = filtreli.Where(pr => (pr.uzmanlikAlani ?? "").ToLower() == uzmanlik);
        }

        filtreli = sorgu.SiralamaAlani?.ToLower() switch
        {
            "personelsayisi" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(pr => pr.personelSayisi)
                : filtreli.OrderBy(pr => pr.personelSayisi),

            "calismasuresi" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(pr => pr.calismaSuresi)
                : filtreli.OrderBy(pr => pr.calismaSuresi),

            "uzmanlikalani" => sorgu.SiralamaYonu?.ToLower() == "desc"
                ? filtreli.OrderByDescending(pr => pr.uzmanlikAlani)
                : filtreli.OrderBy(pr => pr.uzmanlikAlani),

            _ => sorgu.SiralamaYonu?.ToLower() == "asc"
                ? filtreli.OrderBy(pr => pr.raporId)
                : filtreli.OrderByDescending(pr => pr.raporId)
        };

        var toplamKayitSayisi = filtreli.Count();

        var liste = filtreli
            .Skip((sorgu.SayfaNumarasi - 1) * sorgu.SayfaBoyutu)
            .Take(sorgu.SayfaBoyutu)
            .Select(pr => new PersonelRaporuListeDto
            {
                RaporId = pr.raporId,
                RaporAdi = pr.Rapor == null ? null : pr.Rapor.raporAdi,
                RaporTuru = pr.Rapor == null ? null : pr.Rapor.raporTuru,
                RaporOlusturmaTarihi = pr.Rapor == null ? null : pr.Rapor.raporOlusturmaTarihi,
                UzmanlikAlani = pr.uzmanlikAlani,
                PersonelSayisi = pr.personelSayisi,
                CalismaSuresi = pr.calismaSuresi
            })
            .ToList();

        var sayfaliSonuc = PagedResult<PersonelRaporuListeDto>.Create(
            liste,
            sorgu.SayfaNumarasi,
            sorgu.SayfaBoyutu,
            toplamKayitSayisi
        );

        return ApiResponse<PagedResult<PersonelRaporuListeDto>>.Ok(sayfaliSonuc);
    }

    public async Task<ApiResponse<PersonelRaporuDetayDto>> RaporIdIleGetirAsync(int raporId)
    {
        var personelRaporu = await _personelRaporuRepository.RaporIdIleGetirAsync(raporId);

        if (personelRaporu is null)
            return ApiResponse<PersonelRaporuDetayDto>.Fail("Personel raporu bulunamadı.");

        return ApiResponse<PersonelRaporuDetayDto>.Ok(DetayDtoyaDonustur(personelRaporu));
    }

    public async Task<ApiResponse<PersonelRaporuDetayDto>> OlusturAsync(PersonelRaporuOlusturDto dto)
    {
        var varMi = await _personelRaporuRepository.VarMiAsync(dto.RaporId);

        if (varMi)
            return ApiResponse<PersonelRaporuDetayDto>.Fail("Bu rapor için personel raporu zaten oluşturulmuş.");

        var personelRaporu = new PersonelRaporu
        {
            raporId = dto.RaporId,
            uzmanlikAlani = dto.UzmanlikAlani,
            personelSayisi = dto.PersonelSayisi,
            calismaSuresi = dto.CalismaSuresi
        };

        await _personelRaporuRepository.EkleAsync(personelRaporu);
        await _personelRaporuRepository.DegisiklikleriKaydetAsync();

        return ApiResponse<PersonelRaporuDetayDto>.Ok(
            DetayDtoyaDonustur(personelRaporu),
            "Personel raporu başarıyla oluşturuldu."
        );
    }

    public async Task<ApiResponse<PersonelRaporuDetayDto>> GuncelleAsync(int raporId, PersonelRaporuGuncelleDto dto)
    {
        var personelRaporu = await _personelRaporuRepository.RaporIdIleGetirAsync(raporId);

        if (personelRaporu is null)
            return ApiResponse<PersonelRaporuDetayDto>.Fail("Personel raporu bulunamadı.");

        personelRaporu.uzmanlikAlani = dto.UzmanlikAlani;
        personelRaporu.personelSayisi = dto.PersonelSayisi;
        personelRaporu.calismaSuresi = dto.CalismaSuresi;

        await _personelRaporuRepository.DegisiklikleriKaydetAsync();

        return ApiResponse<PersonelRaporuDetayDto>.Ok(
            DetayDtoyaDonustur(personelRaporu),
            "Personel raporu başarıyla güncellendi."
        );
    }

    public async Task<ApiResponse<bool>> SilAsync(int raporId)
    {
        var personelRaporu = await _personelRaporuRepository.RaporIdIleGetirAsync(raporId);

        if (personelRaporu is null)
            return ApiResponse<bool>.Fail("Personel raporu bulunamadı.");

        return ApiResponse<bool>.Ok(true, "Personel raporu bulundu. Bu entity'de durum alanı olmadığı için pasife alma yapılmadı.");
    }

    private static PersonelRaporuDetayDto DetayDtoyaDonustur(PersonelRaporu personelRaporu)
    {
        return new PersonelRaporuDetayDto
        {
            RaporId = personelRaporu.raporId,
            RaporAdi = personelRaporu.Rapor == null ? null : personelRaporu.Rapor.raporAdi,
            RaporTuru = personelRaporu.Rapor == null ? null : personelRaporu.Rapor.raporTuru,
            RaporAciklamasi = personelRaporu.Rapor == null ? null : personelRaporu.Rapor.raporAciklamasi,
            RaporOlusturmaTarihi = personelRaporu.Rapor == null ? null : personelRaporu.Rapor.raporOlusturmaTarihi,
            RaporDosyaYolu = personelRaporu.Rapor == null ? null : personelRaporu.Rapor.raporDosyaYolu,
            ZamanAraligi = personelRaporu.Rapor == null ? null : personelRaporu.Rapor.zamanAraligi,
            UzmanlikAlani = personelRaporu.uzmanlikAlani,
            PersonelSayisi = personelRaporu.personelSayisi,
            CalismaSuresi = personelRaporu.calismaSuresi
        };
    }
}
