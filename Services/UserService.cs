using navami.Models;
using System.Collections.Generic;
using System.Linq;

namespace navami.Services
{
    //public class UserService
    //{
    //    private readonly List<User> _users = new()
    //    {
    //        new User { UserId = 9, Username = "user1", Email = "user1@example.com", Role = "User", Mobile = "0987654321", Password = "password123" },

    //        new User { UserId = 1, Username = "admin", Email = "admin@example.com", Role = "Admin", Mobile = "1234567890", Password = "password123" },
    //        new User { UserId = 2, Username = "dev", Email = "dev@example.com", Role = "Dev", Mobile = "1234567890", Password = "password123" }

    //    };

    //    // Get all users
    //    public List<User> GetAllUsers() => _users;

    //    // Get a user by ID
    //    public User GetUserById(int id)
    //    {
    //        return _users.FirstOrDefault(r => r.UserId == id);
    //    }

    //    // Add a new user
    //    public void AddUser(User user)
    //    {
    //        user.UserId = _users.Count > 0 ? _users.Max(u => u.UserId) + 1 : 1; // Ensure unique UserId
    //        _users.Add(user);
    //    }

    //    // Update an existing user
    //    public void UpdateUser(User user)
    //    {
    //        var existingUser = GetUserById(user.UserId);
    //        if (existingUser != null)
    //        {
    //            existingUser.Username = user.Username;
    //            existingUser.Email = user.Email;
    //            existingUser.Role = user.Role;
    //            existingUser.Mobile = user.Mobile;
    //            existingUser.Password = user.Password;
    //        }
    //    }

    //    // Delete a user by ID
    //    public void DeleteUser(int id) => _users.RemoveAll(r => r.UserId == id);
    //}
}