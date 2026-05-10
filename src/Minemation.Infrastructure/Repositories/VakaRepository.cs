using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Minemation.Application.Interfaces;
using Minemation.Domain.Entities;
using Minemation.Infrastructure.Persistence;

namespace Minemation.Infrastructure.Repositories;

public class VakaRepository : IVakaRepository
{
    private readonly MinemationDbContext _context;

    public VakaRepository(MinemationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Vaka>> TumunuGetirAsync()
    {
        return await _context.Vaka
            .AsNoTracking()
            .Include(v => v.Personel)
            .Include(v => v.RaporlayanEkipman)
            .Include(v => v.IlgiliEkipman)
            .ToListAsync();
    }

    public async Task<Vaka?> IdIleGetirAsync(int id)
    {
        return await _context.Vaka
            .Include(v => v.Personel)
            .Include(v => v.RaporlayanEkipman)
            .Include(v => v.IlgiliEkipman)
            .FirstOrDefaultAsync(v => v.vakaId == id);
    }

    public async Task EkleAsync(Vaka vaka)
    {
        await _context.Vaka.AddAsync(vaka);
    }

    public async Task DegisiklikleriKaydetAsync()
    {
        await _context.SaveChangesAsync();
    }
}
