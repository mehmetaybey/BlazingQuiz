namespace BlazingQuiz.Shared.DTOs;

public record UserDto(Guid Id,string Name,string Email,string Phone,bool IsApproved);