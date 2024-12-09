using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDeliveryManagement.Models
{
    public class IngredientUsage
    {
        public int IngredientUsageId { get; set; }

        [ForeignKey("MenuItem")]
        [Required(ErrorMessage ="Item Id is required")]
        public int ItemId { get; set; }

        [Required(ErrorMessage = "Ingredient name is required.")]
        [MaxLength(100, ErrorMessage = "Ingredient name cannot exceed 100 characters.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Ingredient name must contain only letters and spaces.")]
        public string IngredientName { get; set; }


        [Required(ErrorMessage = "Quantity is required.")]
        [Range(0.01, 1000, ErrorMessage = "Quantity must be between 0.01 and 1,000.")]
        public decimal Quantity { get; set; }
    }
}
