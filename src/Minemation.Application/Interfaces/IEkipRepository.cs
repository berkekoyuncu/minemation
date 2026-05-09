using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Domain.Entities;

namespace Minemation.Application.Interfaces;

public interface IEkipRepository
{
    Task<List<Ekip>> TumunuGetirAsync();

    Task<Ekip?> IdIleGetirAsync(int id);

    Task EkleAsync(Ekip ekip);

    Task DegisiklikleriKaydetAsync();
}
