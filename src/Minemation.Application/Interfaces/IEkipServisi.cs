using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Application.Common;
using Minemation.Application.DTOs;

namespace Minemation.Application.Interfaces;

public interface IEkipServisi
{
    Task<ApiResponse<PagedResult<EkipListeDto>>> TumunuGetirAsync(EkipSorguParametreleri sorgu);

    Task<ApiResponse<EkipDetayDto>> IdIleGetirAsync(int id);

    Task<ApiResponse<EkipDetayDto>> OlusturAsync(EkipOlusturDto dto);

    Task<ApiResponse<EkipDetayDto>> GuncelleAsync(int id, EkipGuncelleDto dto);

    Task<ApiResponse<bool>> SilAsync(int id);
}
