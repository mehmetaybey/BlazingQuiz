using BlazingQuiz.Shared.DTOs;
using Refit;

namespace BlazingQuiz.Web.Api
{
    [Headers("Authorization : Bearer")]
    public interface IQuizApi
    {
        [Post("/api/quizes")]
        Task<QuizApiResponse> SaveQuizAsync(QuizSaveDto dto);
        
        [Get("/api/quizes")]
        Task<QuizListDto[]> GetQuizesAsunc();
        
        [Get("/api/quizes/{quizId}/questions")]
        Task<QuizListDto[]> GetQuizQuestionsAsync(Guid quizId);
    }
}
