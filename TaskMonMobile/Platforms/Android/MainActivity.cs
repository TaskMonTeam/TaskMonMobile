using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using CommunityToolkit.Mvvm.Messaging;

namespace TaskMonMobile;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop,
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode |
                           ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]


[IntentFilter(new[] { Intent.ActionView }, 
    Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable }, 
    DataScheme = "http", 
    DataHost = "taskmon.com", 
    DataPathPrefix = "/sureveys/invite" )]

[IntentFilter(new[] { Intent.ActionView }, 
    Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable }, 
    DataScheme = "http", 
    DataHost = "taskmon.com", 
    DataPathPrefix = "/groups/invite" )]

[IntentFilter(new[] { Intent.ActionView }, 
    Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable }, 
    DataScheme = "https", 
    DataHost = "taskmon.com", 
    DataPathPrefix = "/sureveys/invite" )]

[IntentFilter(new[] { Intent.ActionView }, 
    Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable }, 
    DataScheme = "https", 
    DataHost = "taskmon.com", 
    DataPathPrefix = "/groups/invite" )]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        ProcessIntent(Intent);
    }

    protected override void OnNewIntent(Intent intent)
    {
        base.OnNewIntent(intent);
        ProcessIntent(intent);
    }

    private void ProcessIntent(Intent intent)
    {
        var uri = intent?.Data;
        if (uri != null)
        {
            string path = uri.Path;
            if (!string.IsNullOrEmpty(path))
            {
                if (path.StartsWith("/sureveys/invite/"))
                {
                    string surveyId = path.Replace("/sureveys/invite/", "");
                    if (Guid.TryParse(surveyId, out _))
                    {
                        Preferences.Set("PendingDeepLinkType", (int)DeepLinkType.Survey);
                        Preferences.Set("PendingDeepLinkId", surveyId);
                        Preferences.Set("HasPendingDeepLink", true);
                        
                        WeakReferenceMessenger.Default.Send(new DeepLinkMessage(DeepLinkType.Survey, surveyId));
                    }
                    else
                    {
                        WeakReferenceMessenger.Default.Send(new DeepLinkMessage(DeepLinkType.Survey, string.Empty));
                    }
                }
                else if (path.StartsWith("/groups/invite/"))
                {
                    string groupId = path.Replace("/groups/invite/", "");
                    if (Guid.TryParse(groupId, out _))
                    {
                        Preferences.Set("PendingDeepLinkType", (int)DeepLinkType.Group);
                        Preferences.Set("PendingDeepLinkId", groupId);
                        Preferences.Set("HasPendingDeepLink", true);
                        
                        WeakReferenceMessenger.Default.Send(new DeepLinkMessage(DeepLinkType.Group, groupId));
                    }
                    else
                    {
                        WeakReferenceMessenger.Default.Send(new DeepLinkMessage(DeepLinkType.Group, string.Empty));
                    }
                }
            }
        }
    }
}