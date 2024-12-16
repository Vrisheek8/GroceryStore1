using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using CombinedAPI.Controllers;
using CombinedAPI.Models;
using CombinedAPI.Interfaces;
using System;
using System.Collections.Generic;

namespace CombinedAPI.Tests
{
  public class SaleControllerTests
  {
    // Mock objects
    private readonly Mock<ISaleEngine> _mockSaleEngine;
    private readonly SaleController _controller;

    public SaleControllerTests()
    {
      _mockSaleEngine = new Mock<ISaleEngine>();
      _controller = new SaleController(_mockSaleEngine.Object);
    }

    // Test for GetSaleById()
    [Fact]
    public void GetSaleById_ReturnsOkResult_WhenSaleExists()
    {
      // Arrange
      var saleId = 1;
      var expectedSale = new Sale { SaleID = saleId, startDate = DateTime.Now, endDate = DateTime.Now.AddDays(10), IsPercentage = true, DiscountAmount = 10 };
      _mockSaleEngine.Setup(s => s.GetSaleById(saleId)).Returns(expectedSale);

      // Act
      var result = _controller.GetSaleById(saleId);

      // Assert
      var okResult = Assert.IsType<OkObjectResult>(result);
      var returnedSale = Assert.IsAssignableFrom<Sale>(okResult.Value);
      Assert.Equal(expectedSale.SaleID, returnedSale.SaleID);
    }

    [Fact]
    public void GetSaleById_ReturnsNotFound_WhenSaleDoesNotExist()
    {
      // Arrange
      var saleId = 1;
      _mockSaleEngine.Setup(s => s.GetSaleById(saleId)).Returns((Sale)null);

      // Act
      var result = _controller.GetSaleById(saleId);

      // Assert
      Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public void GetSaleById_ReturnsBadRequest_WhenExceptionOccurs()
    {
      // Arrange
      var saleId = 1;
      _mockSaleEngine.Setup(s => s.GetSaleById(saleId)).Throws(new Exception("Error"));

      // Act
      var result = _controller.GetSaleById(saleId);

      // Assert
      var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
      Assert.Equal("Error", badRequestResult.Value);
    }

    // Test for GetAllSales()
    [Fact]
    public void GetAllSales_ReturnsOkResult_WhenSalesExist()
    {
      // Arrange
      var expectedSales = new List<Sale>
        {
          new Sale { SaleID = 1, startDate = DateTime.Now, endDate = DateTime.Now.AddDays(10), IsPercentage = true, DiscountAmount = 10 },
          new Sale { SaleID = 2, startDate = DateTime.Now, endDate = DateTime.Now.AddDays(5), IsPercentage = false, DiscountAmount = 5 }
        };
      _mockSaleEngine.Setup(s => s.GetAllSales()).Returns(expectedSales);

      // Act
      var result = _controller.GetAllSales();

      // Assert
      var okResult = Assert.IsType<OkObjectResult>(result);
      var returnedSales = Assert.IsAssignableFrom<List<Sale>>(okResult.Value);
      Assert.Equal(expectedSales.Count, returnedSales.Count);
    }

    [Fact]
    public void GetAllSales_ReturnsNotFound_WhenNoSalesExist()
    {
      // Arrange
      _mockSaleEngine.Setup(s => s.GetAllSales()).Returns((List<Sale>)null);

      // Act
      var result = _controller.GetAllSales();

      // Assert
      var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
      Assert.Equal("No sales going on right now!", notFoundResult.Value);
    }

    // Test for UpdateSale()
    [Fact]
    public void UpdateSale_ReturnsOkResult_WhenSaleIsUpdated()
    {
      // Arrange
      var saleUpdateRequest = new SaleUpdateRequest
      {
        SaleID = 1,
        startDate = DateTime.Now,
        endDate = DateTime.Now.AddDays(10),
        IsPercentage = true,
        DiscountAmount = 15
      };
      var sale = new Sale
      {
        SaleID = saleUpdateRequest.SaleID,
        startDate = saleUpdateRequest.startDate,
        endDate = saleUpdateRequest.endDate,
        IsPercentage = saleUpdateRequest.IsPercentage,
        DiscountAmount = saleUpdateRequest.DiscountAmount
      };
      _mockSaleEngine.Setup(s => s.UpdateSale(It.IsAny<Sale>())).Returns(true);

      // Act
      var result = _controller.UpdateSale(saleUpdateRequest);

      // Assert
      var okResult = Assert.IsType<OkObjectResult>(result);
      Assert.Equal("Sale update successfully", okResult.Value);
    }

    [Fact]
    public void UpdateSale_ReturnsBadRequest_WhenSaleUpdateFails()
    {
      // Arrange
      var saleUpdateRequest = new SaleUpdateRequest
      {
        SaleID = 1,
        startDate = DateTime.Now,
        endDate = DateTime.Now.AddDays(10),
        IsPercentage = true,
        DiscountAmount = 10
      };
      var sale = new Sale
      {
        SaleID = saleUpdateRequest.SaleID,
        startDate = saleUpdateRequest.startDate,
        endDate = saleUpdateRequest.endDate,
        IsPercentage = saleUpdateRequest.IsPercentage,
        DiscountAmount = saleUpdateRequest.DiscountAmount
      };
      _mockSaleEngine.Setup(s => s.UpdateSale(sale)).Returns(false);

      // Act
      var result = _controller.UpdateSale(saleUpdateRequest);

      // Assert
      var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
      Assert.Equal("Failed to update sale.", badRequestResult.Value);
    }

    // Test for AddSale()
    [Fact]
    public void AddSale_ReturnsOkResult_WhenSaleIsAdded()
    {
      // Arrange
      var saleUpdateRequest = new SaleUpdateRequest
      {
        SaleID = 1,
        startDate = DateTime.Now,
        endDate = DateTime.Now.AddDays(10),
        IsPercentage = true,
        DiscountAmount = 20
      };
      var sale = new Sale
      {
        SaleID = saleUpdateRequest.SaleID,
        startDate = saleUpdateRequest.startDate,
        endDate = saleUpdateRequest.endDate,
        IsPercentage = saleUpdateRequest.IsPercentage,
        DiscountAmount = saleUpdateRequest.DiscountAmount
      };
      _mockSaleEngine.Setup(s => s.AddSale(It.IsAny<Sale>())).Returns(true);

      // Act
      var result = _controller.AddSale(saleUpdateRequest);

      // Assert
      var okResult = Assert.IsType<OkObjectResult>(result);
      Assert.Equal("Sale added successfully", okResult.Value);
    }

    [Fact]
    public void AddSale_ReturnsBadRequest_WhenSaleAdditionFails()
    {
      // Arrange
      var saleUpdateRequest = new SaleUpdateRequest
      {
        SaleID = 1,
        startDate = DateTime.Now,
        endDate = DateTime.Now.AddDays(10),
        IsPercentage = true,
        DiscountAmount = 10
      };
      var sale = new Sale
      {
        SaleID = saleUpdateRequest.SaleID,
        startDate = saleUpdateRequest.startDate,
        endDate = saleUpdateRequest.endDate,
        IsPercentage = saleUpdateRequest.IsPercentage,
        DiscountAmount = saleUpdateRequest.DiscountAmount
      };
      _mockSaleEngine.Setup(s => s.AddSale(sale)).Returns(false);

      // Act
      var result = _controller.AddSale(saleUpdateRequest);

      // Assert
      var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
      Assert.Equal("Failed to add sale.", badRequestResult.Value);
    }
  }
}
