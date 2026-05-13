using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Minemation.Application.Interfaces;
using Minemation.Domain.Entities;
using Minemation.Infrastructure.Persistence;

namespace Minemation.Infrastructure.Repositories;

public class ElAletleriRepository : IElAletleriRepository
{
    private readonly MinemationDbContext _context;

    public ElAletleriRepository(MinemationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ElAletleri>> TumunuGetirAsync()
    {
        return await _context.ElAletleri
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<ElAletleri?> EkipmanIdIleGetirAsync(int ekipmanId)
    {
        return await _context.ElAletleri
            .FirstOrDefaultAsync(e => e.ekipmanId == ekipmanId);
    }

    public async Task<bool> VarMiAsync(int ekipmanId)
    {
        return await _context.ElAletleri
            .AnyAsync(e => e.ekipmanId == ekipmanId);
    }

    public async Task EkleAsync(ElAletleri elAletleri)
    {
        await _context.ElAletleri.AddAsync(elAletleri);
    }

    public async Task DegisiklikleriKaydetAsync()
    {
        await _context.SaveChangesAsync();
    }
}
