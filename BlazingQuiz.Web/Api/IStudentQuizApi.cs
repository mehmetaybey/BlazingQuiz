using BlazingQuiz.Shared.DTOs;
using Refit;

namespace BlazingQuiz.Web.Api;

[Headers("Authorization: Bearer ")]
public interface IStudentQuizApi
{
    [Get("/api/student/available-quizzes")]
    Task<QuizListDto[]> GetActiveQuizzesAsync(Guid categoryId);
    
    [Post("/api/student/quiz/{quizId}/start")]
    Task<QuizApiResponse<Guid>> StartQuizAsync(Guid quizId);

    [Get("/api/student/my-quizzes")]
    Task<PagedResult<StudentQuizDto>> GetStudentQuizzesAsync(int startIndex, int pageSize);

    [Get("/api/student/quiz/{studentQuizId}/next-question")]
    Task<QuizApiResponse<QuestionDto?>> GetNextQuestionForQuizAsync(Guid studentQuizId);
    
    [Post("/api/student/quiz/{studentQuizId}/save-response")]
    Task<QuizApiResponse> SaveQuestionResponseAsync(Guid studentQuizId,StudentQuizQuestionResponseDto dto);

    [Post("/api/student/quiz/{studentQuizId}/submit")]
    Task<QuizApiResponse> SubmitQuizAsync(Guid studentQuizId);
    
    [Post("/api/student/quiz/{studentQuizId}/exit")]
    Task<QuizApiResponse> ExitQuizAsync(Guid studentQuizId);
    
    [Post("/api/student/quiz/{studentQuizId}/auto-submit")]
    Task<QuizApiResponse> AutoSubmitQuizAsync(Guid studentQuizId);
}