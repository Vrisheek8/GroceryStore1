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

    public Cart getUserCart(int userId)
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

        string query = "SELECT * FROM CheckoutCart WHERE userID = @userID";
        using (SqlCommand cmd = new SqlCommand(query, connection))
        {
          cmd.Parameters.AddWithValue("@userID", userId);
          using (SqlDataReader reader = cmd.ExecuteReader())
          {
            while (reader.Read())
            {
              cart.cartId = reader.GetInt32(reader.GetOrdinal("cartID"));
              cart.userId = reader.GetInt32(reader.GetOrdinal("userID"));

              int productId = reader.GetInt32(reader.GetOrdinal("productID"));
              int amount = reader.GetInt32(reader.GetOrdinal("quantity"));
              double price = getCost(productId, amount);

              tempItemList[productId] = amount;
              tempPrice += price;
            }

            cart.totalPrice = tempPrice;
          }
        }
      }

      return cart;
    }




    public bool addToCart(int userId, int productId, int amount)
    {
      Cart tempCart = getUserCart(userId);
      if (tempCart.itemList.ContainsKey(productId))
      {
        return updateAmount(userId, productId, (tempCart.itemList[productId] + amount));
      }
      using (SqlConnection connection = new SqlConnection(_connectionString))
      {
        connection.Open();
        string query = "INSERT INTO CheckoutCart (cartID, userID, productID, quantity) VALUES(@cartID, @userID, @productID, @quantity)";
        using (SqlCommand cmd = new SqlCommand(query, connection))
        {
          cmd.Parameters.AddWithValue("@cartID", tempCart.cartId);
          cmd.Parameters.AddWithValue("@userID", userId);
          cmd.Parameters.AddWithValue("@productID", productId);
          cmd.Parameters.AddWithValue("@quantity", amount);
          int rowsAffected = cmd.ExecuteNonQuery();
          return rowsAffected > 0;
        }
      }
    }

    public bool removeFromCart(int userId, int productId)
    {
      using (SqlConnection connection = new SqlConnection(_connectionString))
      {
        connection.Open();
        string query = "DELETE FROM CheckoutCart WHERE userID = @userID AND productID = @productID";
        using (SqlCommand cmd = new SqlCommand(query, connection))
        {
          cmd.Parameters.AddWithValue("@userID", userId);
          cmd.Parameters.AddWithValue("@productID", productId);

          int rowsAffected = cmd.ExecuteNonQuery();
          return rowsAffected > 0;
        }
      }
    }

    public bool updateAmount(int userId, int productId, int amount)
    {
      using (SqlConnection connection = new SqlConnection(_connectionString))
      {
        connection.Open();
        var query = "UPDATE CheckoutCart SET quantity = @quantity WHERE userID = @userID AND productID = @productID";
        using (SqlCommand cmd = new SqlCommand(query, connection))
        {
          cmd.Parameters.AddWithValue("@quantity", amount);
          cmd.Parameters.AddWithValue("@userID", userId);
          cmd.Parameters.AddWithValue("@productID", productId);

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

      if (getUserCart(cart.userId) == null)
      {
        throw new InvalidOperationException("User already has a cart.");
      }

      using (SqlConnection connection = new SqlConnection(_connectionString))
      {
        connection.Open();
        string query = @"INSERT INTO CheckoutCart (userID, productID, quantity) VALUES(@userID, @productID, @quantity)";
        using (SqlCommand cmd = new SqlCommand(query, connection))
        {
          var productId = cart.itemList.First().Key;
          var amount = cart.itemList.First().Value;

          ProductRepository productRepo = new ProductRepository(_connectionString);
          Product product = productRepo.GetProductById(productId);

          cmd.Parameters.AddWithValue("@userID", cart.userId);
          cmd.Parameters.AddWithValue("@productID", product.ProductID);
          cmd.Parameters.AddWithValue("@quantity", amount);


          int rowsAffected = cmd.ExecuteNonQuery();
          return rowsAffected > 0;
        }
      }
    }

    public bool clearCart(int userId)
    {
      using (var connection = new SqlConnection(_connectionString))
      {
        connection.Open();
        var query = "DELETE FROM CheckoutCart WHERE userID = @userID";
        using (SqlCommand cmd = new SqlCommand(query, connection))
        {
          cmd.Parameters.AddWithValue("@userID", userId);
          int rowsAffected = cmd.ExecuteNonQuery();
          return rowsAffected > 0;
        }
      }
    }

    private double getCost(int productId, int amount)
        {
            double cost = 0;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"
              SELECT 
                  p.ProductID,
                  p.Name,
                  p.Description,
                  p.Price,
                  COALESCE(
                      CASE 
                          WHEN s.IsPercentage = 1 THEN p.Price * (1 - s.DiscountAmount / 100.0)
                          ELSE p.Price - s.DiscountAmount
                      END,
                      p.Price
                  ) AS DiscountedPrice
              FROM 
                  Products p
              LEFT JOIN 
                  Sales s ON p.SaleID = s.SaleID AND s.StartDate <= GETDATE() AND GETDATE() <= s.EndDate
              WHERE 
                  p.productID = @productID;";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@productID", productId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            cost = (double)reader.GetDecimal(reader.GetOrdinal("Price"));
                        }
                        cost = cost * amount;
                    }
                }
            }
            return cost;
        }
  }
}
