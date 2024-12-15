using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using CombinedAPI.Models;
using CombinedAPI.Repositories;
using CombinedAPI.Services;
using CombinedAPI.Interfaces;

namespace CombinedAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CartController : ControllerBase
  {
    private readonly ICartManager _cartManager;

    // Constructor for Dependency Injection
    public CartController(ICartManager cartManager)
    {
      _cartManager = cartManager;
    }

    // GET: api/cart/get-cart/{userId}
    [HttpGet("get-cart/{userId}")]
    public IActionResult getUserCart(int userId)
    {
      Console.WriteLine($"Getting Cart by User ID : {userId}");
      var cart = _cartManager.getUserCart(userId);
      if (cart == null)
      {
        Console.WriteLine("Unable to locate Cart.");
      }
      return Ok(cart);
    }

    // GET: api/cart/cartId/product/amount
    [HttpPost("/cart/{userId}/{productId}/{amount}")]
    public IActionResult addToCart(int cartId, int productId, int amount)
    {
      bool success = _cartManager.addToCart(cartId, productId, amount);
      if (!success)
      {
        Console.WriteLine("Unable to add to cart");
      }
      return Ok(success);
    }

    // GET: api/cart/cartId
    // GET: api/product
    [HttpDelete("cart/{userId}/{productId}")]
    public IActionResult removeFromCart(int userId, int productId)
    {
      bool success = _cartManager.removeFromCart(userId, productId);
      if (!success)
      {
        Console.WriteLine("Unable to remove from cart");
      }
      return Ok(success);
    }

    // GET: api/cart/cartId
    // GET: api/product
    // GET: api/cart/amount
    [HttpPut("cart/{userId}/{productId}/{amount}")]
    public IActionResult updateAmount(int userId, int productId, int amount)
    {
      bool success = _cartManager.updateAmount(userId, productId, amount);
      if (!success)
      {
        Console.WriteLine("Unable to update amount in cart");
      }
      return Ok(success);
    }

    // GET: api/cart/initiate-cart
    [HttpPost("initiate-cart/{userId}/{productId}/{amount}")]
    public IActionResult initiateCart(int userId, int productId, int amount, double price)
    {
      SortedDictionary<int, int> itemList = new SortedDictionary<int, int>();
      itemList.Add(productId, amount);
      Cart cart = new Cart
      {
         userId = userId,
         totalPrice = 0,
         itemList = itemList
      };
      bool success = _cartManager.initiateCart(cart);
      if (!success)
      {
        Console.WriteLine("Unable to initiate cart");
      }
      return Ok(success);
    }

    // GET: api/cart/clear/cartId
    [HttpDelete("cart/clear/{userId}")]
    public IActionResult clearCart(int userId)
    {
      bool success = _cartManager.clearCart(userId);
      if (!success)
      {
        Console.WriteLine("Unable to clear cart");
      }
      return Ok(success);
    }
  }
}
