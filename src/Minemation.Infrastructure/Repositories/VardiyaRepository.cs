using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Minemation.Application.Interfaces;
using Minemation.Domain.Entities;
using Minemation.Infrastructure.Persistence;

namespace Minemation.Infrastructure.Repositories;

public class VardiyaRepository : IVardiyaRepository
{
    private readonly MinemationDbContext _context;

    public VardiyaRepository(MinemationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Vardiya>> TumunuGetirAsync()
    {
        return await _context.Vardiya
            .AsNoTracking()
            .Include(v => v.Ekipman)
            .Include(v => v.VardiyaSorumlusuPersonel)
            .Include(v => v.IsgSorumlusuPersonel)
            .Include(v => v.TeknikSorumlusuPersonel)
            .ToListAsync();
    }

    public async Task<Vardiya?> IdIleGetirAsync(int id)
    {
        return await _context.Vardiya
            .Include(v => v.Ekipman)
            .Include(v => v.VardiyaSorumlusuPersonel)
            .Include(v => v.IsgSorumlusuPersonel)
            .Include(v => v.TeknikSorumlusuPersonel)
            .FirstOrDefaultAsync(v => v.vardiyaId == id);
    }

    public async Task EkleAsync(Vardiya vardiya)
    {
        await _context.Vardiya.AddAsync(vardiya);
    }

    public async Task DegisiklikleriKaydetAsync()
    {
        await _context.SaveChangesAsync();
    }
}