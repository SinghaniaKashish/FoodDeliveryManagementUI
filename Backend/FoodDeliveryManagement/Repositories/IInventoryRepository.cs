using FoodDeliveryManagement.Models;

namespace FoodDeliveryManagement.Repositories
{
    public interface IInventoryRepository
    {
        Task<Inventory> GetItem(int inventoryId);
        Task<IEnumerable<Inventory>> GetByRestaurantId(int restaurantId);
        Task<bool> Update(Inventory inventory);
        Task<bool> Add(Inventory inventory);
        Task<bool> Delete(int inventoryId);

    }
}
