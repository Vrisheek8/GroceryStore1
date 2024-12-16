using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using CombinedAPI.Models;
using CombinedAPI.Repositories;
using CombinedAPI.Interfaces;
using CombinedAPI.Configuration;

namespace CombinedAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class UserController : ControllerBase
  {
    private readonly IUserAccessor _userAccessor;
    private readonly IConfiguration _configuration;

    public UserController(IUserAccessor userAccessor, IConfiguration configuration)
    {
      _userAccessor = userAccessor;
      _configuration = configuration;
    }

    // GET: api/user
    [HttpGet]
    public IActionResult GetAllUsers()
    {
      try
      {
        Console.WriteLine("Fetching all users");
        var user = _userAccessor.GetAllUsers();
        if (user == null || !user.Any())
        {
          return NotFound("User not Found");
        }
        return Ok(user);
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error occurred: {ex.Message}");
        return StatusCode(500, "An error occurred while processing your request.");
      }
    }

    // GET: api/user/id/{id}
    [HttpGet("id/{id}")]
    public IActionResult GetUserById(int id)
    {
      try
      {
        Console.WriteLine($"Getting user by ID: {id}");
        var user = _userAccessor.GetUserById(id);
        if (user == null)
        {
          return NotFound("User not found.");
        }
        return Ok(user);
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error occurred: {ex.Message}");
        return StatusCode(500, "An error occurred while processing your request.");
      }
    }

        // GET: api/user/user/{user}
    [HttpGet("user/{user}")]
    public IActionResult GetIdByUser(string user)
    {
      try
      {
        Console.WriteLine($"Getting ID by username: " + user);
        var id = _userAccessor.GetIdByUser(user);
        if (id == null)
        {
          return NotFound("ID not found.");
        }
        return Ok(id);
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error occurred: {ex.Message}");
        return StatusCode(500, "An error occurred while processing your request.");
      }
    }

    // POST: api/user/create
    [HttpPost("create")]
    public IActionResult CreateUser([FromBody] User user)
    {
      try
      {
        Console.WriteLine("Creating new user");
        var success = _userAccessor.CreateUser(user);
        if (!success)
        {
          return BadRequest("Failed to create user.");
        }
        return Ok("User created successfully.");
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error occurred: {ex.Message}");
        return StatusCode(500, "An error occurred while processing your request.");
      }
    }

    // PUT: api/user/update/{id}
    [HttpPut("update/{id}")]
    public IActionResult UpdateUser(int id, [FromBody] User user)
    {
      try
      {
        Console.WriteLine($"Updating user with ID: {id}");
        var success = _userAccessor.UpdateUser(id, user);
        if (!success)
        {
          return NotFound("User not found or failed to update.");
        }
        return Ok("User updated successfully.");
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error occurred: {ex.Message}");
        return StatusCode(500, "An error occurred while processing your request.");
      }
    }

    // DELETE: api/user/delete/{id}
    [HttpDelete("delete/{id}")]
    public IActionResult DeleteUser(int id)
    {
      try
      {
        Console.WriteLine($"Deleting user with ID: {id}");
        var success = _userAccessor.DeleteUser(id);
        if (!success)
        {
          return NotFound("User not found or failed to delete.");
        }
        return Ok("User deleted successfully.");
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error occurred: {ex.Message}");
        return StatusCode(500, "An error occurred while processing your request.");
      }
    }

    // POST: api/user/authenticate
    [HttpPost("authenticate")]
    public IActionResult AuthenticateUser([FromBody] UserAuthenticate userAuthenticate)
    {
      Console.WriteLine($"Authenticating user with username: {userAuthenticate.Username}");

      bool isAuthenticated = _userAccessor.AuthenticateUser(userAuthenticate.Username, userAuthenticate.Password);
      if (!isAuthenticated)
      {
        return Unauthorized("Invalid username or password");
      }

      var JwtSettings = _configuration.GetSection("JwtSettings").Get<JwtSettings>();

      var token = GenerateJwtToken(userAuthenticate.Username);
      return Ok(new { token });
    }


    private string GenerateJwtToken(string username)
    {
      var jwtSettings = new JwtSettings
      {
        SecretKey = _configuration["JwtSettings:Key"],
        Issuer = _configuration["JwtSettings:Issuer"],
        Audience = _configuration["JwtSettings:Audience"],
        ExpiryMinutes = int.Parse(_configuration["JwtSettings:ExpiryMinutes"])
      };

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));
      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

      var claims = new[]
      {
        new Claim(JwtRegisteredClaimNames.Sub, username),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
      };

      var token = new JwtSecurityToken(
        issuer: jwtSettings.Issuer,
        audience: jwtSettings.Audience,
        claims: claims,
        expires: DateTime.Now.AddMinutes(jwtSettings.ExpiryMinutes),
        signingCredentials: creds);
      return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public class UserAuthenticate
    {
      public required string Username { get; set; }
      public required string Password { get; set; }
    }

  }
}
