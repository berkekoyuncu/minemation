using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Application.Common;
using Minemation.Application.DTOs;

namespace Minemation.Application.Interfaces;

public interface ISensorServisi
{
    Task<ApiResponse<PagedResult<SensorListeDto>>> TumunuGetirAsync(SensorSorguParametreleri sorgu);

    Task<ApiResponse<SensorDetayDto>> EkipmanIdIleGetirAsync(int ekipmanId);

    Task<ApiResponse<SensorDetayDto>> OlusturAsync(SensorOlusturDto dto);

    Task<ApiResponse<SensorDetayDto>> GuncelleAsync(int ekipmanId, SensorGuncelleDto dto);

    Task<ApiResponse<bool>> SilAsync(int ekipmanId);
}