using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Domain;

public class DoubleValidationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return new ValidationResult("Value is required.");
        }

        if (double.TryParse(value.ToString(), out _))
        {
        return ValidationResult.Success ?? new ValidationResult("Validation succeeded.");
        }

        return new ValidationResult("Please enter a valid double value.");
    }
}