using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDeliveryManagement.Models
{
    public class Order
    {
        public int OrderId { get; set; }


        [ForeignKey("User")]
        [Required(ErrorMessage = "Customer ID is required.")]
        public int CustomerId { get; set; }


        [ForeignKey("Restaurant")]
        [Required(ErrorMessage = "Restaurant ID is required.")]
        public int RestaurantId { get; set; }


        [Required(ErrorMessage = "Total amount is required.")]
        [Range(0.01, 100000, ErrorMessage = "Total amount must be between 0.01 and 100,000.")]
        [DataType(DataType.Currency)]
        public decimal TotalAmount { get; set; }


        [Required(ErrorMessage = "Order date is required.")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid date format.")]
        public DateTime OrderDate { get; set; }


        [Required(ErrorMessage = "Status is required.")]
        [RegularExpression(@"^(Pending|Accepted|Prepared|OutForDelivery|Delivered|cancelled)$", ErrorMessage = "Status must be 'Pending', 'Accepted', 'OutForDelivery', 'Delivered', 'Prepared'")]
        public string Status { get; set; }


        [Required(ErrorMessage = "Delivery address is required.")]
        [MaxLength(250, ErrorMessage = "Delivery address cannot exceed 250 characters.")]
        public string DeliveryAddress { get; set; }

        [Required(ErrorMessage = "Payment method is required.")]
        [RegularExpression(@"^(Card|Wallet|COD)$", ErrorMessage = "Payment method must be 'Card', 'Wallet', or 'COD'.")]
        public string PaymentMethod { get; set; }


        //navigation
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();


    }

}
