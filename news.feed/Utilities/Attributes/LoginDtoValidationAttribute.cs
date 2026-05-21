using System.ComponentModel.DataAnnotations;
using news.feed.models.Dto.Auth;

namespace news.feed.Utilities.Attributes;

public class LoginDtoValidationAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not LoginDto loginDto)
            return new ValidationResult("Unsupported data type");

        if (string.IsNullOrEmpty(loginDto.Login) && string.IsNullOrEmpty(loginDto.Password))
            return new ValidationResult("Login or password must be provided");
        return ValidationResult.Success;
    }
}