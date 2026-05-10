using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Application.Common;
using Minemation.Application.DTOs;

namespace Minemation.Application.Interfaces;

public interface IAcilDurumIletisimServisi
{
    Task<ApiResponse<PagedResult<AcilDurumIletisimListeDto>>> TumunuGetirAsync(AcilDurumIletisimSorguParametreleri sorgu);

    Task<ApiResponse<AcilDurumIletisimDetayDto>> IdIleGetirAsync(int id);

    Task<ApiResponse<AcilDurumIletisimDetayDto>> OlusturAsync(AcilDurumIletisimOlusturDto dto);

    Task<ApiResponse<AcilDurumIletisimDetayDto>> GuncelleAsync(int id, AcilDurumIletisimGuncelleDto dto);

    Task<ApiResponse<bool>> SilAsync(int id);
}
