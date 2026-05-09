using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Application.Common;
using Minemation.Application.DTOs;

namespace Minemation.Application.Interfaces;

public interface IPersonelServisi
{
    Task<ApiResponse<PagedResult<PersonelListeDto>>> TumunuGetirAsync(PersonelSorguParametreleri sorgu);
    Task<ApiResponse<PersonelDetayDto>> IdIleGetirAsync(int id);
    Task<ApiResponse<PersonelDetayDto>> OlusturAsync(PersonelOlusturDto dto);
    Task<ApiResponse<PersonelDetayDto>> GuncelleAsync(int id, PersonelGuncelleDto dto);
    Task<ApiResponse<bool>> SilAsync(int id);
}
