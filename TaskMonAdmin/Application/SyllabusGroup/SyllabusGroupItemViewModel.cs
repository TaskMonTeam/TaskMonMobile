using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AdminService.Models;

namespace TaskMonAdmin.ViewModels
{
    public partial class SyllabusGroupItemViewModel : ObservableObject
    {
        [ObservableProperty]
        private Guid _id;

        [ObservableProperty]
        private string _title;
        
        [ObservableProperty]
        private DateTime _publishTime;
        
        [ObservableProperty]
        private DateTime? _archiveTime;
        
        [ObservableProperty]
        private Guid _courseId;
        
        public string TitleWithStatus => ArchiveTime.HasValue 
            ? Title
            : $"{Title} (Активний)";

        [RelayCommand]
        private async Task SelectSyllabus()
        {
            var navigationParameter = new Dictionary<string, object>
            {
                { "syllabusId", Id.ToString() },
                { "courseId", CourseId.ToString() }
            };

            await Shell.Current.GoToAsync($"SyllabusPage", navigationParameter);
        }

        public static SyllabusGroupItemViewModel FromModel(SyllabusBrief syllabus)
        {
            return new SyllabusGroupItemViewModel
            {
                Id = syllabus.Id,
                Title = syllabus.Title,
                PublishTime = syllabus.PublishTime,
                ArchiveTime = syllabus.ArchiveTime
            };
        }
    }
}