using InputKit.Shared.Validations;

namespace TaskMonMobile.Common.Validators;

public class NumberValidator : IValidation
{
    public string Message { get; set; }
    
    public int MinValue { get; set; }
    
    public int MaxValue { get; set; }

    public bool Validate(object value)
    {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
        {
            Message = "Введіть значення";
            return false;
        }
        
        if (int.TryParse(value.ToString(), out int number))
        {
            if (number < MinValue)
            {
                Message = $"Введіть більше за {MinValue}";
                return false;
            }

            if (number > MaxValue)
            {
                Message = $"Введіть менше за {MaxValue}";
                return false;
            }
            
            return true;
        }
        
        Message = "Введіть числове значення";
        return false;
    }
}