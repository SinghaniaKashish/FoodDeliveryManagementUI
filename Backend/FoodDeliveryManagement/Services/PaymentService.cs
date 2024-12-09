using FoodDeliveryManagement.Models;
using FoodDeliveryManagement.Repositories;

namespace FoodDeliveryManagement.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        // Get payment by ID
        public async Task<Payment> GetPaymentById(int paymentId)
        {
            return await _paymentRepository.GetById(paymentId);
        }

        // Get payment by Order ID
        public async Task<Payment> GetPaymentByOrderId(int orderId)
        {
            return await _paymentRepository.GetByOrderId(orderId);
        }

        // Add a new payment
        public async Task<bool> AddPayment(Payment payment)
        {
            if (payment == null)
                throw new ArgumentNullException(nameof(payment), "Payment cannot be null.");

            if (payment.Amount <= 0)
                throw new ArgumentException("Payment amount must be greater than zero.");

            return await _paymentRepository.Add(payment);
        }

        // Update the status of a payment
        public async Task<bool> UpdatePaymentStatus(int paymentId, string status)
        {
            var validStatuses = new[] { "pending", "completed", "failed", "refunded" };
            if (!validStatuses.Contains(status.ToLower()))
                throw new ArgumentException($"Invalid status. Valid statuses are: {string.Join(", ", validStatuses)}.");

            return await _paymentRepository.UpdateStatus(paymentId, status);
        }

        // Process a payment
        public async Task<bool> ProcessPaymentAsync(Payment payment)
        {
            if (payment == null)
                throw new ArgumentNullException(nameof(payment), "Payment object cannot be null.");

            if (payment.Amount <= 0)
                throw new ArgumentException("Payment amount must be greater than zero.");

            bool paymentProcessed = true;

            payment.PaymentStatus = paymentProcessed ? "completed" : "failed";
            await _paymentRepository.UpdateStatus(payment.PaymentId, payment.PaymentStatus);

            return paymentProcessed;
        }

    }
}
