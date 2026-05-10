using Microsoft.AspNetCore.Mvc;
using Minemation.Application.DTOs;
using Minemation.Application.Interfaces;

namespace Minemation.Api.Controllers;

[ApiController]
[Route("api/takip-cihazi")]
public class TakipCihaziController : ControllerBase
{
    private readonly ITakipCihaziServisi _takipCihaziServisi;

    public TakipCihaziController(ITakipCihaziServisi takipCihaziServisi)
    {
        _takipCihaziServisi = takipCihaziServisi;
    }

    [HttpGet]
    public async Task<IActionResult> TumunuGetir([FromQuery] TakipCihaziSorguParametreleri sorgu)
    {
        var sonuc = await _takipCihaziServisi.TumunuGetirAsync(sorgu);
        return Ok(sonuc);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> IdIleGetir(int id)
    {
        var sonuc = await _takipCihaziServisi.IdIleGetirAsync(id);

        if (!sonuc.Success)
            return NotFound(sonuc);

        return Ok(sonuc);
    }

    [HttpPost]
    public async Task<IActionResult> Olustur([FromBody] TakipCihaziOlusturDto dto)
    {
        var sonuc = await _takipCihaziServisi.OlusturAsync(dto);

        if (!sonuc.Success)
            return BadRequest(sonuc);

        return Ok(sonuc);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Guncelle(int id, [FromBody] TakipCihaziGuncelleDto dto)
    {
        var sonuc = await _takipCihaziServisi.GuncelleAsync(id, dto);

        if (!sonuc.Success)
            return BadRequest(sonuc);

        return Ok(sonuc);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Sil(int id)
    {
        var sonuc = await _takipCihaziServisi.SilAsync(id);

        if (!sonuc.Success)
            return NotFound(sonuc);

        return Ok(sonuc);
    }
}
