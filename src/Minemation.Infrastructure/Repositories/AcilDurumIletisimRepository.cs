using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Minemation.Application.Interfaces;
using Minemation.Domain.Entities;
using Minemation.Infrastructure.Persistence;

namespace Minemation.Infrastructure.Repositories;

public class AcilDurumIletisimRepository : IAcilDurumIletisimRepository
{
    private readonly MinemationDbContext _context;

    public AcilDurumIletisimRepository(MinemationDbContext context)
    {
        _context = context;
    }

    public async Task<List<AcilDurumIletisim>> TumunuGetirAsync()
    {
        return await _context.AcilDurumIletisim
            .AsNoTracking()
            .Include(a => a.Personel)
            .ToListAsync();
    }

    public async Task<AcilDurumIletisim?> IdIleGetirAsync(int id)
    {
        return await _context.AcilDurumIletisim
            .Include(a => a.Personel)
            .FirstOrDefaultAsync(a => a.acilDurumKisisiId == id);
    }

    public async Task EkleAsync(AcilDurumIletisim acilDurumIletisim)
    {
        await _context.AcilDurumIletisim.AddAsync(acilDurumIletisim);
    }

    public async Task DegisiklikleriKaydetAsync()
    {
        await _context.SaveChangesAsync();
    }
}