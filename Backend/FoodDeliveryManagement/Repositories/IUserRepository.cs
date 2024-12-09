using FoodDeliveryManagement.Models;

namespace FoodDeliveryManagement.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetById(int userId);
        Task<IEnumerable<User>> GetByRole(string role);
        Task<bool> Add(User user);
        Task<bool> Update(User user);
        Task<bool> Delete(int userId);
    }
}
