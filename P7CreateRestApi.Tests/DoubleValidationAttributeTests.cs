using System.ComponentModel.DataAnnotations;
using Dot.Net.WebApi.Domain;

namespace P7CreateRestApi.Tests;

public class DoubleValidationAttributeTests
{
    private static readonly ValidationResult ValidationSuccess = ValidationResult.Success;
    private readonly string ValidationFailMessage = "Please enter a valid double value.";

    [Theory]
    [InlineData("123.45", true)]
    [InlineData("0", true)]
    [InlineData("-123.45", true)]
    [InlineData("abc", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void IsValid_ShouldReturnExpectedResult(string? input, bool resultExpected)
    {
        // Arrange
        DoubleValidationAttribute attribute = new();
        ValidationContext validationContext = new(new object());

        // Act
        ValidationResult? result = attribute.GetValidationResult(input, validationContext);

        // Assert
        if (resultExpected)
        {
            Assert.Equal(ValidationSuccess, result);
        }
        else
        {
            Assert.NotEqual(ValidationSuccess, result);
            Assert.Equal(ValidationFailMessage, result.ErrorMessage);
        }
    }
}