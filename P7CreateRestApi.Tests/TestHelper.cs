using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Dot.Net.WebApi.Domain;

namespace P7CreateRestApi.Tests;

public static class TestHelper
{
    public static bool TryValidateObject(object obj, out List<ValidationResult> results)
    {
        ValidationContext context = new(obj);
        results = new List<ValidationResult>();
        return Validator.TryValidateObject(obj, context, results, true);
    }

    /// <summary>
    /// Validates a string property of an instance of a specified type.
    /// </summary>
    /// <typeparam name="T">The type of the instance.</typeparam>
    /// <param name="instance">The instance whose property is to be validated.</param>
    /// <param name="propertyName">The name of the string property to be validated.</param>
    /// <param name="input">The input string to be validated.</param>
    /// <param name="description">The description of the input string.</param>
    /// <param name="validateMethod">The method used to validate the instance.</param>
    /// <param name="minLength">The minimum length of the input string.</param>
    /// <param name="maxLength">The maximum length of the input string. If null, the maximum length is retrieved from the property's validation attributes.</param>
    /// <param name="mandatory">Indicates whether the input string is mandatory.</param>
    /// <param name="NoSpecialChars">Indicates whether the input string should not contain special characters.</param>
    /// <param name="NoNumbers">Indicates whether the input string should not contain numbers.</param>
    public static void ValidateStringProperty<T>(
        T instance,
        string propertyName,
        string? input,
        string description,
        int minLength = 0,
        int? maxLength = null,
        bool mandatory = false,
        bool NoSpecialChars = false,
        bool NoNumbers = false
    ) where T : IValidatable
    {
        // Arrange

        // Get the property of the specified type and name
        PropertyInfo? property = typeof(T).GetProperty(propertyName);

        // Set the value of the property to the specified input string
        if (property != null)
        {
            // Set the value of the property to the specified input string
            property.SetValue(instance, input);
        }
        else
        {
            throw new ArgumentException($"Property {propertyName} not found in type {typeof(T).Name}");
        }

        // Get the expected validation messages for the property
        List<string> expectedMessages = GetValidationMessages(property);

        // Get the effective maximum length of the input string
        int effectiveMaxLength = maxLength ?? GetMaxLength(property);

        // Act

        // Try to validate the instance using the validation method
        Exception? ex = Record.Exception(() => instance.Validate());

        // Assert

        // Check if the input string is mandatory and null or empty
        if (mandatory && (input == null || input.Trim() == ""))
        {
            // Assert that the validation exception contains the expected validation messages for mandatory input
            AssertValidationException(ex, expectedMessages.Where(msg => msg.Contains($"{propertyName} is mandatory")).ToList());
        }
        // Check if the length of the input string is less than the minimum length
        else if (input != null && input.Length < minLength)
        {
            // Assert that the validation exception contains the expected validation messages for minimum length violation
            AssertValidationException(ex, expectedMessages.Where(msg => msg.Contains($"{propertyName} must be at least {minLength} characters long")).ToList());
        }
        // Check if the length of the input string is greater than the effective maximum length
        else if (input != null && input.Length > effectiveMaxLength)
        {
            // Assert that the validation exception contains the expected validation messages for maximum length violation
            AssertValidationException(ex, expectedMessages.Where(msg => msg.Contains($"{propertyName} can't be longer than {effectiveMaxLength} characters")).ToList());
        }
        // Check if the input string contains special characters and NoSpecialChars is true
        else if (NoSpecialChars && input != null && input.Any(ch => !char.IsLetterOrDigit(ch)))
        {
            // Assert that the validation exception contains the expected validation messages for special characters violation
            AssertValidationException(ex, expectedMessages.Where(msg => msg.Contains($"{propertyName} should contain only letters and spaces")).ToList());
        }
        // Check if the input string contains numbers and NoNumbers is true
        else if (NoNumbers && input != null && input.Any(ch => char.IsDigit(ch)))
        {
            // Assert that the validation exception contains the expected validation messages for numbers violation
            AssertValidationException(ex, expectedMessages.Where(msg => msg.Contains($"{propertyName} should contain only letters and spaces")).ToList());
        }
        // If no validation errors are found, assert that no exception is thrown
        else
        {
            Assert.Null(ex);
        }
    }

    public static List<string> GetValidationMessages(PropertyInfo property)
    {
        object[] attributes = property.GetCustomAttributes(typeof(ValidationAttribute), true);
        return attributes.Select(static attr => ((ValidationAttribute)attr).ErrorMessage).ToList();
    }

    public static void AssertValidationException(Exception? ex, List<string> expectedMessages)
    {
        Assert.NotNull(ex);
        Assert.IsType<ValidationException>(ex);
        ValidationException validationException = (ValidationException)ex;

        bool containsExpectedMessage = expectedMessages.Exists(message => validationException.Message.Contains(message));

        Assert.True(containsExpectedMessage, $"Expected one of the following messages: \"{string.Join(", ", expectedMessages)}\". Actual message: \"{validationException.Message}\".");
    }

    public static int GetMaxLength(PropertyInfo property)
    {
        MaxLengthAttribute? maxLengthAttribute = property.GetCustomAttribute<MaxLengthAttribute>();
        if (maxLengthAttribute != null)
        {
            return maxLengthAttribute.Length;
        }

        StringLengthAttribute? stringLengthAttribute = property.GetCustomAttribute<StringLengthAttribute>();
        if (stringLengthAttribute != null)
        {
            return stringLengthAttribute.MaximumLength;
        }

        throw new InvalidOperationException($"The property '{property.Name}' does not have a MaxLength or StringLength attribute.");
    }
}