using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDeliveryManagement.Models
{
    public class Delivery
    {
        public int DeliveryId { get; set; }

        [ForeignKey("Order")]
        [Required(ErrorMessage = "Order ID is required.")]
        public int OrderId { get; set; }

        
        [Required(ErrorMessage = "Driver ID is required.")]
        public int DriverId { get; set; }


        [DataType(DataType.DateTime, ErrorMessage = "Invalid time format.")]
        public DateTime? DeliveryTime { get; set; }


        [Required(ErrorMessage = "Delivery address is required.")]
        [MaxLength(250, ErrorMessage = "Delivery address cannot exceed 250 characters.")]
        public string DeliveryAddress { get; set; }
        
    }

}
