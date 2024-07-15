// using System.ComponentModel.DataAnnotations;
// using System.Globalization;
// using Dot.Net.WebApi.Domain;

// namespace P7CreateRestApi.Tests;

// public class RatingTests
// {
//     // --------------- VALID DATA ------------------
//     // Rating with all valid data is declarated here because it will be used in all tests
//     public Rating rating = new()
//     {
//         Id = 1,
//         MoodysRating = "MRating",
//         SandPRating = "SPRating",
//         FitchRating = "FRating",
//         OrderNumber = 1
//     };

//     // --------------- VALID DATA ------------------

//     // ALL DATA VALID
//     [Fact]
//     public void Test_Validate_WithValidRating_ShouldNotThrowException()
//     {
//         // Arrange

//         // Act
//         Exception? ex = Record.Exception(() => rating.Validate());

//         // Assert
//         Assert.Null(ex);
//     }

//     // --------------- NUMERIC PROPERTIES TESTS ------------------
    

//     [Theory]
//     [InlineData(byte.MinValue)]
//     [InlineData(1)]
//     [InlineData(byte.MaxValue)]
//     public void Test_Validate_WithValidRating_OrderNumber_ShouldNotThrowException(byte input)
//     {
//         // Arrange
//         rating.OrderNumber = input;

//         // Act
//         Exception? ex = Record.Exception(() => rating.Validate());

//         // Assert
//         Assert.Null(ex);
//         Assert.Equal(rating.OrderNumber, input);
//     }

//     [Fact]
//     public void Test_Validate_WithNullRating_OrderNumber_ShouldNotThrowException()
//     {
//         // Arrange
//         rating.OrderNumber = null;

//         // Act
//         Exception? ex = Record.Exception(() => rating.Validate());

//         // Assert
//         Assert.Null(ex);
//         Assert.Null(rating.OrderNumber);
//     }

//     // --------------- NUMERIC PROPERTIES TESTS ------------------

//     // --------------- STRING PROPERTIES TESTS ------------------

//     // MoodysRating - 10 characters max - Mandatory
//     [Theory]
//     [MemberData(nameof(SampleTestVariables.StringCombinationsTest), MemberType = typeof(SampleTestVariables))]
//     public void Test_Validate_MoodysRating_StringVariation_ShouldReturnExpectedResults(string? input, int code)
//     {
//         TestHelper.ValidateStringProperty(rating, nameof(Rating.MoodysRating), input, code, mandatory: true);
//     }

//     // SandPRating - 10 characters max - Mandatory
//     [Theory]
//     [MemberData(nameof(SampleTestVariables.StringCombinationsTest), MemberType = typeof(SampleTestVariables))]
//     public void Test_Validate_SandPRating_StringVariation_ShouldReturnExpectedResults(string? input, int code)
//     {
//         TestHelper.ValidateStringProperty(rating, nameof(Rating.SandPRating), input, code, maxLength: 10, mandatory: true);
//     }

//     // FitchRating - 10 characters max - Mandatory
//     [Theory]
//     [MemberData(nameof(SampleTestVariables.StringCombinationsTest), MemberType = typeof(SampleTestVariables))]
//     public void Test_Validate_FitchRating_StringVariation_ShouldReturnExpectedResults(string? input, int code)
//     {
//         TestHelper.ValidateStringProperty(rating, nameof(Rating.FitchRating), input, code, maxLength: 10, mandatory: true);
//     }

//     // --------------- STRING PROPERTIES TESTS ------------------
// }