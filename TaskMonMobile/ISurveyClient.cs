using Refit;
using TaskMonMobile.Common.Models;

namespace TaskMonMobile;

public interface ISurveyClient
{
    [Get("/surveys/{id}")]
    Task<Survey> GetSurveyByIdAsync(string id);
}