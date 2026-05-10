using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Domain.Entities;

namespace Minemation.Application.Interfaces;

public interface IVardiyaRepository
{
    Task<List<Vardiya>> TumunuGetirAsync();

    Task<Vardiya?> IdIleGetirAsync(int id);

    Task EkleAsync(Vardiya vardiya);

    Task DegisiklikleriKaydetAsync();
}
