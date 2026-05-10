using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Domain.Entities;

namespace Minemation.Application.Interfaces;

public interface ILogKaydiRepository
{
    Task<List<LogKaydi>> TumunuGetirAsync();

    Task<LogKaydi?> IdIleGetirAsync(int id);

    Task EkleAsync(LogKaydi logKaydi);

    Task DegisiklikleriKaydetAsync();
}
