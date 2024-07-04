using Xunit;
using Dot.Net.WebApi.Domain;
using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Tests;

public class UserTests
{
    private static User user = new()
    {
        UserName = "username",
        Email = "email@email",
        Fullname = "Full name",
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

    // TODO: add fields validation test for user class
    

}
