using Microsoft.EntityFrameworkCore;
using Minemation.Application.Interfaces;
using Minemation.Domain.Entities;
using Minemation.Infrastructure.Persistence;

namespace Minemation.Infrastructure.Repositories;

public class PersonelRepository : IPersonelRepository
{
    private readonly MinemationDbContext _context;

    public PersonelRepository(MinemationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Personel>> TumunuGetirAsync()
    {
        return await _context.Personeller
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Personel?> IdIleGetirAsync(int id)
    {
        return await _context.Personeller
            .FirstOrDefaultAsync(p => p.personelId == id);
    }

    public async Task<bool> TcknVarMiAsync(string tckn, int? haricTutulacakId = null)
    {
        var sorgu = _context.Personeller
            .AsQueryable()
            .Where(p => p.tckn == tckn);

        if (haricTutulacakId.HasValue)
        {
            sorgu = sorgu.Where(p => p.personelId != haricTutulacakId.Value);
        }

        return await sorgu.AnyAsync();
    }

    public async Task EkleAsync(Personel personel)
    {
        await _context.Personeller.AddAsync(personel);
    }

    public async Task DegisiklikleriKaydetAsync()
    {
        await _context.SaveChangesAsync();
    }
}