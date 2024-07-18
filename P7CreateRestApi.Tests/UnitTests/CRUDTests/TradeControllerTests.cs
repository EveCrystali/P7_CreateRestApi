using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Mvc;
using Moq;
using P7CreateRestApi.Controllers;

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
        ActionResult<Trade> result = await _controller.PostTrade(trade);

        // Assert
        CreatedAtActionResult actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Trade returnValue = Assert.IsType<Trade>(actionResult.Value);
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
        ActionResult result = await _controller.GetTrades();

        // Assert
        OkObjectResult actionResult = Assert.IsType<OkObjectResult>(result);
        List<Trade> returnValue = Assert.IsType<List<Trade>>(actionResult.Value);
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
        ActionResult<Trade> result = await _controller.GetTrade(1);

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
        ActionResult<Trade> result = await _controller.GetTrade(1);

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
        IActionResult result = await _controller.PutTrade(1, updatedTrade);

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
        IActionResult result = await _controller.PutTrade(99, updatedTrade);

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
        IActionResult result = await _controller.DeleteTrade(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteTrade_NonExistingId_ShouldReturnNotFound()
    {
        // Act
        IActionResult result = await _controller.DeleteTrade(99);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }
}