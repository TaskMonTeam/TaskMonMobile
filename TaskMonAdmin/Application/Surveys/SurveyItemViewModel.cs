using CommunityToolkit.Mvvm.ComponentModel;
using AdminService.Models;

namespace TaskMonAdmin.ViewModels
{
    public partial class SurveyItemViewModel : ObservableObject
    {
        [ObservableProperty]
        private Guid _id;

        [ObservableProperty]
        private string _title;
        
        [ObservableProperty]
        private string? _description;
        
        [ObservableProperty]
        private DateTime _publishDate;
        
        [ObservableProperty]
        private Guid _syllabusId;
        
        [ObservableProperty]
        private bool _isActive;
        
        public string TitleWithStatus => IsActive 
            ? $"{Title} (Активне)"
            : Title;
        
        public override string ToString()
        {
            return Title;
        }

        public static SurveyItemViewModel FromModel(Survey survey)
        {
            return new SurveyItemViewModel
            {
                Id = survey.Id,
                Title = survey.Title,
                Description = survey.Description,
                PublishDate = survey.PublishDate,
                SyllabusId = survey.SyllabusId,
                IsActive = survey.IsActive
            };
        }
    }
}