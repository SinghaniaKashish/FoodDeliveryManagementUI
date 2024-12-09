using FoodDeliveryManagement.Models;

namespace FoodDeliveryManagement.Repositories
{
    public interface IRestaurantRepository
    {
		public Task<Restaurant> GetById(int restaurantId);
        public Task<IEnumerable<Restaurant>> GetAll();
        public Task<IEnumerable<Restaurant>> Search(string keyword, string cuisineType);
        public Task<bool> Add(Restaurant restaurant);
        public Task<bool> Update(Restaurant restaurant);
        public Task<bool> Delete(int restaurantId);
        public Task<IEnumerable<Restaurant>> GetByOwnerId(int ownerId);

    }
}
