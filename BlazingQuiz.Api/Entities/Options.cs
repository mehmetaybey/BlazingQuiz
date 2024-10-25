using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazingQuiz.Api.Entities;

public class Options
{
    [Key]
    public int Id { get; set; }

    public string Text { get; set; }
    public bool IsCorrect { get; set; }
    public Guid QuestionId { get; set; }

    [ForeignKey(nameof(QuestionId))]
    public virtual Question Question { get; set; }
}