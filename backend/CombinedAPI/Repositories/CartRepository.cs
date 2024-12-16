using CombinedAPI.Models;
using CombinedAPI.Interfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace CombinedAPI.Repositories
{
  public class CartRepository : ICartRepository
  {
    private string _connectionString;

    public CartRepository(string connectionString)
    {
      _connectionString = connectionString;
    }

    public Cart getUserCart(int cartId)
    {
      SortedDictionary<int, int> tempItemList = new SortedDictionary<int, int>();

      Cart cart = new Cart
      {
        cartId = 0,
        userId = 0,
        itemList = tempItemList,
        totalPrice = 0.0
      };

      double tempPrice = 0;

      using (SqlConnection connection = new SqlConnection(_connectionString))
      {
        connection.Open();

        string query = "SELECT * FROM Cart WHERE CartId = @CartId";
        using (SqlCommand cmd = new SqlCommand(query, connection))
        {
          cmd.Parameters.AddWithValue("@CartId", cartId);
          using (SqlDataReader reader = cmd.ExecuteReader())
          {
            while (reader.Read())
            {
              cart.cartId = reader.GetInt32(reader.GetOrdinal("CartId"));
              cart.userId = reader.GetInt32(reader.GetOrdinal("UserId"));

              int productId = reader.GetInt32(reader.GetOrdinal("ProductId"));
              int amount = reader.GetInt32(reader.GetOrdinal("Amount"));
              double price = reader.GetDouble(reader.GetOrdinal("Price"));

              tempItemList[productId] = amount;
              tempPrice += price;
            }

            cart.totalPrice = tempPrice;
          }
        }
      }

      return cart;
    }




    public bool addToCart(int cartId, Product product, int amount)
    {
      Cart tempCart = getUserCart(cartId);
      using (SqlConnection connection = new SqlConnection(_connectionString))
      {
        connection.Open();
        string query = "INSERT INTO Cart (CartId, UserId, ProductID, Amount, Price) VALUES(@CartId, @UserId, @ProductID, @Amount, @Price)";
        using (SqlCommand cmd = new SqlCommand(query, connection))
        {
          cmd.Parameters.AddWithValue("@CartId", cartId);
          cmd.Parameters.AddWithValue("@UserId", tempCart.userId);
          cmd.Parameters.AddWithValue("@ProductID", product.ProductID);
          cmd.Parameters.AddWithValue("@Amount", amount);
          cmd.Parameters.AddWithValue("@Price", product.Price * amount);

          int rowsAffected = cmd.ExecuteNonQuery();
          return rowsAffected > 0;
        }
      }
    }

    public bool removeFromCart(int cartId, Product product)
    {
      using (SqlConnection connection = new SqlConnection(_connectionString))
      {
        connection.Open();
        string query = "DELETE FROM Cart WHERE CartId = @CartId AND ProductId = @ProductId";
        using (SqlCommand cmd = new SqlCommand(query, connection))
        {
          cmd.Parameters.AddWithValue("@CartId", cartId);
          cmd.Parameters.AddWithValue("@ProductId", product.ProductID);

          int rowsAffected = cmd.ExecuteNonQuery();
          return rowsAffected > 0;
        }
      }
    }

    public bool updateAmount(int cartId, Product product, int amount)
    {
      using (SqlConnection connection = new SqlConnection(_connectionString))
      {
        connection.Open();
        var query = "UPDATE Cart SET Amount = @Amount WHERE CartId = @CartId AND ProductID = @ProductID";
        using (SqlCommand cmd = new SqlCommand(query, connection))
        {
          cmd.Parameters.AddWithValue("@Amount", amount);
          cmd.Parameters.AddWithValue("@CartId", cartId);
          cmd.Parameters.AddWithValue("@ProductID", product.ProductID);

          int rowsAffected = cmd.ExecuteNonQuery();
          return rowsAffected > 0;
        }
      }
    }

    public bool initiateCart(Cart cart)
    {
      if (cart.itemList.Count != 1)
      {
        throw new InvalidOperationException("Cannot create new cart instance with more or less than one product.");
      }

      using (SqlConnection connection = new SqlConnection(_connectionString))
      {
        connection.Open();
        string query = @"INSERT INTO Cart (UserId, ProductID, Amount, Price) VALUES(@UserId, @ProductID, @Amount, @Price)";
        using (SqlCommand cmd = new SqlCommand(query, connection))
        {
          var productId = cart.itemList.First().Key;
          var amount = cart.itemList.First().Value;

          ProductRepository productRepo = new ProductRepository(_connectionString);
          Product product = productRepo.GetProductById(productId);

          cmd.Parameters.AddWithValue("@UserId", cart.userId);
          cmd.Parameters.AddWithValue("@ProductID", product.ProductID);
          cmd.Parameters.AddWithValue("@Amount", amount);
          cmd.Parameters.AddWithValue("@Price", product.Price * amount);


          int rowsAffected = cmd.ExecuteNonQuery();
          return rowsAffected > 0;
        }
      }
    }

    public bool clearCart(int cartId)
    {
      using (var connection = new SqlConnection(_connectionString))
      {
        connection.Open();
        var query = "DELETE FROM Cart WHERE CartId = @CartId";
        using (SqlCommand cmd = new SqlCommand(query, connection))
        {
          cmd.Parameters.AddWithValue("@CartId", cartId);
          int rowsAffected = cmd.ExecuteNonQuery();
          return rowsAffected > 0;
        }
      }
    }
  }
}
