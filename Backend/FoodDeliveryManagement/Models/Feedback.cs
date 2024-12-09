using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDeliveryManagement.Models
{
    public class Feedback
    {
        public int FeedbackId { get; set; }


        [ForeignKey("Order")]
        [Required(ErrorMessage = "Order ID is required.")]
        public int OrderId { get; set; }

        [ForeignKey("Restaurant")]
        [Required(ErrorMessage = "Restaurant ID is required.")]
        public int RestaurantId { get; set; }



        [ForeignKey("User")]
        [Required(ErrorMessage = "Customer ID is required.")]
        public int CustomerId { get; set; }

        [ForeignKey("User")]
        [Required(ErrorMessage = "Driver ID is required.")]
        public int DriverId { get; set; }

        [Required(ErrorMessage = "Rating is required.")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int FoodRating { get; set; }

        [Required(ErrorMessage = "Rating is required.")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int DeliveryTimeRating { get; set; }


        [Required(ErrorMessage = "Rating is required.")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int DeliveryDriverRating { get; set; }


        [MaxLength(1000, ErrorMessage = "Comments cannot exceed 1000 characters.")]
        public string? Comments { get; set; }


        [Required(ErrorMessage = "Feedback date is required.")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid date format.")]
        public DateTime FeedbackDate { get; set; }

    }

}
