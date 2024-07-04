using System.ComponentModel.DataAnnotations;
using Dot.Net.WebApi.Domain;
using P7CreateRestApi.Tests;

namespace P7CreateRestApi.Tests;

public class UserTests
{
    private static User user = new()
    {
        UserName = "username",
        Email = "email@email",
        Fullname = "FullName",
        LastLoginDate = DateTime.UtcNow
    };

    [Theory]
    [InlineData("2024-07-04T00:00:00Z", true)]
    [InlineData("2020-07-04T00:00:00Z", false)]
    [InlineData(null, false)]
    public void Test_IsUserActive_UserLastLoginWithin2Years_ShouldReturnTrue(DateTime input, bool resultExpected)
    {
        // Arrange
        user.LastLoginDate = input;

        // Act
        bool result = user.IsUserActive();

        // Assert
        if (resultExpected)
        {
            Assert.True(result, "User should be considered active because last login < 2 years ago");
        }
        else
        {
            Assert.False(result, "User should not be considered active because last login > 2 years ago");
        }
    }

    [Theory]
    [InlineData("", false)]
    [InlineData(" ", false)]
    [InlineData(null, false)]
    [InlineData("FullName", true)]
    [InlineData("FullName with spaces", true)]
    [InlineData("123", false)]
    [InlineData("FullName 123 and spaces", false)]
    [InlineData("!@#$%^&*()", false)]
    [InlineData("!@#$%^&*() spaces", false)]
    [InlineData("123 !@#$%^&*() spaces", false)]
    [InlineData("FullnameThatIsTooLongBecauseTooMuchCharacterSuperiorTo25", false)]
    public void Test_FullName_ShouldReturnExpectedResult(string? input, bool resultExpected)
    {
        // Arrange
        user.Fullname = input;

        // Act
        bool result = ValidationHelper.TryValidateObject(user, out var validationResults);

        // Assert
        if (resultExpected)
        {
            Assert.True(result, "User should be valid because fullname is valid");
        }
        else
        {
            Assert.False(result, "User should not be valid because fullname is not valid");
        }
    }
}