using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Minemation.Application.Common;
using Minemation.Application.DTOs;
using Minemation.Application.Interfaces;
using Minemation.Domain.Entities;

namespace Minemation.Application.Services;

public class KimlikDogrulamaServisi : IKimlikDogrulamaServisi
{
    private readonly IPersonelRepository _personelRepository;
    private readonly ILogKaydiRepository _logKaydiRepository;

    public KimlikDogrulamaServisi(
        IPersonelRepository personelRepository,
        ILogKaydiRepository logKaydiRepository)
    {
        _personelRepository = personelRepository;
        _logKaydiRepository = logKaydiRepository;
    }

    public async Task<ApiResponse<GirisSonucDto>> GirisYapAsync(GirisYapDto dto)
    {
        var kullaniciAdi = dto.KullaniciAdi.Trim();

        Personel? personel;

        if (kullaniciAdi.Contains("@"))
        {
            personel = await _personelRepository.EpostaIleGetirAsync(kullaniciAdi);
        }
        else
        {
            personel = await _personelRepository.TcknIleGetirAsync(kullaniciAdi);
        }

        if (personel is null)
        {
            await BasarisizGirisLoglaAsync(
                kullaniciAdi,
                "Kullanıcı bulunamadı.",
                null
            );

            return ApiResponse<GirisSonucDto>.Fail("Kullanıcı bulunamadı.");
        }

        if (!string.Equals(personel.personelDurumu, "Aktif", StringComparison.OrdinalIgnoreCase))
        {
            await BasarisizGirisLoglaAsync(
                kullaniciAdi,
                "Personel aktif değil.",
                personel.personelId
            );

            return ApiResponse<GirisSonucDto>.Fail("Personel aktif değil.");
        }

        var sifreDogruMu = SifreDogruMu(personel.sifreHash, dto.Sifre);

        if (!sifreDogruMu)
        {
            await BasarisizGirisLoglaAsync(
                kullaniciAdi,
                "Şifre hatalı.",
                personel.personelId
            );

            return ApiResponse<GirisSonucDto>.Fail("Şifre hatalı.");
        }

        // Eski düz metin şifreyle giriş yapılmışsa otomatik hash'e çeviriyoruz.
        if (!HashGibiMi(personel.sifreHash))
        {
            personel.sifreHash = Hashle(dto.Sifre);
        }

        personel.sonGirisTarihi = DateTime.Now;

        await BasariliGirisLoglaAsync(personel);

        await _personelRepository.DegisiklikleriKaydetAsync();

        return ApiResponse<GirisSonucDto>.Ok(
            SonucDtoyaDonustur(personel),
            "Giriş başarılı."
        );
    }

    public async Task<ApiResponse<GirisSonucDto>> RfidIleGirisYapAsync(RfidGirisDto dto)
    {
        var rfid = dto.RfidKartNumarasi.Trim();

        var personel = await _personelRepository.RfidIleGetirAsync(rfid);

        if (personel is null)
        {
            await BasarisizGirisLoglaAsync(
                rfid,
                "RFID kartına bağlı personel bulunamadı.",
                null
            );

            return ApiResponse<GirisSonucDto>.Fail("RFID kartına bağlı personel bulunamadı.");
        }

        if (!string.Equals(personel.personelDurumu, "Aktif", StringComparison.OrdinalIgnoreCase))
        {
            await BasarisizGirisLoglaAsync(
                rfid,
                "RFID girişinde personel aktif değil.",
                personel.personelId
            );

            return ApiResponse<GirisSonucDto>.Fail("Personel aktif değil.");
        }

        personel.sonGirisTarihi = DateTime.Now;

        await BasariliGirisLoglaAsync(personel);

        await _personelRepository.DegisiklikleriKaydetAsync();

        return ApiResponse<GirisSonucDto>.Ok(
            SonucDtoyaDonustur(personel),
            "RFID ile giriş başarılı."
        );
    }

    private async Task BasarisizGirisLoglaAsync(string kullaniciAdi, string mesaj, int? personelId)
    {
        var log = new LogKaydi
        {
            islemTipi = "Başarısız Kimlik Doğrulama",
            logKaydiAciklamasi = $"Başarısız giriş denemesi. Kullanıcı: {kullaniciAdi}. Sebep: {mesaj}",
            logKaydiTarihi = DateTime.Now,
            ipAdresi = "Sistem",
            onemSeviyesi = "Uyarı",
            durum = "Aktif",
            personelId = personelId,
            ekipmanId = null
        };

        await _logKaydiRepository.EkleAsync(log);
        await _logKaydiRepository.DegisiklikleriKaydetAsync();
    }

