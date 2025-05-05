using AdminService.Client;
using Microsoft.Extensions.Logging;
using Refit;
using The49.Maui.ContextMenu;
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
            .UseContextMenu()
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
        builder.Services.AddSingleton<CreateSyllabusPage>();
        builder.Services.AddSingleton<CoursesPage>();
        builder.Services.AddSingleton<CreateCoursePage>();
        builder.Services.AddSingleton<UpdateCoursePage>();
        builder.Services.AddSingleton<SurveyGroupsPage>();
        builder.Services.AddSingleton<SurveysPage>();
        builder.Services.AddSingleton<CreateSurveyGroupPage>();
        builder.Services.AddSingleton<CreateSurveyPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}