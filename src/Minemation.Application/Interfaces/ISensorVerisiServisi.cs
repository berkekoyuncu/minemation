using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Application.Common;
using Minemation.Application.DTOs;

namespace Minemation.Application.Interfaces;

public interface ISensorVerisiServisi
{
    Task<ApiResponse<PagedResult<SensorVerisiListeDto>>> TumunuGetirAsync(SensorVerisiSorguParametreleri sorgu);

    Task<ApiResponse<SensorVerisiDetayDto>> IdIleGetirAsync(int id);

    Task<ApiResponse<SensorVerisiDetayDto>> OlusturAsync(SensorVerisiOlusturDto dto);

    Task<ApiResponse<SensorVerisiDetayDto>> GuncelleAsync(int id, SensorVerisiGuncelleDto dto);

    Task<ApiResponse<bool>> SilAsync(int id);
}
