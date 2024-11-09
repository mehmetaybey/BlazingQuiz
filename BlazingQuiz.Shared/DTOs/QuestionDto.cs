namespace BlazingQuiz.Shared.DTOs;

public class QuestionDto
{
    public Guid Id { get; set; }
    public string Text { get; set; }

    public List<OptionDto> Options { get; set; } = [];
}