using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Application.Common;
using Minemation.Application.DTOs;

namespace Minemation.Application.Interfaces;

public interface IEkipmanServisi
{
    Task<ApiResponse<PagedResult<EkipmanListeDto>>> TumunuGetirAsync(EkipmanSorguParametreleri sorgu);

    Task<ApiResponse<EkipmanDetayDto>> IdIleGetirAsync(int id);

    Task<ApiResponse<EkipmanDetayDto>> OlusturAsync(EkipmanOlusturDto dto);

    Task<ApiResponse<EkipmanDetayDto>> GuncelleAsync(int id, EkipmanGuncelleDto dto);

    Task<ApiResponse<bool>> SilAsync(int id);
}