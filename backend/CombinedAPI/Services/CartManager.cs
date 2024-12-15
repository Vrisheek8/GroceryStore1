using CombinedAPI.Repositories;
using CombinedAPI.Models;
using CombinedAPI.Interfaces;

namespace CombinedAPI.Services
{
  public class CartManager : ICartManager
  {
    private readonly ICartEngine _cartEngine;

    public CartManager(ICartEngine cartEngine)
    {
      _cartEngine = cartEngine;
    }

    public Cart getUserCart(int userId)
    {
      return _cartEngine.getUserCart(userId);
    }

    public bool addToCart(int userId, int productId, int amount)
    {
      return _cartEngine.addToCart(userId, productId, amount);
    }

    public bool removeFromCart(int userId, int productId)
    {
      return _cartEngine.removeFromCart(userId, productId);
    }

    public bool updateAmount(int userId, int productId, int amount)
    {
      return _cartEngine.updateAmount(userId, productId, amount);
    }

    public bool initiateCart(Cart cart)
    {
      return _cartEngine.initiateCart(cart);
    }

    public bool clearCart(int userId)
    {
      return _cartEngine.clearCart(userId);
    }
  }
}
