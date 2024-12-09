using FoodDeliveryManagement.Models;

namespace FoodDeliveryManagement.Services
{
    public interface IPaymentService
    {
        public Task<Payment> GetPaymentById(int paymentId);
        public Task<Payment> GetPaymentByOrderId(int orderId);
        public Task<bool> AddPayment(Payment payment);
        public Task<bool> UpdatePaymentStatus(int paymentId, string status);
    }
}
