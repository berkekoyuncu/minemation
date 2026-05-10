using Microsoft.AspNetCore.Mvc;
using Minemation.Application.DTOs;
using Minemation.Application.Interfaces;

namespace Minemation.Api.Controllers;

[ApiController]
[Route("api/personel-raporu")]
public class PersonelRaporuController : ControllerBase
{
    private readonly IPersonelRaporuServisi _personelRaporuServisi;

    public PersonelRaporuController(IPersonelRaporuServisi personelRaporuServisi)
    {
        _personelRaporuServisi = personelRaporuServisi;
    }

    [HttpGet]
    public async Task<IActionResult> TumunuGetir([FromQuery] PersonelRaporuSorguParametreleri sorgu)
    {
        var sonuc = await _personelRaporuServisi.TumunuGetirAsync(sorgu);
        return Ok(sonuc);
    }

    [HttpGet("{raporId:int}")]
    public async Task<IActionResult> RaporIdIleGetir(int raporId)
    {
        var sonuc = await _personelRaporuServisi.RaporIdIleGetirAsync(raporId);

        if (!sonuc.Success)
            return NotFound(sonuc);

        return Ok(sonuc);
    }

    [HttpPost]
    public async Task<IActionResult> Olustur([FromBody] PersonelRaporuOlusturDto dto)
    {
        var sonuc = await _personelRaporuServisi.OlusturAsync(dto);

        if (!sonuc.Success)
            return BadRequest(sonuc);

        return Ok(sonuc);
    }

    [HttpPut("{raporId:int}")]
    public async Task<IActionResult> Guncelle(int raporId, [FromBody] PersonelRaporuGuncelleDto dto)
    {
        var sonuc = await _personelRaporuServisi.GuncelleAsync(raporId, dto);

        if (!sonuc.Success)
            return BadRequest(sonuc);

        return Ok(sonuc);
    }

    [HttpDelete("{raporId:int}")]
    public async Task<IActionResult> Sil(int raporId)
    {
        var sonuc = await _personelRaporuServisi.SilAsync(raporId);

        if (!sonuc.Success)
            return NotFound(sonuc);

        return Ok(sonuc);
    }
}
