using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Minemation.Application.Interfaces;
using Minemation.Domain.Entities;
using Minemation.Infrastructure.Persistence;

namespace Minemation.Infrastructure.Repositories;

public class TakipCihaziRepository : ITakipCihaziRepository
{
    private readonly MinemationDbContext _context;

    public TakipCihaziRepository(MinemationDbContext context)
    {
        _context = context;
    }

    public async Task<List<TakipCihazi>> TumunuGetirAsync()
    {
        return await _context.TakipCihazi
            .AsNoTracking()
            .Include(t => t.Personel)
            .ToListAsync();
    }

    public async Task<TakipCihazi?> EkipmanIdIleGetirAsync(int ekipmanId)
    {
        return await _context.TakipCihazi
            .Include(t => t.Personel)
            .FirstOrDefaultAsync(t => t.ekipmanId == ekipmanId);
    }

    public async Task<bool> SeriNoVarMiAsync(string seriNo, int? haricTutulacakId = null)
    {
        var sorgu = _context.TakipCihazi
            .AsQueryable()
            .Where(t => t.takipCihaziSeriNo == seriNo);

        if (haricTutulacakId.HasValue)
        {
            sorgu = sorgu.Where(t => t.takipCihaziId != haricTutulacakId.Value);
        }

        return await sorgu.AnyAsync();
    }

    public async Task EkleAsync(TakipCihazi takipCihazi)
    {
        await _context.TakipCihazi.AddAsync(takipCihazi);
    }

    public async Task DegisiklikleriKaydetAsync()
    {
        await _context.SaveChangesAsync();
    }
}
