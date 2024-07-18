using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Dot.Net.WebApi.Domain;

namespace P7CreateRestApi.Tests;

public class TradeTest
{
    // --------------- VALID DATA ------------------
    // ruleName with all valid data is declarated here because it will be used in all tests
    public Trade trade = new()
    {
        TradeId = 1,
        Account = "TestAccount",
        AccountType = "TestAccountType",
        BuyQuantity = 1.0,
        SellQuantity = 1.0,
        BuyPrice = 1.0,
        SellPrice = 1.0,
        TradeDate = DateTime.Now,
        TradeSecurity = "TestTradeSecurity",
        TradeStatus = "TestTradeStatus",
        Trader = "TestTrader",
        Benchmark = "TestBenchmark",
        Book = "TestBook",
        CreationName = "TestCreationName",
        CreationDate = DateTime.Now,
        RevisionName = "TestRevisionName",
        RevisionDate = DateTime.Now,
        DealName = "TestDealName",
        DealType = "TestDealType",
        SourceListId = "TestSourceListId",
        Side = "TestSide"
    };

    // --------------- VALID DATA ------------------

    // ALL DATA VALID
    [Fact]
    public void Test_Validate_WithValidRuleName_ShouldNotThrowException()
    {
        // Arrange

        // Act
        Exception? ex = Record.Exception(() => trade.Validate());

        // Assert
        Assert.Null(ex);
    }

    // --------------- STRING PROPERTIES TESTS ------------------

    // Account - 50 characters max - Mandatory
    [Theory]
    [MemberData(nameof(SampleTestVariables.GetStringCombinationsTest), 50, MemberType = typeof(SampleTestVariables))]
    public void Test_Validate_Account_StringVariation_ShouldReturnExpectedResults(string? input, string description)
    {
        TestHelper.ValidateStringProperty(trade, nameof(Trade.Account), input, description, mandatory: true);
    }

    // AccountType - 50 characters max - Mandatory
    [Theory]
    [MemberData(nameof(SampleTestVariables.GetStringCombinationsTest), 50, MemberType = typeof(SampleTestVariables))]
    public void Test_Validate_AccountType_StringVariation_ShouldReturnExpectedResults(string? input, string description)
    {
        TestHelper.ValidateStringProperty(trade, nameof(Trade.AccountType), input, description, mandatory: true);
    }

    // TradeSecurity - 50 characters max - Mandatory
    [Theory]
    [MemberData(nameof(SampleTestVariables.GetStringCombinationsTest), 50, MemberType = typeof(SampleTestVariables))]
    public void Test_Validate_TradeSecurity_StringVariation_ShouldReturnExpectedResults(string? input, string description)
    {
        TestHelper.ValidateStringProperty(trade, nameof(Trade.TradeSecurity), input, description, mandatory: true);
    }

    // TradeStatus - 50 characters max - Mandatory
    [Theory]
    [MemberData(nameof(SampleTestVariables.GetStringCombinationsTest), 50, MemberType = typeof(SampleTestVariables))]
    public void Test_Validate_TradeStatus_StringVariation_ShouldReturnExpectedResults(string? input, string description)
    {
        TestHelper.ValidateStringProperty(trade, nameof(Trade.TradeStatus), input, description, mandatory: true);
    }

    // Trader - 50 characters max - Mandatory
    [Theory]
    [MemberData(nameof(SampleTestVariables.GetStringCombinationsTest), 50, MemberType = typeof(SampleTestVariables))]
    public void Test_Validate_Trader_StringVariation_ShouldReturnExpectedResults(string? input, string description)
    {
        TestHelper.ValidateStringProperty(trade, nameof(Trade.Trader), input, description, mandatory: true);
    }

    // Benchmark - 100 characters max - Mandatory
    [Theory]
    [MemberData(nameof(SampleTestVariables.GetStringCombinationsTest), 100, MemberType = typeof(SampleTestVariables))]
    public void Test_Validate_Benchmark_StringVariation_ShouldReturnExpectedResults(string? input, string description)
    {
        TestHelper.ValidateStringProperty(trade, nameof(Trade.Benchmark), input, description, mandatory: true);
    }

    // Book - 50 characters max - Mandatory
    [Theory]
    [MemberData(nameof(SampleTestVariables.GetStringCombinationsTest), 50, MemberType = typeof(SampleTestVariables))]
    public void Test_Validate_Book_StringVariation_ShouldReturnExpectedResults(string? input, string description)
    {
        TestHelper.ValidateStringProperty(trade, nameof(Trade.Book), input, description, mandatory: true);
    }

