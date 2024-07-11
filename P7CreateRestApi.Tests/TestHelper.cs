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

    public static void ValidateStringProperty<T>(T instance, string propertyName, string? input, int code, Func<T, Action> validateMethod)
    {
        // Arrange
        var property = typeof(T).GetProperty(propertyName);
        property.SetValue(instance, input);

        var expectedMessages = GetValidationMessages(property);
        int maxLength = GetMaxLength(property);

        // Act
        Exception? ex = Record.Exception(() => validateMethod(instance)());

        // Assert
        if (SampleTestVariables.stringMandatory.Contains(code))
        {
            AssertValidationException(ex, expectedMessages.Take(2).ToList());
        }
        else if (SampleTestVariables.moreThanNChar(maxLength).Contains(code))
        {
            AssertValidationException(ex, expectedMessages.Skip(2).Take(1).ToList());
        }
        else if (SampleTestVariables.lessThanNChar(maxLength).Contains(code))
        {
            Assert.Null(ex);
        }
        else
        {
            Assert.Fail($"Unexpected result for input {input}");
        }
    }

    public static List<string> GetValidationMessages(PropertyInfo property)
    {
        var attributes = property.GetCustomAttributes(typeof(ValidationAttribute), true);
        return attributes.Select(attr => ((ValidationAttribute)attr).ErrorMessage).ToList();
    }

    public static void AssertValidationException(Exception? ex, List<string> expectedMessages)
    {
        Assert.NotNull(ex);
        Assert.IsType<ValidationException>(ex);
        var validationException = (ValidationException)ex;

        bool containsExpectedMessage = expectedMessages.Any(message => validationException.Message.Contains(message));

        Assert.True(containsExpectedMessage, $"Expected one of the following messages: {string.Join(", ", expectedMessages)}. Actual message: {validationException.Message}");
    }


    public static int GetMaxLength(PropertyInfo property)
    {
        var maxLengthAttribute = property.GetCustomAttribute<MaxLengthAttribute>();
        if (maxLengthAttribute != null)
        {
            return maxLengthAttribute.Length;
        }

        var stringLengthAttribute = property.GetCustomAttribute<StringLengthAttribute>();
        if (stringLengthAttribute != null)
        {
            return stringLengthAttribute.MaximumLength;
        }

        throw new InvalidOperationException($"The property '{property.Name}' does not have a MaxLength or StringLength attribute.");
    }

}