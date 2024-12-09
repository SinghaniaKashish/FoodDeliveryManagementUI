using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryManagement.Models
{
    public class User
    {
        public int UserId { get; set; }


        [Required(ErrorMessage = "Username is required.")]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Username must be alphanumeric and can contain spaces.")]
        [MaxLength(100, ErrorMessage = "Username cannot exceed 100 characters.")]
        public string Username { get; set; }


        [Required(ErrorMessage = "Password is required.")]
        public string PasswordHash { get; set; }


        [Required(ErrorMessage = "Role is required.")]
        [RegularExpression(@"^(Admin|Customer|Driver|Owner)$", ErrorMessage = "Role must be either 'Admin', 'Customer', 'Owner' or 'Driver'.")]
        public string Role { get; set; }


        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone number should be 10 digit")]
        public string PhoneNumber { get; set; }

        public bool IsActive { get; set; } = true;

    }

}