    private async Task BasariliGirisLoglaAsync(Personel personel)
    {
        var log = new LogKaydi
        {
            islemTipi = "Kimlik Doğrulama",
            logKaydiAciklamasi = $"{personel.personelAdi} {personel.personelSoyadi} sisteme giriş yaptı.",
            logKaydiTarihi = DateTime.Now,
            ipAdresi = "Sistem",
            onemSeviyesi = "Bilgi",
            durum = "Aktif",
            personelId = personel.personelId,
            ekipmanId = null
        };

        await _logKaydiRepository.EkleAsync(log);
    }

    private static bool SifreDogruMu(string kayitliSifreHash, string girilenSifre)
    {
        if (string.IsNullOrWhiteSpace(kayitliSifreHash))
            return false;

        // Eski düz metin şifreleri geçici olarak destekliyoruz.
        if (!HashGibiMi(kayitliSifreHash))
            return kayitliSifreHash == girilenSifre;

        var girilenHash = Hashle(girilenSifre);

        return kayitliSifreHash == girilenHash;
    }

    private static string Hashle(string sifre)
    {
        using var sha256 = SHA256.Create();

        var bytes = Encoding.UTF8.GetBytes(sifre);
        var hashBytes = sha256.ComputeHash(bytes);

        return Convert.ToHexString(hashBytes);
    }

    private static bool HashGibiMi(string deger)
    {
        return deger.Length == 64 &&
               deger.All(c => Uri.IsHexDigit(c));
    }

    private static bool SifreGecerliMi(string sifre)
    {
        if (string.IsNullOrWhiteSpace(sifre) || sifre.Length < 8)
            return false;

        var buyukHarfVar = sifre.Any(char.IsUpper);
        var kucukHarfVar = sifre.Any(char.IsLower);
        var rakamVar = sifre.Any(char.IsDigit);
        var ozelKarakterVar = sifre.Any(c => !char.IsLetterOrDigit(c));

        return buyukHarfVar && kucukHarfVar && rakamVar && ozelKarakterVar;
    }

    private static GirisSonucDto SonucDtoyaDonustur(Personel personel)
    {
        return new GirisSonucDto
        {
            PersonelId = personel.personelId,
            PersonelAdi = personel.personelAdi,
            PersonelSoyadi = personel.personelSoyadi,
            Eposta = personel.eposta,
            KullaniciRolu = personel.kullaniciRolu,
            PersonelDurumu = personel.personelDurumu,
            SonGirisTarihi = personel.sonGirisTarihi
        };
    }

    public async Task<ApiResponse<bool>> SifreDegistirAsync(SifreDegistirDto dto)
    {
        var personel = await _personelRepository.IdIleGetirAsync(dto.PersonelId);

        if (personel is null)
            return ApiResponse<bool>.Fail("Personel bulunamadı.");

        if (!SifreDogruMu(personel.sifreHash, dto.EskiSifre))
            return ApiResponse<bool>.Fail("Eski şifre hatalı.");

        if (!SifreGecerliMi(dto.YeniSifre))
            return ApiResponse<bool>.Fail("Yeni şifre en az 8 karakter olmalı; büyük harf, küçük harf, rakam ve özel karakter içermelidir.");

        personel.sifreHash = Hashle(dto.YeniSifre);
        personel.sonGirisTarihi = DateTime.Now;

        await _personelRepository.DegisiklikleriKaydetAsync();

        return ApiResponse<bool>.Ok(true, "Şifre başarıyla değiştirildi.");
    }

    public async Task<ApiResponse<bool>> SifreBelirleAsync(SifreBelirleDto dto)
    {
        var personel = await _personelRepository.IdIleGetirAsync(dto.PersonelId);

        if (personel is null)
            return ApiResponse<bool>.Fail("Personel bulunamadı.");

        if (!SifreGecerliMi(dto.YeniSifre))
            return ApiResponse<bool>.Fail("Yeni şifre en az 8 karakter olmalı; büyük harf, küçük harf, rakam ve özel karakter içermelidir.");

        personel.sifreHash = Hashle(dto.YeniSifre);

        await _personelRepository.DegisiklikleriKaydetAsync();

        return ApiResponse<bool>.Ok(true, "Şifre başarıyla belirlendi.");
    }
}
