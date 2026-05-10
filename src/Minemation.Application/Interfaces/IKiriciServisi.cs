using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Application.Common;
using Minemation.Application.DTOs;

namespace Minemation.Application.Interfaces;

public interface IKiriciServisi
{
    Task<ApiResponse<PagedResult<KiriciListeDto>>> TumunuGetirAsync(KiriciSorguParametreleri sorgu);

    Task<ApiResponse<KiriciDetayDto>> EkipmanIdIleGetirAsync(int ekipmanId);

    Task<ApiResponse<KiriciDetayDto>> OlusturAsync(KiriciOlusturDto dto);

    Task<ApiResponse<KiriciDetayDto>> GuncelleAsync(int ekipmanId, KiriciGuncelleDto dto);

    Task<ApiResponse<bool>> SilAsync(int ekipmanId);
}
