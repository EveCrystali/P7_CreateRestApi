
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

public class TradeControllerTests : TestBase<Trade>
{
    private readonly TradeController _controller;

    public TradeControllerTests()
    {
        _controller = new TradeController(_context, _mockUpdateService.Object);
    }

    private Trade CreateValidTrade(int i)
    {
        return new Trade
        {
            // Set valid properties
            TradeId = i,
            Account = "ValidAccount",
            AccountType = "ValidAccountType",
            TradeSecurity = "ValidTradeSecurity",
            TradeStatus = "ValidTradeStatus",
            Trader = "ValidTrader",
            Benchmark = "ValidBenchmark",
            Book = "ValidBook",
            CreationName = "ValidCreationName",
            RevisionName = "ValidRevisionName",
            DealName = "ValidDealName",
            DealType = "ValidDealType",
            SourceListId = "ValidSourceListId",
            Side = "ValidSide"
        };
    }

    [Fact]
    public async Task PostTrade_ValidData_ShouldReturnCreatedAtAction()
    {
        // Arrange
        Trade trade = CreateValidTrade(1);

        // Act
        var result = await _controller.PostTrade(trade);

        // Assert
        var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnValue = Assert.IsType<Trade>(actionResult.Value);
        Assert.Equal(trade, returnValue);
    }

    [Fact]
    public async Task GetTrades_ShouldReturnTrades()
    {
        // Arrange
        List<Trade> trades = new()
        {
         CreateValidTrade(1),
         CreateValidTrade(2)
        };

        _context.Trades.AddRange(trades);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.GetTrades();

        // Assert
        var actionResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<Trade>>(actionResult.Value);
        Assert.Equal(2, returnValue.Count);
    }

    [Fact]
    public async Task GetTrade_ExistingId_ShouldReturnTrade()
    {
        // Arrange
        Trade trade = CreateValidTrade(1);
        _context.Trades.Add(trade);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.GetTrade(1);

        // Assert
        ActionResult<Trade> actionResult = Assert.IsType<ActionResult<Trade>>(result);
        OkObjectResult okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        Trade returnValue = Assert.IsType<Trade>(okResult.Value);
        Assert.Equal(trade, returnValue);
    }

    [Fact]
    public async Task GetTrade_NonExistingId_ShouldReturnNotFound()
    {
        // Act
        var result = await _controller.GetTrade(1);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task PutTrade_ValidUpdate_ShouldReturnNoContent()
    {
        // Arrange
        Trade trade = CreateValidTrade(1);
        _context.Trades.Add(trade);
        await _context.SaveChangesAsync();

        Trade updatedTrade = new()
        {
            // Set valid properties
            TradeId = 1,
            Account = "UpdatedValidAccount",
            AccountType = "UpdatedValidAccountType",
            TradeSecurity = "UpdatedValidTradeSecurity",
            TradeStatus = "UpdatedValidTradeStatus",
            Trader = "UpdatedValidTrader",
            Benchmark = "UpdatedValidBenchmark",
            Book = "UpdatedValidBook",
            CreationName = "UpdatedValidCreationName",
            RevisionName = "UpdatedValidRevisionName",
            DealName = "UpdatedValidDealName",
            DealType = "UpdatedValidDealType",
            SourceListId = "UpdatedValidSourceListId",
            Side = "UpdatedValidSide"
        };

        _mockUpdateService.Setup(s => s.UpdateEntity(It.IsAny<int>(), It.IsAny<Trade>(), It.IsAny<Func<Trade, bool>>(), It.IsAny<Func<Trade, int>>()))
        .ReturnsAsync(new NoContentResult());

        // Act
        var result = await _controller.PutTrade(1, updatedTrade);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task PutTrade_InvalidUpdate_ShouldReturnNotFound()
    {
        // Arrange
        Trade updatedTrade = CreateValidTrade(99);

        _mockUpdateService
            .Setup(s => s.UpdateEntity(It.IsAny<int>(), It.IsAny<Trade>(), It.IsAny<Func<Trade, bool>>(), It.IsAny<Func<Trade, int>>()))
            .ReturnsAsync(new NotFoundObjectResult("Not Found"));

        // Act
        var result = await _controller.PutTrade(99, updatedTrade);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task DeleteTrade_ExistingId_ShouldReturnNoContent()
    {
        // Arrange
        Trade trade = CreateValidTrade(1);
        _context.Trades.Add(trade);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.DeleteTrade(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteTrade_NonExistingId_ShouldReturnNotFound()
    {
        // Act
        var result = await _controller.DeleteTrade(99);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }
}
