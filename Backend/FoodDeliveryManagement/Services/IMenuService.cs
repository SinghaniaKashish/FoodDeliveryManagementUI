using FoodDeliveryManagement.Models;

namespace FoodDeliveryManagement.Services
{
    public interface IMenuService
    {
        public Task<MenuItem> GetMenuItemById(int itemId);
        public Task<IEnumerable<MenuItem>> GetMenuByRestaurantId(int restaurantId);
        public Task<bool> AddMenuItem(MenuItem menuItem);
        public Task<bool> UpdateMenuItem(int menuId, string? name, string? category, decimal? price, string? cuisine, bool? availability, bool? isVeg, string? imagePath, List<IngredientUsage>? ingredients);
        public Task<bool> DeleteMenuItem(int itemId);
        public Task BulkMenuItemAdd(IEnumerable<MenuItem> menuItems);
    }
}
