
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


namespace P7CreateRestApi.Tests;


public class BidListControllerTests
{
    private readonly LocalDbContext _context;
    private readonly Mock<UpdateService> _mockUpdateService;
    private readonly BidListController _controller;


    private readonly UpdateService updateService;

    public BidListControllerTests()
    {

         var options = new DbContextOptionsBuilder<LocalDbContext>()
                        .UseInMemoryDatabase(databaseName: "TestDatabase")
                        .Options;

        _context = new LocalDbContext(options);


        _mockUpdateService = new Mock<UpdateService>();

        _controller = new BidListController(_context, _mockUpdateService.Object);
    }

    [Fact]
    public async Task PostBidList_ValidData_ShouldReturnCreatedAtAction()
    {
        // Arrange
        var bidList = new BidList
        {
            // Set valid properties
            BidListId = 1,
            Account = "ValidAccount",
            BidType = "ValidBidType",
            Benchmark = "ValidBenchmark",
            Commentary = "ValidCommentary",
            BidSecurity = "ValidSecurity",
            BidStatus = "ValidStatus",
            Trader = "ValidTrader",
            Book = "ValidBook",
            CreationName = "ValidCreationName",
            RevisionName = "ValidRevisionName",
            DealName = "ValidDealName",
            DealType = "ValidDealType",
            SourceListId = "ValidSourceListId",
            Side = "ValidSide"
        };

        // Act
        var result = await _controller.PostBidList(bidList);

        // Assert
        var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnValue = Assert.IsType<BidList>(actionResult.Value);
        Assert.Equal(bidList, returnValue);
    }
}
