using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Domain.Entities;

namespace Minemation.Application.Interfaces;

public interface IPersonelRaporuRepository
{
    Task<List<PersonelRaporu>> TumunuGetirAsync();

    Task<PersonelRaporu?> RaporIdIleGetirAsync(int raporId);

    Task<bool> VarMiAsync(int raporId);

    Task EkleAsync(PersonelRaporu personelRaporu);

    Task DegisiklikleriKaydetAsync();
}
