namespace BlazingQuiz.Shared;

public interface IAppState
{
    string? LoadingText { get; }
    void ShowLoader(string loadingText);
    void HideLoader();
    event Action? OnToggleLoader;
    
    event Action<string>? OnShowError;
    void ShowError(string errorText);
}