using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDeliveryManagement.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }

        [ForeignKey("Order")]
        [Required(ErrorMessage = "Order ID is required.")]
        public int OrderId { get; set; }


        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.01, 100000, ErrorMessage = "Amount must be between 0.01 and 100,000.")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }


        [Required(ErrorMessage = "Payment method is required.")]
        [RegularExpression(@"^(Card|Wallet|COD)$", ErrorMessage = "Payment method must be 'Card', 'Wallet', or 'COD'.")]
        public string PaymentMethod { get; set; }


        [Required(ErrorMessage = "Payment status is required.")]
        [RegularExpression(@"^(Paid|Failed)$", ErrorMessage = "Payment status must be 'Paid' or 'Failed'.")]
        public string PaymentStatus { get; set; }


        [DataType(DataType.DateTime, ErrorMessage = "Invalid date format.")]
        public DateTime PaymentDate { get; set; }

       
    }

}
