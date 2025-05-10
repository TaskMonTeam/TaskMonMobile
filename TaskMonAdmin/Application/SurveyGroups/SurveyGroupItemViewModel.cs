using CommunityToolkit.Mvvm.ComponentModel;
using AdminService.Models;
using CommunityToolkit.Mvvm.Input;

namespace TaskMonAdmin.ViewModels
{
    public partial class SurveyGroupItemViewModel : ObservableObject
    {
        [ObservableProperty]
        private Guid _id;

        [ObservableProperty]
        private string _title;
        
        [ObservableProperty]
        private string _description;
        
        [ObservableProperty]
        private DateTime _publishDate;
        
        [ObservableProperty]
        private bool _isActive;
        
        [ObservableProperty]
        private List<Guid> _surveysIds;
        
        public string TitleWithStatus => IsActive 
            ? $"{Title} (Активне)"
            : Title;
        
        [RelayCommand]
        private async Task CopySurveyGroupLink()
        {
            var link = $"https://taskmon.com/groups/invite/{Id}";
            await Clipboard.SetTextAsync(link);
        }

        public static SurveyGroupItemViewModel FromModel(SurveyGroup surveyGroup)
        {
            return new SurveyGroupItemViewModel
            {
                Id = surveyGroup.Id,
                Title = surveyGroup.Title,
                Description = surveyGroup.Description,
                PublishDate = surveyGroup.PublishDate,
                IsActive = surveyGroup.IsActive,
                SurveysIds = surveyGroup.SurveysIds.ToList()
            };
        }
    }
}