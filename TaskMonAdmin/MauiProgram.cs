using AdminService.Client;
using CommunityToolkit.Maui;
using LiveChartsCore.SkiaSharpView.Maui;
using Microsoft.Extensions.Logging;
using Refit;
using SkiaSharp.Views.Maui.Controls.Hosting;
using The49.Maui.ContextMenu;
using UraniumUI;
using StatisticsService.Client;

namespace TaskMonAdmin;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseUraniumUI()
            .UseContextMenu()
            .UseUraniumUIMaterial()
            .UseSkiaSharp()
            .UseLiveCharts()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddRefitClient<ITaskMonAdminClient>()
            .ConfigureHttpClient(c => c
                .BaseAddress = new Uri("https://taskmonadminmock-f9gxe2aeckc6e7h6.polandcentral-01.azurewebsites.net"));
        
        builder.Services.AddRefitClient<IStatisticsClient>()
            .ConfigureHttpClient(c => c
                .BaseAddress = new Uri("https://taskmonstatisticsmock-dfbqatddducegqeb.polandcentral-01.azurewebsites.net"));
        
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
        builder.Services.AddSingleton<TimelineGroupPage>();
        builder.Services.AddSingleton<TimelinePage>();
        builder.Services.AddSingleton<LinkPage>();
        builder.Services.AddSingleton<ResultsSurveyGroupsPage>();
        builder.Services.AddSingleton<ResultsSurveysPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}