using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Domain;

public class DoubleValidationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return new ValidationResult("Please enter a valid double value.");
        }

        if (double.TryParse(value.ToString(), out _))
        {
            return ValidationResult.Success;
        }

        return new ValidationResult("Please enter a valid double value.");
    }
}