
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Domain;
using Moq;
using P7CreateRestApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Dot.Net.WebApi.Services;
using Microsoft.EntityFrameworkCore;
using Dot.Net.WebApi;
using Microsoft.Extensions.DependencyInjection;
using Dot.Net.WebApi.Helpers;
using Microsoft.AspNetCore.Http;
using P7CreateRestApi.Tests;


namespace P7CreateRestApi.Tests;

public class RuleNameControllerTests : TestBase<RuleName>
{
    private readonly RuleNameController _controller;

    public RuleNameControllerTests()
    {
        _controller = new RuleNameController(_context, _mockUpdateService.Object);
    }

    private RuleName CreateValidRuleName(int i)
    {
        return new RuleName
        {
            // Set valid properties
           Id = i,
           Name = "ValidRuleName",
           Description = "ValidDescription",
           Json = "ValidJson",
           Template = "ValidTemplate",
           SqlStr = "ValidSqlStr",
           SqlPart = "ValidSqlPart"
        };
    }

    [Fact]
    public async Task PostRuleName_ValidData_ShouldReturnCreatedAtAction()
    {
        // Arrange
        RuleName ruleName = CreateValidRuleName(1);

        // Act
        var result = await _controller.PostRuleName(ruleName);

        // Assert
        var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnValue = Assert.IsType<RuleName>(actionResult.Value);
        Assert.Equal(ruleName, returnValue);
    }

    [Fact]
    public async Task GetRuleNames_ShouldReturnRuleNames()
    {
        // Arrange
        List<RuleName> ruleNames = new()
        {
         CreateValidRuleName(1),
         CreateValidRuleName(2)
        };

        _context.RuleNames.AddRange(ruleNames);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.GetRuleNames();

        // Assert
        var actionResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<RuleName>>(actionResult.Value);
        Assert.Equal(2, returnValue.Count);
    }

    [Fact]
    public async Task GetRuleName_ExistingId_ShouldReturnRuleName()
    {
        // Arrange
        RuleName ruleName = CreateValidRuleName(1);
        _context.RuleNames.Add(ruleName);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.GetRuleName(1);

        // Assert
        ActionResult<RuleName> actionResult = Assert.IsType<ActionResult<RuleName>>(result);
        OkObjectResult okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        RuleName returnValue = Assert.IsType<RuleName>(okResult.Value);
        Assert.Equal(ruleName, returnValue);
    }

    [Fact]
    public async Task GetRuleName_NonExistingId_ShouldReturnNotFound()
    {
        // Act
        var result = await _controller.GetRuleName(1);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task PutRuleName_ValidUpdate_ShouldReturnNoContent()
    {
        // Arrange
        RuleName ruleName = CreateValidRuleName(1);
        _context.RuleNames.Add(ruleName);
        await _context.SaveChangesAsync();

        RuleName updatedRuleName = new()
        {
            // Set valid properties
            Id = 1,
            Name = "UpdatedRuleName",
            Description = "UpdatedDescription",
            Json = "UpdatedJson",
            Template = "UpdatedTemplate",
            SqlStr = "UpdatedSqlStr",
            SqlPart = "UpdatedSqlPart"
        };

        _mockUpdateService.Setup(s => s.UpdateEntity(It.IsAny<int>(), It.IsAny<RuleName>(), It.IsAny<Func<RuleName, bool>>(), It.IsAny<Func<RuleName, int>>()))
        .ReturnsAsync(new NoContentResult());

        // Act
        var result = await _controller.PutRuleName(1, updatedRuleName);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task PutRuleName_InvalidUpdate_ShouldReturnNotFound()
    {
        // Arrange
        RuleName updatedRuleName = CreateValidRuleName(99);

        _mockUpdateService
            .Setup(s => s.UpdateEntity(It.IsAny<int>(), It.IsAny<RuleName>(), It.IsAny<Func<RuleName, bool>>(), It.IsAny<Func<RuleName, int>>()))
            .ReturnsAsync(new NotFoundObjectResult("Not Found"));

        // Act
        var result = await _controller.PutRuleName(99, updatedRuleName);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task DeleteRuleName_ExistingId_ShouldReturnNoContent()
    {
        // Arrange
        RuleName ruleName = CreateValidRuleName(1);
        _context.RuleNames.Add(ruleName);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.DeleteRuleName(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteRuleName_NonExistingId_ShouldReturnNotFound()
    {
        // Act
        var result = await _controller.DeleteRuleName(99);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }
}
