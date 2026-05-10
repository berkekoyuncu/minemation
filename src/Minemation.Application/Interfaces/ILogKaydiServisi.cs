using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Application.Common;
using Minemation.Application.DTOs;

namespace Minemation.Application.Interfaces;

public interface ILogKaydiServisi
{
    Task<ApiResponse<PagedResult<LogKaydiListeDto>>> TumunuGetirAsync(LogKaydiSorguParametreleri sorgu);

    Task<ApiResponse<LogKaydiDetayDto>> IdIleGetirAsync(int id);

    Task<ApiResponse<LogKaydiDetayDto>> OlusturAsync(LogKaydiOlusturDto dto);

    Task<ApiResponse<LogKaydiDetayDto>> GuncelleAsync(int id, LogKaydiGuncelleDto dto);

    Task<ApiResponse<bool>> SilAsync(int id);
}
