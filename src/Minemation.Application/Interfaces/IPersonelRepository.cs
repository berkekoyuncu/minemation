using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Domain.Entities;

namespace Minemation.Application.Interfaces;

public interface IPersonelRepository
{
    Task<List<Personel>> TumunuGetirAsync();
    Task<Personel?> IdIleGetirAsync(int id);
    Task<bool> TcknVarMiAsync(string tckn, int? haricTutulacakId = null);
    Task EkleAsync(Personel personel);
    Task DegisiklikleriKaydetAsync();
}
