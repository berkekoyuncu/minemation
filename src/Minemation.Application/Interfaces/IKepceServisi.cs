using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Application.Common;
using Minemation.Application.DTOs;

namespace Minemation.Application.Interfaces;

public interface IKepceServisi
{
    Task<ApiResponse<PagedResult<KepceListeDto>>> TumunuGetirAsync(KepceSorguParametreleri sorgu);

    Task<ApiResponse<KepceDetayDto>> EkipmanIdIleGetirAsync(int ekipmanId);

    Task<ApiResponse<KepceDetayDto>> OlusturAsync(KepceOlusturDto dto);

    Task<ApiResponse<KepceDetayDto>> GuncelleAsync(int ekipmanId, KepceGuncelleDto dto);

    Task<ApiResponse<bool>> SilAsync(int ekipmanId);
}
