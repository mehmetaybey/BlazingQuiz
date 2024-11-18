using BlazingQuiz.Shared.DTOs;
using Refit;

namespace BlazingQuiz.Web.Api;

[Headers("Authorization: Bearer ")]
public interface IStudentQuizApi
{
    [Get("/api/student/available-quizzes")]
    Task<QuizListDto[]> GetActiveQuizzesAsync(Guid categoryId);
}