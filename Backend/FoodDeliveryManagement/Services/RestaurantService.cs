using FoodDeliveryManagement.Models;
using FoodDeliveryManagement.Repositories;

namespace FoodDeliveryManagement.Services
{
    public class RestaurantService: IRestaurantService
    {
        private readonly IRestaurantRepository _restaurantRepo;

        
        public RestaurantService(IRestaurantRepository restaurantRepo)
        {
            _restaurantRepo = restaurantRepo;
        }



        //Repo funtions
        public async Task<Restaurant> GetRestaurantById(int restaurantId)
        {
            var restaurant = await _restaurantRepo.GetById(restaurantId);
            if (restaurant == null)
                throw new KeyNotFoundException($"Restaurant with ID {restaurantId} does not exist.");
            return restaurant;
        }
        public async Task<IEnumerable<Restaurant>> GetAllRestaurant()
        {
            return await _restaurantRepo.GetAll();
        }
        public async Task<IEnumerable<Restaurant>> SearchRestaurant(string? keyword, string? cuisineType)
        {
            return await _restaurantRepo.Search(keyword, cuisineType);
        }
        public async Task<bool> AddRestaurant(Restaurant restaurant)
        {
            if (string.IsNullOrWhiteSpace(restaurant.Name))
                throw new ArgumentException("Restaurant name cannot be null or empty.");
            if (restaurant.CuisineTypes == null || restaurant.CuisineTypes.Count == 0)
                throw new ArgumentException("At least one cuisine type must be provided.");

            return await _restaurantRepo.Add(restaurant);
        }
        public async Task<bool> UpdateRestaurant(Restaurant restaurant)
        {
            
            return await _restaurantRepo.Update(restaurant);
        }
        public async Task<bool> DeleteRestaurant(int restaurantId)
        {
            var restaurant = await _restaurantRepo.GetById(restaurantId);
            if (restaurant == null)
                throw new KeyNotFoundException($"Restaurant with ID {restaurantId} does not exist.");

            return await _restaurantRepo.Delete(restaurantId);
        }

        public async Task<IEnumerable<Restaurant>> GetRestaurantByOwnerId(int ownerId)
        {
            return await _restaurantRepo.GetByOwnerId(ownerId);
        }
    }
}
