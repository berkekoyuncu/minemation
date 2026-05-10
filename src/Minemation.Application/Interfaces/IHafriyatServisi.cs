using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Application.Common;
using Minemation.Application.DTOs;

namespace Minemation.Application.Interfaces;

public interface IHafriyatServisi
{
    Task<ApiResponse<PagedResult<HafriyatListeDto>>> TumunuGetirAsync(HafriyatSorguParametreleri sorgu);

    Task<ApiResponse<HafriyatDetayDto>> EkipmanIdIleGetirAsync(int ekipmanId);

    Task<ApiResponse<HafriyatDetayDto>> OlusturAsync(HafriyatOlusturDto dto);

    Task<ApiResponse<HafriyatDetayDto>> GuncelleAsync(int ekipmanId, HafriyatGuncelleDto dto);

    Task<ApiResponse<bool>> SilAsync(int ekipmanId);
}
