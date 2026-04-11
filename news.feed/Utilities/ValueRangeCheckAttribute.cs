using System.ComponentModel.DataAnnotations;

namespace news.feed.Utilities;

public class ValueRangeCheckAttribute : ValidationAttribute
{
    private readonly int _min;
    private readonly int _max;

    public ValueRangeCheckAttribute(int min, int max)
    {
        _min = min;
        _max = max;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not int intValue)
            return new ValidationResult($"The {validationContext.DisplayName} field must be an integer.");
        
        return intValue.InRange(_min, _max)
            ? ValidationResult.Success
            : new ValidationResult($"The {validationContext.DisplayName} field must be between {_min} and {_max}.");
    }
}