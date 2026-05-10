using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Domain.Entities;

namespace Minemation.Application.Interfaces;

public interface IKiriciRepository
{
    Task<List<Kirici>> TumunuGetirAsync();

    Task<Kirici?> EkipmanIdIleGetirAsync(int ekipmanId);

    Task<bool> VarMiAsync(int ekipmanId);

    Task EkleAsync(Kirici kirici);

    Task DegisiklikleriKaydetAsync();
}