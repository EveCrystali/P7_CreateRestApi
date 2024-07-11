using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Mvc.TagHelpers;

namespace P7CreateRestApi.Tests;

public class BidListTests
{
    // --------------- VALID DATA ------------------
    // BidList with all valid data is declarated here because it will be used in all tests
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
    // --------------- VALID DATA ------------------

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

    // --------------- STRING PROPERTIES TESTS ------------------

    // Now testing sample string combinations on the different property

    // Account - 50 characters max - Mandatory
    [Theory]
    [MemberData(nameof(SampleTestVariables.StringCombinationsTest), MemberType = typeof(SampleTestVariables))]
    public void Test_Validate_Account_StringVariation_ShouldReturnExpectedResults(string? input, int code)
    {
        TestHelper.ValidateStringProperty(bidList, nameof(BidList.Account), input, code, instance => instance.Validate);
    }

    // BidType - 50 characters max - Mandatory
    [Theory]
    [MemberData(nameof(SampleTestVariables.StringCombinationsTest), MemberType = typeof(SampleTestVariables))]
    public void Test_Validate_BidType_StringVariation_ShouldReturnExpectedResults(string? input, int code)
    {
        TestHelper.ValidateStringProperty(bidList, nameof(BidList.BidType), input, code, instance => instance.Validate);
    }

    // Benchmark - 100 characters max - Mandatory
    [Theory]
    [MemberData(nameof(SampleTestVariables.StringCombinationsTest), MemberType = typeof(SampleTestVariables))]
    public void Test_Validate_Benchmark_StringVariation_ShouldReturnExpectedResults(string? input, int code)
    {
        TestHelper.ValidateStringProperty(bidList, nameof(BidList.Benchmark), input, code, instance => instance.Validate);
    }

    // Commentary - 500 characters max - Mandatory
    [Theory]
    [MemberData(nameof(SampleTestVariables.StringCombinationsTest), MemberType = typeof(SampleTestVariables))]
    public void Test_Validate_Commentary_StringVariation_ShouldReturnExpectedResults(string? input, int code)
    {
        TestHelper.ValidateStringProperty(bidList, nameof(BidList.Commentary), input, code, instance => instance.Validate);
    }

    // BidSecurity - 50 characters max - Mandatory
    [Theory]
    [MemberData(nameof(SampleTestVariables.StringCombinationsTest), MemberType = typeof(SampleTestVariables))]
    public void Test_Validate_BidSecurity_StringVariation_ShouldReturnExpectedResults(string? input, int code)
    {
        TestHelper.ValidateStringProperty(bidList, nameof(BidList.BidSecurity), input, code, instance => instance.Validate);
    }

    // BidStatus - 50 characters max - Mandatory
    [Theory]
    [MemberData(nameof(SampleTestVariables.StringCombinationsTest), MemberType = typeof(SampleTestVariables))]
    public void Test_Validate_BidStatus_StringVariation_ShouldReturnExpectedResults(string? input, int code)
    {
        TestHelper.ValidateStringProperty(bidList, nameof(BidList.BidStatus), input, code, instance => instance.Validate);
    }

    // Trader - 50 characters max - Mandatory
    [Theory]
    [MemberData(nameof(SampleTestVariables.StringCombinationsTest), MemberType = typeof(SampleTestVariables))]
    public void Test_Validate_Trader_StringVariation_ShouldReturnExpectedResults(string? input, int code)
    {
        TestHelper.ValidateStringProperty(bidList, nameof(BidList.Trader), input, code, instance => instance.Validate);
    }

    // Book - 50 characters max - Mandatory
    [Theory]
    [MemberData(nameof(SampleTestVariables.StringCombinationsTest), MemberType = typeof(SampleTestVariables))]
    public void Test_Validate_Book_StringVariation_ShouldReturnExpectedResults(string? input, int code)
    {
        TestHelper.ValidateStringProperty(bidList, nameof(BidList.Book), input, code, instance => instance.Validate);
    }

    // CreationName - 50 characters max - Mandatory
    [Theory]
    [MemberData(nameof(SampleTestVariables.StringCombinationsTest), MemberType = typeof(SampleTestVariables))]
    public void Test_Validate_CreationName_StringVariation_ShouldReturnExpectedResults(string? input, int code)
    {
        TestHelper.ValidateStringProperty(bidList, nameof(BidList.CreationName), input, code, instance => instance.Validate);
    }

