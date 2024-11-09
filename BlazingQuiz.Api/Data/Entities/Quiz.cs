using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazingQuiz.Api.Data.Entities;

public class Quiz
{
    [Key]
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public int TotalQuestions { get; set; }
    public int TimeInMinutes { get; set; }
    public bool IsActive { get; set; }

    public Guid CategoryId { get; set; }
    [ForeignKey(nameof(CategoryId))]
    public virtual Category? Category { get; set; }

    public ICollection<Questions> Questions { get;} = [];
}