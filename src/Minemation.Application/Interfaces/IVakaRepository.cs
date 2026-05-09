using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Domain.Entities;

namespace Minemation.Application.Interfaces;

public interface IVakaRepository
{
    Task<List<Vaka>> TumunuGetirAsync();

    Task<Vaka?> IdIleGetirAsync(int id);

    Task EkleAsync(Vaka vaka);

    Task DegisiklikleriKaydetAsync();
}