using BlazingQuiz.Shared.DTOs;

namespace BlazingQuiz.Shared;

public class QuizState
{
    public Guid StudentQuizId { get; private set; }
    public QuizListDto? Quiz { get; private set; }

    public void StartQuiz(QuizListDto? quiz,Guid studentQuizId) => (Quiz,StudentQuizId) = (quiz,studentQuizId);
    public void StopQuiz()=>(Quiz,StudentQuizId) = (null,Guid.Empty);
}