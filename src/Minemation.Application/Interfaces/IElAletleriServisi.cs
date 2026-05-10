using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Application.Common;
using Minemation.Application.DTOs;

namespace Minemation.Application.Interfaces;

public interface IElAletleriServisi
{
    Task<ApiResponse<PagedResult<ElAletleriListeDto>>> TumunuGetirAsync(ElAletleriSorguParametreleri sorgu);

    Task<ApiResponse<ElAletleriDetayDto>> EkipmanIdIleGetirAsync(int ekipmanId);

    Task<ApiResponse<ElAletleriDetayDto>> OlusturAsync(ElAletleriOlusturDto dto);

    Task<ApiResponse<ElAletleriDetayDto>> GuncelleAsync(int ekipmanId, ElAletleriGuncelleDto dto);

    Task<ApiResponse<bool>> SilAsync(int ekipmanId);
}
