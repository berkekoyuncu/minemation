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
            .Include(p => p.SaglikBilgileri)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Personel?> IdIleGetirAsync(int id)
    {
        return await _context.Personeller
            .Include(p => p.SaglikBilgileri)   // Detay sayfasında sağlık bilgilerini görmek için
            .Include(p => p.AcilDurumKisileri) // Acil durum kişilerini görmek için
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

    public async Task<Personel?> EpostaIleGetirAsync(string eposta)
    {
        return await _context.Personeller
            .FirstOrDefaultAsync(p => p.eposta == eposta);
    }

    public async Task<Personel?> TcknIleGetirAsync(string tckn)
    {
        return await _context.Personeller
            .FirstOrDefaultAsync(p => p.tckn == tckn);
    }
        public async Task<Personel?> RfidIleGetirAsync(string rfidKartNumarasi)
    {
        return await _context.Personeller
            .FirstOrDefaultAsync(p => p.rfidKartNumarasi == rfidKartNumarasi);
    }
}

