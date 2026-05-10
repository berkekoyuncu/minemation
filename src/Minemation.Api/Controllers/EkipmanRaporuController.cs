using Microsoft.AspNetCore.Mvc;
using Minemation.Application.DTOs;
using Minemation.Application.Interfaces;

namespace Minemation.Api.Controllers;

[ApiController]
[Route("api/ekipman-raporu")]
public class EkipmanRaporuController : ControllerBase
{
    private readonly IEkipmanRaporuServisi _ekipmanRaporuServisi;

    public EkipmanRaporuController(IEkipmanRaporuServisi ekipmanRaporuServisi)
    {
        _ekipmanRaporuServisi = ekipmanRaporuServisi;
    }

    [HttpGet]
    public async Task<IActionResult> TumunuGetir([FromQuery] EkipmanRaporuSorguParametreleri sorgu)
    {
        var sonuc = await _ekipmanRaporuServisi.TumunuGetirAsync(sorgu);
        return Ok(sonuc);
    }

    [HttpGet("{raporId:int}")]
    public async Task<IActionResult> RaporIdIleGetir(int raporId)
    {
        var sonuc = await _ekipmanRaporuServisi.RaporIdIleGetirAsync(raporId);

        if (!sonuc.Success)
            return NotFound(sonuc);

        return Ok(sonuc);
    }

    [HttpPost]
    public async Task<IActionResult> Olustur([FromBody] EkipmanRaporuOlusturDto dto)
    {
        var sonuc = await _ekipmanRaporuServisi.OlusturAsync(dto);

        if (!sonuc.Success)
            return BadRequest(sonuc);

        return Ok(sonuc);
    }

    [HttpPut("{raporId:int}")]
    public async Task<IActionResult> Guncelle(int raporId, [FromBody] EkipmanRaporuGuncelleDto dto)
    {
        var sonuc = await _ekipmanRaporuServisi.GuncelleAsync(raporId, dto);

        if (!sonuc.Success)
            return BadRequest(sonuc);

        return Ok(sonuc);
    }

    [HttpDelete("{raporId:int}")]
    public async Task<IActionResult> Sil(int raporId)
    {
        var sonuc = await _ekipmanRaporuServisi.SilAsync(raporId);

        if (!sonuc.Success)
            return NotFound(sonuc);

        return Ok(sonuc);
    }
}