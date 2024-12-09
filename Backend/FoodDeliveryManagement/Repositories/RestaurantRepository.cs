using System.Net;
using System.Numerics;
using FoodDeliveryManagement.Data;
using FoodDeliveryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryManagement.Repositories
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly FoodDeliveryDbContext _context;

        public RestaurantRepository(FoodDeliveryDbContext context)
        {
            _context = context;
        }


        public async Task<Restaurant> GetById(int restaurantId)
        {
            var restaurant = await _context.Restaurants.FindAsync(restaurantId);
            if(restaurant == null)
                throw new KeyNotFoundException($"Restaurant with id { restaurantId } not found");
            return restaurant;
        }

        //getall
        public async Task<IEnumerable<Restaurant>> GetAll()
        {
            var restaurants = await _context.Restaurants.ToListAsync();
            return restaurants;
        }

        public async Task<IEnumerable<Restaurant>> GetByOwnerId(int ownerId)
        {
            var restaurants = await _context.Restaurants.Where(res => res.UserId == ownerId).ToListAsync();
            return restaurants;
        }
        public async Task<IEnumerable<Restaurant>> Search(string? keyword, string? cuisineType)
        {
            
            var query = _context.Restaurants.AsQueryable();

            if (!string.IsNullOrEmpty(cuisineType))
            {
                query = query.Where(res => res.CuisineTypes.Any(ct => EF.Functions.Like(ct, $"%{cuisineType}%")));

            }

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(res => EF.Functions.Like(res.Name, $"%{keyword}%"));
            }
            
            var restaurants = await query.ToListAsync();
            return restaurants;
        }

        public async Task<bool> Add(Restaurant restaurant)
        {
            _context.Restaurants.Add(restaurant);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> Update(Restaurant restaurant)
        {
            try
            {
                var existingRestaurant = await _context.Restaurants.FindAsync(restaurant.RestaurantId);
                if (existingRestaurant == null)
                    return false;

                existingRestaurant.Name = restaurant.Name;
                existingRestaurant.Address = restaurant.Address;
                existingRestaurant.CuisineTypes = restaurant.CuisineTypes;
                existingRestaurant.Phone = restaurant.Phone;
                existingRestaurant.HoursOfOperation = restaurant.HoursOfOperation;
                existingRestaurant.Status = restaurant.Status;
                existingRestaurant.ImagePath = restaurant.ImagePath;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating restaurant: {ex.Message}");
                return false;
            }
        }


        public async Task<bool> Delete(int restaurantId)
        {
            var res = await _context.Restaurants.FindAsync(restaurantId);
            if (res == null)
                throw new KeyNotFoundException($"Restaurant with ID {restaurantId} not found.");

            _context.Restaurants.Remove(res);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
