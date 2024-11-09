using BlazingQuiz.Api.Services;
using BlazingQuiz.Shared.DTOs;

namespace BlazingQuiz.Api.Endpoints
{
    public static class QuizEndpoints
    {
        public static IEndpointRouteBuilder MapQuizEndpoints(this IEndpointRouteBuilder app)
        {
            var quizGroup = app.MapGroup("/api/quizes").RequireAuthorization();

            quizGroup.MapPost("", async (QuizSaveDto dto, QuizService service) =>
            {
                if (dto.Question.Count == 0)
                    return Results.BadRequest("Please provide Questions");
                if(dto.Question.Count != dto.TotalQuestions)
                    return Results.BadRequest("Total Questions should match with provided questions");

                return Results.Ok(await service.SaveQuizAsync(dto));

            });
            quizGroup.MapGet("", async (QuizService service) 
                => Results.Ok(await service.GetQuizesAsync()));

            quizGroup.MapGet("{quizId:guid}/questions", async (Guid quizId, QuizService service) 
                => Results.Ok(await service.GetQuizQuestionsAsync(quizId)));
            
            quizGroup.MapGet("{quizId:guid}", async (Guid quizId, QuizService service) 
                => Results.Ok(await service.GetQuizToEditAsync(quizId)));

            return app;
        }
    }
}
