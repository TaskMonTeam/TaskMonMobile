using AdminService.Client;
using Auth0.OidcClient;
using CommunityToolkit.Maui;
using ImportService.Client;
using LiveChartsCore.SkiaSharpView.Maui;
using Microsoft.Extensions.Logging;
using Refit;
using SkiaSharp.Views.Maui.Controls.Hosting;
using The49.Maui.ContextMenu;
using UraniumUI;
using StatisticsService.Client;
using TaskMonAdmin.Services;
using AuthDelegatingHandlerImport = ImportService.Client.AuthDelegatingHandler;
using AuthDelegatingHandlerStatistics = StatisticsService.Client.AuthDelegatingHandler;
using AuthDelegatingHandlerAdmin = AdminService.Client.AuthDelegatingHandler;
using ITokenSupplierAdmin = AdminService.Client.ITokenSupplier;
using ITokenSupplierImport = ImportService.Client.ITokenSupplier;
using ITokenSupplierStatistics = StatisticsService.Client.ITokenSupplier;

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
        
        builder.Services.AddSingleton<ISecureStorage>(SecureStorage.Default);
        
        builder.Services.AddSingleton<AuthDelegatingHandlerAdmin>();
        builder.Services.AddSingleton<AuthDelegatingHandlerStatistics>();
        builder.Services.AddSingleton<AuthDelegatingHandlerImport>();
        
        builder.Services.AddSingleton(new Auth0Client(new Auth0ClientOptions
        {
            Domain = "tasked-app.eu.auth0.com",
            ClientId = "RKGbGFMYFlLDBa0u19xYRqWWvpOq5Gg5",
            RedirectUri = "taskmon://callback/",
            PostLogoutRedirectUri = "taskmon://callback/",
            Scope = "openid profile"
        }));
        
        builder.Services.AddSingleton<Auth0Service>();
        builder.Services.AddSingleton<ITokenSupplierAdmin, TokenSupplierAdmin>();
        builder.Services.AddSingleton<ITokenSupplierImport, TokenSupplierImport>();
        builder.Services.AddSingleton<ITokenSupplierStatistics, TokenSupplierStatistics>();

        builder.Services.AddRefitClient<ITaskMonAdminClient>()
            .ConfigureHttpClient(c => c
                .BaseAddress = new Uri("https://taskmonadminmock-f9gxe2aeckc6e7h6.polandcentral-01.azurewebsites.net"))
            .AddHttpMessageHandler<AuthDelegatingHandlerAdmin>();
        
        builder.Services.AddRefitClient<IStatisticsClient>()
            .ConfigureHttpClient(c => c
                .BaseAddress = new Uri("https://taskmonstatisticsmock-dfbqatddducegqeb.polandcentral-01.azurewebsites.net"))
            .AddHttpMessageHandler<AuthDelegatingHandlerStatistics>();
        
        builder.Services.AddRefitClient<IImportClient>()
            .ConfigureHttpClient(c => c
                .BaseAddress = new Uri("https://taskmonimportmock-fwcthjbxcscyf6dc.polandcentral-01.azurewebsites.net"))
            .AddHttpMessageHandler<AuthDelegatingHandlerImport>();
        
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
        builder.Services.AddSingleton<DiagramsGroupPage>();
        builder.Services.AddSingleton<DiagramsPage>();
        builder.Services.AddSingleton<LinkPage>();
        builder.Services.AddSingleton<ResultsSurveyGroupsPage>();
        builder.Services.AddSingleton<ResultsSurveysPage>();
        builder.Services.AddSingleton<ImportSyllabusPage>();
        builder.Services.AddSingleton<ImportedSyllabusesPage>();
        builder.Services.AddSingleton<SyllabusImportedPage>();
        builder.Services.AddSingleton<LoginPage>();
        builder.Services.AddSingleton<LoadingPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}