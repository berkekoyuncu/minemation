using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Application.Common;
using Minemation.Application.DTOs;

namespace Minemation.Application.Interfaces;

public interface IVakaRaporuServisi
{
    Task<ApiResponse<PagedResult<VakaRaporuListeDto>>> TumunuGetirAsync(VakaRaporuSorguParametreleri sorgu);

    Task<ApiResponse<VakaRaporuDetayDto>> RaporIdIleGetirAsync(int raporId);

    Task<ApiResponse<VakaRaporuDetayDto>> OlusturAsync(VakaRaporuOlusturDto dto);

    Task<ApiResponse<VakaRaporuDetayDto>> GuncelleAsync(int raporId, VakaRaporuGuncelleDto dto);

    Task<ApiResponse<bool>> SilAsync(int raporId);
}
