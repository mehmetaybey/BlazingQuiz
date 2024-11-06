using System.ComponentModel.DataAnnotations;

namespace BlazingQuiz.Shared.DTOs;

public class QuizListDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid  CategoryId { get; set; }
    public string CategoryName { get; set; }
    
    public int TotalQuestions { get; set; }

    public int TimeInMinutes { get; set; }
    public bool IsActive { get; set; }
    public List<QuestionDto> Question { get; set; } = [];

}