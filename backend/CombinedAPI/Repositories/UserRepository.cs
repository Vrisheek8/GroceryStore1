using Microsoft.Data.SqlClient;
using CombinedAPI.Models;
using CombinedAPI.Interfaces;

namespace CombinedAPI.Repositories
{
  public class UserRepository : IUserRepository
  {
    private readonly string _connectionString;

    public UserRepository(string connectionString)
    {
      _connectionString = connectionString;
    }

    public List<User> GetAllUsers()
    {
      var users = new List<User>();
      using (var connection = new SqlConnection(_connectionString))
      {
        connection.Open();
        var query = "SELECT * FROM Users";
        using (SqlCommand cmd = new SqlCommand(query, connection))
        {
          using (SqlDataReader reader = cmd.ExecuteReader())
          {
            while (reader.Read())
            {
              users.Add(new User
              {
                userID = GetSafeInt32(reader, "UserID"),
                username = GetSafeString(reader, "Username"),
                firstName = GetSafeString(reader, "FirstName"),
                lastName = GetSafeString(reader, "LastName"),
                address = GetSafeString(reader, "Address"),
                email = GetSafeString(reader, "Email"),
                phoneNumber = GetSafeString(reader, "PhoneNumber"),
                password = GetSafeString(reader, "Password"),
                creditcardNumber = GetSafeString(reader, "CreditCardNumber"),
                creditcardExpDate = GetSafeString(reader, "CreditCardExpDate"),
                cvv = GetSafeInt32(reader, "CVV"),
                shippingLocation = GetSafeString(reader, "ShippingLocation")
              });
            }
          }
        }
      }
      return users;
    }

    public User GetUserById(int id)
    {
      using (var connection = new SqlConnection(_connectionString))
      {
        connection.Open();
        var query = "SELECT * FROM Users WHERE UserID = @Id";
        using (SqlCommand cmd = new SqlCommand(query, connection))
        {
          cmd.Parameters.AddWithValue("@Id", id);
          using (SqlDataReader reader = cmd.ExecuteReader())
          {
            if (reader.Read())
            {
              return new User
              {
                userID = GetSafeInt32(reader, "UserID"),
                username = GetSafeString(reader, "Username"),
                firstName = GetSafeString(reader, "FirstName"),
                lastName = GetSafeString(reader, "LastName"),
                address = GetSafeString(reader, "Address"),
                email = GetSafeString(reader, "Email"),
                phoneNumber = GetSafeString(reader, "PhoneNumber"),
                password = GetSafeString(reader, "Password"),
                creditcardNumber = GetSafeString(reader, "CreditCardNumber"),
                creditcardExpDate = GetSafeString(reader, "CreditCardExpDate"),
                cvv = GetSafeInt32(reader, "CVV"),
                shippingLocation = GetSafeString(reader, "ShippingLocation")
              };
            }
          }
        }
      }
      return null;
    }

    public bool CreateUser(User user)
    {
      using (var connection = new SqlConnection(_connectionString))
      {
        var query = @"INSERT INTO Users (Username, FirstName, LastName, Address, Email, PhoneNumber, Password, CreditCardNumber, CreditCardExpDate, CVV, ShippingLocation) 
                            VALUES (@Username, @FirstName, @LastName, @Address, @Email, @PhoneNumber, @Password, @CreditCardNumber, @CreditCardExpDate, @CVV, @ShippingLocation);";

        using (SqlCommand cmd = new SqlCommand(query, connection))
        {
          connection.Open();
          cmd.Parameters.AddWithValue("@Username", user.username);
          cmd.Parameters.AddWithValue("@FirstName", user.firstName);
          cmd.Parameters.AddWithValue("@LastName", user.lastName);
          cmd.Parameters.AddWithValue("@Address", user.address);
          cmd.Parameters.AddWithValue("@Email", user.email);
          cmd.Parameters.AddWithValue("@PhoneNumber", user.phoneNumber);
          cmd.Parameters.AddWithValue("@Password", user.password);
          cmd.Parameters.AddWithValue("@CreditCardNumber", user.creditcardNumber);
          cmd.Parameters.AddWithValue("@CreditCardExpDate", user.creditcardExpDate);
          cmd.Parameters.AddWithValue("@CVV", user.cvv);
          cmd.Parameters.AddWithValue("@ShippingLocation", user.shippingLocation);

          int rowsAffected = cmd.ExecuteNonQuery();
          return rowsAffected > 0;
        }
      }
    }

