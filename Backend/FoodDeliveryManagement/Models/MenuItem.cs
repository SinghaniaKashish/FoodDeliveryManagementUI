using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDeliveryManagement.Models
{
    public class MenuItem
    {
        [Key]
        public int ItemId { get; set; }


        [ForeignKey("Restaurant")]
        [Required(ErrorMessage = "Restaurant ID is required.")]
        public int RestaurantId { get; set; }
        public Restaurant? Restaurant { get; set; }

        [Required(ErrorMessage = "Menu item name is required.")]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Category is required.")]
        [MaxLength(50, ErrorMessage = "Category cannot exceed 50 characters.")]
        public string Category { get; set; }


        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, 10000, ErrorMessage = "Price must be between 0.01 and 10,000.")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }


        [Required(ErrorMessage = "Cuisine type is required.")]
        [MaxLength(100, ErrorMessage = "Cuisine type cannot exceed 100 characters.")]
        public string CuisineType { get; set; }


        public bool Availability { get; set; } = true;


        public bool IsVeg { get; set; }

        public string? ImagePath { get; set; }

        public ICollection<IngredientUsage> Ingredients { get; set; } = new List<IngredientUsage>();


    }
}
