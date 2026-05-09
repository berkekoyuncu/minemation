using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Minemation.Application.Interfaces;
using Minemation.Domain.Entities;
using Minemation.Infrastructure.Persistence;

namespace Minemation.Infrastructure.Repositories;

public class EkipmanRepository : IEkipmanRepository
{
    private readonly MinemationDbContext _context;

    public EkipmanRepository(MinemationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Ekipman>> TumunuGetirAsync()
    {
        return await _context.Ekipman
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Ekipman?> IdIleGetirAsync(int id)
    {
        return await _context.Ekipman
            .FirstOrDefaultAsync(e => e.ekipmanId == id);
    }

    public async Task<bool> SeriNoVarMiAsync(string seriNo, int? haricTutulacakId = null)
    {
        var sorgu = _context.Ekipman
            .AsQueryable()
            .Where(e => e.seriNo == seriNo);

        if (haricTutulacakId.HasValue)
        {
            sorgu = sorgu.Where(e => e.ekipmanId != haricTutulacakId.Value);
        }

        return await sorgu.AnyAsync();
    }

    public async Task EkleAsync(Ekipman ekipman)
    {
        await _context.Ekipman.AddAsync(ekipman);
    }

    public async Task DegisiklikleriKaydetAsync()
    {
        await _context.SaveChangesAsync();
    }
}