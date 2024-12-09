using FoodDeliveryManagement.Data;
using FoodDeliveryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryManagement.Repositories
{
    public class UserRepository : IUserRepository 
    {
        private readonly FoodDeliveryDbContext _context;

        public UserRepository(FoodDeliveryDbContext context)
        {
            _context = context;
        }

        // Get a user by ID
        public async Task<User> GetById(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            return user;
        }

        // Get users by role
        public async Task<IEnumerable<User>> GetByRole(string role)
        {
            return await _context.Users
                .Where(u => u.Role.Equals(role, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();
        }

        // Add a new user
        public async Task<bool> Add(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        // Update an existing user
        public async Task<bool> Update(User user)
        {
            var existingUser = await _context.Users.FindAsync(user.UserId);
            if (existingUser == null)
                throw new KeyNotFoundException($"User with ID {user.UserId} not found.");

            existingUser.Username = user.Username;
            existingUser.Email = user.Email;
            existingUser.Role = user.Role;
            existingUser.PhoneNumber = user.PhoneNumber;

            _context.Users.Update(existingUser);
            await _context.SaveChangesAsync();
            return true;
        }

        // Delete a user by ID
        public async Task<bool> Delete(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {userId} not found.");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
