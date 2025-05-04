using CommunityToolkit.Mvvm.ComponentModel;

namespace TaskMonAdmin.ViewModels
{
    public partial class SurveyPickerItemViewModel : ObservableObject
    {
        [ObservableProperty]
        private SurveyItemViewModel? _selectedSurvey;
    }
}