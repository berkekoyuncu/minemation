using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Domain.Entities;

namespace Minemation.Application.Interfaces;

public interface IAksiyonRepository
{
    Task<List<Aksiyon>> TumunuGetirAsync();

    Task<Aksiyon?> IdIleGetirAsync(int id);

    Task EkleAsync(Aksiyon aksiyon);

    Task DegisiklikleriKaydetAsync();
}
