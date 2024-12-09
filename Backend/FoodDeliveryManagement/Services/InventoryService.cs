using FoodDeliveryManagement.Models;
using FoodDeliveryManagement.Repositories;

namespace FoodDeliveryManagement.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _inventoryRepository;

        public InventoryService(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public async Task<Inventory> GetInventoryItem(int inventoryId)
        {
            return await _inventoryRepository.GetItem(inventoryId);
        }

        public async Task<IEnumerable<Inventory>> GetInventoryByRestaurantId(int restaurantId)
        {
            return await _inventoryRepository.GetByRestaurantId(restaurantId);
        }

        public async Task<bool> UpdateInventory(Inventory inventory)
        {
            return await _inventoryRepository.Update(inventory);
        }

        public async Task<bool> AddInventoryItem(Inventory inventory)
        {
            return await _inventoryRepository.Add(inventory);
        }

        public async Task<bool> DeleteInventoryItem(int inventoryId)
        {
            return await _inventoryRepository.Delete(inventoryId);
        }
    }
}
