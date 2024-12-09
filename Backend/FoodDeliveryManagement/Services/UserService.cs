using FoodDeliveryManagement.Models;
using FoodDeliveryManagement.Repositories;

namespace FoodDeliveryManagement.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetUserById(int userId)
        {
            return await _userRepository.GetById(userId);
        }

        public async Task<IEnumerable<User>> GetUsersByRole(string role)
        {
            return await _userRepository.GetByRole(role);
        }

        public async Task<bool> AddUser(User user)
        {
            return await _userRepository.Add(user);
        }

        public async Task<bool> UpdateUser(User user)
        {
            return await _userRepository.Update(user);
        }

        public async Task<bool> DeleteUser(int userId)
        {
            return await _userRepository.Delete(userId);
        }
    }
}
