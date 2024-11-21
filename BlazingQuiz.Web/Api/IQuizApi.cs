using System;
using System.Threading.Tasks;
using BlazingQuiz.Shared.DTOs;
using Refit;

namespace BlazingQuiz.Web.Api
{
    [Headers("Authorization: Bearer")]
    public interface IQuizApi
    {
        [Post("/api/quizzes")]
        Task<QuizApiResponse> SaveQuizAsync(QuizSaveDto dto);

        [Get("/api/quizzes")]
        Task<QuizListDto[]> GetQuizzesAsync();

        [Get("/api/quizzes/{quizId}/questions")]
        Task<QuestionDto[]> GetQuizQuestionsAsync(Guid quizId);

        [Get("/api/quizzes/{quizId}")]
        Task<QuizSaveDto?> GetQuizToEditAsync(Guid quizId);
    }
}