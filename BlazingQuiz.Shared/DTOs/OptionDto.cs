using System.ComponentModel.DataAnnotations;

namespace BlazingQuiz.Shared.DTOs;

public class OptionDto
{
    [Key]
    public int Id { get; set; }
    public string Text { get; set; }
    public bool IsCorrect { get; set; }

}