using InputKit.Shared.Validations;

namespace TaskMonMobile.Common.Validators;

public class NumberValidator : IValidation
{
    public string Message { get; set; }
    
    public int MinValue { get; set; } = int.MinValue;
    
    public int MaxValue { get; set; } = int.MaxValue;

    public bool IsRequired { get; set; }

    public bool Validate(object value)
    {
        if (!IsRequired && (value == null || string.IsNullOrWhiteSpace(value.ToString())))
        {
            return true;
        }
        
        if (value != null && int.TryParse(value.ToString(), out int number))
        {
            if (number < MinValue)
            {
                Message = $"Значення має бути не менше {MinValue}";
                return false;
            }

            if (number > MaxValue)
            {
                Message = $"Значення має бути не більше {MaxValue}";
                return false;
            }
            
            return true;
        }
        
        Message = "Введіть коректне числове значення";
        return false;
    }
}