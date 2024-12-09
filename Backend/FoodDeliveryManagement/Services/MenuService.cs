using FoodDeliveryManagement.Models;
using FoodDeliveryManagement.Repositories;

namespace FoodDeliveryManagement.Services
{
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository _menuRepository;

        public MenuService(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public Task<MenuItem> GetMenuItemById(int menuId)
        {
            return _menuRepository.GetById(menuId);
        }

        public Task<IEnumerable<MenuItem>> GetMenuByRestaurantId(int restaurantId)
        {
            return _menuRepository.GetByRestaurantId(restaurantId);
        }

        public Task<bool> AddMenuItem(MenuItem menuItem)
        {
            if (string.IsNullOrEmpty(menuItem.Name) || menuItem.Price <= 0)
                throw new ArgumentException("Menu item must have a valid name and price.");

            return _menuRepository.Add(menuItem);
        }

        public Task<bool> UpdateMenuItem(
            int menuId,
            string? name,
            string? category,
            decimal? price,
            string? cuisine,
            bool? availability,
            bool? isVeg,
            string? imagePath,
            List<IngredientUsage>? ingredients)
        {
            return _menuRepository.Update(menuId, name, category, price, cuisine, availability, isVeg, imagePath, ingredients);
        }


        public Task<bool> DeleteMenuItem(int menuId)
        {
            return _menuRepository.Delete(menuId);
        }

        public async Task BulkMenuItemAdd(IEnumerable<MenuItem> menuItems)
        {
            foreach (var menuItem in menuItems)
            {
                if (string.IsNullOrWhiteSpace(menuItem.Name))
                {
                    throw new ArgumentException("Menu item name is required.");
                }
            }
            await _menuRepository.BulkAdd(menuItems);
        }
    }
}
