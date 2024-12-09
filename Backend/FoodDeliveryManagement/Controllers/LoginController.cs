using System.Security.Cryptography;
using System.Text;
using FoodDeliveryManagement.Data;
using FoodDeliveryManagement.Models;
using FoodDeliveryManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly FoodDeliveryDbContext _dbContext;
        private readonly JwtService _jwtsvc;
        private readonly IUserService _userService;
        public LoginController(FoodDeliveryDbContext dbContext, JwtService jwtsvc, IUserService userService)
        {
            _dbContext = dbContext;
            _jwtsvc = jwtsvc;
            _userService = userService;
        }


        //Register User
        [HttpPost("signup")]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp(User request)
        {

            if (await _dbContext.Users.AnyAsync(u => u.Email == request.Email))
            {
                return BadRequest(new { message = "Email is already registered." });
            }

            request.PasswordHash = HashPassword(request.PasswordHash);

            bool userCreated = await _userService.AddUser(request);
            if (userCreated)
            {
                return Ok(new { message = "User Registered Successfully" });
            }
            else
            {
                return StatusCode(500, new { message = "User registration failed. Please try again." });

            }
        }



        //Login User

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequest request)
            {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if(user == null || !VerifyPassword(request.Password, user.PasswordHash))
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            var token = _jwtsvc.GenerateToken(user.Username, user.Role);

            return Ok(new
            {
                message = "Login successful.",
                token,
                userId = user.UserId,
                role = user.Role
            });

        }

        //Hash Password
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        //Verify Password
        private bool VerifyPassword(string password, string hashedPassword)
        {
            var hashedInput = HashPassword(password);
            return hashedInput == hashedPassword;
        }

    }
}
