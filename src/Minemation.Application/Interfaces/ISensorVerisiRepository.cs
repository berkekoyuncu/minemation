using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Domain.Entities;

namespace Minemation.Application.Interfaces;

public interface ISensorVerisiRepository
{
    Task<List<SensorVerisi>> TumunuGetirAsync();

    Task<SensorVerisi?> IdIleGetirAsync(int id);

    Task EkleAsync(SensorVerisi sensorVerisi);

    Task DegisiklikleriKaydetAsync();
}
