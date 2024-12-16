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

    public Cart getUserCart(int cartId)
    {
      return _cartEngine.getUserCart(cartId);
    }

    public bool addToCart(int cartId, Product product, int amount)
    {
      return _cartEngine.addToCart(cartId, product, amount);
    }

    public bool removeFromCart(int cartId, Product product)
    {
      return _cartEngine.removeFromCart(cartId, product);
    }

    public bool updateAmount(int cartId, Product product, int amount)
    {
      return _cartEngine.updateAmount(cartId, product, amount);
    }

    public bool initiateCart(Cart cart)
    {
      return _cartEngine.initiateCart(cart);
    }

    public bool clearCart(int cartId)
    {
      return _cartEngine.clearCart(cartId);
    }
  }
}