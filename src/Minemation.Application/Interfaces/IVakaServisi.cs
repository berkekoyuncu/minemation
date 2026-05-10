using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Application.Common;
using Minemation.Application.DTOs;

namespace Minemation.Application.Interfaces;

public interface IVakaServisi
{
    Task<ApiResponse<PagedResult<VakaListeDto>>> TumunuGetirAsync(VakaSorguParametreleri sorgu);

    Task<ApiResponse<VakaDetayDto>> IdIleGetirAsync(int id);

    Task<ApiResponse<VakaDetayDto>> OlusturAsync(VakaOlusturDto dto);

    Task<ApiResponse<VakaDetayDto>> GuncelleAsync(int id, VakaGuncelleDto dto);

    Task<ApiResponse<bool>> SilAsync(int id);
}
