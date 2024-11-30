using BlazingQuiz.Mobile.Services;
using BlazingQuiz.Shared;
using BlazingQuiz.Shared.Components.Api;
using BlazingQuiz.Shared.Components.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using Refit;

#if ANDROID
using System.Net.Security;
using Xamarin.Android.Net;
using System.Security.Cryptography.X509Certificates;

#elif IOS
using Security;

#endif


namespace BlazingQuiz.Mobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts => { fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"); });

        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif
        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddSingleton<QuizAuthStateProvider>();
        builder.Services.AddSingleton<AuthenticationStateProvider>(p => p.GetRequiredService<QuizAuthStateProvider>());
        builder.Services.AddAuthorizationCore();

        builder.Services.AddSingleton<IStorageService, StorageService>()
            .AddSingleton<IAppState, AppState>()
            .AddSingleton<QuizState>()
            .AddSingleton<IPlatform,MobilePlatform>();

        ConfigureRefit(builder.Services);

        return builder.Build();
    }

    private static readonly string ApiBaseUrl = DeviceInfo.Platform == DevicePlatform.Android
        ? "https://10.0.2.2:7189"
        : "https://localhost:7189";

    static void ConfigureRefit(IServiceCollection service)
    {

        service.AddRefitClient<IAuthApi>(GetRefitSettings)
            .ConfigureHttpClient(SetHttpClient);
        
        service.AddRefitClient<ICategoryApi>(GetRefitSettings)
            .ConfigureHttpClient(SetHttpClient);
        
        service.AddRefitClient<IStudentQuizApi>(GetRefitSettings)
            .ConfigureHttpClient(SetHttpClient);
        static void SetHttpClient(HttpClient httpClient) => httpClient.BaseAddress = new Uri(ApiBaseUrl);



        static RefitSettings GetRefitSettings(IServiceProvider sp)
        {
            var authStateProvider = sp.GetRequiredService<QuizAuthStateProvider>();

            return new RefitSettings
            {
                AuthorizationHeaderValueGetter = (_, __) => Task.FromResult(authStateProvider.User?.Token ?? ""),
                HttpMessageHandlerFactory = () =>
                {
 #if ANDROID          
                    var androidMessageHandler = new AndroidMessageHandler();
                    androidMessageHandler.ServerCertificateCustomValidationCallback
                        = (HttpRequestMessage requestMessage, X509Certificate2 certificate2, X509Chain? chain,
                                SslPolicyErrors sslPolicyErrors)
                            => certificate2.Issuer == "CN=localhost" || sslPolicyErrors == SslPolicyErrors.None;

                    return androidMessageHandler;
#elif  IOS
                    var nsUrlSessionHandler = new NSUrlSessionHandler
                    {
                        TrustOverrideForUrl = (NSUrlSessionHandler sender, string url, SecTrust trust) =>
                                url.StartsWith("https//localhost")
                    };
                    return nsUrlSessionHandler;
                   
#endif
                    return null;
                }
            };
        }
    }
}