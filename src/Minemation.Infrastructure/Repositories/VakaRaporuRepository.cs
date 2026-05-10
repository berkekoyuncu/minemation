using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Minemation.Application.Interfaces;
using Minemation.Domain.Entities;
using Minemation.Infrastructure.Persistence;

namespace Minemation.Infrastructure.Repositories;

public class VakaRaporuRepository : IVakaRaporuRepository
{
    private readonly MinemationDbContext _context;

    public VakaRaporuRepository(MinemationDbContext context)
    {
        _context = context;
    }

    public async Task<List<VakaRaporu>> TumunuGetirAsync()
    {
        return await _context.VakaRaporu
            .AsNoTracking()
            .Include(vr => vr.Rapor)
            .Include(vr => vr.Personel)
            .Include(vr => vr.RaporlayanEkipman)
            .ToListAsync();
    }

    public async Task<VakaRaporu?> RaporIdIleGetirAsync(int raporId)
    {
        return await _context.VakaRaporu
            .Include(vr => vr.Rapor)
            .Include(vr => vr.Personel)
            .Include(vr => vr.RaporlayanEkipman)
            .FirstOrDefaultAsync(vr => vr.raporId == raporId);
    }

    public async Task<bool> VarMiAsync(int raporId)
    {
        return await _context.VakaRaporu
            .AnyAsync(vr => vr.raporId == raporId);
    }

    public async Task EkleAsync(VakaRaporu vakaRaporu)
    {
        await _context.VakaRaporu.AddAsync(vakaRaporu);
    }

    public async Task DegisiklikleriKaydetAsync()
    {
        await _context.SaveChangesAsync();
    }
}
