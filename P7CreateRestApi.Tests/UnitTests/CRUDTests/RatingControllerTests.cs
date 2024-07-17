﻿
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
        var result = await _controller.PostRating(rating);

        // Assert
        var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnValue = Assert.IsType<Rating>(actionResult.Value);
        Assert.Equal(rating, returnValue);
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
        var result = await _controller.GetRatings();

        // Assert
        var actionResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<Rating>>(actionResult.Value);
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
        var result = await _controller.GetRating(1);

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
        var result = await _controller.GetRating(1);

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
        var result = await _controller.PutRating(1, updatedRating);

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
        var result = await _controller.PutRating(99, updatedRating);

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
        var result = await _controller.DeleteRating(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteRating_NonExistingId_ShouldReturnNotFound()
    {
        // Act
        var result = await _controller.DeleteRating(99);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }
}
