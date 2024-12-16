using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using CombinedAPI.Controllers;
using CombinedAPI.Models;
using CombinedAPI.Interfaces;

namespace CombinedAPI.Tests
{
  public class ProductControllerTests
  {
    // Mock objects
    private readonly Mock<IProductManager> _mockProductManager;
    private readonly ProductController _controller;

    public ProductControllerTests()
    {
      _mockProductManager = new Mock<IProductManager>();
      _controller = new ProductController(_mockProductManager.Object);
    }

    // Test for GetProductById()
    [Fact]
    public void GetProductById_ReturnsOkResult_WhenProductExists()
    {
      // Arrange
      var productID = 1;
      var expectedProduct = new Product { ProductID = productID, Name = "Product1" };
      _mockProductManager.Setup(s => s.GetProductById(productID)).Returns(expectedProduct);

      // Act
      var result = _controller.GetProductById(productID);

      // Assert
      var okResult = Assert.IsType<OkObjectResult>(result);
      var returnedProduct = Assert.IsAssignableFrom<Product>(okResult.Value);
      Assert.Equal(expectedProduct.ProductID, returnedProduct.ProductID);
    }

    [Fact]
    public void GetProductById_ReturnsNotFound_WhenProductDoesNotExist()
    {
      // Arrange
      var productId = 1;
      _mockProductManager.Setup(s => s.GetProductById(productId)).Returns((Product)null);

      // Act
      var result = _controller.GetProductById(productId);

      // Assert
      Assert.IsType<NotFoundObjectResult>(result);
    }

    // Test for GetProductByName()
    [Fact]
    public void GetProductByName_ReturnsOkResult_WhenProductExists()
    {
      // Arrange
      var productName = "Product1";
      var expectedProduct = new Product { ProductID = 1, Name = productName };
      _mockProductManager.Setup(s => s.GetProductByName(productName)).Returns(expectedProduct);

      // Act
      var result = _controller.GetProductByName(productName);

      // Assert
      var okResult = Assert.IsType<OkObjectResult>(result);
      var returnedProduct = Assert.IsAssignableFrom<Product>(okResult.Value);
      Assert.Equal(expectedProduct.Name, returnedProduct.Name);
    }

    [Fact]
    public void GetProductByName_ReturnsNotFound_WhenProductDoesNotExist()
    {
      // Arrange
      var productName = "NonExistentProduct";
      _mockProductManager.Setup(s => s.GetProductByName(productName)).Returns((Product)null);

      // Act
      var result = _controller.GetProductByName(productName);

      // Assert
      Assert.IsType<NotFoundObjectResult>(result);
    }

    // Test for GetProductByCategory()
    [Fact]
    public void GetProductByCategory_ReturnsOkResult_WhenProductsExistInCategory()
    {
      // Arrange
      var categoryId = 1;
      var expectedProducts = new List<Product>
        {
          new Product { ProductID = 1, Name = "Product1", CategoryID = categoryId },
          new Product { ProductID = 2, Name = "Product2", CategoryID = categoryId }
        };
      _mockProductManager.Setup(s => s.GetProductByCategory(categoryId)).Returns(expectedProducts);

      // Act
      var result = _controller.GetProductByCategory(categoryId);

      // Assert
      var okResult = Assert.IsType<OkObjectResult>(result);
      var returnedProducts = Assert.IsAssignableFrom<List<Product>>(okResult.Value);
      Assert.Equal(expectedProducts.Count, returnedProducts.Count);
    }

    [Fact]
    public void GetProductByCategory_ReturnsNotFound_WhenNoProductsExistInCategory()
    {
      // Arrange
      var categoryId = 1;
      _mockProductManager.Setup(s => s.GetProductByCategory(categoryId)).Returns(new List<Product>());

      // Act
      var result = _controller.GetProductByCategory(categoryId);

      // Assert
      Assert.IsType<NotFoundObjectResult>(result);
    }

    // Test for GetAllProducts()
    [Fact]
    public void GetAllProducts_ReturnsOkResult_WhenProductsExist()
    {
      // Arrange
      var expectedProducts = new List<Product>
        {
          new Product { ProductID = 1, Name = "Product1" },
          new Product { ProductID = 2, Name = "Product2" }
        };
      _mockProductManager.Setup(s => s.GetAllProducts()).Returns(expectedProducts);

      // Act
      var result = _controller.GetAllProducts();

      // Assert
      var okResult = Assert.IsType<OkObjectResult>(result);
      var returnedProducts = Assert.IsAssignableFrom<List<Product>>(okResult.Value);
      Assert.Equal(expectedProducts.Count, returnedProducts.Count);
    }

    [Fact]
    public void GetAllProducts_ReturnsNotFound_WhenNoProductsExist()
    {
      // Arrange
      _mockProductManager.Setup(s => s.GetAllProducts()).Returns(new List<Product>());

      // Act
      var result = _controller.GetAllProducts();

      // Assert
      Assert.IsType<NotFoundObjectResult>(result);
    }

    // Test for UpdateProductStock()
    [Fact]
    public void UpdateProductStock_ReturnsOkResult_WhenStockIsUpdated()
    {
      // Arrange
      var request = new UpdateStockRequest { productId = 1, stock = 10 };
      _mockProductManager.Setup(s => s.UpdateProductStock(request.productId, request.stock)).Returns(true);

      // Act
      var result = _controller.UpdateProductStock(request);

      // Assert
      var okResult = Assert.IsType<OkObjectResult>(result);
      Assert.Equal("Stock updated successfully.", okResult.Value);
    }

    [Fact]
    public void UpdateProductStock_ReturnsBadRequest_WhenStockUpdateFails()
    {
      // Arrange
      var request = new UpdateStockRequest { productId = 1, stock = -5 };
      _mockProductManager.Setup(s => s.UpdateProductStock(request.productId, request.stock)).Returns(false);

      // Act
      var result = _controller.UpdateProductStock(request);

      // Assert
      var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
      Assert.Equal("Failed to update stock.", badRequestResult.Value);
    }
  }
}
