using System.ComponentModel.DataAnnotations;
using Dot.Net.WebApi.Domain;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace P7CreateRestApi.Tests;

public class BidListTests
{
    public BidList bidList = new()
    {
        BidListId = 1,
        Account = "Account",
        BidType = "BidType",
        BidQuantity = 1,
        AskQuantity = 1,
        Bid = 1,
        Ask = 1,
        Benchmark = "Benchmark",
        BidListDate = DateTime.UtcNow,
        Commentary = "Commentary",
        BidSecurity = "BidSecurity",
        BidStatus = "BidStatus",
        Trader = "Trader",
        Book = "Book",
        CreationName = "CreationName",
        CreationDate = DateTime.UtcNow,
        RevisionName = "RevisionName",
        RevisionDate = DateTime.UtcNow,
        DealName = "DealName",
        DealType = "DealType",
        SourceListId = "SourceListId",
        Side = "Side"
    };

    public const string? stringNull = null;
    public const string? string51 = "50string6666666666666666666666666666666666666666666";
    public const string? string101 = "100string66666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666";
    private const string? string501 = "501string66666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666" 
                                        + "6666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666"
                                        + "6666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666"
                                        + "6666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666"
                                        + "6666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666"
                                        + "6666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666";
    public const string? stringEmpty = "";
    public const string? stringSmall = "string";
    public const string? stringWithNumbers = "1234567890";
    public const string? stringWithSpecialChars = "!@#$%^&*()";


    [Fact]
    public void Test_Validate_WithValidBidList_ShouldNotThrowException()
    {
        // Arrange

        // Act
        Exception? ex = Record.Exception(() => bidList.Validate());

        // Assert
        Assert.Null(ex);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(int.MinValue + 1)]
    [InlineData(int.MaxValue - 1)]
    public void Test_Validate_WithValidBidList_GoodBidListId_ShouldNotThrowException(int input)
    {
        // Arrange
        bidList.BidListId = input;

        // Act
        Exception? ex = Record.Exception(() => bidList.Validate());

        // Assert
        Assert.Null(ex);
        Assert.Equal(bidList.BidListId, input);
    }

    [Theory]
    [InlineData(stringNull)]
    [InlineData(stringEmpty)]
    [InlineData(string51)]
    public void Test_Validate_WithInvalidBidList_ShouldThrowValidationException(string? input)
    {
        // Arrange
        var expectedMessages = new List<string>
            {
                "Account is mandatory",
                "Account must be a string",
                "Account can't be longer than 50 characters"
            };

        bidList.Account = input;

        // Act
        Exception? ex = Record.Exception(() => bidList.Validate());

        // Assert
        Assert.NotNull(ex);
        Assert.IsType<ValidationException>(ex);
        var validationException = (ValidationException)ex;
        bool containsExpectedMessage = false;
        foreach (var message in expectedMessages)
        {
            if (validationException.Message.Contains(message))
            {
                containsExpectedMessage = true;
                break;
            }
        }
        Assert.True(containsExpectedMessage, $"Expected one of the following messages: {string.Join(", ", expectedMessages)}. Actual message: {validationException.Message}");
    }

    [Theory]
    [InlineData(stringNull)]
    [InlineData(stringEmpty)]
    [InlineData(string51)]
    public void Test_Validate_WithInvalidBidType_ShouldThrowValidationException(string? input)
    {
        // Arrange
        var expectedMessages = new List<string>
            {
                "BidType is mandatory",
                "BidType must be a string",
                "BidType can't be longer than 50 characters"
            };
        bidList.BidType = input;

        // Act
        Exception? ex = Record.Exception(() => bidList.Validate());

        // Assert
        Assert.NotNull(ex);
        Assert.IsType<ValidationException>(ex);
        var validationException = (ValidationException)ex;
        bool containsExpectedMessage = false;
        foreach (var message in expectedMessages)
        {
            if (validationException.Message.Contains(message))
            {
                containsExpectedMessage = true;
                break;
            }
        }
        Assert.True(containsExpectedMessage, $"Expected one of the following messages: {string.Join(", ", expectedMessages)}. Actual message: {validationException.Message}");
    }


    [Theory]
    [InlineData(stringNull)]
    [InlineData(stringEmpty)]
    [InlineData(string101)]
    public void Test_Validate_WithInvalidBenchmark_ShouldThrowValidationException(string? input)
    {
        // Arrange
        var expectedMessages = new List<string>
            {
                "Benchmark is mandatory",
                "Benchmark must be a string",
                "Benchmark can't be longer than 100 characters"
            };
        bidList.Benchmark = input;

        // Act
        Exception? ex = Record.Exception(() => bidList.Validate());

        // Assert
        Assert.NotNull(ex);
        Assert.IsType<ValidationException>(ex);
        var validationException = (ValidationException)ex;
        bool containsExpectedMessage = false;
        foreach (var message in expectedMessages)
        {
            if (validationException.Message.Contains(message))
            {
                containsExpectedMessage = true;
                break;
            }
        }
        Assert.True(containsExpectedMessage, $"Expected one of the following messages: {string.Join(", ", expectedMessages)}. Actual message: {validationException.Message}");
    }

    [Theory]
    [InlineData(stringNull)]
    [InlineData(stringEmpty)]
    [InlineData(string501)]
    public void Test_Validate_WithInvalidCommentary_ShouldThrowValidationException(string? input)
    {
        // Arrange
        var expectedMessages = new List<string>
            {
                "Commentary is mandatory",
                "Commentary must be a string",
                "Commentary can't be longer than 500 characters"
            };
        bidList.Commentary = input;

        // Act
        Exception? ex = Record.Exception(() => bidList.Validate());

        // Assert
        Assert.NotNull(ex);
        Assert.IsType<ValidationException>(ex);
        var validationException = (ValidationException)ex;
        bool containsExpectedMessage = false;
        foreach (var message in expectedMessages)
        {
            if (validationException.Message.Contains(message))
            {
                containsExpectedMessage = true;
                break;
            }
        }
        Assert.True(containsExpectedMessage, $"Expected one of the following messages: {string.Join(", ", expectedMessages)}. Actual message: {validationException.Message}");
    }

}