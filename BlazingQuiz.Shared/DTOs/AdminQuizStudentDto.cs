namespace BlazingQuiz.Shared.DTOs;

public class AdminQuizStudentDto
{
    public string Name { get; set; }
    public DateTime StartedOn { get; set; }
    public DateTime? CompletedOn { get; set; }
    public int Score { get; set; }
    public string Status { get; set; }
}