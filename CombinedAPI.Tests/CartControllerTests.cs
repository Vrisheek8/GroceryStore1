using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using CombinedAPI.Controllers;
using CombinedAPI.Models;
using CombinedAPI.Interfaces;

public class CartControllerTests
{
  private readonly Mock<ICartManager> _mockCartManager;
  private readonly CartController _controller;

  public CartControllerTests()
  {
    _mockCartManager = new Mock<ICartManager>();
    _controller = new CartController(_mockCartManager.Object);
  }

  [Fact]
  public void GetUserCart_ReturnsOkResult_WhenCartExists()
  {
    // Arrange
    int cartId = 1;
    var cart = new Cart { cartId = cartId };
    _mockCartManager.Setup(m => m.getUserCart(cartId)).Returns(cart);

    // Act
    var result = _controller.getUserCart(cartId);

    // Assert
    var okResult = Assert.IsType<OkObjectResult>(result);
    Assert.Equal(cart, okResult.Value);
  }

  [Fact]
  public void GetUserCart_ReturnsNotFound_WhenCartDoesNotExist()
  {
    // Arrange
    int cartId = 1;
    _mockCartManager.Setup(m => m.getUserCart(cartId)).Returns((Cart)null);

    // Act
    var result = _controller.getUserCart(cartId);

    // Assert
    var okResult = Assert.IsType<OkObjectResult>(result);
    Assert.Null(okResult.Value);
  }

  [Fact]
  public void AddToCart_ReturnsOkResult_WhenProductIsAddedSuccessfully()
  {
    // Arrange
    int cartId = 1;
    var product = new Product { ProductID = 1, Name = "Test Product" };
    int amount = 2;
    _mockCartManager.Setup(m => m.addToCart(cartId, product, amount)).Returns(true);

    // Act
    var result = _controller.addToCart(cartId, product, amount);

    // Assert
    var okResult = Assert.IsType<OkObjectResult>(result);
    Assert.True((bool)okResult.Value);
  }

  [Fact]
  public void AddToCart_ReturnsBadRequest_WhenProductCannotBeAdded()
  {
    // Arrange
    int cartId = 1;
    var product = new Product { ProductID = 1, Name = "Test Product" };
    int amount = 2;
    _mockCartManager.Setup(m => m.addToCart(cartId, product, amount)).Returns(false);

    // Act
    var result = _controller.addToCart(cartId, product, amount);

    // Assert
    var okResult = Assert.IsType<OkObjectResult>(result);
    Assert.False((bool)okResult.Value);
  }

  [Fact]
  public void RemoveFromCart_ReturnsOkResult_WhenProductIsRemovedSuccessfully()
  {
    // Arrange
    int cartId = 1;
    var product = new Product { ProductID = 1, Name = "Test Product" };
    _mockCartManager.Setup(m => m.removeFromCart(cartId, product)).Returns(true);

    // Act
    var result = _controller.removeFromCart(cartId, product);

    // Assert
    var okResult = Assert.IsType<OkObjectResult>(result);
    Assert.True((bool)okResult.Value);
  }

  [Fact]
  public void RemoveFromCart_ReturnsBadRequest_WhenProductCannotBeRemoved()
  {
    // Arrange
    int cartId = 1;
    var product = new Product { ProductID = 1, Name = "Test Product" };
    _mockCartManager.Setup(m => m.removeFromCart(cartId, product)).Returns(false);

    // Act
    var result = _controller.removeFromCart(cartId, product);

    // Assert
    var okResult = Assert.IsType<OkObjectResult>(result);
    Assert.False((bool)okResult.Value);
  }

  [Fact]
  public void UpdateAmount_ReturnsOkResult_WhenAmountIsUpdatedSuccessfully()
  {
    // Arrange
    int cartId = 1;
    var product = new Product { ProductID = 1, Name = "Test Product" };
    int amount = 2;
    _mockCartManager.Setup(m => m.updateAmount(cartId, product, amount)).Returns(true);

    // Act
    var result = _controller.updateAmount(cartId, product, amount);

    // Assert
    var okResult = Assert.IsType<OkObjectResult>(result);
    Assert.True((bool)okResult.Value);
  }

  [Fact]
  public void UpdateAmount_ReturnsBadRequest_WhenAmountCannotBeUpdated()
  {
    // Arrange
    int cartId = 1;
    var product = new Product { ProductID = 1, Name = "Test Product" };
    int amount = 2;
    _mockCartManager.Setup(m => m.updateAmount(cartId, product, amount)).Returns(false);

    // Act
    var result = _controller.updateAmount(cartId, product, amount);

    // Assert
    var okResult = Assert.IsType<OkObjectResult>(result);
    Assert.False((bool)okResult.Value);
  }

  [Fact]
  public void InitiateCart_ReturnsOkResult_WhenCartIsInitiatedSuccessfully()
  {
    // Arrange
    var cart = new Cart { cartId = 1 };
    _mockCartManager.Setup(m => m.initiateCart(cart)).Returns(true);

    // Act
    var result = _controller.initiateCart(cart);

    // Assert
    var okResult = Assert.IsType<OkObjectResult>(result);
    Assert.True((bool)okResult.Value);
  }

  [Fact]
  public void InitiateCart_ReturnsBadRequest_WhenCartCannotBeInitiated()
  {
    // Arrange
    var cart = new Cart { cartId = 1 };
    _mockCartManager.Setup(m => m.initiateCart(cart)).Returns(false);

    // Act
    var result = _controller.initiateCart(cart);

    // Assert
    var okResult = Assert.IsType<OkObjectResult>(result);
    Assert.False((bool)okResult.Value);
  }

  [Fact]
  public void ClearCart_ReturnsOkResult_WhenCartIsClearedSuccessfully()
  {
    // Arrange
    int cartId = 1;
    _mockCartManager.Setup(m => m.clearCart(cartId)).Returns(true);

    // Act
    var result = _controller.clearCart(cartId);

    // Assert
    var okResult = Assert.IsType<OkObjectResult>(result);
    Assert.True((bool)okResult.Value);
  }

  [Fact]
  public void ClearCart_ReturnsBadRequest_WhenCartCannotBeCleared()
  {
    // Arrange
    int cartId = 1;
    _mockCartManager.Setup(m => m.clearCart(cartId)).Returns(false);

    // Act
    var result = _controller.clearCart(cartId);

    // Assert
    var okResult = Assert.IsType<OkObjectResult>(result);
    Assert.False((bool)okResult.Value);
  }
}
