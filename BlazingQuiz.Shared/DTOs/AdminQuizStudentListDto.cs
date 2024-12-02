namespace BlazingQuiz.Shared.DTOs;

public class AdminQuizStudentListDto
{
    public string QuizName { get; set; }
    public string CategoryName { get; set; }
    public PagedResult<AdminQuizStudentDto> Students { get; set; } 

}