    // RevisionName - 50 characters max - Mandatory
    [Theory]
    [MemberData(nameof(SampleTestVariables.StringCombinationsTest), MemberType = typeof(SampleTestVariables))]
    public void Test_Validate_RevisionName_StringVariation_ShouldReturnExpectedResults(string? input, int code)
    {
        TestHelper.ValidateStringProperty(bidList, nameof(BidList.RevisionName), input, code, instance => instance.Validate);
    }

    // DealName - 50 characters max - Mandatory
    [Theory]
    [MemberData(nameof(SampleTestVariables.StringCombinationsTest), MemberType = typeof(SampleTestVariables))]
    public void Test_Validate_DealName_StringVariation_ShouldReturnExpectedResults(string? input, int code)
    {
        TestHelper.ValidateStringProperty(bidList, nameof(BidList.DealName), input, code, instance => instance.Validate);
    }

    // SourceListId - 25 characters max - Mandatory
    [Theory]
    [MemberData(nameof(SampleTestVariables.StringCombinationsTest), MemberType = typeof(SampleTestVariables))]
    public void Test_Validate_SourceListId_StringVariation_ShouldReturnExpectedResults(string? input, int code)
    {
        TestHelper.ValidateStringProperty(bidList, nameof(BidList.SourceListId), input, code, instance => instance.Validate);
    }

    // Side - 50 characters max - Mandatory
    [Theory]
    [MemberData(nameof(SampleTestVariables.StringCombinationsTest), MemberType = typeof(SampleTestVariables))]
    public void Test_Validate_Side_StringVariation_ShouldReturnExpectedResults(string? input, int code)
    {
        TestHelper.ValidateStringProperty(bidList, nameof(BidList.Side), input, code, instance => instance.Validate);
    }


    // --------------- STRING PROPERTIES TESTS ------------------

    // --------------- NUMERIC PROPERTIES TESTS ------------------
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
    [InlineData(double.MinValue + 1)]
    [InlineData(0)]
    [InlineData(double.MaxValue - 1)]
    public void Test_Validate_WithValidBidList_GoodBidQuantity_ShouldNotThrowException(double input)
    {
        // Arrange
        bidList.BidQuantity = input;

        // Act
        Exception? ex = Record.Exception(() => bidList.Validate());

        // Assert
        Assert.Null(ex);
        Assert.Equal(bidList.BidQuantity, input);
    }

    [Theory]
    [InlineData(double.MinValue + 1)]
    [InlineData(0)]
    [InlineData(double.MaxValue - 1)]
    public void Test_Validate_WithValidBidList_GoodAskQuantity_ShouldNotThrowException(double input)
    {
        // Arrange
        bidList.AskQuantity = input;

        // Act
        Exception? ex = Record.Exception(() => bidList.Validate());

        // Assert
        Assert.Null(ex);
        Assert.Equal(bidList.AskQuantity, input);
    }

    [Theory]
    [InlineData(double.MinValue + 1)]
    [InlineData(0)]
    [InlineData(double.MaxValue - 1)]
    public void Test_Validate_WithValidBidList_GoodBid_ShouldNotThrowException(double input)
    {
        // Arrange
        bidList.Bid = input;

        // Act
        Exception? ex = Record.Exception(() => bidList.Validate());

        // Assert
        Assert.Null(ex);
        Assert.Equal(bidList.Bid, input);
    }

    [Theory]
    [InlineData(double.MinValue + 1)]
    [InlineData(0)]
    [InlineData(double.MaxValue - 1)]
    public void Test_Validate_WithValidBidList_GoodAsk_ShouldNotThrowException(double input)
    {
        // Arrange
        bidList.Ask = input;

        // Act
        Exception? ex = Record.Exception(() => bidList.Validate());

        // Assert
        Assert.Null(ex);
        Assert.Equal(bidList.Ask, input);
    }

    // --------------- NUMERIC PROPERTIES TESTS ------------------

