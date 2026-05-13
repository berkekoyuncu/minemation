using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Minemation.Application.Interfaces;
using Minemation.Domain.Entities;
using Minemation.Infrastructure.Persistence;

namespace Minemation.Infrastructure.Repositories;

public class KiriciRepository : IKiriciRepository
{
    private readonly MinemationDbContext _context;

    public KiriciRepository(MinemationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Kirici>> TumunuGetirAsync()
    {
        return await _context.Kirici
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Kirici?> EkipmanIdIleGetirAsync(int ekipmanId)
    {
        return await _context.Kirici
            .FirstOrDefaultAsync(k => k.ekipmanId == ekipmanId);
    }

    public async Task<bool> VarMiAsync(int ekipmanId)
    {
        return await _context.Kirici
            .AnyAsync(k => k.ekipmanId == ekipmanId);
    }

    public async Task EkleAsync(Kirici kirici)
    {
        await _context.Kirici.AddAsync(kirici);
    }

    public async Task DegisiklikleriKaydetAsync()
    {
        await _context.SaveChangesAsync();
    }
}