    // CreationName - 50 characters max - Mandatory
    [Theory]
    [MemberData(nameof(SampleTestVariables.GetStringCombinationsTest), 50, MemberType = typeof(SampleTestVariables))]
    public void Test_Validate_CreationName_StringVariation_ShouldReturnExpectedResults(string? input, string description)
    {
        TestHelper.ValidateStringProperty(trade, nameof(Trade.CreationName), input, description, mandatory: true);
    }

    // RevisionName - 50 characters max - Mandatory
    [Theory]
    [MemberData(nameof(SampleTestVariables.GetStringCombinationsTest), 50, MemberType = typeof(SampleTestVariables))]
    public void Test_Validate_RevisionName_StringVariation_ShouldReturnExpectedResults(string? input, string description)
    {
        TestHelper.ValidateStringProperty(trade, nameof(Trade.RevisionName), input, description, mandatory: true);
    }

    // DealName - 50 characters max - Mandatory
    [Theory]
    [MemberData(nameof(SampleTestVariables.GetStringCombinationsTest), 50, MemberType = typeof(SampleTestVariables))]
    public void Test_Validate_DealName_StringVariation_ShouldReturnExpectedResults(string? input, string description)
    {
        TestHelper.ValidateStringProperty(trade, nameof(Trade.DealName), input, description, mandatory: true);
    }

    // DealType - 50 characters max - Mandatory
    [Theory]
    [MemberData(nameof(SampleTestVariables.GetStringCombinationsTest), 50, MemberType = typeof(SampleTestVariables))]
    public void Test_Validate_DealType_StringVariation_ShouldReturnExpectedResults(string? input, string description)
    {
        TestHelper.ValidateStringProperty(trade, nameof(Trade.DealType), input, description, mandatory: true);
    }

    // SourceListId - 25 characters max - Mandatory
    [Theory]
    [MemberData(nameof(SampleTestVariables.GetStringCombinationsTest), 25, MemberType = typeof(SampleTestVariables))]
    public void Test_Validate_SourceListId_StringVariation_ShouldReturnExpectedResults(string? input, string description)
    {
        TestHelper.ValidateStringProperty(trade, nameof(Trade.SourceListId), input, description, mandatory: true);
    }

    // Side - 50 characters max - Mandatory
    [Theory]
    [MemberData(nameof(SampleTestVariables.GetStringCombinationsTest), 50, MemberType = typeof(SampleTestVariables))]
    public void Test_Validate_Side_StringVariation_ShouldReturnExpectedResults(string? input, string description)
    {
        TestHelper.ValidateStringProperty(trade, nameof(Trade.Side), input, description, mandatory: true);
    }

    // --------------- STRING PROPERTIES TESTS ------------------

