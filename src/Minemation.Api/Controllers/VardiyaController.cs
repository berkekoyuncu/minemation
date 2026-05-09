using Microsoft.AspNetCore.Mvc;
using Minemation.Application.DTOs;
using Minemation.Application.Interfaces;

namespace Minemation.Api.Controllers;

[ApiController]
[Route("api/vardiya")]
public class VardiyaController : ControllerBase
{
    private readonly IVardiyaServisi _vardiyaServisi;

    public VardiyaController(IVardiyaServisi vardiyaServisi)
    {
        _vardiyaServisi = vardiyaServisi;
    }

    [HttpGet]
    public async Task<IActionResult> TumunuGetir([FromQuery] VardiyaSorguParametreleri sorgu)
    {
        var sonuc = await _vardiyaServisi.TumunuGetirAsync(sorgu);
        return Ok(sonuc);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> IdIleGetir(int id)
    {
        var sonuc = await _vardiyaServisi.IdIleGetirAsync(id);

        if (!sonuc.Success)
            return NotFound(sonuc);

        return Ok(sonuc);
    }

    [HttpPost]
    public async Task<IActionResult> Olustur([FromBody] VardiyaOlusturDto dto)
    {
        var sonuc = await _vardiyaServisi.OlusturAsync(dto);

        if (!sonuc.Success)
            return BadRequest(sonuc);

        return Ok(sonuc);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Guncelle(int id, [FromBody] VardiyaGuncelleDto dto)
    {
        var sonuc = await _vardiyaServisi.GuncelleAsync(id, dto);

        if (!sonuc.Success)
            return BadRequest(sonuc);

        return Ok(sonuc);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Sil(int id)
    {
        var sonuc = await _vardiyaServisi.SilAsync(id);

        if (!sonuc.Success)
            return NotFound(sonuc);

        return Ok(sonuc);
    }
}
