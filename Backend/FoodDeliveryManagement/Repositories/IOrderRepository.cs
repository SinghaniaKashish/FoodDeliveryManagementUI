using FoodDeliveryManagement.Models;

namespace FoodDeliveryManagement.Repositories
{
    public interface IOrderRepository
    {
        public Task<Order> GetById(int orderId);
        public Task<IEnumerable<Order>> GetByRestaurantId(int restaurantId);  
        public Task<bool> Add(Order order);
        public Task<bool> Cancel(int orderId);

    }
}
