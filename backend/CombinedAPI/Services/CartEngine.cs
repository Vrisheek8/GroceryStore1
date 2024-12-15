using CombinedAPI.Repositories;
using CombinedAPI.Models;
using CombinedAPI.Interfaces;

namespace CombinedAPI.Services
{
  public class CartEngine : ICartEngine
  {
    private readonly ICartRepository _cartRepository;

    public CartEngine(ICartRepository cartRepository)
    {
      _cartRepository = cartRepository;
    }

    public Cart getUserCart(int userId)
    {
      var cart = _cartRepository.getUserCart(userId);
      if (cart == null)
      {
        throw new ArgumentException($"No cart found with User ID {userId}");
      }
      return cart;
    }

    public bool addToCart(int userId, int product, int amount)
    {
      var cart = _cartRepository.getUserCart(userId);
      if (cart == null)
      {
        throw new ArgumentException($"No cart found with User ID {userId}");
      }

      return _cartRepository.addToCart(userId, product, amount);
    }

    public bool removeFromCart(int userId, int product)
    {
      var cart = _cartRepository.getUserCart(userId);
      if (cart == null)
      {
        throw new ArgumentException($"No cart found with User ID {userId}");
      }

      return _cartRepository.removeFromCart(userId, product);
    }

    public bool updateAmount(int userId, int productId, int amount)
    {
      var cart = _cartRepository.getUserCart(userId);
      if (cart == null)
      {
        throw new ArgumentException($"No cart found with User ID {userId}");
      }

      return _cartRepository.updateAmount(userId, productId, amount);
    }

    public bool initiateCart(Cart cart)
    {
      return _cartRepository.initiateCart(cart);
    }

    public bool clearCart(int userId)
    {
      var cart = _cartRepository.getUserCart(userId);
      if (cart == null)
      {
        throw new ArgumentException($"No cart found with User ID {userId}");
      }

      return _cartRepository.clearCart(userId);
    }
  }
}
