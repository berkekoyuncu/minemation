using Microsoft.AspNetCore.Mvc;
using Minemation.Application.DTOs;
using Minemation.Application.Interfaces;

namespace Minemation.Api.Controllers;

[ApiController]
[Route("api/aksiyon")]
public class AksiyonController : ControllerBase
{
    private readonly IAksiyonServisi _aksiyonServisi;

    public AksiyonController(IAksiyonServisi aksiyonServisi)
    {
        _aksiyonServisi = aksiyonServisi;
    }

    [HttpGet]
    public async Task<IActionResult> TumunuGetir([FromQuery] AksiyonSorguParametreleri sorgu)
    {
        var sonuc = await _aksiyonServisi.TumunuGetirAsync(sorgu);
        return Ok(sonuc);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> IdIleGetir(int id)
    {
        var sonuc = await _aksiyonServisi.IdIleGetirAsync(id);

        if (!sonuc.Success)
            return NotFound(sonuc);

        return Ok(sonuc);
    }

    [HttpPost]
    public async Task<IActionResult> Olustur([FromBody] AksiyonOlusturDto dto)
    {
        var sonuc = await _aksiyonServisi.OlusturAsync(dto);

        if (!sonuc.Success)
            return BadRequest(sonuc);

        return Ok(sonuc);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Guncelle(int id, [FromBody] AksiyonGuncelleDto dto)
    {
        var sonuc = await _aksiyonServisi.GuncelleAsync(id, dto);

        if (!sonuc.Success)
            return BadRequest(sonuc);

        return Ok(sonuc);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Sil(int id)
    {
        var sonuc = await _aksiyonServisi.SilAsync(id);

        if (!sonuc.Success)
            return NotFound(sonuc);

        return Ok(sonuc);
    }
}