    public bool UpdateUser(int id, User user)
    {
      using (var connection = new SqlConnection(_connectionString))
      {
        var query = @"UPDATE Users 
                              SET Username = @Username, FirstName = @FirstName, LastName = @LastName, Address = @Address, Email = @Email, PhoneNumber = @PhoneNumber, Password = @Password, CreditCardNumber = @CreditCardNumber, CreditCardExpDate = @CreditCardExpDate, CVV = @CVV, ShippingLocation = @ShippingLocation 
                              WHERE UserID = @Id";
        using (SqlCommand cmd = new SqlCommand(query, connection))
        {
          connection.Open();
          cmd.Parameters.AddWithValue("@Username", user.username);
          cmd.Parameters.AddWithValue("@FirstName", user.firstName);
          cmd.Parameters.AddWithValue("@LastName", user.lastName);
          cmd.Parameters.AddWithValue("@Address", user.address);
          cmd.Parameters.AddWithValue("@Email", user.email);
          cmd.Parameters.AddWithValue("@PhoneNumber", user.phoneNumber);
          cmd.Parameters.AddWithValue("@Password", user.password);
          cmd.Parameters.AddWithValue("@CreditCardNumber", user.creditcardNumber);
          cmd.Parameters.AddWithValue("@CreditCardExpDate", user.creditcardExpDate);
          cmd.Parameters.AddWithValue("@CVV", user.cvv);
          cmd.Parameters.AddWithValue("@ShippingLocation", user.shippingLocation);
          cmd.Parameters.AddWithValue("@Id", id);

          int rowsAffected = cmd.ExecuteNonQuery();
          return rowsAffected > 0;
        }
      }
    }

    public bool DeleteUser(int id)
    {
      using (var connection = new SqlConnection(_connectionString))
      {
        var query = "DELETE FROM Users WHERE UserID = @Id";
        using (SqlCommand cmd = new SqlCommand(query, connection))
        {
          connection.Open();
          cmd.Parameters.AddWithValue("@Id", id);

          int rowsAffected = cmd.ExecuteNonQuery();
          return rowsAffected > 0;
        }
      }
    }
    public bool AuthenticateUser(string username, string password)
    {
      using (var connection = new SqlConnection(_connectionString))
      {
        var query = "SELECT username, password FROM Users WHERE username = @username";

        using (SqlCommand cmd = new SqlCommand(query, connection))
        {
          connection.Open();
          cmd.Parameters.AddWithValue("@username", username);

          using (var reader = cmd.ExecuteReader())
          {
            if (reader.Read())
            {
              string storedPassword = reader.GetString(reader.GetOrdinal("password"));

              if (password == storedPassword)
              {
                return true;
              }
            }
          }
        }
      }
      return false;
    }

    // helper method to prevent NULL reference assignment
    private string GetSafeString(SqlDataReader reader, string columnName)
    {
      var ordinal = reader.GetOrdinal(columnName);
      return reader.IsDBNull(ordinal) ? "N/A" : reader.GetString(ordinal);
    }

    private int GetSafeInt32(SqlDataReader reader, string columnName)
    {
      var ordinal = reader.GetOrdinal(columnName);
      return reader.IsDBNull(ordinal) ? 0 : reader.GetInt32(ordinal);
    }

    private decimal GetSafeDecimal(SqlDataReader reader, string columnName)
    {
      var ordinal = reader.GetOrdinal(columnName);
      return reader.IsDBNull(ordinal) ? 0 : reader.GetDecimal(ordinal);
    }

    private DateTime GetSafeDateTime(SqlDataReader reader, string columnName)
    {
      var ordinal = reader.GetOrdinal(columnName);
      return reader.IsDBNull(ordinal) ? DateTime.MinValue : reader.GetDateTime(ordinal);
    }
  }
}