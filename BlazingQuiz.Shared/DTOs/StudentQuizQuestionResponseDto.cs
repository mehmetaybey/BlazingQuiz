namespace BlazingQuiz.Shared.DTOs
{
    public record StudentQuizQuestionResponseDto(Guid StudentQuizId, Guid QuestionId, int OptionId);
}
