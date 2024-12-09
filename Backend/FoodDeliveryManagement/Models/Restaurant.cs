using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDeliveryManagement.Models
{
    public class Restaurant
    {
        public int RestaurantId { get; set; }

        [ForeignKey("User")]
        [Required]
        public int UserId { get; set; }  //owner id

        [Required(ErrorMessage = "Restaurant name is required.")]
        [MaxLength(150, ErrorMessage = "Restaurant name cannot exceed 150 characters.")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Address is required.")]
        [MaxLength(250, ErrorMessage = "Address cannot exceed 250 characters.")]
        public string Address { get; set; }


        [Required(ErrorMessage = "Cuisine type is required.")]
        [MaxLength(500, ErrorMessage = "Cuisine type cannot exceed 500 characters.")]
        public List<string> CuisineTypes { get; set; } = new List<string>();



        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string Phone { get; set; }



        [Required(ErrorMessage = "Hours of operation are required.")]
        public string HoursOfOperation { get; set; }


        public bool Status { get; set; }  // 0 closed 1 open 

        public string? ImagePath { get; set; }


        //navigation
        public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
    }

}
