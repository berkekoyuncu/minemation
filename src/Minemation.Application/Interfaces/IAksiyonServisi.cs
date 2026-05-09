using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Application.Common;
using Minemation.Application.DTOs;

namespace Minemation.Application.Interfaces;

public interface IAksiyonServisi
{
    Task<ApiResponse<PagedResult<AksiyonListeDto>>> TumunuGetirAsync(AksiyonSorguParametreleri sorgu);

    Task<ApiResponse<AksiyonDetayDto>> IdIleGetirAsync(int id);

    Task<ApiResponse<AksiyonDetayDto>> OlusturAsync(AksiyonOlusturDto dto);

    Task<ApiResponse<AksiyonDetayDto>> GuncelleAsync(int id, AksiyonGuncelleDto dto);

    Task<ApiResponse<bool>> SilAsync(int id);
}
