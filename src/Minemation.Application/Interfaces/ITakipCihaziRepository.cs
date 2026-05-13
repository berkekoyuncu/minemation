using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Domain.Entities;

namespace Minemation.Application.Interfaces;

public interface ITakipCihaziRepository
{
    Task<List<TakipCihazi>> TumunuGetirAsync();

    Task<TakipCihazi?> IdIleGetirAsync(int id);
    Task<TakipCihazi?> EkipmanIdIleGetirAsync(int ekipmanId);

    Task<bool> SeriNoVarMiAsync(string seriNo, int? haricTutulacakId = null);

    Task EkleAsync(TakipCihazi takipCihazi);

    Task DegisiklikleriKaydetAsync();
}
