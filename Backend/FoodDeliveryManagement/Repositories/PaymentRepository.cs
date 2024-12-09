using FoodDeliveryManagement.Data;
using FoodDeliveryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryManagement.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly FoodDeliveryDbContext _context;

        public PaymentRepository(FoodDeliveryDbContext context)
        {
            _context = context;
        }

        // Get payment by ID
        public async Task<Payment> GetById(int paymentId)
        {
            var payment = await _context.Payments.FindAsync(paymentId);
            if (payment == null)
                throw new KeyNotFoundException($"Payment with ID {paymentId} not found.");
            return payment;
        }

        // Get payments for a specific order
        public async Task<Payment> GetByOrderId(int orderId)
        {
            var payment = await _context.Payments.FindAsync(orderId);
            if (payment == null)
                throw new Exception($"No payments found for Order ID {orderId}.");
            return payment;
        }

        // Add a new payment record
        public async Task<bool> Add(Payment payment)
        {
            if (payment == null)
                throw new ArgumentNullException(nameof(payment), "Payment object cannot be null.");

            if (payment.Amount <= 0)
                throw new ArgumentException("Payment amount must be greater than zero.");

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return true;
        }

        // Update the status of an existing payment
        public async Task<bool> UpdateStatus(int paymentId, string status)
        {
            var payment = await _context.Payments.FindAsync(paymentId);
            if (payment == null)
                throw new KeyNotFoundException($"Payment with ID {paymentId} not found.");

            var validStatuses = new[] { "pending", "completed", "failed", "refunded" };
            if (!validStatuses.Contains(status.ToLower()))
                throw new ArgumentException($"Invalid status. Valid statuses are: {string.Join(", ", validStatuses)}.");

            payment.PaymentStatus = status;
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
