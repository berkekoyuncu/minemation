using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Minemation.Application.Interfaces;
using Minemation.Domain.Entities;
using Minemation.Infrastructure.Persistence;

namespace Minemation.Infrastructure.Repositories;

public class PersonelRaporuRepository : IPersonelRaporuRepository
{
    private readonly MinemationDbContext _context;

    public PersonelRaporuRepository(MinemationDbContext context)
    {
        _context = context;
    }

    public async Task<List<PersonelRaporu>> TumunuGetirAsync()
    {
        return await _context.PersonelRaporu
            .AsNoTracking()
            .Include(pr => pr.Rapor)
            .ToListAsync();
    }

    public async Task<PersonelRaporu?> RaporIdIleGetirAsync(int raporId)
    {
        return await _context.PersonelRaporu
            .Include(pr => pr.Rapor)
            .FirstOrDefaultAsync(pr => pr.raporId == raporId);
    }

    public async Task<bool> VarMiAsync(int raporId)
    {
        return await _context.PersonelRaporu
            .AnyAsync(pr => pr.raporId == raporId);
    }

    public async Task EkleAsync(PersonelRaporu personelRaporu)
    {
        await _context.PersonelRaporu.AddAsync(personelRaporu);
    }

    public async Task DegisiklikleriKaydetAsync()
    {
        await _context.SaveChangesAsync();
    }
}