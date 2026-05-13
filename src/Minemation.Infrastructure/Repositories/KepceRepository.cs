using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Minemation.Application.Interfaces;
using Minemation.Domain.Entities;
using Minemation.Infrastructure.Persistence;

namespace Minemation.Infrastructure.Repositories;

public class KepceRepository : IKepceRepository
{
    private readonly MinemationDbContext _context;

    public KepceRepository(MinemationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Kepce>> TumunuGetirAsync()
    {
        return await _context.Kepce
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Kepce?> EkipmanIdIleGetirAsync(int ekipmanId)
    {
        return await _context.Kepce
            .FirstOrDefaultAsync(k => k.ekipmanId == ekipmanId);
    }

    public async Task<bool> VarMiAsync(int ekipmanId)
    {
        return await _context.Kepce
            .AnyAsync(k => k.ekipmanId == ekipmanId);
    }

    public async Task<bool> PlakaVarMiAsync(string plaka, int? haricTutulacakEkipmanId = null)
    {
        var sorgu = _context.Kepce
            .AsQueryable()
            .Where(k => k.plaka == plaka);

        if (haricTutulacakEkipmanId.HasValue)
        {
            sorgu = sorgu.Where(k => k.ekipmanId != haricTutulacakEkipmanId.Value);
        }

        return await sorgu.AnyAsync();
    }

    public async Task EkleAsync(Kepce kepce)
    {
        await _context.Kepce.AddAsync(kepce);
    }

    public async Task DegisiklikleriKaydetAsync()
    {
        await _context.SaveChangesAsync();
    }
}
