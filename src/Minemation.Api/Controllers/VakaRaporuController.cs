using Microsoft.AspNetCore.Mvc;
using Minemation.Application.DTOs;
using Minemation.Application.Interfaces;

namespace Minemation.Api.Controllers;

[ApiController]
[Route("api/vaka-raporu")]
public class VakaRaporuController : ControllerBase
{
    private readonly IVakaRaporuServisi _vakaRaporuServisi;

    public VakaRaporuController(IVakaRaporuServisi vakaRaporuServisi)
    {
        _vakaRaporuServisi = vakaRaporuServisi;
    }

    [HttpGet]
    public async Task<IActionResult> TumunuGetir([FromQuery] VakaRaporuSorguParametreleri sorgu)
    {
        var sonuc = await _vakaRaporuServisi.TumunuGetirAsync(sorgu);
        return Ok(sonuc);
    }

    [HttpGet("{raporId:int}")]
    public async Task<IActionResult> RaporIdIleGetir(int raporId)
    {
        var sonuc = await _vakaRaporuServisi.RaporIdIleGetirAsync(raporId);

        if (!sonuc.Success)
            return NotFound(sonuc);

        return Ok(sonuc);
    }

    [HttpPost]
    public async Task<IActionResult> Olustur([FromBody] VakaRaporuOlusturDto dto)
    {
        var sonuc = await _vakaRaporuServisi.OlusturAsync(dto);

        if (!sonuc.Success)
            return BadRequest(sonuc);

        return Ok(sonuc);
    }

    [HttpPut("{raporId:int}")]
    public async Task<IActionResult> Guncelle(int raporId, [FromBody] VakaRaporuGuncelleDto dto)
    {
        var sonuc = await _vakaRaporuServisi.GuncelleAsync(raporId, dto);

        if (!sonuc.Success)
            return BadRequest(sonuc);

        return Ok(sonuc);
    }

    [HttpDelete("{raporId:int}")]
    public async Task<IActionResult> Sil(int raporId)
    {
        var sonuc = await _vakaRaporuServisi.SilAsync(raporId);

        if (!sonuc.Success)
            return NotFound(sonuc);

        return Ok(sonuc);
    }
}
