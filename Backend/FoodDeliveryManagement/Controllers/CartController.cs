using FoodDeliveryManagement.Data;
using FoodDeliveryManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly FoodDeliveryDbContext _context;

        public CartController(FoodDeliveryDbContext context)
        {
            _context = context;
        }

        [HttpGet("{userId}")]
        [Authorize(Roles = "Customer")]

        public async Task<IActionResult> GetCart(int userId)
        {
            var cartItems = await _context.Cart
                .Where(c => c.UserId == userId)
                .Include(c => c.MenuItem)
                .Select(c => new CartItemDTO
                {
                    MenuItemId = c.MenuItemId,
                    ItemName = c.MenuItem.Name,
                    Quantity = c.Quantity,
                    Price = c.Price,
                    TotalPrice = c.Quantity * c.Price
                })
                .ToListAsync();

            return Ok(new { status = "success", data = cartItems });
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]

        public async Task<IActionResult> AddToCart(CartItem cartItem)
        {
            if (cartItem == null || cartItem.MenuItemId <= 0 || cartItem.UserId <= 0 || cartItem.Quantity <= 0)
            {
                return BadRequest(new { status = "error", message = "Invalid cart item data." });
            }

            var menuItem = await _context.MenuItems.FindAsync(cartItem.MenuItemId);
            if (menuItem == null)
            {
                return NotFound(new { status = "error", message = "Menu item does not exist." });
            }

            var existingCartItem = await _context.Cart
                .FirstOrDefaultAsync(ci => ci.UserId == cartItem.UserId && ci.MenuItemId == cartItem.MenuItemId);

            if (existingCartItem != null)
            {
                existingCartItem.Quantity += cartItem.Quantity;
                _context.Update(existingCartItem);
            }
            else
            {
                cartItem.Price = menuItem.Price; 
                _context.Cart.Add(cartItem);
            }

            await _context.SaveChangesAsync();
            return Ok(new { status = "success", data = cartItem });
        }

        [HttpDelete("{userId}")]
        [Authorize(Roles = "Customer")]

        public async Task<IActionResult> ClearCart(int userId)
        {
            var userCart = _context.Cart.Where(c => c.UserId == userId);
            _context.Cart.RemoveRange(userCart);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{userId}/{menuItemId}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> RemoveCartItem(int userId, int menuItemId)
        {
            var cartItem = await _context.Cart
                .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.MenuItemId == menuItemId);

            if (cartItem == null)
            {
                return NotFound(new { status = "error", message = "Cart item not found." });
            }

            _context.Cart.Remove(cartItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }



        [HttpPatch("{userId}/increment/{menuItemId}")]
        [Authorize(Roles = "Customer")]

        public async Task<IActionResult> IncrementQuantity(int userId, int menuItemId)
        {
            var cartItem = await _context.Cart
                .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.MenuItemId == menuItemId);

            if (cartItem == null)
                return NotFound("Cart item not found.");

            cartItem.Quantity++;
            _context.Update(cartItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }



        [HttpPatch("{userId}/decrement/{menuItemId}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> DecrementQuantity(int userId, int menuItemId)
        {
            var cartItem = await _context.Cart
                .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.MenuItemId == menuItemId);

            if (cartItem == null)
                return NotFound("Cart item not found.");

            if (cartItem.Quantity > 1)
            {
                cartItem.Quantity--;
                _context.Update(cartItem);
                await _context.SaveChangesAsync();
            }

            return NoContent();
        }
    }
    }
