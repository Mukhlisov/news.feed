using System.ComponentModel.DataAnnotations;
using news.feed.models.Dto;
using news.feed.Services.News;

namespace news.feed.Utilities.Attributes;

public sealed class ProgramValidationAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) => value switch
    {
        string program => ValidateStringValue(program, validationContext),
        CreateNewsDto saveNewsDto => ValidateStringValue(saveNewsDto.Program, validationContext),
        null => new ValidationResult("Input data is required"),
        _ => new ValidationResult("Unsupported data type")
    };

    private static ValidationResult? ValidateStringValue(string program, ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(program))
            return new ValidationResult("Program name must be a non-empty string");
        var programValidator = validationContext.GetRequiredService<ProgramValidator>();

        var isValid = programValidator.CheckProgramIsValidSync(program);
        return isValid ? ValidationResult.Success : new ValidationResult($"Program '{program}' is not valid");
    }
}