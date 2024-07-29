using System.ComponentModel.DataAnnotations;
using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using P7CreateRestApi.Controllers;

namespace P7CreateRestApi.Tests;

public class CurvePointControllerTests : TestBase<CurvePoint>
{
    private readonly CurvePointController _controller;

    public CurvePointControllerTests()
    {
        _controller = new CurvePointController(_context, _mockUpdateService.Object);
    }

    private CurvePoint CreateValidCurvePoint(int i)
    {
        return new CurvePoint
        {
            // Set valid properties
            Id = i,
        };
    }

    [Fact]
    public async Task PostCurvePoint_ValidData_ShouldReturnCreatedAtAction()
    {
        // Arrange
        CurvePoint curvePoint = CreateValidCurvePoint(1);

        // Act
        ActionResult<CurvePoint> result = await _controller.PostCurvePoint(curvePoint);

        // Assert
        CreatedAtActionResult actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        CurvePoint returnValue = Assert.IsType<CurvePoint>(actionResult.Value);
        Assert.Equal(curvePoint, returnValue);
    }

    [Fact]
    public async Task GetCurvePoints_ShouldReturnCurvePoints()
    {
        // Arrange
        List<CurvePoint> curvePoints =
        [
         CreateValidCurvePoint(1),
         CreateValidCurvePoint(2)
        ];

        _context.CurvePoints.AddRange(curvePoints);
        await _context.SaveChangesAsync();

        // Act
        ActionResult result = await _controller.GetCurvePoints();

        // Assert
        OkObjectResult actionResult = Assert.IsType<OkObjectResult>(result);
        List<CurvePoint> returnValue = Assert.IsType<List<CurvePoint>>(actionResult.Value);
        Assert.Equal(2, returnValue.Count);
    }

    [Fact]
    public async Task GetCurvePoint_ExistingId_ShouldReturnCurvePoint()
    {
        // Arrange
        CurvePoint curvePoint = CreateValidCurvePoint(1);
        _context.CurvePoints.Add(curvePoint);
        await _context.SaveChangesAsync();

        // Act
        ActionResult<CurvePoint> result = await _controller.GetCurvePoint(1);

        // Assert
        ActionResult<CurvePoint> actionResult = Assert.IsType<ActionResult<CurvePoint>>(result);
        OkObjectResult okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        CurvePoint returnValue = Assert.IsType<CurvePoint>(okResult.Value);
        Assert.Equal(curvePoint, returnValue);
    }

    [Fact]
    public async Task GetCurvePoint_NonExistingId_ShouldReturnNotFound()
    {
        // Act
        ActionResult<CurvePoint> result = await _controller.GetCurvePoint(1);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task PutCurvePoint_ValidUpdate_ShouldReturnNoContent()
    {
        // Arrange
        CurvePoint curvePoint = CreateValidCurvePoint(1);
        _context.CurvePoints.Add(curvePoint);
        await _context.SaveChangesAsync();

        CurvePoint updatedCurvePoint = new()
        {
            // Set valid properties
            Id = 1,
            Term = 2.0
        };

        _mockUpdateService.Setup(s => s.UpdateEntity(It.IsAny<int>(), It.IsAny<CurvePoint>(), It.IsAny<Func<CurvePoint, bool>>(), It.IsAny<Func<CurvePoint, int>>()))
        .ReturnsAsync(new NoContentResult());

        // Act
        IActionResult result = await _controller.PutCurvePoint(1, updatedCurvePoint);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task PutCurvePoint_InvalidUpdate_ShouldReturnNotFound()
    {
        // Arrange
        CurvePoint updatedCurvePoint = CreateValidCurvePoint(99);

        _mockUpdateService
            .Setup(s => s.UpdateEntity(It.IsAny<int>(), It.IsAny<CurvePoint>(), It.IsAny<Func<CurvePoint, bool>>(), It.IsAny<Func<CurvePoint, int>>()))
            .ReturnsAsync(new NotFoundObjectResult("Not Found"));

        // Act
        IActionResult result = await _controller.PutCurvePoint(99, updatedCurvePoint);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task DeleteCurvePoint_ExistingId_ShouldReturnNoContent()
    {
        // Arrange
        CurvePoint curvePoint = CreateValidCurvePoint(1);
        _context.CurvePoints.Add(curvePoint);
        await _context.SaveChangesAsync();

        // Act
        IActionResult result = await _controller.DeleteCurvePoint(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteCurvePoint_NonExistingId_ShouldReturnNotFound()
    {
        // Act
        IActionResult result = await _controller.DeleteCurvePoint(99);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }
}