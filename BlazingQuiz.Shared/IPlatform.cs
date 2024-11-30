namespace BlazingQuiz.Shared;

public interface IPlatform
{
    bool IsMobileApp { get; }
    bool IsWeb { get; }
}