using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImportService.Client;
using Refit;

namespace TaskMonAdmin.ViewModels;

public partial class ImportSyllabusPageViewModel : ObservableObject
{
    private readonly IImportClient _importClient;
    private FileResult _selectedFile;

    [ObservableProperty]
    private string _fileName;

    [ObservableProperty]
    private bool _isFileSelected;

    public ImportSyllabusPageViewModel(IImportClient importClient)
    {
        _importClient = importClient;
        IsFileSelected = false;
        FileName = "Файл не обрано";
    }

    [RelayCommand]
    private async Task PickAndImportFile()
    {
        try
        {
            _selectedFile = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Оберіть PDF файл",
                FileTypes = FilePickerFileType.Pdf
            });

            if (_selectedFile != null)
            {
                FileName = _selectedFile.FileName;
                IsFileSelected = true;
            }
            else
            {
                FileName = "Файл не обрано";
                IsFileSelected = false;
            }
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Помилка", $"Помилка при виборі файлу: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    private async Task ImportSyllabus()
    {
        if (_selectedFile == null)
        {
            await Application.Current.MainPage.DisplayAlert("Помилка", "Будь ласка, оберіть файл перед імпортом", "OK");
            return;
        }

        try
        {
            using var stream = await _selectedFile.OpenReadAsync();
            byte[] fileBytes;
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                fileBytes = memoryStream.ToArray();
            }

            var fileToSend = new ByteArrayPart(fileBytes, _selectedFile.FileName, "application/pdf");

            await _importClient.ImportSyllabus(fileToSend);

            await Application.Current.MainPage.DisplayAlert("Успіх", "Сілабус успішно імпортовано", "OK");
            
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Помилка", $"Помилка при імпорті сілабусу: {ex.Message}", "OK");
        }
    }
}