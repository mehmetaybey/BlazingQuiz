namespace BlazingQuiz.Api.Data.Entities;

public class StudentQuizQuestion
{
    public Guid StudentQuizId { get; set; }
    public Guid QuestionId { get; set; }

    public virtual StudentQuiz StudentQuiz { get; set; } 
    public virtual Questions Question { get; set; } 
}