using FoodDeliveryManagement.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryManagement.Models
{
    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }

        [ForeignKey("User")]
        [Required]
        public int UserId { get; set; }

        [ForeignKey("MenuItem")]
        [Required]
        public int MenuItemId { get; set; }

        public MenuItem? MenuItem { get; set; } 

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [NotMapped]
        public decimal TotalPrice => Quantity * Price;
    }


}