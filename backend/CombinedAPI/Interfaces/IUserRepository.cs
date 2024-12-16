using CombinedAPI.Models;

using System.Data.SqlClient;

namespace CombinedAPI.Interfaces
{
  public interface IUserRepository
  {
      bool CreateUser(User user);
      bool UpdateUser(int id, User user);
      bool DeleteUser(int id);
      public User GetUserById(int id);
      public User GetIdByUser(string user); 
      List<User> GetAllUsers();
      bool AuthenticateUser(string username, string password);
  }
}