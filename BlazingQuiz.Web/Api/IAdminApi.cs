using System;
using System.Threading.Tasks;
using BlazingQuiz.Shared;
using BlazingQuiz.Shared.DTOs;
using Refit;

namespace BlazingQuiz.Web.Api;

[Headers("Authorization: Bearer")]
public interface IAdminApi
{
    [Get("/api/users")]
    Task<PagedResult<UserDto>> GetUsersAsync(UserApprovedFilter approvedType, int startIndex, int pageSize);

    [Patch("/api/users/{userId}/toggle-status")]
    Task ToggleUserApprovedStatusAsync(Guid userId);

    [Get("/api/admin/home-data")]
    Task<AdminHomeDataDto> GetHomeDataAsync();
}