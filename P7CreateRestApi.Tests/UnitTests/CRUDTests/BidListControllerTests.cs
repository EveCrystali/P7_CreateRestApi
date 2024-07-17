
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


namespace P7CreateRestApi.Tests;

public class BidListControllerTests
{

    static BidListControllerTests()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        serviceCollection.AddDbContext<LocalDbContext>(options =>
            options.UseInMemoryDatabase("TestDatabase"));
        serviceCollection.AddScoped(typeof(IUpdateService<>), typeof(UpdateService<>));
        serviceCollection.AddHttpContextAccessor();  
        serviceCollection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); 

        var serviceProvider = serviceCollection.BuildServiceProvider();
        ServiceProviderHelper.Initialize(serviceProvider);
    }

    private readonly LocalDbContext _context;
    private readonly Mock<IUpdateService<BidList>> _mockUpdateService;
    private readonly BidListController _controller;

    public BidListControllerTests()
    {
        var options = new DbContextOptionsBuilder<LocalDbContext>()
                      .UseInMemoryDatabase("TestDatabase")
                      .Options;

        _context = new LocalDbContext(options);
        _mockUpdateService = new Mock<IUpdateService<BidList>>();

        var httpContextAccessor = new HttpContextAccessor();

        // Simulate an HTTP context
        var context = new DefaultHttpContext();
        httpContextAccessor.HttpContext = context;

        _controller = new BidListController(_context, _mockUpdateService.Object);

        // Reset the database (usefull to run tests in parallel)
        _context.BidLists.RemoveRange(_context.BidLists);
        _context.SaveChanges();
    }

    [Fact]
    public async Task PostBidList_ValidData_ShouldReturnCreatedAtAction()
    {
        // Arrange
        BidList bidList = new()
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

    [Fact]
    public async Task GetBidLists_ShouldReturnBidLists()
    {
        // Arrange
        List<BidList> bidLists = new()
        {
        new() {
            // Set valid properties
            BidListId = 2,
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
        },
        new() {
            // Set valid properties
            BidListId = 3,
            Account = "ValidAccount2",
            BidType = "ValidBidType2",
            Benchmark = "ValidBenchmark2",
            Commentary = "ValidCommentary2",
            BidSecurity = "ValidSecurity2",
            BidStatus = "ValidStatus2",
            Trader = "ValidTrader2",
            Book = "ValidBook2",
            CreationName = "ValidCreationName2",
            RevisionName = "ValidRevisionName2",
            DealName = "ValidDealName2",
            DealType = "ValidDealType2",
            SourceListId = "ValidSourceListId2",
            Side = "ValidSide2"
        }
    };
        _context.BidLists.AddRange(bidLists);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.GetBidLists();

        // Assert
        var actionResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<BidList>>(actionResult.Value);
        Assert.Equal(2, returnValue.Count);
    }

    [Fact]
    public async Task GetBidList_ExistingId_ShouldReturnBidList()
    {
        // Arrange
        BidList bidList = new()
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
        _context.BidLists.Add(bidList);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.GetBidList(1);

        // Assert
        ActionResult<BidList> actionResult = Assert.IsType<ActionResult<BidList>>(result);
        OkObjectResult okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        BidList returnValue = Assert.IsType<BidList>(okResult.Value);
        Assert.Equal(bidList, returnValue);
    }

    [Fact]
    public async Task GetBidList_NonExistingId_ShouldReturnNotFound()
    {
        // Act
        var result = await _controller.GetBidList(1);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task PutBidList_ValidUpdate_ShouldReturnNoContent()
    {
        // Arrange
        BidList bidList = new()
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
        _context.BidLists.Add(bidList);
        await _context.SaveChangesAsync();

        BidList updatedBidList = new()
        {
            // Set valid properties
            BidListId = 1,
            Account = "UpdatedAccount",
            BidType = "UpdatedType",
            Benchmark = "UpdatedBenchmark",
            Commentary = "UpdatedCommentary",
            BidSecurity = "UpdatedSecurity",
            BidStatus = "UpdatedStatus",
            Trader = "UpdatedTrader",
            Book = "UpdatedBook",
            CreationName = "UpdatedCreationName",
            RevisionName = "UpdatedRevisionName",
            DealName = "UpdatedDealName",
            DealType = "UpdatedDealType",
            SourceListId = "UpdatedSourceListId",
            Side = "UpdatedSide"
        };

        _mockUpdateService.Setup(s => s.UpdateEntity(It.IsAny<int>(), It.IsAny<BidList>(), It.IsAny<Func<BidList, bool>>(), It.IsAny<Func<BidList, int>>()))
        .ReturnsAsync(new NoContentResult());

        // Act
        var result = await _controller.PutBidList(1, updatedBidList);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task PutBidList_InvalidUpdate_ShouldReturnNotFound()
    {
        // Arrange
        BidList updatedBidList = new()
        {
            BidListId = 99,
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

        _mockUpdateService
            .Setup(s => s.UpdateEntity(It.IsAny<int>(), It.IsAny<BidList>(), It.IsAny<Func<BidList, bool>>(), It.IsAny<Func<BidList, int>>()))
            .ReturnsAsync(new NotFoundObjectResult("Not Found"));

        // Act
        var result = await _controller.PutBidList(99, updatedBidList);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task DeleteBidList_ExistingId_ShouldReturnNoContent()
    {
        // Arrange
        BidList bidList = new()
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
        _context.BidLists.Add(bidList);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.DeleteBidList(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteBidList_NonExistingId_ShouldReturnNotFound()
    {
        // Act
        var result = await _controller.DeleteBidList(99);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }
}
