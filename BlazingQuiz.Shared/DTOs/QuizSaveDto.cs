using System.ComponentModel.DataAnnotations;

namespace BlazingQuiz.Shared.DTOs;

public class QuizSaveDto
{
    public Guid Id { get; set; }
    [Required,MaxLength(100)] 
    public string Name { get; set; }
    [Range(1,int.MaxValue,ErrorMessage = "Category is Required")]
    public Guid CategoryId { get; set; }
    
    [Range(1,int.MaxValue,ErrorMessage = "Please provide valid Number of questions")]
    public int TotalQuestions { get; set; }

    [Range(1,120,ErrorMessage = "Please provide valid time in minutes")]
    public int TimeInMinutes { get; set; }
    public bool IsActive { get; set; }
    public List<QuestionDto> Question { get; set; } = [];

}

public class OptionDto
{
    [Key]
    public Guid Id { get; set; }
    public string Text { get; set; }
    public bool IsCorrect { get; set; }

}

public class QuestionDto
{
    public Guid Id { get; set; }
    public string Text { get; set; }

    public List<OptionDto> Option { get; set; } = [];
}