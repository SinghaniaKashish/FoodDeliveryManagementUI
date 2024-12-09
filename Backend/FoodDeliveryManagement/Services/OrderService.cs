using FoodDeliveryManagement.Models;
using FoodDeliveryManagement.Repositories;

namespace FoodDeliveryManagement.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Order> GetOrderById(int orderId)
        {
            try
            {
                return await _orderRepository.GetById(orderId);
            }
            catch (KeyNotFoundException)
            {
                throw new Exception($"Order with ID {orderId} not found.");
            }
        }


        public async Task<IEnumerable<Order>> GetOrdersByRestaurantId(int restaurantId)
        {
            var orders = await _orderRepository.GetByRestaurantId(restaurantId);
            if (!orders.Any())
                throw new Exception($"No orders found for restaurant with ID {restaurantId}.");
            return orders;
        }

        public async Task<bool> PlaceOrder(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order), "Order cannot be null.");

            if (order.OrderDetails == null || !order.OrderDetails.Any())
                throw new ArgumentException("Order must contain at least one order detail.");

            return await _orderRepository.Add(order);
        }

        

        public async Task<bool> CancelOrder(int orderId)
        {
            var order = await _orderRepository.GetById(orderId);
            if (order == null)
                throw new Exception($"Order with ID {orderId} not found.");

            if (order.Status.ToLower() == "cancelled")
                throw new Exception("Order is already cancelled.");

            var timeSinceOrder = DateTime.UtcNow - order.OrderDate;
            if (timeSinceOrder.TotalMinutes > 2)
                throw new Exception("Order cannot be cancelled as more than 2 minutes have passed since it was placed.");

            return await _orderRepository.Cancel(orderId);
        }
    }
}
