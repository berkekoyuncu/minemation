using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Minemation.Application.Interfaces;
using Minemation.Domain.Entities;
using Minemation.Infrastructure.Persistence;

namespace Minemation.Infrastructure.Repositories;

public class SaglikBilgileriRepository : ISaglikBilgileriRepository
{
    private readonly MinemationDbContext _context;

    public SaglikBilgileriRepository(MinemationDbContext context)
    {
        _context = context;
    }

    public async Task<List<SaglikBilgileri>> TumunuGetirAsync()
    {
        return await _context.SaglikBilgileri
            .AsNoTracking()
            .Include(s => s.Personel)
            .ToListAsync();
    }

    public async Task<SaglikBilgileri?> PersonelIdIleGetirAsync(int personelId)
    {
        return await _context.SaglikBilgileri
            .Include(s => s.Personel)
            .FirstOrDefaultAsync(s => s.personelId == personelId);
    }

    public async Task<bool> VarMiAsync(int personelId)
    {
        return await _context.SaglikBilgileri
            .AnyAsync(s => s.personelId == personelId);
    }

    public async Task EkleAsync(SaglikBilgileri saglikBilgileri)
    {
        await _context.SaglikBilgileri.AddAsync(saglikBilgileri);
    }

    public async Task DegisiklikleriKaydetAsync()
    {
        await _context.SaveChangesAsync();
    }
}