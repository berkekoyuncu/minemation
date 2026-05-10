using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Domain.Entities;

namespace Minemation.Application.Interfaces;

public interface ISensorRepository
{
    Task<List<Sensor>> TumunuGetirAsync();

    Task<Sensor?> EkipmanIdIleGetirAsync(int ekipmanId);

    Task<bool> EkipmandaSensorVarMiAsync(int ekipmanId);

    Task EkleAsync(Sensor sensor);

    Task DegisiklikleriKaydetAsync();
}