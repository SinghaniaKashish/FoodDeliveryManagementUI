using FoodDeliveryManagement.Models;

namespace FoodDeliveryManagement.Services
{
    public interface IOrderService
    {
        public Task<Order> GetOrderById(int orderId);
        public Task<IEnumerable<Order>> GetOrdersByRestaurantId(int restaurantId);
        public Task<bool> PlaceOrder(Order order);
        public Task<bool> CancelOrder(int orderId);
    }
}
