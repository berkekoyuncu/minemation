using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Application.Common;
using Minemation.Application.DTOs;

namespace Minemation.Application.Interfaces;

public interface IEkipmanRaporuServisi
{
    Task<ApiResponse<PagedResult<EkipmanRaporuListeDto>>> TumunuGetirAsync(EkipmanRaporuSorguParametreleri sorgu);

    Task<ApiResponse<EkipmanRaporuDetayDto>> RaporIdIleGetirAsync(int raporId);

    Task<ApiResponse<EkipmanRaporuDetayDto>> OlusturAsync(EkipmanRaporuOlusturDto dto);

    Task<ApiResponse<EkipmanRaporuDetayDto>> GuncelleAsync(int raporId, EkipmanRaporuGuncelleDto dto);

    Task<ApiResponse<bool>> SilAsync(int raporId);
}