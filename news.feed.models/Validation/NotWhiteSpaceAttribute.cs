using System.ComponentModel.DataAnnotations;

namespace news.feed.models.Validation;

public class NotWhiteSpaceAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is not string str)
            return true;

        return !string.IsNullOrWhiteSpace(str);
    }

    public override string FormatErrorMessage(string name)
    {
        return $"{name} cannot be empty or whitespace only.";
    }
}
