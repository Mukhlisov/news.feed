using System.ComponentModel.DataAnnotations;
using news.feed.Services;

namespace news.feed.Utilities;

public sealed class ProgramValidationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not string program || string.IsNullOrWhiteSpace(program))
            return new ValidationResult("Program name must be a non-empty string");
        var programValidator = validationContext.GetRequiredService<ProgramValidator>();

        var isValid = programValidator.CheckProgramIsValid(program).GetAwaiter().GetResult();
        return isValid ? ValidationResult.Success! : new ValidationResult($"Program '{program}' is not valid");
    }
}