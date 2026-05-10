using Microsoft.AspNetCore.Mvc;
using Minemation.Application.DTOs;
using Minemation.Application.Interfaces;

namespace Minemation.Api.Controllers;

[ApiController]
[Route("api/kimlik-dogrulama")]
public class KimlikDogrulamaController : ControllerBase
{
    private readonly IKimlikDogrulamaServisi _kimlikDogrulamaServisi;

    public KimlikDogrulamaController(IKimlikDogrulamaServisi kimlikDogrulamaServisi)
    {
        _kimlikDogrulamaServisi = kimlikDogrulamaServisi;
    }

    [HttpPost("giris")]
    public async Task<IActionResult> GirisYap([FromBody] GirisYapDto dto)
    {
        var sonuc = await _kimlikDogrulamaServisi.GirisYapAsync(dto);

        if (!sonuc.Success)
            return BadRequest(sonuc);

        return Ok(sonuc);
    }

    [HttpPost("rfid-giris")]
    public async Task<IActionResult> RfidIleGirisYap([FromBody] RfidGirisDto dto)
    {
        var sonuc = await _kimlikDogrulamaServisi.RfidIleGirisYapAsync(dto);

        if (!sonuc.Success)
            return BadRequest(sonuc);

        return Ok(sonuc);
    }

    [HttpPost("sifre-degistir")]
    public async Task<IActionResult> SifreDegistir([FromBody] SifreDegistirDto dto)
    {
        var sonuc = await _kimlikDogrulamaServisi.SifreDegistirAsync(dto);

        if (!sonuc.Success)
            return BadRequest(sonuc);

        return Ok(sonuc);
    }

    [HttpPost("sifre-belirle")]
    public async Task<IActionResult> SifreBelirle([FromBody] SifreBelirleDto dto)
    {
        var sonuc = await _kimlikDogrulamaServisi.SifreBelirleAsync(dto);

        if (!sonuc.Success)
            return BadRequest(sonuc);

        return Ok(sonuc);
    }
}