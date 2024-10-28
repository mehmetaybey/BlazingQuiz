using BlazingQuiz.Web;
using BlazingQuiz.Web.Api;
using BlazingQuiz.Web.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Refit;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddCascadingAuthenticationState(); 
builder.Services.AddScoped<QuizAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(p => p.GetRequiredService<QuizAuthStateProvider>());
builder.Services.AddAuthorizationCore();

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

ConfigureRefit(builder.Services);

await builder.Build().RunAsync();

static void ConfigureRefit(IServiceCollection service)
{
    service.AddRefitClient<IAuthApi>()
        .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:7189"));
}
