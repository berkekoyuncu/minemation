using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Application.Common;
using Minemation.Application.DTOs;

namespace Minemation.Application.Interfaces;

public interface IRaporServisi
{
    Task<ApiResponse<PagedResult<RaporListeDto>>> TumunuGetirAsync(RaporSorguParametreleri sorgu);

    Task<ApiResponse<RaporDetayDto>> IdIleGetirAsync(int id);

    Task<ApiResponse<RaporDetayDto>> OlusturAsync(RaporOlusturDto dto);

    Task<ApiResponse<RaporDetayDto>> GuncelleAsync(int id, RaporGuncelleDto dto);

    Task<ApiResponse<bool>> SilAsync(int id);
}