    // -------------- DATETIME PROPERTIES TESTS ------------------

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("InvalidDateString")]
    [InlineData("08/07/2024 15:06")]
    [InlineData("2024-07-08T15:06:02.998Z")]
    public void Test_Validate_WithValidBidList_CreationDate_ShouldReturnAnException(string? input)
    {
        bidList.CreationDate = null;
        if (!string.IsNullOrEmpty(input) && DateTime.TryParseExact(input, "yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out DateTime tempDate))
        {
            bidList.CreationDate = tempDate;
            Exception? ex = Record.Exception(() => bidList.Validate());
            Assert.Null(ex);
            Assert.Equal(bidList.CreationDate, DateTime.ParseExact(input, "yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture));
        }
        else
        {
            if (!string.IsNullOrEmpty(input))
            {
                try
                {
                    bidList.CreationDate = DateTime.Parse(input);
                    bidList.Validate();
                }
                catch (Exception ex)
                {
                    Assert.NotNull(ex);
                    if (ex.GetType() != typeof(ValidationException) && ex.GetType() != typeof(System.FormatException))
                    {
                        Assert.Fail("Unexpected exception type: " + ex.GetType() + "\n with message: " + ex.Message + "\n The exception should be a ValidationException or a FormatException");
                    }
                }
            }
            else
            {
                // As CreationDate is nullable DateTime? if input is null : no exception should be thrown
                Exception? ex = Record.Exception(() => bidList.Validate());
                Assert.Null(ex);
            }
        }
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("InvalidDateString")]
    [InlineData("08/07/2024 15:06")]
    [InlineData("2024-07-08T15:06:02.998Z")]
    public void Test_Validate_WithValidBidList_BidListDate_ShouldReturnAnException(string? input)
    {
        bidList.BidListDate = null;
        if (!string.IsNullOrEmpty(input) && DateTime.TryParseExact(input, "yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out DateTime tempDate))
        {
            bidList.BidListDate = tempDate;
            Exception? ex = Record.Exception(() => bidList.Validate());
            Assert.Null(ex);
            Assert.Equal(bidList.BidListDate, DateTime.ParseExact(input, "yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture));
        }
        else
        {
            if (!string.IsNullOrEmpty(input))
            {
                try
                {
                    bidList.BidListDate = DateTime.Parse(input);
                    bidList.Validate();
                }
                catch (Exception ex)
                {
                    Assert.NotNull(ex);
                    if (ex.GetType() != typeof(ValidationException) && ex.GetType() != typeof(System.FormatException))
                    {
                        Assert.Fail("Unexpected exception type: " + ex.GetType() + "\n with message: " + ex.Message + "\n The exception should be a ValidationException or a FormatException");
                    }
                }
            }
            else
            {
                // As BidListDate is nullable DateTime? if input is null : no exception should be thrown
                Exception? ex = Record.Exception(() => bidList.Validate());
                Assert.Null(ex);
            }
        }
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("InvalidDateString")]
    [InlineData("08/07/2024 15:06")]
    [InlineData("2024-07-08T15:06:02.998Z")]
    public void Test_Validate_WithValidBidList_RevisionDate_ShouldReturnAnException(string? input)
    {
        bidList.RevisionDate = null;
        if (!string.IsNullOrEmpty(input) && DateTime.TryParseExact(input, "yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out DateTime tempDate))
        {
            bidList.RevisionDate = tempDate;
            Exception? ex = Record.Exception(() => bidList.Validate());
            Assert.Null(ex);
            Assert.Equal(bidList.RevisionDate, DateTime.ParseExact(input, "yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture));
        }
        else
        {
            if (!string.IsNullOrEmpty(input))
            {
                try
                {
                    bidList.RevisionDate = DateTime.Parse(input);
                    bidList.Validate();
                }
                catch (Exception ex)
                {
                    Assert.NotNull(ex);
                    if (ex.GetType() != typeof(ValidationException) && ex.GetType() != typeof(System.FormatException))
                    {
                        Assert.Fail("Unexpected exception type: " + ex.GetType() + "\n with message: " + ex.Message + "\n The exception should be a ValidationException or a FormatException");
                    }
                }
            }
            else
            {
                // As RevisionDate is nullable DateTime? if input is null : no exception should be thrown
                Exception? ex = Record.Exception(() => bidList.Validate());
                Assert.Null(ex);
            }
        }
    }

    // -------------- DATETIME PROPERTIES TESTS ------------------
}