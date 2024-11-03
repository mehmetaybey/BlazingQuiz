using BlazingQuiz.Api.Services;
using BlazingQuiz.Shared.DTOs;

namespace BlazingQuiz.Api.Endpoints
{
    public static class QuizEndpoints
    {
        public static IEndpointRouteBuilder MapEndpointRouteBuilder(this IEndpointRouteBuilder app)
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
            return app;
        }
    }
}
