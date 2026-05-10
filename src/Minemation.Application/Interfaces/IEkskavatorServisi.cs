using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Application.Common;
using Minemation.Application.DTOs;

namespace Minemation.Application.Interfaces;

public interface IEkskavatorServisi
{
    Task<ApiResponse<PagedResult<EkskavatorListeDto>>> TumunuGetirAsync(EkskavatorSorguParametreleri sorgu);

    Task<ApiResponse<EkskavatorDetayDto>> EkipmanIdIleGetirAsync(int ekipmanId);

    Task<ApiResponse<EkskavatorDetayDto>> OlusturAsync(EkskavatorOlusturDto dto);

    Task<ApiResponse<EkskavatorDetayDto>> GuncelleAsync(int ekipmanId, EkskavatorGuncelleDto dto);

    Task<ApiResponse<bool>> SilAsync(int ekipmanId);
}