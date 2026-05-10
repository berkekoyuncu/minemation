using Microsoft.AspNetCore.Mvc;
using Minemation.Application.DTOs;
using Minemation.Application.Interfaces;

namespace Minemation.Api.Controllers;

[ApiController]
[Route("api/kirici")]
public class KiriciController : ControllerBase
{
    private readonly IKiriciServisi _kiriciServisi;

    public KiriciController(IKiriciServisi kiriciServisi)
    {
        _kiriciServisi = kiriciServisi;
    }

    [HttpGet]
    public async Task<IActionResult> TumunuGetir([FromQuery] KiriciSorguParametreleri sorgu)
    {
        var sonuc = await _kiriciServisi.TumunuGetirAsync(sorgu);
        return Ok(sonuc);
    }

    [HttpGet("{ekipmanId:int}")]
    public async Task<IActionResult> EkipmanIdIleGetir(int ekipmanId)
    {
        var sonuc = await _kiriciServisi.EkipmanIdIleGetirAsync(ekipmanId);

        if (!sonuc.Success)
            return NotFound(sonuc);

        return Ok(sonuc);
    }

    [HttpPost]
    public async Task<IActionResult> Olustur([FromBody] KiriciOlusturDto dto)
    {
        var sonuc = await _kiriciServisi.OlusturAsync(dto);

        if (!sonuc.Success)
            return BadRequest(sonuc);

        return Ok(sonuc);
    }

    [HttpPut("{ekipmanId:int}")]
    public async Task<IActionResult> Guncelle(int ekipmanId, [FromBody] KiriciGuncelleDto dto)
    {
        var sonuc = await _kiriciServisi.GuncelleAsync(ekipmanId, dto);

        if (!sonuc.Success)
            return BadRequest(sonuc);

        return Ok(sonuc);
    }

    [HttpDelete("{ekipmanId:int}")]
    public async Task<IActionResult> Sil(int ekipmanId)
    {
        var sonuc = await _kiriciServisi.SilAsync(ekipmanId);

        if (!sonuc.Success)
            return NotFound(sonuc);

        return Ok(sonuc);
    }
}