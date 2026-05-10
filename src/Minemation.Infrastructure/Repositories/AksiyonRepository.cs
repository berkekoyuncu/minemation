using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Minemation.Application.Interfaces;
using Minemation.Domain.Entities;
using Minemation.Infrastructure.Persistence;

namespace Minemation.Infrastructure.Repositories;

public class AksiyonRepository : IAksiyonRepository
{
    private readonly MinemationDbContext _context;

    public AksiyonRepository(MinemationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Aksiyon>> TumunuGetirAsync()
    {
        return await _context.Aksiyon
            .AsNoTracking()
            .Include(a => a.Ekip)
            .Include(a => a.Vaka)
            .ToListAsync();
    }

    public async Task<Aksiyon?> IdIleGetirAsync(int id)
    {
        return await _context.Aksiyon
            .Include(a => a.Ekip)
            .Include(a => a.Vaka)
            .FirstOrDefaultAsync(a => a.mudahaleId == id);
    }

    public async Task EkleAsync(Aksiyon aksiyon)
    {
        await _context.Aksiyon.AddAsync(aksiyon);
    }

    public async Task DegisiklikleriKaydetAsync()
    {
        await _context.SaveChangesAsync();
    }
}
