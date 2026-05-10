using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Domain.Entities;

namespace Minemation.Application.Interfaces;

public interface IKepceRepository
{
    Task<List<Kepce>> TumunuGetirAsync();

    Task<Kepce?> EkipmanIdIleGetirAsync(int ekipmanId);

    Task<bool> VarMiAsync(int ekipmanId);

    Task<bool> PlakaVarMiAsync(string plaka, int? haricTutulacakEkipmanId = null);

    Task EkleAsync(Kepce kepce);

    Task DegisiklikleriKaydetAsync();
}
