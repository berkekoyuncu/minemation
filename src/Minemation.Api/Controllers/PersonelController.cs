using Microsoft.AspNetCore.Mvc;
using Minemation.Application.DTOs;
using Minemation.Application.Interfaces;

namespace Minemation.Api.Controllers;

[ApiController]
[Route("api/personel")]
public class PersonelController : ControllerBase
{
    private readonly IPersonelServisi _personelServisi;

    public PersonelController(IPersonelServisi personelServisi)
    {
        _personelServisi = personelServisi;
    }

    [HttpGet]
    public async Task<IActionResult> TumunuGetir([FromQuery] PersonelSorguParametreleri sorgu)
    {
        var sonuc = await _personelServisi.TumunuGetirAsync(sorgu);
        return Ok(sonuc);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> IdIleGetir(int id)
    {
        var sonuc = await _personelServisi.IdIleGetirAsync(id);

        if (!sonuc.Success)
            return NotFound(sonuc);

        return Ok(sonuc);
    }

    [HttpPost]
    public async Task<IActionResult> Olustur([FromBody] PersonelOlusturDto dto)
    {
        var sonuc = await _personelServisi.OlusturAsync(dto);

        if (!sonuc.Success)
            return BadRequest(sonuc);

        return Ok(sonuc);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Guncelle(int id, [FromBody] PersonelGuncelleDto dto)
    {
        var sonuc = await _personelServisi.GuncelleAsync(id, dto);

        if (!sonuc.Success)
            return BadRequest(sonuc);

        return Ok(sonuc);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Sil(int id)
    {
        var sonuc = await _personelServisi.SilAsync(id);

        if (!sonuc.Success)
            return NotFound(sonuc);

        return Ok(sonuc);
    }
}