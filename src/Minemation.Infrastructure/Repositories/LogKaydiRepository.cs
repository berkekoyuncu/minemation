using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Minemation.Application.Interfaces;
using Minemation.Domain.Entities;
using Minemation.Infrastructure.Persistence;

namespace Minemation.Infrastructure.Repositories;

public class LogKaydiRepository : ILogKaydiRepository
{
    private readonly MinemationDbContext _context;

    public LogKaydiRepository(MinemationDbContext context)
    {
        _context = context;
    }

    public async Task<List<LogKaydi>> TumunuGetirAsync()
    {
        return await _context.LogKaydi
            .AsNoTracking()
            .Include(l => l.Personel)
            .Include(l => l.Ekipman)
            .ToListAsync();
    }

    public async Task<LogKaydi?> IdIleGetirAsync(int id)
    {
        return await _context.LogKaydi
            .Include(l => l.Personel)
            .Include(l => l.Ekipman)
            .FirstOrDefaultAsync(l => l.logKaydiID == id);
    }

    public async Task EkleAsync(LogKaydi logKaydi)
    {
        await _context.LogKaydi.AddAsync(logKaydi);
    }

    public async Task DegisiklikleriKaydetAsync()
    {
        await _context.SaveChangesAsync();
    }
}
