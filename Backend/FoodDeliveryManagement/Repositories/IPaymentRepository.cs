using FoodDeliveryManagement.Models;

namespace FoodDeliveryManagement.Repositories
{
    public interface IPaymentRepository
    {
        public Task<Payment> GetById(int paymentId);
        public Task<Payment> GetByOrderId(int orderId);
        public Task<bool> Add(Payment payment);
        public Task<bool> UpdateStatus(int paymentId, string status);

    }
}
