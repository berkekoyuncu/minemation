using System;
using System.Collections.Generic;
using System.Text;
using Minemation.Application.Common;
using Minemation.Application.DTOs;

namespace Minemation.Application.Interfaces;

public interface ISaglikBilgileriServisi
{
    Task<ApiResponse<PagedResult<SaglikBilgileriListeDto>>> TumunuGetirAsync(SaglikBilgileriSorguParametreleri sorgu);

    Task<ApiResponse<SaglikBilgileriDetayDto>> PersonelIdIleGetirAsync(int personelId);

    Task<ApiResponse<SaglikBilgileriDetayDto>> OlusturAsync(SaglikBilgileriOlusturDto dto);

    Task<ApiResponse<SaglikBilgileriDetayDto>> GuncelleAsync(int personelId, SaglikBilgileriGuncelleDto dto);

    Task<ApiResponse<bool>> SilAsync(int personelId);
}