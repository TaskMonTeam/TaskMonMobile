using System.Text.Json;
using TaskMonMobile.Common.Models;
using TaskMonMobile.ViewModels;

namespace TaskMonMobile;

public partial class MainPage : ContentPage
{
    private readonly MainPageViewModel _viewModel;

    public MainPage(ISurveyClient surveyClient)
    {
        InitializeComponent();
        _viewModel = new MainPageViewModel(surveyClient);
        BindingContext = _viewModel;
        
        Loaded += OnPageLoaded;
    }
    
    private async void OnPageLoaded(object sender, EventArgs e)
    {
        await _viewModel.LoadSurveyDataAsync();
    }
}