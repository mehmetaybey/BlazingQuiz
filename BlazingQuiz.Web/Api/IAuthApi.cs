using System.Threading.Tasks;
using BlazingQuiz.Shared.DTOs;
using Refit;

namespace BlazingQuiz.Web.Api
{
    public interface IAuthApi
    {
        [Post("/api/auth/login")]
        Task<AuthResponseDto>LoginAsync(LoginDto loginDto);

        [Post("/api/auth/register")]
        Task<QuizApiResponse> RegisterAsync(RegisterDto registerDto);
    }
}
