using Microsoft.AspNetCore.Mvc;
using Minemation.Application.DTOs;
using Minemation.Application.Interfaces;

namespace Minemation.Api.Controllers;

[ApiController]
[Route("api/sensor-verisi")]
public class SensorVerisiController : ControllerBase
{
    private readonly ISensorVerisiServisi _sensorVerisiServisi;

    public SensorVerisiController(ISensorVerisiServisi sensorVerisiServisi)
    {
        _sensorVerisiServisi = sensorVerisiServisi;
    }

    [HttpGet]
    public async Task<IActionResult> TumunuGetir([FromQuery] SensorVerisiSorguParametreleri sorgu)
    {
        var sonuc = await _sensorVerisiServisi.TumunuGetirAsync(sorgu);
        return Ok(sonuc);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> IdIleGetir(int id)
    {
        var sonuc = await _sensorVerisiServisi.IdIleGetirAsync(id);

        if (!sonuc.Success)
            return NotFound(sonuc);

        return Ok(sonuc);
    }

    [HttpPost]
    public async Task<IActionResult> Olustur([FromBody] SensorVerisiOlusturDto dto)
    {
        var sonuc = await _sensorVerisiServisi.OlusturAsync(dto);

        if (!sonuc.Success)
            return BadRequest(sonuc);

        return Ok(sonuc);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Guncelle(int id, [FromBody] SensorVerisiGuncelleDto dto)
    {
        var sonuc = await _sensorVerisiServisi.GuncelleAsync(id, dto);

        if (!sonuc.Success)
            return BadRequest(sonuc);

        return Ok(sonuc);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Sil(int id)
    {
        var sonuc = await _sensorVerisiServisi.SilAsync(id);

        if (!sonuc.Success)
            return NotFound(sonuc);

        return Ok(sonuc);
    }
}
