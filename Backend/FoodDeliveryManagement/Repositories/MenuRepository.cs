using FoodDeliveryManagement.Data;
using FoodDeliveryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryManagement.Repositories
{
    public class MenuRepository : IMenuRepository
    {
        private readonly FoodDeliveryDbContext _context;
        public MenuRepository(FoodDeliveryDbContext context)
        {
            _context = context;
        }


        //public async Task<MenuItem> GetById(int itemId)
        //{
        //    var menuItem = await _context.MenuItems.FindAsync(itemId).Include(m => m.Ingredients);
        //    if(menuItem == null) 
        //        throw new KeyNotFoundException($"Menu Item with id {itemId} not found.");
        //    return menuItem;
        //}
        public async Task<MenuItem> GetById(int itemId)
        {
            var menuItem = await _context.MenuItems
                .Include(m => m.Ingredients)
                .FirstOrDefaultAsync(m => m.ItemId == itemId);

            if (menuItem == null)
                throw new KeyNotFoundException($"Menu Item with id {itemId} not found.");

            return menuItem;
        }


        public async Task<IEnumerable<MenuItem>> GetByRestaurantId(int restaurantId) 
        {
            var restaurantMenu = await _context.MenuItems
                .Where(item => item.RestaurantId == restaurantId)
                .Include(m => m.Ingredients).ToListAsync();
            return restaurantMenu;
        }

        public async Task<bool> Add(MenuItem menuItem)
        {
            if (string.IsNullOrWhiteSpace(menuItem.Name))
                throw new ArgumentException("Menu Item Name cannot be null or empty.");
            if (menuItem.Price <= 0)
                throw new ArgumentException("Price must be greater than zero.");

            _context.MenuItems.Add(menuItem);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> Update(
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
            var menuItem = await _context.MenuItems.FindAsync(menuId);
            if (menuItem == null)
                throw new KeyNotFoundException($"Menu Item with ID {menuId} not found.");

            if (!string.IsNullOrEmpty(name))
                menuItem.Name = name;

            if (!string.IsNullOrEmpty(category))
                menuItem.Category = category;
            if (!string.IsNullOrEmpty(imagePath))
                menuItem.ImagePath = imagePath;

            if (price.HasValue)
                menuItem.Price = price.Value;

            if (!string.IsNullOrEmpty(cuisine))
                menuItem.CuisineType = cuisine;

            if (availability.HasValue)
                menuItem.Availability = availability.Value;

            if (isVeg.HasValue)
                menuItem.IsVeg = isVeg.Value;

            if (ingredients != null && ingredients.Count > 0)
                menuItem.Ingredients = ingredients;

            _context.MenuItems.Update(menuItem);
            await _context.SaveChangesAsync();
            return true;
        }



        public async Task<bool> Delete(int itemId)
        {
            var item = await _context.MenuItems.FindAsync(itemId);
            if (item == null)
                throw new KeyNotFoundException($"Menu Item with ID {itemId} not found.");

            _context.MenuItems.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task BulkAdd(IEnumerable<MenuItem> menuItems)
        {
            await _context.MenuItems.AddRangeAsync(menuItems);
            await _context.SaveChangesAsync();
        }

    }
}
