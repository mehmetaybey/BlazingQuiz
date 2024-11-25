using System;
using BlazingQuiz.Web;
using BlazingQuiz.Web.Api;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Refit;
using System.Net.Http;
using System.Threading.Tasks;
using BlazingQuiz.Shared;
using BlazingQuiz.Shared.Components.Api;
using BlazingQuiz.Shared.Components.Auth;
using BlazingQuiz.Web.Services;
using Microsoft.Extensions.DependencyInjection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddCascadingAuthenticationState(); 
builder.Services.AddSingleton<QuizAuthStateProvider>();
builder.Services.AddSingleton<AuthenticationStateProvider>(p => p.GetRequiredService<QuizAuthStateProvider>());
builder.Services.AddAuthorizationCore();

builder.Services.AddSingleton<IAppState, AppState>()
    .AddSingleton<QuizState>()
    .AddSingleton<IStorageService,StorageService>();

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

ConfigureRefit(builder.Services);

await builder.Build().RunAsync();

static void ConfigureRefit(IServiceCollection service)
{
    const string baseUrl = "https://localhost:7189";
    static void SetHttpClient(HttpClient httpClient) => httpClient.BaseAddress = new Uri(baseUrl);

    service.AddRefitClient<IAuthApi>()
        .ConfigureHttpClient(SetHttpClient);

    service.AddRefitClient<IQuizApi>(GetRefitSettings)
        .ConfigureHttpClient(SetHttpClient);

    service.AddRefitClient<ICategoryApi>(GetRefitSettings)
        .ConfigureHttpClient(SetHttpClient);

    service.AddRefitClient<IAdminApi>(GetRefitSettings)
        .ConfigureHttpClient(SetHttpClient);
    
    service.AddRefitClient<IStudentQuizApi>(GetRefitSettings)
        .ConfigureHttpClient(SetHttpClient);

    static RefitSettings GetRefitSettings(IServiceProvider sp)
    {
        var authStateProvider = sp.GetRequiredService<QuizAuthStateProvider>();

        return new RefitSettings
        {
            AuthorizationHeaderValueGetter = (_,__) =>Task.FromResult(authStateProvider.User?.Token ?? "")
        };
    }
    
}
