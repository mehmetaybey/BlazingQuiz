using System;
using System.Threading.Tasks;
using BlazingQuiz.Shared;
using BlazingQuiz.Shared.DTOs;
using Refit;

namespace BlazingQuiz.Web.Api;

[Headers("Authorization: Bearer")]
public interface IAdminApi
{
    [Get("/api/admin/users")]
    Task<PagedResult<UserDto>> GetUsersAsync(UserApprovedFilter approvedType, int startIndex, int pageSize);

    [Get("/api/admin/quizzes/{quizId}/students")]
    Task<AdminQuizStudentListDto> GetQuizStudentAsync(Guid quizId, int startIndex, int pageSize, bool fetchQuizInfo);
    
    [Patch("/api/admin/users/{userId}/toggle-status")]
    Task ToggleUserApprovedStatusAsync(Guid userId);

    [Get("/api/admin/home-data")]
    Task<AdminHomeDataDto> GetHomeDataAsync();
}