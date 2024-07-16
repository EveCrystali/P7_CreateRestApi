using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Dot.Net.WebApi.Domain;

namespace P7CreateRestApi.Tests;

public class CurvePointTests
{
    // --------------- VALID DATA ------------------
    // CurvePoint with all valid data is declarated here because it will be used in all tests
    public CurvePoint curvePoint = new()
    {
        Id = 1,
        CurveId = 1,
        AsOfDate = DateTime.Now,
        Term = 1,
        CurvePointValue = 1,
        CreationDate = DateTime.Now
    };

    // --------------- VALID DATA ------------------

    // ALL DATA VALID
    [Fact]
    public void Test_Validate_WithValidCurvePoint_ShouldNotThrowException()
    {
        // Arrange

        // Act
        Exception? ex = Record.Exception(() => curvePoint.Validate());

        // Assert
        Assert.Null(ex);
    }

    // --------------- NUMERIC PROPERTIES TESTS ------------------
    [Theory]
    [InlineData(0)]
    [InlineData(int.MinValue + 1)]
    [InlineData(int.MaxValue - 1)]
    public void Test_Validate_WithValidCurvePoint_Id_ShouldNotThrowException(int input)
    {
        // Arrange
        curvePoint.Id = input;

        // Act
        Exception? ex = Record.Exception(() => curvePoint.Validate());

        // Assert
        Assert.Null(ex);
        Assert.Equal(curvePoint.Id, input);
    }

    [Theory]
    [InlineData(byte.MinValue)]
    [InlineData(1)]
    [InlineData(byte.MaxValue)]
    public void Test_Validate_WithValidCurvePoint_CurveId_ShouldNotThrowException(byte input)
    {
        // Arrange
        curvePoint.CurveId = input;

        // Act
        Exception? ex = Record.Exception(() => curvePoint.Validate());

        // Assert
        Assert.Null(ex);
        Assert.Equal(curvePoint.CurveId, input);
    }

    [Fact]
    public void Test_Validate_WithNullCurvePoint_CurveId_ShouldNotThrowException()
    {
        // Arrange
        curvePoint.CurveId = null;

        // Act
        Exception? ex = Record.Exception(() => curvePoint.Validate());

        // Assert
        Assert.Null(ex);
        Assert.Null(curvePoint.CurveId);
    }

    [Theory]
    [InlineData(double.MinValue + 1)]
    [InlineData(0)]
    [InlineData(double.MaxValue - 1)]
    public void Test_Validate_WithValidCurvePoint_Term_ShouldNotThrowException(double input)
    {
        // Arrange
        curvePoint.Term = input;

        // Act
        Exception? ex = Record.Exception(() => curvePoint.Validate());

        // Assert
        Assert.Null(ex);
        Assert.Equal(curvePoint.Term, input);
    }

    [Fact]
    public void Test_Validate_WithValidCurvePoint_TermIsNull_ShouldNotThrowException()
    {
        // Arrange
        curvePoint.Term = null;

        // Act
        Exception? ex = Record.Exception(() => curvePoint.Validate());

        // Assert
        Assert.Null(ex);
        Assert.Null(curvePoint.Term);
    }

    [Theory]
    [InlineData(double.MinValue + 1)]
    [InlineData(0)]
    [InlineData(double.MaxValue - 1)]
    public void Test_Validate_WithValidCurvePoint_CurvePointValue_ShouldNotThrowException(double input)
    {
        // Arrange
        curvePoint.CurvePointValue = input;

        // Act
        Exception? ex = Record.Exception(() => curvePoint.Validate());

        // Assert
        Assert.Null(ex);
        Assert.Equal(curvePoint.CurvePointValue, input);
    }

    [Fact]
    public void Test_Validate_WithValidCurvePoint_CurvePointValueIsNull_ShouldNotThrowException()
    {
        // Arrange
        curvePoint.CurvePointValue = null;

        // Act
        Exception? ex = Record.Exception(() => curvePoint.Validate());

        // Assert
        Assert.Null(ex);
        Assert.Null(curvePoint.CurvePointValue);
    }

    // --------------- NUMERIC PROPERTIES TESTS ------------------

    // -------------- DATETIME PROPERTIES TESTS ------------------

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("InvalidDateString")]
    [InlineData("08/07/2024 15:06")]
    [InlineData("2024-07-08T15:06:02.998Z")]
    public void Test_Validate_WithValidcurvePoint_AsOfDate_ShouldReturnAnException(string? input)
    {
        curvePoint.AsOfDate = null;
        if (!string.IsNullOrEmpty(input) && DateTime.TryParseExact(input, "yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out DateTime tempDate))
        {
            curvePoint.AsOfDate = tempDate;
            Exception? ex = Record.Exception(() => curvePoint.Validate());
            Assert.Null(ex);
            Assert.Equal(curvePoint.AsOfDate, DateTime.ParseExact(input, "yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture));
        }
        else
        {
            if (!string.IsNullOrEmpty(input))
            {
                try
                {
                    curvePoint.AsOfDate = DateTime.Parse(input);
                    curvePoint.Validate();
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
                // As AsOfDate is nullable DateTime? if input is null : no exception should be thrown
                Exception? ex = Record.Exception(() => curvePoint.Validate());
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
    public void Test_Validate_WithValidCurvePoint_CreationDate_ShouldReturnAnException(string? input)
    {
        curvePoint.CreationDate = null;
        if (!string.IsNullOrEmpty(input) && DateTime.TryParseExact(input, "yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out DateTime tempDate))
        {
            curvePoint.CreationDate = tempDate;
            Exception? ex = Record.Exception(() => curvePoint.Validate());
            Assert.Null(ex);
            Assert.Equal(curvePoint.CreationDate, DateTime.ParseExact(input, "yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture));
        }
        else
        {
            if (!string.IsNullOrEmpty(input))
            {
                try
                {
                    curvePoint.CreationDate = DateTime.Parse(input);
                    curvePoint.Validate();
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
                Exception? ex = Record.Exception(() => curvePoint.Validate());
                Assert.Null(ex);
            }
        }
    }

    // -------------- DATETIME PROPERTIES TESTS ------------------
}