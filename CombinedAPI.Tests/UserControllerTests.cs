using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using CombinedAPI.Controllers;
using CombinedAPI.Models;
using CombinedAPI.Interfaces;
using CombinedAPI.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace CombinedAPI.Tests
{
  public class UserControllerTests
  {
    // mock objects
    private readonly Mock<IUserAccessor> _mockUserAccessor;
    private readonly Mock<IConfiguration> _mockUserConfiguration;
    private readonly UserController _controller;

    public UserControllerTests()
    {
      _mockUserAccessor = new Mock<IUserAccessor>();
      _mockUserConfiguration = new Mock<IConfiguration>();
      _controller = new UserController(_mockUserAccessor.Object, _mockUserConfiguration.Object);
    }

    // Test for GetAllUsers()
    [Fact]
    public void GetAllUsers_ReturnsOkResult_WhenUsersExist()
    {
      // Arrange
      var expectedUsers = new List<User>
        {
          new User { userID = 1, username = "User1" },
          new User { userID = 2, username = "User2" }
        };
      _mockUserAccessor.Setup(s => s.GetAllUsers()).Returns(expectedUsers);

      // Act
      var result = _controller.GetAllUsers();

      // Assert
      var okResult = Assert.IsType<OkObjectResult>(result);
      var returnedUsers = Assert.IsAssignableFrom<List<User>>(okResult.Value);
      Assert.Equal(expectedUsers.Count, returnedUsers.Count);
    }

    [Fact]
    public void GetAllUsers_ReturnsNotFound_WhenNoUsersExist()
    {
      // Arrange
      _mockUserAccessor.Setup(s => s.GetAllUsers()).Returns(new List<User>());

      // Act
      var result = _controller.GetAllUsers();

      // Assert
      Assert.IsType<NotFoundObjectResult>(result);
    }

    // Test for CreateUser()
    [Fact]
    public void CreateUser_ReturnsOkResult_WhenUserIsCreated()
    {
      // Arrange
      var newUser = new User { userID = 3, username = "NewUser" };
      _mockUserAccessor.Setup(s => s.CreateUser(It.IsAny<User>())).Returns(true);

      // Act
      var result = _controller.CreateUser(newUser);

      // Assert
      var okResult = Assert.IsType<OkObjectResult>(result);
      Assert.Equal("User created successfully.", okResult.Value);
    }

    [Fact]
    public void CreateUser_ReturnsBadRequest_WhenUserCreationFails()
    {
      // Arrange
      var newUser = new User { userID = 3, username = "InvalidUser" };
      _mockUserAccessor.Setup(s => s.CreateUser(It.IsAny<User>())).Returns(false);

      // Act
      var result = _controller.CreateUser(newUser);

      // Assert
      var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
      Assert.Equal("Failed to create user.", badRequestResult.Value);
    }

    // Test for UpdateUser method
    [Fact]
    public void UpdateUser_ReturnsOkResult_WhenUserIsUpdated()
    {
      // Arrange
      var userId = 1;
      var userToUpdate = new User { userID = userId, username = "UpdatedUser" };
      _mockUserAccessor.Setup(s => s.UpdateUser(userId, It.IsAny<User>())).Returns(true);

      // Act
      var result = _controller.UpdateUser(userId, userToUpdate);

      // Assert
      var okResult = Assert.IsType<OkObjectResult>(result);
      Assert.Equal("User updated successfully.", okResult.Value);
    }

    [Fact]
    public void UpdateUser_ReturnsNotFound_WhenUserDoesNotExist()
    {
      // Arrange
      var userId = 1;
      var userToUpdate = new User { userID = userId, username = "NonExistentUser" };
      _mockUserAccessor.Setup(s => s.UpdateUser(userId, It.IsAny<User>())).Returns(false);

      // Act
      var result = _controller.UpdateUser(userId, userToUpdate);

      // Assert
      Assert.IsType<NotFoundObjectResult>(result);
    }

    // Test for DeleteUser method
    [Fact]
    public void DeleteUser_ReturnsOkResult_WhenUserIsDeleted()
    {
      // Arrange
      var userId = 1;
      _mockUserAccessor.Setup(s => s.DeleteUser(userId)).Returns(true);

      // Act
      var result = _controller.DeleteUser(userId);

      // Assert
      var okResult = Assert.IsType<OkObjectResult>(result);
      Assert.Equal("User deleted successfully.", okResult.Value);
    }

    [Fact]
    public void DeleteUser_ReturnsNotFound_WhenUserDoesNotExist()
    {
      // Arrange
      var userId = 1;
      _mockUserAccessor.Setup(s => s.DeleteUser(userId)).Returns(false);

      // Act
      var result = _controller.DeleteUser(userId);

      // Assert
      Assert.IsType<NotFoundObjectResult>(result);
    }
  }
}
