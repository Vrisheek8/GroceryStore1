using CombinedAPI.Models;
using CombinedAPI.Interfaces;

namespace CombinedAPI.Repositories
{
  public class CartAccessor : ICartAccessor
  {
    private readonly ICartRepository _cartRepository;

    public CartAccessor(ICartRepository cartRepository)
    {
      _cartRepository = cartRepository;
    }

    public Cart getUserCart(int userId)
    {
      return _cartRepository.getUserCart(userId);
    }

    public bool addToCart(int userId, int productId, int amount)
    {
      var cart = _cartRepository.getUserCart(userId);
      if (cart == null)
      {
        throw new ArgumentException($"No cart found with User ID {userId}");
      }

      return _cartRepository.addToCart(userId, productId, amount);
    }

    public bool removeFromCart(int userId, int productId)
    {
      var cart = _cartRepository.getUserCart(userId);
      if (cart == null)
      {
        throw new ArgumentException($"No cart found with User ID {userId}");
      }

      return _cartRepository.removeFromCart(userId, productId);
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
