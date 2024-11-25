using BlazingQuiz.Shared.DTOs;
using Refit;

namespace BlazingQuiz.Shared.Components.Api
{
    public interface IAuthApi
    {
        [Post("/api/auth/login")]
        Task<AuthResponseDto>LoginAsync(LoginDto loginDto);

        [Post("/api/auth/register")]
        Task<QuizApiResponse> RegisterAsync(RegisterDto registerDto);
    }
}
