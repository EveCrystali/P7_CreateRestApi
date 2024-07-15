using Dot.Net.WebApi.Domain;

namespace P7CreateRestApi.Tests;

public class RuleNameTest
{
    // --------------- VALID DATA ------------------
    // ruleName with all valid data is declarated here because it will be used in all tests
    public RuleName ruleName = new()
    {
        Id = 1,
        Name = "TestName",
        Description = "TestDescription",
        Json = "TestJson",
        Template = "TestTemplate",
        SqlStr = "TestSqlStr",
        SqlPart = "TestSqlPart"
    };

    // --------------- VALID DATA ------------------

    // ALL DATA VALID
    [Fact]
    public void Test_Validate_WithValidRuleName_ShouldNotThrowException()
    {
        // Arrange

        // Act
        Exception? ex = Record.Exception(() => ruleName.Validate());

        // Assert
        Assert.Null(ex);
    }

    // --------------- STRING PROPERTIES TESTS ------------------

    // Name - 50 characters max - Mandatory
    [Theory]
    [MemberData(nameof(SampleTestVariables.GetStringCombinationsTest), 50, MemberType = typeof(SampleTestVariables))]
    public void Test_Validate_Name_StringVariation_ShouldReturnExpectedResults(string? input, string description)
    {
        TestHelper.ValidateStringProperty(ruleName, nameof(RuleName.Name), input, description, mandatory: true);
    }

    // Description - 500 characters max - Mandatory
    [Theory]
    [MemberData(nameof(SampleTestVariables.GetStringCombinationsTest), 500, MemberType = typeof(SampleTestVariables))]
    public void Test_Validate_Description_StringVariation_ShouldReturnExpectedResults(string? input, string description)
    {
        TestHelper.ValidateStringProperty(ruleName, nameof(RuleName.Description), input, description, mandatory: true);
    }

    // Json - 5000 characters max - Mandatory
    [Theory]
    [MemberData(nameof(SampleTestVariables.GetStringCombinationsTest), 5000, MemberType = typeof(SampleTestVariables))]
    public void Test_Validate_Json_StringVariation_ShouldReturnExpectedResults(string? input, string description)
    {
        TestHelper.ValidateStringProperty(ruleName, nameof(RuleName.Json), input, description, mandatory: true);
    }

    // Template - 1000 characters max - Mandatory
    [Theory]
    [MemberData(nameof(SampleTestVariables.GetStringCombinationsTest), 1000, MemberType = typeof(SampleTestVariables))]
    public void Test_Validate_Template_StringVariation_ShouldReturnExpectedResults(string? input, string description)
    {
        TestHelper.ValidateStringProperty(ruleName, nameof(RuleName.Template), input, description, mandatory: true);
    }

    // SqlStr - 1000 characters max - Mandatory
    [Theory]
    [MemberData(nameof(SampleTestVariables.GetStringCombinationsTest), 1000, MemberType = typeof(SampleTestVariables))]
    public void Test_Validate_SqlStr_StringVariation_ShouldReturnExpectedResults(string? input, string description)
    {
        TestHelper.ValidateStringProperty(ruleName, nameof(RuleName.SqlStr), input, description, mandatory: true);
    }

    // SqlPart - 1000 characters max - Mandatory
    [Theory]
    [MemberData(nameof(SampleTestVariables.GetStringCombinationsTest), 1000, MemberType = typeof(SampleTestVariables))]
    public void Test_Validate_SqlPart_StringVariation_ShouldReturnExpectedResults(string? input, string description)
    {
        TestHelper.ValidateStringProperty(ruleName, nameof(RuleName.SqlPart), input, description, mandatory: true);
    }


    // --------------- STRING PROPERTIES TESTS ------------------
}