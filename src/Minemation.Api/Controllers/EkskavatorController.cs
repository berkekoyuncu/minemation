using Microsoft.AspNetCore.Mvc;
using Minemation.Application.DTOs;
using Minemation.Application.Interfaces;

namespace Minemation.Api.Controllers;

[ApiController]
[Route("api/ekskavator")]
public class EkskavatorController : ControllerBase
{
    private readonly IEkskavatorServisi _ekskavatorServisi;

    public EkskavatorController(IEkskavatorServisi ekskavatorServisi)
    {
        _ekskavatorServisi = ekskavatorServisi;
    }

    [HttpGet]
    public async Task<IActionResult> TumunuGetir([FromQuery] EkskavatorSorguParametreleri sorgu)
    {
        var sonuc = await _ekskavatorServisi.TumunuGetirAsync(sorgu);
        return Ok(sonuc);
    }

    [HttpGet("{ekipmanId:int}")]
    public async Task<IActionResult> EkipmanIdIleGetir(int ekipmanId)
    {
        var sonuc = await _ekskavatorServisi.EkipmanIdIleGetirAsync(ekipmanId);

        if (!sonuc.Success)
            return NotFound(sonuc);

        return Ok(sonuc);
    }

    [HttpPost]
    public async Task<IActionResult> Olustur([FromBody] EkskavatorOlusturDto dto)
    {
        var sonuc = await _ekskavatorServisi.OlusturAsync(dto);

        if (!sonuc.Success)
            return BadRequest(sonuc);

        return Ok(sonuc);
    }

    [HttpPut("{ekipmanId:int}")]
    public async Task<IActionResult> Guncelle(int ekipmanId, [FromBody] EkskavatorGuncelleDto dto)
    {
        var sonuc = await _ekskavatorServisi.GuncelleAsync(ekipmanId, dto);

        if (!sonuc.Success)
            return BadRequest(sonuc);

        return Ok(sonuc);
    }

    [HttpDelete("{ekipmanId:int}")]
    public async Task<IActionResult> Sil(int ekipmanId)
    {
        var sonuc = await _ekskavatorServisi.SilAsync(ekipmanId);

        if (!sonuc.Success)
            return NotFound(sonuc);

        return Ok(sonuc);
    }
}