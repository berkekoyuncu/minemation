using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Domain.Entities;

namespace Minemation.Application.Interfaces;

public interface ISaglikBilgileriRepository
{
    Task<List<SaglikBilgileri>> TumunuGetirAsync();

    Task<SaglikBilgileri?> PersonelIdIleGetirAsync(int personelId);

    Task<bool> VarMiAsync(int personelId);

    Task EkleAsync(SaglikBilgileri saglikBilgileri);

    Task DegisiklikleriKaydetAsync();
}
