using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Domain.Entities;

namespace Minemation.Application.Interfaces;

public interface IHafriyatRepository
{
    Task<List<Hafriyat>> TumunuGetirAsync();

    Task<Hafriyat?> EkipmanIdIleGetirAsync(int ekipmanId);

    Task<bool> VarMiAsync(int ekipmanId);

    Task<bool> PlakaVarMiAsync(string plaka, int? haricTutulacakEkipmanId = null);

    Task EkleAsync(Hafriyat hafriyat);

    Task DegisiklikleriKaydetAsync();
}
