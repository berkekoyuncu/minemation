using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Domain.Entities;

namespace Minemation.Application.Interfaces;

public interface IRaporRepository
{
    Task<List<Rapor>> TumunuGetirAsync();

    Task<Rapor?> IdIleGetirAsync(int id);

    Task EkleAsync(Rapor rapor);

    Task DegisiklikleriKaydetAsync();
}
