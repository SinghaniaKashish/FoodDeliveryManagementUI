using FoodDeliveryManagement.Models;

namespace FoodDeliveryManagement.Services
{
    public interface IRestaurantService
    {

        public Task<Restaurant> GetRestaurantById(int restaurantId);
        public Task<IEnumerable<Restaurant>> GetAllRestaurant();
        public Task<IEnumerable<Restaurant>> SearchRestaurant(string? keyword, string? cuisineType);
        public Task<bool> AddRestaurant(Restaurant restaurant);
        public Task<bool> UpdateRestaurant(Restaurant restaurant);
        public Task<bool> DeleteRestaurant(int restaurantId);
        public Task<IEnumerable<Restaurant>> GetRestaurantByOwnerId(int ownerId);
    }
}
