using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Domain.Entities;

namespace Minemation.Application.Interfaces;

public interface IElAletleriRepository
{
    Task<List<ElAletleri>> TumunuGetirAsync();

    Task<ElAletleri?> EkipmanIdIleGetirAsync(int ekipmanId);

    Task<bool> VarMiAsync(int ekipmanId);

    Task EkleAsync(ElAletleri elAletleri);

    Task DegisiklikleriKaydetAsync();
}