namespace BlazingQuiz.Shared;

public class AppState :IAppState
{
    public string? LoadingText { get; private set; } 
    public event Action? OnToggleLoader;
   
    public void HideLoader()
    {
        LoadingText = null;
        OnToggleLoader?.Invoke();
        
    }

    public void ShowLoader(string loadingText)
    {
        LoadingText = loadingText;
        OnToggleLoader?.Invoke();
    }
    public event Action<string>? OnShowError;
    public void ShowError(string errorText)=> OnShowError?.Invoke(errorText);


   
}