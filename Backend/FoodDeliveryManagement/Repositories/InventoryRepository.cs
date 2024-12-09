using FoodDeliveryManagement.Data;
using FoodDeliveryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryManagement.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly FoodDeliveryDbContext _context;

        public InventoryRepository(FoodDeliveryDbContext context)
        {
            _context = context;
        }

        // Get an inventory item by ID
        public async Task<Inventory> GetItem(int inventoryId)
        {
            var inventory = await _context.Inventories.FindAsync(inventoryId);
            if (inventory == null)
                throw new KeyNotFoundException($"Inventory item with ID {inventoryId} not found.");
            return inventory;
        }

        // Get inventory items by restaurant ID
        public async Task<IEnumerable<Inventory>> GetByRestaurantId(int restaurantId)
        {
            return await _context.Inventories
                .Where(i => i.RestaurantId == restaurantId).ToListAsync();
        }

        // Update inventory 
        public async Task<bool> Update(Inventory inventory)
        {
            var existingInventory = await _context.Inventories.FindAsync(inventory.InventoryId);
            if (existingInventory == null)
                throw new KeyNotFoundException($"Inventory item with ID {inventory.InventoryId} not found.");

            if (inventory.Quantity < 0)
                throw new ArgumentException("Quantity cannot be less than zero.");

            existingInventory.Quantity = inventory.Quantity;
            _context.Inventories.Update(existingInventory);
            await _context.SaveChangesAsync();
            return true;
        }

        // Add a new inventory item
        public async Task<bool> Add(Inventory inventory)
        {
            _context.Inventories.Add(inventory);
            await _context.SaveChangesAsync();
            return true;
        }

        // Delete an inventory item by ID
        public async Task<bool> Delete(int inventoryId)
        {
            var inventory = await _context.Inventories.FindAsync(inventoryId);
            if (inventory == null)
                throw new KeyNotFoundException($"Inventory item with ID {inventoryId} not found.");

            _context.Inventories.Remove(inventory);
            await _context.SaveChangesAsync();
            return true;
        }



    }


}
