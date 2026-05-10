using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Application.Common;
using Minemation.Application.DTOs;

namespace Minemation.Application.Interfaces;

public interface IKimlikDogrulamaServisi
{
    Task<ApiResponse<GirisSonucDto>> GirisYapAsync(GirisYapDto dto);

    Task<ApiResponse<GirisSonucDto>> RfidIleGirisYapAsync(RfidGirisDto dto);

    Task<ApiResponse<bool>> SifreDegistirAsync(SifreDegistirDto dto);

    Task<ApiResponse<bool>> SifreBelirleAsync(SifreBelirleDto dto);
}
