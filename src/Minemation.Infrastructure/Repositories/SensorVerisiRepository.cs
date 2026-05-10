using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Minemation.Application.Interfaces;
using Minemation.Domain.Entities;
using Minemation.Infrastructure.Persistence;

namespace Minemation.Infrastructure.Repositories;

public class SensorVerisiRepository : ISensorVerisiRepository
{
    private readonly MinemationDbContext _context;

    public SensorVerisiRepository(MinemationDbContext context)
    {
        _context = context;
    }

    public async Task<List<SensorVerisi>> TumunuGetirAsync()
    {
        return await _context.SensorVerisi
            .AsNoTracking()
            .Include(sv => sv.Sensor)
                .ThenInclude(s => s.Ekipman)
            .Include(sv => sv.Vardiya)
            .ToListAsync();
    }

    public async Task<SensorVerisi?> IdIleGetirAsync(int id)
    {
        return await _context.SensorVerisi
            .Include(sv => sv.Sensor)
                .ThenInclude(s => s.Ekipman)
            .Include(sv => sv.Vardiya)
            .FirstOrDefaultAsync(sv => sv.sensorVerisiId == id);
    }

    public async Task EkleAsync(SensorVerisi sensorVerisi)
    {
        await _context.SensorVerisi.AddAsync(sensorVerisi);
    }

    public async Task DegisiklikleriKaydetAsync()
    {
        await _context.SaveChangesAsync();
    }
}
