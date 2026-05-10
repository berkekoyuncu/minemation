using Microsoft.AspNetCore.Mvc;
using Minemation.Application.DTOs;
using Minemation.Application.Interfaces;

namespace Minemation.Api.Controllers;

[ApiController]
[Route("api/hafriyat")]
public class HafriyatController : ControllerBase
{
    private readonly IHafriyatServisi _hafriyatServisi;

    public HafriyatController(IHafriyatServisi hafriyatServisi)
    {
        _hafriyatServisi = hafriyatServisi;
    }

    [HttpGet]
    public async Task<IActionResult> TumunuGetir([FromQuery] HafriyatSorguParametreleri sorgu)
    {
        var sonuc = await _hafriyatServisi.TumunuGetirAsync(sorgu);
        return Ok(sonuc);
    }

    [HttpGet("{ekipmanId:int}")]
    public async Task<IActionResult> EkipmanIdIleGetir(int ekipmanId)
    {
        var sonuc = await _hafriyatServisi.EkipmanIdIleGetirAsync(ekipmanId);

        if (!sonuc.Success)
            return NotFound(sonuc);

        return Ok(sonuc);
    }

    [HttpPost]
    public async Task<IActionResult> Olustur([FromBody] HafriyatOlusturDto dto)
    {
        var sonuc = await _hafriyatServisi.OlusturAsync(dto);

        if (!sonuc.Success)
            return BadRequest(sonuc);

        return Ok(sonuc);
    }

    [HttpPut("{ekipmanId:int}")]
    public async Task<IActionResult> Guncelle(int ekipmanId, [FromBody] HafriyatGuncelleDto dto)
    {
        var sonuc = await _hafriyatServisi.GuncelleAsync(ekipmanId, dto);

        if (!sonuc.Success)
            return BadRequest(sonuc);

        return Ok(sonuc);
    }

    [HttpDelete("{ekipmanId:int}")]
    public async Task<IActionResult> Sil(int ekipmanId)
    {
        var sonuc = await _hafriyatServisi.SilAsync(ekipmanId);

        if (!sonuc.Success)
            return NotFound(sonuc);

        return Ok(sonuc);
    }
}