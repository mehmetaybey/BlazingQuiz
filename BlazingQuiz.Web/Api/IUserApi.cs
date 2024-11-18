using System;
using System.Threading.Tasks;
using BlazingQuiz.Shared;
using BlazingQuiz.Shared.DTOs;
using Refit;

namespace BlazingQuiz.Web.Api;

[Headers("Authorization: Bearer")]
public interface IUserApi
{
    [Get("/api/users")]
    Task<PagedResult<UserDto>> GetUsersAsync(UserApprovedFilter approvedType, int startIndex, int pageSize);

    [Patch("/api/users/{userId}/toogle-status")]
    Task ToggleUserApprovedStatusAsync(Guid userId);
}