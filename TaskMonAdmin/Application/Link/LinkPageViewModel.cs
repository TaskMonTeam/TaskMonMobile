using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace TaskMonAdmin.ViewModels
{
    [QueryProperty(nameof(Link), nameof(Link))]
    public partial class LinkPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _link;

        [RelayCommand]
        private async Task CopyToClipboard()
        {
            if (!string.IsNullOrEmpty(Link))
            {
                await Clipboard.SetTextAsync(Link);
            }
        }
    }
}