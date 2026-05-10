using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Minemation.Application.Interfaces;
using Minemation.Domain.Entities;
using Minemation.Infrastructure.Persistence;

namespace Minemation.Infrastructure.Repositories;

public class EkipRepository : IEkipRepository
{
    private readonly MinemationDbContext _context;

    public EkipRepository(MinemationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Ekip>> TumunuGetirAsync()
    {
        return await _context.Ekip
            .AsNoTracking()
            .Include(e => e.Personel)
            .Include(e => e.Vardiya)
            .ToListAsync();
    }

    public async Task<Ekip?> IdIleGetirAsync(int id)
    {
        return await _context.Ekip
            .Include(e => e.Personel)
            .Include(e => e.Vardiya)
            .FirstOrDefaultAsync(e => e.ekipId == id);
    }

    public async Task EkleAsync(Ekip ekip)
    {
        await _context.Ekip.AddAsync(ekip);
    }

    public async Task DegisiklikleriKaydetAsync()
    {
        await _context.SaveChangesAsync();
    }
}