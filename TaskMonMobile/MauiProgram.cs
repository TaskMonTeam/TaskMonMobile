using Auth0.OidcClient;
using Microsoft.Extensions.Logging;
using Refit;
using SurveyService.Client;
using SurveyService.Models;
using TaskMonMobile.Services;
using UraniumUI;

namespace TaskMonMobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseUraniumUI()
            .UseUraniumUIMaterial() 
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        
        builder.Services.AddSingleton<ISecureStorage>(SecureStorage.Default);
        
        builder.Services.AddSingleton<AuthDelegatingHandler>();
        
          builder.Services.AddSingleton(new Auth0Client(new Auth0ClientOptions
          {
              Domain = "tasked-app.eu.auth0.com",
              ClientId = "RKGbGFMYFlLDBa0u19xYRqWWvpOq5Gg5",
              RedirectUri = "taskmon://callback/",
              PostLogoutRedirectUri = "taskmon://callback/",
              Scope = "openid profile"
          }));
        
        builder.Services.AddSingleton<Auth0Service>();
        builder.Services.AddSingleton<ITokenSupplier, TokenSupplier>();

        builder.Services.AddRefitClient<ISurveyClient>()
            .ConfigureHttpClient(c => c
                .BaseAddress = new Uri("https://surveysmock-cwchemf4gtgbgcfz.polandcentral-01.azurewebsites.net"))
            .AddHttpMessageHandler<AuthDelegatingHandler>();
        
        builder.Services.AddSingleton<LoadingPage>();
        builder.Services.AddSingleton<LoginPage>();
        builder.Services.AddSingleton<SurveyPage>();
        builder.Services.AddSingleton<SurveyGroupPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}