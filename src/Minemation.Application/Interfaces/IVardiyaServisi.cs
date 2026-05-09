using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Application.Common;
using Minemation.Application.DTOs;

namespace Minemation.Application.Interfaces;

public interface IVardiyaServisi
{
    Task<ApiResponse<PagedResult<VardiyaListeDto>>> TumunuGetirAsync(VardiyaSorguParametreleri sorgu);

    Task<ApiResponse<VardiyaDetayDto>> IdIleGetirAsync(int id);

    Task<ApiResponse<VardiyaDetayDto>> OlusturAsync(VardiyaOlusturDto dto);

    Task<ApiResponse<VardiyaDetayDto>> GuncelleAsync(int id, VardiyaGuncelleDto dto);

    Task<ApiResponse<bool>> SilAsync(int id);
}