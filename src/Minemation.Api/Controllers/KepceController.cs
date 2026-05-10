using Microsoft.AspNetCore.Mvc;
using Minemation.Application.DTOs;
using Minemation.Application.Interfaces;

namespace Minemation.Api.Controllers;

[ApiController]
[Route("api/kepce")]
public class KepceController : ControllerBase
{
    private readonly IKepceServisi _kepceServisi;

    public KepceController(IKepceServisi kepceServisi)
    {
        _kepceServisi = kepceServisi;
    }

    [HttpGet]
    public async Task<IActionResult> TumunuGetir([FromQuery] KepceSorguParametreleri sorgu)
    {
        var sonuc = await _kepceServisi.TumunuGetirAsync(sorgu);
        return Ok(sonuc);
    }

    [HttpGet("{ekipmanId:int}")]
    public async Task<IActionResult> EkipmanIdIleGetir(int ekipmanId)
    {
        var sonuc = await _kepceServisi.EkipmanIdIleGetirAsync(ekipmanId);

        if (!sonuc.Success)
            return NotFound(sonuc);

        return Ok(sonuc);
    }

    [HttpPost]
    public async Task<IActionResult> Olustur([FromBody] KepceOlusturDto dto)
    {
        var sonuc = await _kepceServisi.OlusturAsync(dto);

        if (!sonuc.Success)
            return BadRequest(sonuc);

        return Ok(sonuc);
    }

    [HttpPut("{ekipmanId:int}")]
    public async Task<IActionResult> Guncelle(int ekipmanId, [FromBody] KepceGuncelleDto dto)
    {
        var sonuc = await _kepceServisi.GuncelleAsync(ekipmanId, dto);

        if (!sonuc.Success)
            return BadRequest(sonuc);

        return Ok(sonuc);
    }

    [HttpDelete("{ekipmanId:int}")]
    public async Task<IActionResult> Sil(int ekipmanId)
    {
        var sonuc = await _kepceServisi.SilAsync(ekipmanId);

        if (!sonuc.Success)
            return NotFound(sonuc);

        return Ok(sonuc);
    }
}