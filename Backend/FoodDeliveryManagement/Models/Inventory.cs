using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryManagement.Models
{
    public class Inventory
    {
        public int InventoryId { get; set; }


        [Required(ErrorMessage = "Restaurant ID is required.")]
        public int RestaurantId { get; set; }


        [Required(ErrorMessage = "Ingredient name is required.")]
        [MaxLength(100, ErrorMessage = "Ingredient name cannot exceed 100 characters.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Ingredient name must contain only letters and spaces.")]
        public string IngredientName { get; set; }

        
        [Required(ErrorMessage = "Quantity is required.")]
        [Range(0, 10000, ErrorMessage = "Quantity must be between 0 and 10,000.")]
        public int Quantity { get; set; }


        [Required(ErrorMessage = "Reorder level is required.")]
        [Range(0, 10000, ErrorMessage = "Reorder level must be between 0 and 10,000.")]
        public int ReorderLevel { get; set; }


        [Required(ErrorMessage = "Status is required.")]
        [RegularExpression(@"^(In Stock|Out Of Stock|Low Stock)$", ErrorMessage = "Status must be 'In Stock', 'Out of Stock', or 'Low Stock'.")]
        public string Status { get; set; }

        
    }

}
