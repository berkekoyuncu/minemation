using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Domain.Entities;

namespace Minemation.Application.Interfaces;

public interface IEkipmanRaporuRepository
{
    Task<List<EkipmanRaporu>> TumunuGetirAsync();

    Task<EkipmanRaporu?> RaporIdIleGetirAsync(int raporId);

    Task<bool> VarMiAsync(int raporId);

    Task EkleAsync(EkipmanRaporu ekipmanRaporu);

    Task DegisiklikleriKaydetAsync();
}