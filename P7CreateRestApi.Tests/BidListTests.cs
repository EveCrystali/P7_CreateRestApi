using System.ComponentModel.DataAnnotations;
using Dot.Net.WebApi.Domain;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Linq;

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

    // ALL DATA VALID
    [Fact]
    public void Test_Validate_WithValidBidList_ShouldNotThrowException()
    {
        // Arrange

        // Act
        Exception? ex = Record.Exception(() => bidList.Validate());

        // Assert
        Assert.Null(ex);
    }

    // Now testing sample string combinations on the different property

    // Account
    [Theory]
    [MemberData(nameof(SampleTestVariables.StringCombinationsTest), MemberType = typeof(SampleTestVariables))]
    public void Test_Validate_Account_StringVariation_ShouldReturnExpectedResults(string? input, int code)
    {
        // Arrange
        bidList.Account = input;
        int i = 0;

        var expectedMessages = new List<string>
        {
            "Account is mandatory",
            "Account must be a string",
            "Account can't be longer than 50 characters"
        };

        // Act
        Exception? ex = Record.Exception(() => bidList.Validate());

        // Assert
        // [Required(ErrorMessage = "Account is mandatory")]
        // [DataType(DataType.Text, ErrorMessage = "Account must be a string")]
        if (SampleTestVariables.stringMandatory.Contains(code))
        {
            Assert.NotNull(ex);
            Assert.IsType<ValidationException>(ex);
            var validationException = (ValidationException)ex;

            // Check if the exception message contains any of the expected messages
            bool containsExpectedMessage = expectedMessages.Any(message => validationException.Message.Contains(message));

            Assert.True(containsExpectedMessage, $"Expected one of the following messages: " +
                $"\"Account is mandatory\",\r\n or \"Account must be a string\". Actual message: {validationException.Message}");
        }
        // [MaxLength(50, ErrorMessage = "Account can't be longer than 50 characters")]
        else if (SampleTestVariables.moreThan51Char.Contains(code))
        {
            Assert.NotNull(ex);
            Assert.IsType<ValidationException>(ex);
            var validationException = (ValidationException)ex;

            // Check if the exception message contains the length message
            bool containsExpectedMessage = validationException.Message.Contains(expectedMessages[2]);

            Assert.True(containsExpectedMessage, $"Expected message: {expectedMessages[2]}. Actual message: {validationException.Message}");
        }
        // Valid DATA
        else if (SampleTestVariables.lessThan51Char.Contains(code))
        {
            Assert.Null(ex);
            Assert.Equal(bidList.Account, input);
        }
        else
        {
            i++;
            Assert.Fail("Unexpected result {i} times");
        }
    }

    // BidType
    [Theory]
    [MemberData(nameof(SampleTestVariables.StringCombinationsTest), MemberType = typeof(SampleTestVariables))]
    public void Test_Validate_BidType_StringVariation_ShouldReturnExpectedResults(string? input, int code)
    {
        // Arrange
        bidList.BidType = input;
        int i = 0;

        var expectedMessages = new List<string>
        {
            "BidType is mandatory",
            "BidType must be a string",
            "BidType can't be longer than 50 characters"
        };

        // Act
        Exception? ex = Record.Exception(() => bidList.Validate());

        // Assert
        // [Required(ErrorMessage = "BidType is mandatory")]
        // [DataType(DataType.Text, ErrorMessage = "BidType must be a string")]
        if (SampleTestVariables.stringMandatory.Contains(code))
        {
            Assert.NotNull(ex);
            Assert.IsType<ValidationException>(ex);
            var validationException = (ValidationException)ex;

            // Check if the exception message contains any of the expected messages
            bool containsExpectedMessage = expectedMessages.Any(message => validationException.Message.Contains(message));

            Assert.True(containsExpectedMessage, $"Expected one of the following messages: " +
                $"\"BidType is mandatory\",\r\n or \"BidType must be a string\". Actual message: {validationException.Message}");
        }
        // [MaxLength(50, ErrorMessage = "BidType can't be longer than 50 characters")]
        else if (SampleTestVariables.moreThan51Char.Contains(code))
        {
            Assert.NotNull(ex);
            Assert.IsType<ValidationException>(ex);
            var validationException = (ValidationException)ex;

            // Check if the exception message contains the length message
            bool containsExpectedMessage = validationException.Message.Contains(expectedMessages[2]);

            Assert.True(containsExpectedMessage, $"Expected message: {expectedMessages[2]}. Actual message: {validationException.Message}");
        }
        // Valid DATA
        else if (SampleTestVariables.lessThan51Char.Contains(code))
        {
            Assert.Null(ex);
            Assert.Equal(bidList.BidType, input);
        }
        else
        {
            i++;
            Assert.Fail("Unexpected result {i} times");
        }
    }

    // BidSecurity
    [Theory]
    [MemberData(nameof(SampleTestVariables.StringCombinationsTest), MemberType = typeof(SampleTestVariables))]
    public void Test_Validate_BidSecurity_StringVariation_ShouldReturnExpectedResults(string? input, int code)
    {
        // Arrange
        bidList.BidSecurity = input;
        int i = 0;

        var expectedMessages = new List<string>
        {
            "BidSecurity is mandatory",
            "BidSecurity must be a string",
            "BidSecurity can't be longer than 50 characters"
        };

        // Act
        Exception? ex = Record.Exception(() => bidList.Validate());

        // Assert
        // [Required(ErrorMessage = "BidSecurity is mandatory")]
        // [DataType(DataType.Text, ErrorMessage = "BidSecurity must be a string")]
        if (SampleTestVariables.stringMandatory.Contains(code))
        {
            Assert.NotNull(ex);
            Assert.IsType<ValidationException>(ex);
            var validationException = (ValidationException)ex;

            // Check if the exception message contains any of the expected messages
            bool containsExpectedMessage = expectedMessages.Any(message => validationException.Message.Contains(message));

            Assert.True(containsExpectedMessage, $"Expected one of the following messages: " +
                $"\"BidSecurity is mandatory\",\r\n or \"BidSecurity must be a string\". Actual message: {validationException.Message}");
        }
        // [MaxLength(50, ErrorMessage = "BidSecurity can't be longer than 50 characters")]
        else if (SampleTestVariables.moreThan51Char.Contains(code))
        {
            Assert.NotNull(ex);
            Assert.IsType<ValidationException>(ex);
            var validationException = (ValidationException)ex;

            // Check if the exception message contains the length message
            bool containsExpectedMessage = validationException.Message.Contains(expectedMessages[2]);

            Assert.True(containsExpectedMessage, $"Expected message: {expectedMessages[2]}. Actual message: {validationException.Message}");
        }
        // Valid DATA
        else if (SampleTestVariables.lessThan51Char.Contains(code))
        {
            Assert.Null(ex);
            Assert.Equal(bidList.BidSecurity, input);
        }
        else
        {
            i++;
            Assert.Fail("Unexpected result {i} times");
        }
    }

    // BidStatus
    [Theory]
    [MemberData(nameof(SampleTestVariables.StringCombinationsTest), MemberType = typeof(SampleTestVariables))]
    public void Test_Validate_BidStatus_StringVariation_ShouldReturnExpectedResults(string? input, int code)
    {
        // Arrange
        bidList.BidStatus = input;
        int i = 0;

        var expectedMessages = new List<string>
        {
            "BidStatus is mandatory",
            "BidStatus must be a string",
            "BidStatus can't be longer than 50 characters"
        };

        // Act
        Exception? ex = Record.Exception(() => bidList.Validate());

        // Assert
        // [Required(ErrorMessage = "BidStatus is mandatory")]
        // [DataType(DataType.Text, ErrorMessage = "BidStatus must be a string")]
        if (SampleTestVariables.stringMandatory.Contains(code))
        {
            Assert.NotNull(ex);
            Assert.IsType<ValidationException>(ex);
            var validationException = (ValidationException)ex;

            // Check if the exception message contains any of the expected messages
            bool containsExpectedMessage = expectedMessages.Any(message => validationException.Message.Contains(message));

            Assert.True(containsExpectedMessage, $"Expected one of the following messages: " +
                $"\"BidStatus is mandatory\",\r\n or \"BidStatus must be a string\". Actual message: {validationException.Message}");
        }
        // [MaxLength(50, ErrorMessage = "BidStatus can't be longer than 50 characters")]
        else if (SampleTestVariables.moreThan51Char.Contains(code))
        {
            Assert.NotNull(ex);
            Assert.IsType<ValidationException>(ex);
            var validationException = (ValidationException)ex;

            // Check if the exception message contains the length message
            bool containsExpectedMessage = validationException.Message.Contains(expectedMessages[2]);

            Assert.True(containsExpectedMessage, $"Expected message: {expectedMessages[2]}. Actual message: {validationException.Message}");
        }
        // Valid DATA
        else if (SampleTestVariables.lessThan51Char.Contains(code))
        {
            Assert.Null(ex);
            Assert.Equal(bidList.BidStatus, input);
        }
        else
        {
            i++;
            Assert.Fail("Unexpected result {i} times");
        }
    }


    // Trader
    [Theory]
    [MemberData(nameof(SampleTestVariables.StringCombinationsTest), MemberType = typeof(SampleTestVariables))]
    public void Test_Validate_Trader_StringVariation_ShouldReturnExpectedResults(string? input, int code)
    {
        // Arrange
        bidList.Trader = input;
        int i = 0;

        var expectedMessages = new List<string>
        {
            "Trader is mandatory",
            "Trader must be a string",
            "Trader can't be longer than 50 characters"
        };

        // Act
        Exception? ex = Record.Exception(() => bidList.Validate());

        // Assert
        // [Required(ErrorMessage = "Trader is mandatory")]
        // [DataType(DataType.Text, ErrorMessage = "Trader must be a string")]
        if (SampleTestVariables.stringMandatory.Contains(code))
        {
            Assert.NotNull(ex);
            Assert.IsType<ValidationException>(ex);
            var validationException = (ValidationException)ex;

            // Check if the exception message contains any of the expected messages
            bool containsExpectedMessage = expectedMessages.Any(message => validationException.Message.Contains(message));

            Assert.True(containsExpectedMessage, $"Expected one of the following messages: " +
                $"\"Trader is mandatory\",\r\n or \"Trader must be a string\". Actual message: {validationException.Message}");
        }
        // [MaxLength(50, ErrorMessage = "Trader can't be longer than 50 characters")]
        else if (SampleTestVariables.moreThan51Char.Contains(code))
        {
            Assert.NotNull(ex);
            Assert.IsType<ValidationException>(ex);
            var validationException = (ValidationException)ex;

            // Check if the exception message contains the length message
            bool containsExpectedMessage = validationException.Message.Contains(expectedMessages[2]);

            Assert.True(containsExpectedMessage, $"Expected message: {expectedMessages[2]}. Actual message: {validationException.Message}");
        }
        // Valid DATA
        else if (SampleTestVariables.lessThan51Char.Contains(code))
        {
            Assert.Null(ex);
            Assert.Equal(bidList.Trader, input);
        }
        else
        {
            i++;
            Assert.Fail("Unexpected result {i} times");
        }
    }

    // Book
    [Theory]
    [MemberData(nameof(SampleTestVariables.StringCombinationsTest), MemberType = typeof(SampleTestVariables))]
    public void Test_Validate_Book_StringVariation_ShouldReturnExpectedResults(string? input, int code)
    {
        // Arrange
        bidList.Book = input;
        int i = 0;

        var expectedMessages = new List<string>
        {
            "Book is mandatory",
            "Book must be a string",
            "Book can't be longer than 50 characters"
        };

        // Act
        Exception? ex = Record.Exception(() => bidList.Validate());

        // Assert
        // [Required(ErrorMessage = "Book is mandatory")]
        // [DataType(DataType.Text, ErrorMessage = "Book must be a string")]
        if (SampleTestVariables.stringMandatory.Contains(code))
        {
            Assert.NotNull(ex);
            Assert.IsType<ValidationException>(ex);
            var validationException = (ValidationException)ex;

            // Check if the exception message contains any of the expected messages
            bool containsExpectedMessage = expectedMessages.Any(message => validationException.Message.Contains(message));

            Assert.True(containsExpectedMessage, $"Expected one of the following messages: " +
                $"\"Book is mandatory\",\r\n or \"Book must be a string\". Actual message: {validationException.Message}");
        }
        // [MaxLength(50, ErrorMessage = "Book can't be longer than 50 characters")]
        else if (SampleTestVariables.moreThan51Char.Contains(code))
        {
            Assert.NotNull(ex);
            Assert.IsType<ValidationException>(ex);
            var validationException = (ValidationException)ex;

            // Check if the exception message contains the length message
            bool containsExpectedMessage = validationException.Message.Contains(expectedMessages[2]);

            Assert.True(containsExpectedMessage, $"Expected message: {expectedMessages[2]}. Actual message: {validationException.Message}");
        }
        // Valid DATA
        else if (SampleTestVariables.lessThan51Char.Contains(code))
        {
            Assert.Null(ex);
            Assert.Equal(bidList.Book, input);
        }
        else
        {
            i++;
            Assert.Fail("Unexpected result {i} times");
        }
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
    [InlineData]
    public void Test_Validate_WithValidBidList_DataTypeTextVarying_ShouldNotThrowException()
    {
        // Arrange

        // Act
        Exception? ex = Record.Exception(() => bidList.Validate());

        // Assert
        Assert.Null(ex);
    }
}