using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Domain.Entities;

namespace Minemation.Application.Interfaces;

public interface IEkipmanRepository
{
    Task<List<Ekipman>> TumunuGetirAsync();

    Task<Ekipman?> IdIleGetirAsync(int id);

    Task<bool> SeriNoVarMiAsync(string seriNo, int? haricTutulacakId = null);

    Task EkleAsync(Ekipman ekipman);

    Task DegisiklikleriKaydetAsync();
}
