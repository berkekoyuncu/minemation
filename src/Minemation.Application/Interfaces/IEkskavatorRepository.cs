using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Domain.Entities;

namespace Minemation.Application.Interfaces;

public interface IEkskavatorRepository
{
    Task<List<Ekskavator>> TumunuGetirAsync();

    Task<Ekskavator?> EkipmanIdIleGetirAsync(int ekipmanId);

    Task<bool> VarMiAsync(int ekipmanId);

    Task<bool> PlakaVarMiAsync(string plaka, int? haricTutulacakEkipmanId = null);

    Task EkleAsync(Ekskavator ekskavator);

    Task DegisiklikleriKaydetAsync();
}