using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Domain.Entities;

namespace Minemation.Application.Interfaces;

public interface IAcilDurumIletisimRepository
{
    Task<List<AcilDurumIletisim>> TumunuGetirAsync();

    Task<AcilDurumIletisim?> IdIleGetirAsync(int id);

    Task EkleAsync(AcilDurumIletisim acilDurumIletisim);

    Task DegisiklikleriKaydetAsync();
}