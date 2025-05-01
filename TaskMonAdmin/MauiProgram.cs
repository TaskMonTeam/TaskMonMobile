using AdminService.Client;
using Microsoft.Extensions.Logging;
using Refit;
using UraniumUI;

namespace TaskMonAdmin;

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

        builder.Services.AddRefitClient<ITaskMonAdminClient>()
            .ConfigureHttpClient(c => c
                .BaseAddress = new Uri("https://taskmonadminmock-f9gxe2aeckc6e7h6.polandcentral-01.azurewebsites.net"));
        
        builder.Services.AddSingleton<SyllabusPage>();
        builder.Services.AddSingleton<SyllabusGroupPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}