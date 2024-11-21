using BlazingQuiz.Api.Services;
using BlazingQuiz.Shared;
using BlazingQuiz.Shared.DTOs;
using System.Security.Claims;

namespace BlazingQuiz.Api.Endpoints;

public static class StudentQuizEndpoints
{
    public static Guid GetStudentId(this ClaimsPrincipal user)
        => Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
    

    public static IEndpointRouteBuilder MapStudentQuizEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/student")
            .RequireAuthorization(p=>p.RequireRole(nameof(UserRole.Student)));

        group.MapGet("/available-quizzes", async (Guid categoryId, StudentQuizService service) 
            => Results.Ok( await service.GetActiveQuizzesAsync(categoryId)));
            
        
        var quizGroup= group.MapGroup("/quiz");
        quizGroup.MapPost("/{quizId:guid}/start", async (Guid quizId, StudentQuizService service, ClaimsPrincipal user)
            => Results.Ok(await service.StartQuizAsync(user.GetStudentId(), quizId)));

        quizGroup.MapGet("/{studentQuizId:guid}/next-question", async (Guid studentQuizId, StudentQuizService service, ClaimsPrincipal user)
            => Results.Ok(await service.GetNextQuestionForQuizAsync(studentQuizId,user.GetStudentId())));

        quizGroup.MapPost("/{studentQuizId:guid}/save-response", async (Guid studentQuizId, StudentQuizQuestionResponseDto dto, StudentQuizService service, ClaimsPrincipal user) =>
        {
            // Gelen DTO null mu kontrol et
            if (dto == null)
            {
                return Results.BadRequest("Request body is missing or invalid.");
            }

            // DTO'daki StudentQuizId ile URL'deki studentQuizId geliyor mu kontrol et
            if (studentQuizId != dto.StudentQuizId)
            {
                return Results.Unauthorized();
            }

            // Kullanıcının kimliğini al
            var studentIdValue = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(studentIdValue) || !Guid.TryParse(studentIdValue, out Guid studentId))
            {
                return Results.Unauthorized();
            }

            // Servis katmanından sonucu al
            var result = await service.SaveQuestionResponseAsync(dto, studentId);

            // Servis cevabı başarısız olduysa BadRequest döndür
            if (!result.IsSuccess)
            {
                return Results.BadRequest(result.ErrorMessage ?? "An error occurred while saving the response.");
            }

            // Ba�ar�l�ysa Ok döndür
            return Results.Ok(result);
        });


        quizGroup.MapPost("/{studentQuizId:guid}/submit", async (Guid studentQuizId, StudentQuizService service, ClaimsPrincipal user)
            => Results.Ok(await service.SubmitQuizAsync(studentQuizId, user.GetStudentId())));

        quizGroup.MapPost("/{studentQuizId:guid}/auto-submit", async (Guid studentQuizId, StudentQuizService service, ClaimsPrincipal user)
            => Results.Ok(await service.AutoSubmitQuizAsync(studentQuizId, user.GetStudentId())));

        quizGroup.MapPost("/{studentQuizId:guid}/exit", async (Guid studentQuizId, StudentQuizService service, ClaimsPrincipal user)
            => Results.Ok(await service.ExitQuizAsync(studentQuizId, user.GetStudentId())));


        return app;
    }
}