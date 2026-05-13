using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Application.Common;
using Minemation.Application.DTOs;

namespace Minemation.Application.Interfaces;

public interface ITakipCihaziServisi
{
    Task<ApiResponse<PagedResult<TakipCihaziListeDto>>> TumunuGetirAsync(TakipCihaziSorguParametreleri sorgu);

    Task<ApiResponse<TakipCihaziDetayDto>> EkipmanIdIleGetirAsync(int ekipmanId);

    Task<ApiResponse<TakipCihaziDetayDto>> OlusturAsync(TakipCihaziOlusturDto dto);

    Task<ApiResponse<TakipCihaziDetayDto>> GuncelleAsync(int id, TakipCihaziGuncelleDto dto);

    Task<ApiResponse<bool>> SilAsync(int id);
}
