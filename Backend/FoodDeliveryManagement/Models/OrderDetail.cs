using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryManagement.Models
{
    public class OrderDetail
    {
        [Key]
        public int OrderDetailId { get; set; }

        [Required]
        [ForeignKey("Order")]
        public int OrderId { get; set; }

        [Required]
        [ForeignKey("MenuItem")]
        public int ItemId { get; set; }
        public virtual MenuItem? MenuItem { get; set; }


        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }


        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }


        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }


        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public void CalculateTotalPrice()
        {
            TotalPrice = Quantity * Price;
        }
    }
}
