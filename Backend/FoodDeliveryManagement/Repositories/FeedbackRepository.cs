using FoodDeliveryManagement.Data;
using FoodDeliveryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryManagement.Repositories
{
    public class FeedbackRepository : IFeedbackRepository
    {

        private readonly FoodDeliveryDbContext _context;

        public FeedbackRepository(FoodDeliveryDbContext context)
        {
            _context = context;
        }

        // Get feedback by restaurant ID
        public async Task<IEnumerable<Feedback>> GetByRestaurantId(int restaurantId)
        {
            var feedbacks = await _context.Feedbacks
                                          .Where(f => f.RestaurantId == restaurantId).ToListAsync();
            return feedbacks;
        }

        // Add new feedback
        public async Task<bool> Add(Feedback feedback)
        {
            if (feedback == null)
                throw new ArgumentNullException(nameof(feedback), "Feedback object cannot be null.");

            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();
            return true;
        }

        // Delete feedback by ID
        public async Task<bool> Delete(int feedbackId)
        {
            var feedback = await _context.Feedbacks.FindAsync(feedbackId);
            if (feedback == null)
                throw new KeyNotFoundException($"Feedback with ID {feedbackId} not found.");

            _context.Feedbacks.Remove(feedback);
            await _context.SaveChangesAsync();
            return true;
        }
        

    }
}
