using CombinedAPI.Models;
using CombinedAPI.Interfaces;

namespace CombinedAPI.Repositories
{
  public class UserAccessor : IUserAccessor
  {
    private IUserRepository _userRepository;

    public UserAccessor(IUserRepository userRepository)
    {
      _userRepository = userRepository;
    }

    public List<User> GetAllUsers()
    {
      return _userRepository.GetAllUsers();
    }

    public User GetUserById(int userId)
    {
      return _userRepository.GetUserById(userId);
    }

    public User GetIdByUser(string user) 
    {
      return _userRepository.GetIdByUser(user);
    }

    public bool CreateUser(User user)
    {
      return _userRepository.CreateUser(user);
    }

    public bool UpdateUser(int id, User user)
    {
      return _userRepository.UpdateUser(id, user);
    }

    public bool DeleteUser(int id)
    {
      return _userRepository.DeleteUser(id);
    }
    public bool AuthenticateUser(string username, string password)
    {
      return _userRepository.AuthenticateUser(username, password);
    }
  }
}