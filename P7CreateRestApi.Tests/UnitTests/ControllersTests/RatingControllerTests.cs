using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Mvc;
using Moq;
using P7CreateRestApi.Controllers;

namespace P7CreateRestApi.Tests;

public class RatingControllerTests : TestBase<Rating>
{
    private readonly RatingController _controller;

    public RatingControllerTests()
    {
        _controller = new RatingController(_context, _mockUpdateService.Object);
    }

    private Rating CreateValidRating(int i)
    {
        return new Rating
        {
            // Set valid properties
            Id = i,
            MoodysRating = "ValidM",
            SandPRating = "ValidSP",
            FitchRating = "ValidF",
        };
    }

    [Fact]
    public async Task PostRating_ValidData_ShouldReturnCreatedAtAction()
    {
        // Arrange
        Rating rating = CreateValidRating(1);

        // Act
        ActionResult<Rating> result = await _controller.PostRating(rating);

        // Assert
        CreatedAtActionResult actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Rating returnValue = Assert.IsType<Rating>(actionResult.Value);
        Assert.Equal(rating, returnValue);
    }

    [Fact]
    public async Task PostRating_InValidData_ShouldReturnBadRequest()
    {
        // Arrange
        Rating rating = CreateValidRating(1);
        // Set an invalid properties
        rating.MoodysRating = null;

        // Act
        ActionResult<Rating> result = await _controller.PostRating(rating);
        BadRequestObjectResult? badRequestResult = result.Result as BadRequestObjectResult;

        // Assert
        Assert.NotNull(badRequestResult);
        Assert.IsType<string>(badRequestResult.Value);
    }

    [Fact]
    public async Task GetRatings_ShouldReturnRatings()
    {
        // Arrange
        List<Rating> ratings = new()
        {
         CreateValidRating(1),
         CreateValidRating(2)
        };

        _context.Ratings.AddRange(ratings);
        await _context.SaveChangesAsync();

        // Act
        ActionResult result = await _controller.GetRatings();

        // Assert
        OkObjectResult actionResult = Assert.IsType<OkObjectResult>(result);
        List<Rating> returnValue = Assert.IsType<List<Rating>>(actionResult.Value);
        Assert.Equal(2, returnValue.Count);
    }

    [Fact]
    public async Task GetRating_ExistingId_ShouldReturnRating()
    {
        // Arrange
        Rating rating = CreateValidRating(1);
        _context.Ratings.Add(rating);
        await _context.SaveChangesAsync();

        // Act
        ActionResult<Rating> result = await _controller.GetRating(1);

        // Assert
        ActionResult<Rating> actionResult = Assert.IsType<ActionResult<Rating>>(result);
        OkObjectResult okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        Rating returnValue = Assert.IsType<Rating>(okResult.Value);
        Assert.Equal(rating, returnValue);
    }

    [Fact]
    public async Task GetRating_NonExistingId_ShouldReturnNotFound()
    {
        // Act
        ActionResult<Rating> result = await _controller.GetRating(1);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task PutRating_ValidUpdate_ShouldReturnNoContent()
    {
        // Arrange
        Rating rating = CreateValidRating(1);
        _context.Ratings.Add(rating);
        await _context.SaveChangesAsync();

        Rating updatedRating = new()
        {
            // Set valid properties
            Id = 1,
            MoodysRating = "UpdatedMoodysRating",
            SandPRating = "UpdatedSandPRating",
            FitchRating = "UpdatedFitchRating",
            OrderNumber = 2
        };

        _mockUpdateService.Setup(s => s.UpdateEntity(It.IsAny<int>(), It.IsAny<Rating>(), It.IsAny<Func<Rating, bool>>(), It.IsAny<Func<Rating, int>>()))
        .ReturnsAsync(new NoContentResult());

        // Act
        IActionResult result = await _controller.PutRating(1, updatedRating);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task PutRating_InvalidUpdate_ShouldReturnNotFound()
    {
        // Arrange
        Rating updatedRating = CreateValidRating(99);

        _mockUpdateService
            .Setup(s => s.UpdateEntity(It.IsAny<int>(), It.IsAny<Rating>(), It.IsAny<Func<Rating, bool>>(), It.IsAny<Func<Rating, int>>()))
            .ReturnsAsync(new NotFoundObjectResult("Not Found"));

        // Act
        IActionResult result = await _controller.PutRating(99, updatedRating);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task DeleteRating_ExistingId_ShouldReturnNoContent()
    {
        // Arrange
        Rating rating = CreateValidRating(1);
        _context.Ratings.Add(rating);
        await _context.SaveChangesAsync();

        // Act
        IActionResult result = await _controller.DeleteRating(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteRating_NonExistingId_ShouldReturnNotFound()
    {
        // Act
        IActionResult result = await _controller.DeleteRating(99);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }
}