using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace P7CreateRestApi.Tests;

public class UpdateServiceTests
{
    private readonly LocalDbContext _context;
    private readonly UpdateService<BidList> _updateService;

    public UpdateServiceTests()
    {
        var options = new DbContextOptionsBuilder<LocalDbContext>()
             // To avoid conflit of primary keys when testing with the same database, we create a new unique in-memory database
             .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new LocalDbContext(options);
        _updateService = new UpdateService<BidList>(_context);
    }

    private static BidList CreateValidBidList(int i)
    {
        return new BidList
        {
            // Set valid properties
            BidListId = i,
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
    }

    [Fact]
    public async Task UpdateEntity_IdMismatch_ShouldReturnBadRequest()
    {
        // Arrange
        BidList bidList = CreateValidBidList(1);
        Func<BidList, int> getIdFunc = b => b.BidListId;

        // Act
        IActionResult result = await _updateService.UpdateEntity(2, bidList, b => true, getIdFunc);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task UpdateEntity_EntityNotExists_ShouldReturnNotFound()
    {
        // Arrange
        BidList bidList = CreateValidBidList(1);
        Func<BidList, int> getIdFunc = b => b.BidListId;

        // Act
        IActionResult result = await _updateService.UpdateEntity(1, bidList, b => false, getIdFunc);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task UpdateEntity_ValidEntity_ShouldReturnNoContent()
    {
        // Arrange
        BidList bidList = CreateValidBidList(1);
        _context.BidLists.Add(bidList);
        await _context.SaveChangesAsync();

        BidList updatedBidList = bidList;
        updatedBidList.Account = "UpdatedAccount";
        Func<BidList, int> getIdFunc = b => b.BidListId;

        // Act
        IActionResult result = await _updateService.UpdateEntity(1, updatedBidList, b => _context.BidLists.Any(e => e.BidListId == b.BidListId), getIdFunc);

        // Assert
        Assert.IsType<NoContentResult>(result);

        BidList? updatedBidListDb = await _context.BidLists.FindAsync(1);
        Assert.Equal("UpdatedAccount", updatedBidListDb.Account);
    }

    [Fact]
    public async Task UpdateEntity_InvalidEntity_ShouldReturnBadRequest()
    {
        // Arrange
        BidList bidList = CreateValidBidList(1);
        _context.BidLists.Add(bidList);
        await _context.SaveChangesAsync();

        BidList invalidBidList = bidList;
        invalidBidList.Account = null;
        Func<BidList, int> getIdFunc = b => b.BidListId;

        // Act
        IActionResult result = await _updateService.UpdateEntity(1, invalidBidList, b => true, getIdFunc);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task UpdateEntity_PartialUpdate_ShouldNotAffectOtherFields()
    {
        // Arrange
        BidList bidList = CreateValidBidList(1);
        _context.BidLists.Add(bidList);
        await _context.SaveChangesAsync();

        BidList updatedBidList = bidList;
        updatedBidList.Account = "UpdatedAccount";
        Func<BidList, int> getIdFunc = b => b.BidListId;

        // Act
        IActionResult result = await _updateService.UpdateEntity(1, updatedBidList, b => _context.BidLists.Any(e => e.BidListId == b.BidListId), getIdFunc);

        // Assert
        Assert.IsType<NoContentResult>(result);

        BidList? updatedBidListDb = await _context.BidLists.FindAsync(1);
        Assert.Equal("UpdatedAccount", updatedBidListDb.Account);
        // Ensure other fields are not changed (BidType for example)
        Assert.Equal("ValidBidType", updatedBidListDb.BidType);
    }

    /// <summary>
    /// This test method verifies that the UpdateEntity method handles concurrent updates correctly.
    /// It creates two separate context instances, each with their own in-memory database, and updates the same entity concurrently.
    /// The test then asserts that the entity is correctly updated in the database and that the correct account value is selected.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous completion of the test.</returns>
    [Fact]
    public async Task UpdateEntity_ConcurrentUpdates_ShouldHandleCorrectly()
    {
        // Arrange
        // Generate a unique database name for each test run
        var databaseName = Guid.NewGuid().ToString();
        // Create options for the in-memory database
        var options = new DbContextOptionsBuilder<LocalDbContext>()
            .UseInMemoryDatabase(databaseName)
            .Options;

        // Seed the database before trying to fetch data to simulate concurrent updates 
        // Ensure the data is correctly initialized and available for each context instance
        using (var context = new LocalDbContext(options))
        {
            var bidList = CreateValidBidList(1);
            context.BidLists.Add(bidList);
            await context.SaveChangesAsync();
        }

        // Create two separate context instances with the same options
        using var context1 = new LocalDbContext(options);
        using var context2 = new LocalDbContext(options);

        // Fetch the bid list from the context instances
        var updatedBidList1 = context1.BidLists.AsNoTracking().First(b => b.BidListId == 1);
        updatedBidList1.Account = "UpdatedAccount1";

        var updatedBidList2 = context2.BidLists.AsNoTracking().First(b => b.BidListId == 1);
        updatedBidList2.Account = "UpdatedAccount2";

        // Create update service instances for each context instance
        var updateService1 = new UpdateService<BidList>(context1);
        var updateService2 = new UpdateService<BidList>(context2);
        Func<BidList, int> getIdFunc = b => b.BidListId;

        // Act
        // Update the bid list in both context instances concurrently
        var result1 = await updateService1.UpdateEntity(1, updatedBidList1, b => context1.BidLists.Any(e => e.BidListId == b.BidListId), getIdFunc);
        var result2 = await updateService2.UpdateEntity(1, updatedBidList2, b => context2.BidLists.Any(e => e.BidListId == b.BidListId), getIdFunc);

        // Assert
        // Verify that the update was successful in both context instances
        Assert.IsType<NoContentResult>(result1);
        Assert.IsType<NoContentResult>(result2);

        // Create a separate context instance to fetch the updated bid list
        using var finalContext = new LocalDbContext(options);
        var updatedBidListDb = await finalContext.BidLists.FindAsync(1);
        // Verify that the account value is one of the expected values
        Assert.True(updatedBidListDb.Account == "UpdatedAccount1" || updatedBidListDb.Account == "UpdatedAccount2");
    }


    /// <summary>
    /// This test method verifies that the UpdateEntity method returns a ConflictResult
    /// when a DbUpdateConcurrencyException is thrown during the update operation.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous completion of the test.</returns>
    [Fact]
    public async Task UpdateEntity_DbUpdateConcurrencyException_ShouldReturnConflict()
    {
        // Arrange
        // Create in-memory database options with a unique name
        var options = new DbContextOptionsBuilder<LocalDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        // Create a new context using the in-memory database
        using var context = new TestDbContext(options);

        // Create an instance of the UpdateService with the context
        var updateService = new UpdateService<BidList>(context);

        // Create a valid bid list and add it to the context
        var bidList = CreateValidBidList(1);
        context.BidLists.Add(bidList);
        await context.SaveChangesAsync();

        // Update the account property of the bid list
        bidList.Account = "UpdatedAccount";

        // Set the flag to throw a concurrency exception
        context.ThrowConcurrencyException = true;

        // Act
        // Update the bid list using the UpdateEntity method
        var result = await updateService.UpdateEntity(1, bidList,
            b => context.BidLists.Any(e => e.BidListId == b.BidListId),
            b => b.BidListId);

        // Assert
        // Verify that the result is of type ConflictResult
        Assert.IsType<ConflictResult>(result);
    }
}