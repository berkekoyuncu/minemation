using Microsoft.AspNetCore.Mvc;
using Minemation.Application.DTOs;
using Minemation.Application.Interfaces;

namespace Minemation.Api.Controllers;

[ApiController]
[Route("api/ekipman")]
public class EkipmanController : ControllerBase
{
    private readonly IEkipmanServisi _ekipmanServisi;

    public EkipmanController(IEkipmanServisi ekipmanServisi)
    {
        _ekipmanServisi = ekipmanServisi;
    }

    [HttpGet]
    public async Task<IActionResult> TumunuGetir([FromQuery] EkipmanSorguParametreleri sorgu)
    {
        var sonuc = await _ekipmanServisi.TumunuGetirAsync(sorgu);
        return Ok(sonuc);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> IdIleGetir(int id)
    {
        var sonuc = await _ekipmanServisi.IdIleGetirAsync(id);

        if (!sonuc.Success)
            return NotFound(sonuc);

        return Ok(sonuc);
    }

    [HttpPost]
    public async Task<IActionResult> Olustur([FromBody] EkipmanOlusturDto dto)
    {
        var sonuc = await _ekipmanServisi.OlusturAsync(dto);

        if (!sonuc.Success)
            return BadRequest(sonuc);

        return Ok(sonuc);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Guncelle(int id, [FromBody] EkipmanGuncelleDto dto)
    {
        var sonuc = await _ekipmanServisi.GuncelleAsync(id, dto);

        if (!sonuc.Success)
            return BadRequest(sonuc);

        return Ok(sonuc);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Sil(int id)
    {
        var sonuc = await _ekipmanServisi.SilAsync(id);

        if (!sonuc.Success)
            return NotFound(sonuc);

        return Ok(sonuc);
    }
}
