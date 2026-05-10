using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Application.Common;
using Minemation.Application.DTOs;

namespace Minemation.Application.Interfaces;

public interface IPersonelRaporuServisi
{
    Task<ApiResponse<PagedResult<PersonelRaporuListeDto>>> TumunuGetirAsync(PersonelRaporuSorguParametreleri sorgu);

    Task<ApiResponse<PersonelRaporuDetayDto>> RaporIdIleGetirAsync(int raporId);

    Task<ApiResponse<PersonelRaporuDetayDto>> OlusturAsync(PersonelRaporuOlusturDto dto);

    Task<ApiResponse<PersonelRaporuDetayDto>> GuncelleAsync(int raporId, PersonelRaporuGuncelleDto dto);

    Task<ApiResponse<bool>> SilAsync(int raporId);
}
