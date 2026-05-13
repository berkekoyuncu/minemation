using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Minemation.Application.Interfaces;
using Minemation.Domain.Entities;
using Minemation.Infrastructure.Persistence;

namespace Minemation.Infrastructure.Repositories;

public class HafriyatRepository : IHafriyatRepository
{
    private readonly MinemationDbContext _context;

    public HafriyatRepository(MinemationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Hafriyat>> TumunuGetirAsync()
    {
        return await _context.Hafriyat
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Hafriyat?> EkipmanIdIleGetirAsync(int ekipmanId)
    {
        return await _context.Hafriyat
            .FirstOrDefaultAsync(h => h.ekipmanId == ekipmanId);
    }

    public async Task<bool> VarMiAsync(int ekipmanId)
    {
        return await _context.Hafriyat
            .AnyAsync(h => h.ekipmanId == ekipmanId);
    }

    public async Task<bool> PlakaVarMiAsync(string plaka, int? haricTutulacakEkipmanId = null)
    {
        var sorgu = _context.Hafriyat
            .AsQueryable()
            .Where(h => h.plaka == plaka);

        if (haricTutulacakEkipmanId.HasValue)
        {
            sorgu = sorgu.Where(h => h.ekipmanId != haricTutulacakEkipmanId.Value);
        }

        return await sorgu.AnyAsync();
    }

    public async Task EkleAsync(Hafriyat hafriyat)
    {
        await _context.Hafriyat.AddAsync(hafriyat);
    }

    public async Task DegisiklikleriKaydetAsync()
    {
        await _context.SaveChangesAsync();
    }
}
