namespace FoodDeliveryManagement.Models
{
    public class CartItemDTO
    {
        public int MenuItemId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
    }
}