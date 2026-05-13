using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Minemation.Application.Interfaces;
using Minemation.Domain.Entities;
using Minemation.Infrastructure.Persistence;

namespace Minemation.Infrastructure.Repositories;

public class SensorRepository : ISensorRepository
{
    private readonly MinemationDbContext _context;

    public SensorRepository(MinemationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Sensor>> TumunuGetirAsync()
    {
        return await _context.Sensor
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Sensor?> EkipmanIdIleGetirAsync(int ekipmanId)
    {
        return await _context.Sensor
            .FirstOrDefaultAsync(s => s.ekipmanId == ekipmanId);
    }

    public async Task<bool> EkipmandaSensorVarMiAsync(int ekipmanId)
    {
        return await _context.Sensor
            .AnyAsync(s => s.ekipmanId == ekipmanId);
    }

    public async Task EkleAsync(Sensor sensor)
    {
        await _context.Sensor.AddAsync(sensor);
    }

    public async Task DegisiklikleriKaydetAsync()
    {
        await _context.SaveChangesAsync();
    }
}
