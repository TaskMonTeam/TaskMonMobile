using TaskMonAdmin.ViewModels;

namespace TaskMonAdmin;

public partial class LinkPage : ContentPage
{
    public LinkPage()
    {
        InitializeComponent();
        BindingContext = new LinkPageViewModel();
    }
}