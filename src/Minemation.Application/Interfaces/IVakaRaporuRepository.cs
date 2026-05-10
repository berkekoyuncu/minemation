using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Domain.Entities;

namespace Minemation.Application.Interfaces;

public interface IVakaRaporuRepository
{
    Task<List<VakaRaporu>> TumunuGetirAsync();

    Task<VakaRaporu?> RaporIdIleGetirAsync(int raporId);

    Task<bool> VarMiAsync(int raporId);

    Task EkleAsync(VakaRaporu vakaRaporu);

    Task DegisiklikleriKaydetAsync();
}
