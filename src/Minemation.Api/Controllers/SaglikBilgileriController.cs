using Microsoft.AspNetCore.Mvc;
using Minemation.Application.DTOs;
using Minemation.Application.Interfaces;

namespace Minemation.Api.Controllers;

[ApiController]
[Route("api/saglik-bilgileri")]
public class SaglikBilgileriController : ControllerBase
{
    private readonly ISaglikBilgileriServisi _saglikBilgileriServisi;

    public SaglikBilgileriController(ISaglikBilgileriServisi saglikBilgileriServisi)
    {
        _saglikBilgileriServisi = saglikBilgileriServisi;
    }

    [HttpGet]
    public async Task<IActionResult> TumunuGetir([FromQuery] SaglikBilgileriSorguParametreleri sorgu)
    {
        var sonuc = await _saglikBilgileriServisi.TumunuGetirAsync(sorgu);
        return Ok(sonuc);
    }

    [HttpGet("{personelId:int}")]
    public async Task<IActionResult> PersonelIdIleGetir(int personelId)
    {
        var sonuc = await _saglikBilgileriServisi.PersonelIdIleGetirAsync(personelId);

        if (!sonuc.Success)
            return NotFound(sonuc);

        return Ok(sonuc);
    }

    [HttpPost]
    public async Task<IActionResult> Olustur([FromBody] SaglikBilgileriOlusturDto dto)
    {
        var sonuc = await _saglikBilgileriServisi.OlusturAsync(dto);

        if (!sonuc.Success)
            return BadRequest(sonuc);

        return Ok(sonuc);
    }

    [HttpPut("{personelId:int}")]
    public async Task<IActionResult> Guncelle(int personelId, [FromBody] SaglikBilgileriGuncelleDto dto)
    {
        var sonuc = await _saglikBilgileriServisi.GuncelleAsync(personelId, dto);

        if (!sonuc.Success)
            return BadRequest(sonuc);

        return Ok(sonuc);
    }

    [HttpDelete("{personelId:int}")]
    public async Task<IActionResult> Sil(int personelId)
    {
        var sonuc = await _saglikBilgileriServisi.SilAsync(personelId);

        if (!sonuc.Success)
            return NotFound(sonuc);

        return Ok(sonuc);
    }
}