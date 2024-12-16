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

    public Cart getUserCart(int cartId)
    {
      var cart = _cartRepository.getUserCart(cartId);
      if (cart == null)
      {
        throw new ArgumentException($"No cart found with user ID {cartId}");
      }
      return cart;
    }

    public bool addToCart(int cartId, Product product, int amount)
    {
      var cart = _cartRepository.getUserCart(cartId);
      if (cart == null)
      {
        throw new ArgumentException($"No cart found with ID {cartId}");
      }

      return _cartRepository.addToCart(cartId, product, amount);
    }

    public bool removeFromCart(int cartId, Product product)
    {
      var cart = _cartRepository.getUserCart(cartId);
      if (cart == null)
      {
        throw new ArgumentException($"No cart found with ID {cartId}");
      }

      return _cartRepository.removeFromCart(cartId, product);
    }

    public bool updateAmount(int cartId, Product product, int amount)
    {
      var cart = _cartRepository.getUserCart(cartId);
      if (cart == null)
      {
        throw new ArgumentException($"No cart found with ID {cartId}");
      }

      return _cartRepository.updateAmount(cartId, product, amount);
    }

    public bool initiateCart(Cart cart)
    {
      return _cartRepository.initiateCart(cart);
    }

    public bool clearCart(int cartId)
    {
      var cart = _cartRepository.getUserCart(cartId);
      if (cart == null)
      {
        throw new ArgumentException($"No cart found with ID {cartId}");
      }

      return _cartRepository.clearCart(cartId);
    }
  }
}