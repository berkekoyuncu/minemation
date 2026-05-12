using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Minemation.Application.Interfaces;
using Minemation.Domain.Entities;
using Minemation.Infrastructure.Persistence;

namespace Minemation.Infrastructure.Repositories;

public class RaporRepository : IRaporRepository
{
    private readonly MinemationDbContext _context;

    public RaporRepository(MinemationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Rapor>> TumunuGetirAsync()
    {
        return await _context.Rapor
            .AsNoTracking()
            .Include(r => r.Personel)
            .Include(r => r.Ekipman)
            .ToListAsync();
    }

    public async Task<Rapor?> IdIleGetirAsync(int id)
    {
        return await _context.Rapor
            .Include(r => r.Personel)
            .Include(r => r.Ekipman)
            .FirstOrDefaultAsync(r => r.raporId == id);
    }

    public async Task EkleAsync(Rapor rapor)
    {
        await _context.Rapor.AddAsync(rapor);
    }

    public void Sil(Rapor rapor)
    {
        _context.Rapor.Remove(rapor);
    }

    public async Task DegisiklikleriKaydetAsync()
    {
        await _context.SaveChangesAsync();
    }
}
