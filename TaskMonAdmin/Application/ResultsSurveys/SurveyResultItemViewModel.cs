using CommunityToolkit.Mvvm.ComponentModel;
using StatisticsService.Client.Models;

namespace TaskMonAdmin.ViewModels
{
    public partial class SurveyResultItemViewModel : ObservableObject
    {
        [ObservableProperty]
        private Guid _surveyId;

        [ObservableProperty]
        private string _title;
        
        [ObservableProperty]
        private string _description;
        
        [ObservableProperty]
        private bool _isActive;
        
        [ObservableProperty]
        private int _partialSubmissions;
        
        [ObservableProperty]
        private int _completeSubmissions;
        
        public string TitleWithStatus => IsActive 
            ? $"{Title} (Активне)"
            : Title;
            
        public static SurveyResultItemViewModel FromModel(SurveyResults result)
        {
            return new SurveyResultItemViewModel
            {
                SurveyId = result.SurveyId,
                Title = result.Title,
                Description = result.Description,
                IsActive = result.IsActive,
                PartialSubmissions = result.Submissions.PartialSubmissions,
                CompleteSubmissions = result.Submissions.CompleteSubmissions
            };
        }
    }
}