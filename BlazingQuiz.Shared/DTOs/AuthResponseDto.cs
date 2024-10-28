namespace BlazingQuiz.Shared.DTOs;

public record AuthResponseDto(LoggedInUser User, string? ErrorMessage = null)
{
    public bool HasError => ErrorMessage != null;
    
}
