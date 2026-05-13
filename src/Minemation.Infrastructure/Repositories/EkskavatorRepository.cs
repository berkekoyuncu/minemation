using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Minemation.Application.Interfaces;
using Minemation.Domain.Entities;
using Minemation.Infrastructure.Persistence;

namespace Minemation.Infrastructure.Repositories;

public class EkskavatorRepository : IEkskavatorRepository
{
    private readonly MinemationDbContext _context;

    public EkskavatorRepository(MinemationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Ekskavator>> TumunuGetirAsync()
    {
        return await _context.Ekskavator
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Ekskavator?> EkipmanIdIleGetirAsync(int ekipmanId)
    {
        return await _context.Ekskavator
            .FirstOrDefaultAsync(e => e.ekipmanId == ekipmanId);
    }

    public async Task<bool> VarMiAsync(int ekipmanId)
    {
        return await _context.Ekskavator
            .AnyAsync(e => e.ekipmanId == ekipmanId);
    }

    public async Task<bool> PlakaVarMiAsync(string plaka, int? haricTutulacakEkipmanId = null)
    {
        var sorgu = _context.Ekskavator
            .AsQueryable()
            .Where(e => e.plaka == plaka);

        if (haricTutulacakEkipmanId.HasValue)
        {
            sorgu = sorgu.Where(e => e.ekipmanId != haricTutulacakEkipmanId.Value);
        }

        return await sorgu.AnyAsync();
    }

    public async Task EkleAsync(Ekskavator ekskavator)
    {
        await _context.Ekskavator.AddAsync(ekskavator);
    }

    public async Task DegisiklikleriKaydetAsync()
    {
        await _context.SaveChangesAsync();
    }
}
