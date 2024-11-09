using System.ComponentModel.DataAnnotations;

namespace BlazingQuiz.Shared.DTOs;

public class QuizSaveDto
{
    public Guid Id { get; set; }

    [Required,MaxLength(100)] 
    public string Name { get; set; }

    public Guid CategoryId { get; set; }
    
    [Range(1,int.MaxValue,ErrorMessage = "Please provide valid Number of questions")]
    public int TotalQuestions { get; set; }

    [Range(1,120,ErrorMessage = "Please provide valid time in minutes")]
    public int TimeInMinutes { get; set; }
    public bool IsActive { get; set; }
    public List<QuestionDto> Question { get; set; } = [];

    public string? Validate()
    {
      if(TotalQuestions != Question.Count) 
          return "Total Questions and Questions count should be same";

      if (Question.Any(q => string.IsNullOrWhiteSpace(q.Text)))
          return "Question Text is required for questions";
  
      if (Question.Any(q => q.Options.Count < 2))
          return "At-Least 2 options are required for each question";

      if (Question.Any(q => !q.Options.Any(o => o.IsCorrect)))
            return "At-Least 1 correct option is required for each question";



      return null;

    }

}


      
