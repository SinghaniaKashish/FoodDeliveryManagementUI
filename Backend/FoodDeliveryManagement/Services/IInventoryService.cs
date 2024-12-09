using FoodDeliveryManagement.Models;

namespace FoodDeliveryManagement.Services
{
    public interface IInventoryService
    {
        Task<Inventory> GetInventoryItem(int inventoryId);
        Task<IEnumerable<Inventory>> GetInventoryByRestaurantId(int restaurantId);
        Task<bool> UpdateInventory(Inventory inventory);
        Task<bool> AddInventoryItem(Inventory inventory);
        Task<bool> DeleteInventoryItem(int inventoryId);
    }
}
