using Microsoft.AspNetCore.Mvc;
using Minemation.Application.DTOs;
using Minemation.Application.Interfaces;

namespace Minemation.Api.Controllers;

[ApiController]
[Route("api/log-kaydi")]
public class LogKaydiController : ControllerBase
{
    private readonly ILogKaydiServisi _logKaydiServisi;

    public LogKaydiController(ILogKaydiServisi logKaydiServisi)
    {
        _logKaydiServisi = logKaydiServisi;
    }

    [HttpGet]
    public async Task<IActionResult> TumunuGetir([FromQuery] LogKaydiSorguParametreleri sorgu)
    {
        var sonuc = await _logKaydiServisi.TumunuGetirAsync(sorgu);
        return Ok(sonuc);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> IdIleGetir(int id)
    {
        var sonuc = await _logKaydiServisi.IdIleGetirAsync(id);

        if (!sonuc.Success)
            return NotFound(sonuc);

        return Ok(sonuc);
    }

    [HttpPost]
    public async Task<IActionResult> Olustur([FromBody] LogKaydiOlusturDto dto)
    {
        var sonuc = await _logKaydiServisi.OlusturAsync(dto);

        if (!sonuc.Success)
            return BadRequest(sonuc);

        return Ok(sonuc);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Guncelle(int id, [FromBody] LogKaydiGuncelleDto dto)
    {
        var sonuc = await _logKaydiServisi.GuncelleAsync(id, dto);

        if (!sonuc.Success)
            return BadRequest(sonuc);

        return Ok(sonuc);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Sil(int id)
    {
        var sonuc = await _logKaydiServisi.SilAsync(id);

        if (!sonuc.Success)
            return NotFound(sonuc);

        return Ok(sonuc);
    }
}