    // -------------- DATETIME PROPERTIES TESTS ------------------

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("InvalidDateString")]
    [InlineData("08/07/2024 15:06")]
    [InlineData("2024-07-08T15:06:02.998Z")]
    public void Test_Validate_WithValidtrade_TradeDate_ShouldReturnAnException(string? input)
    {
        trade.TradeDate = null;
        if (!string.IsNullOrEmpty(input) && DateTime.TryParseExact(input, "yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out DateTime tempDate))
        {
            trade.TradeDate = tempDate;
            Exception? ex = Record.Exception(() => trade.Validate());
            Assert.Null(ex);
            Assert.Equal(trade.TradeDate, DateTime.ParseExact(input, "yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture));
        }
        else
        {
            if (!string.IsNullOrEmpty(input))
            {
                try
                {
                    trade.TradeDate = DateTime.Parse(input);
                    trade.Validate();
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
                // As TradeDate is nullable DateTime? if input is null : no exception should be thrown
                Exception? ex = Record.Exception(() => trade.Validate());
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
    public void Test_Validate_WithValidtrade_CreationDate_ShouldReturnAnException(string? input)
    {
        trade.CreationDate = null;
        if (!string.IsNullOrEmpty(input) && DateTime.TryParseExact(input, "yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out DateTime tempDate))
        {
            trade.CreationDate = tempDate;
            Exception? ex = Record.Exception(() => trade.Validate());
            Assert.Null(ex);
            Assert.Equal(trade.CreationDate, DateTime.ParseExact(input, "yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture));
        }
        else
        {
            if (!string.IsNullOrEmpty(input))
            {
                try
                {
                    trade.CreationDate = DateTime.Parse(input);
                    trade.Validate();
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
                Exception? ex = Record.Exception(() => trade.Validate());
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
    public void Test_Validate_WithValidtrade_RevisionDate_ShouldReturnAnException(string? input)
    {
        trade.RevisionDate = null;
        if (!string.IsNullOrEmpty(input) && DateTime.TryParseExact(input, "yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out DateTime tempDate))
        {
            trade.RevisionDate = tempDate;
            Exception? ex = Record.Exception(() => trade.Validate());
            Assert.Null(ex);
            Assert.Equal(trade.RevisionDate, DateTime.ParseExact(input, "yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture));
        }
        else
        {
            if (!string.IsNullOrEmpty(input))
            {
                try
                {
                    trade.RevisionDate = DateTime.Parse(input);
                    trade.Validate();
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
                Exception? ex = Record.Exception(() => trade.Validate());
                Assert.Null(ex);
            }
        }
    }

    // -------------- DATETIME PROPERTIES TESTS ------------------

    // --------------- NUMERIC PROPERTIES TESTS ------------------

    // BuyQuantity
    [Theory]
    [InlineData(double.MinValue + 1)]
    [InlineData(0)]
    [InlineData(double.MaxValue - 1)]
    public void Test_Validate_WithValidTrade_BuyQuantity_ShouldNotThrowException(double input)
    {
        // Arrange
        trade.BuyQuantity = input;

        // Act
        Exception? ex = Record.Exception(() => trade.Validate());

        // Assert
        Assert.Null(ex);
        Assert.Equal(trade.BuyQuantity, input);
    }

    [Fact]
    public void Test_Validate_WithValidtrade_BuyQuantityIsNull_ShouldNotThrowException()
    {
        // Arrange
        trade.BuyQuantity = null;

        // Act
        Exception? ex = Record.Exception(() => trade.Validate());

        // Assert
        Assert.Null(ex);
        Assert.Null(trade.BuyQuantity);
    }

    // SellQuantity
    [Theory]
    [InlineData(double.MinValue + 1)]
    [InlineData(0)]
    [InlineData(double.MaxValue - 1)]
    public void Test_Validate_WithValidTrade_SellQuantity_ShouldNotThrowException(double input)
    {
        // Arrange
        trade.SellQuantity = input;

        // Act
        Exception? ex = Record.Exception(() => trade.Validate());

        // Assert
        Assert.Null(ex);
        Assert.Equal(trade.SellQuantity, input);
    }

    [Fact]
    public void Test_Validate_WithValidtrade_SellQuantityIsNull_ShouldNotThrowException()
    {
        // Arrange
        trade.SellQuantity = null;

        // Act
        Exception? ex = Record.Exception(() => trade.Validate());

        // Assert
        Assert.Null(ex);
        Assert.Null(trade.SellQuantity);
    }

    // BuyPrice
    [Theory]
    [InlineData(double.MinValue + 1)]
    [InlineData(0)]
    [InlineData(double.MaxValue - 1)]
    public void Test_Validate_WithValidTrade_BuyPrice_ShouldNotThrowException(double input)
    {
        // Arrange
        trade.BuyPrice = input;

        // Act
        Exception? ex = Record.Exception(() => trade.Validate());

        // Assert
        Assert.Null(ex);
        Assert.Equal(trade.BuyPrice, input);
    }

    [Fact]
    public void Test_Validate_WithValidtrade_BuyPriceIsNull_ShouldNotThrowException()
    {
        // Arrange
        trade.BuyPrice = null;

        // Act
        Exception? ex = Record.Exception(() => trade.Validate());

        // Assert
        Assert.Null(ex);
        Assert.Null(trade.BuyPrice);
    }

    // SellPrice
    [Theory]
    [InlineData(double.MinValue + 1)]
    [InlineData(0)]
    [InlineData(double.MaxValue - 1)]
    public void Test_Validate_WithValidTrade_SellPrice_ShouldNotThrowException(double input)
    {
        // Arrange
        trade.SellPrice = input;

        // Act
        Exception? ex = Record.Exception(() => trade.Validate());

        // Assert
        Assert.Null(ex);
        Assert.Equal(trade.SellPrice, input);
    }

    [Fact]
    public void Test_Validate_WithValidtrade_SellPriceIsNull_ShouldNotThrowException()
    {
        // Arrange
        trade.SellPrice = null;

        // Act
        Exception? ex = Record.Exception(() => trade.Validate());

        // Assert
        Assert.Null(ex);
        Assert.Null(trade.SellPrice);
    }

    // --------------- NUMERIC PROPERTIES TESTS ------------------
}