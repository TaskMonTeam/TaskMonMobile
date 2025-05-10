using CommunityToolkit.Mvvm.ComponentModel;
using StatisticsService.Client.Models;

namespace TaskMonAdmin.ViewModels
{
    public partial class SurveyGroupResultItemViewModel : ObservableObject
    {
        [ObservableProperty]
        private Guid _groupId;

        [ObservableProperty]
        private string _title;
        
        [ObservableProperty]
        private string _description;
        
        [ObservableProperty]
        private bool _isActive;
        
        [ObservableProperty]
        private int _subjects;
        
        [ObservableProperty]
        private int _partialSubmissions;
        
        [ObservableProperty]
        private int _completeSubmissions;
        
        public string TitleWithStatus => IsActive 
            ? $"{Title} (Активне)"
            : Title;
            
        public static SurveyGroupResultItemViewModel FromModel(SurveyGroupResults result)
        {
            return new SurveyGroupResultItemViewModel
            {
                GroupId = result.GroupId,
                Title = result.Title,
                Description = result.Description,
                IsActive = result.IsActive,
                Subjects = result.Subjects,
                PartialSubmissions = result.Submissions.PartialSubmissions,
                CompleteSubmissions = result.Submissions.CompleteSubmissions
            };
        }
    }
}