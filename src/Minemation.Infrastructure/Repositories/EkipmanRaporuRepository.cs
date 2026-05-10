using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Minemation.Application.Interfaces;
using Minemation.Domain.Entities;
using Minemation.Infrastructure.Persistence;

namespace Minemation.Infrastructure.Repositories;

public class EkipmanRaporuRepository : IEkipmanRaporuRepository
{
    private readonly MinemationDbContext _context;

    public EkipmanRaporuRepository(MinemationDbContext context)
    {
        _context = context;
    }

    public async Task<List<EkipmanRaporu>> TumunuGetirAsync()
    {
        return await _context.EkipmanRaporu
            .AsNoTracking()
            .Include(er => er.Rapor)
            .ToListAsync();
    }

    public async Task<EkipmanRaporu?> RaporIdIleGetirAsync(int raporId)
    {
        return await _context.EkipmanRaporu
            .Include(er => er.Rapor)
            .FirstOrDefaultAsync(er => er.raporId == raporId);
    }

    public async Task<bool> VarMiAsync(int raporId)
    {
        return await _context.EkipmanRaporu
            .AnyAsync(er => er.raporId == raporId);
    }

    public async Task EkleAsync(EkipmanRaporu ekipmanRaporu)
    {
        await _context.EkipmanRaporu.AddAsync(ekipmanRaporu);
    }

    public async Task DegisiklikleriKaydetAsync()
    {
        await _context.SaveChangesAsync();
    }
}
