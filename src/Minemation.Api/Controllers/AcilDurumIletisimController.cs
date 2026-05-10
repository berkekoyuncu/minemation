using Microsoft.AspNetCore.Mvc;
using Minemation.Application.DTOs;
using Minemation.Application.Interfaces;

namespace Minemation.Api.Controllers;

[ApiController]
[Route("api/acil-durum-iletisim")]
public class AcilDurumIletisimController : ControllerBase
{
    private readonly IAcilDurumIletisimServisi _acilDurumIletisimServisi;

    public AcilDurumIletisimController(IAcilDurumIletisimServisi acilDurumIletisimServisi)
    {
        _acilDurumIletisimServisi = acilDurumIletisimServisi;
    }

    [HttpGet]
    public async Task<IActionResult> TumunuGetir([FromQuery] AcilDurumIletisimSorguParametreleri sorgu)
    {
        var sonuc = await _acilDurumIletisimServisi.TumunuGetirAsync(sorgu);
        return Ok(sonuc);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> IdIleGetir(int id)
    {
        var sonuc = await _acilDurumIletisimServisi.IdIleGetirAsync(id);

        if (!sonuc.Success)
            return NotFound(sonuc);

        return Ok(sonuc);
    }

    [HttpPost]
    public async Task<IActionResult> Olustur([FromBody] AcilDurumIletisimOlusturDto dto)
    {
        var sonuc = await _acilDurumIletisimServisi.OlusturAsync(dto);

        if (!sonuc.Success)
            return BadRequest(sonuc);

        return Ok(sonuc);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Guncelle(int id, [FromBody] AcilDurumIletisimGuncelleDto dto)
    {
        var sonuc = await _acilDurumIletisimServisi.GuncelleAsync(id, dto);

        if (!sonuc.Success)
            return BadRequest(sonuc);

        return Ok(sonuc);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Sil(int id)
    {
        var sonuc = await _acilDurumIletisimServisi.SilAsync(id);

        if (!sonuc.Success)
            return NotFound(sonuc);

        return Ok(sonuc);
    }
}