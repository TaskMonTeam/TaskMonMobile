using Microsoft.Extensions.Logging;
using Refit;
using SurveyService.Client;
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
        
        builder.Services.AddRefitClient<ISurveyClient>().ConfigureHttpClient(c => c.BaseAddress = new Uri("https://surveysmock-cwchemf4gtgbgcfz.polandcentral-01.azurewebsites.net"));
        //builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<SurveyPage>();
        builder.Services.AddTransient<SurveyGroupPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}