using FoodDeliveryManagement.Models;

namespace FoodDeliveryManagement.Services
{
    public interface IUserService
    {
        Task<User> GetUserById(int userId);
        Task<IEnumerable<User>> GetUsersByRole(string role);
        Task<bool> AddUser(User user);
        Task<bool> UpdateUser(User user);
        Task<bool> DeleteUser(int userId);
    }
}
