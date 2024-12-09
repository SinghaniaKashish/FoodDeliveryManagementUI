using FoodDeliveryManagement.Models;

namespace FoodDeliveryManagement.Repositories
{
    public interface IMenuRepository
    {
        public Task<MenuItem> GetById(int itemId);
        public Task<IEnumerable<MenuItem>> GetByRestaurantId(int restaurantId);
        public Task<bool> Add(MenuItem menuItem);
        public  Task<bool> Update(int menuId, string? name, string? category, decimal? price, string? cuisine, bool? availability, bool? isVeg, string? imagePath, List<IngredientUsage>? ingredients);
        public Task<bool> Delete(int itemId);
        public Task BulkAdd(IEnumerable<MenuItem> menuItems);

    }
}
