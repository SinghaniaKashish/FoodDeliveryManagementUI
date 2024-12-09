using FoodDeliveryManagement.Models;
using FoodDeliveryManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodDeliveryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // GET:by id
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetPaymentById(int id)
        {
            var payment = await _paymentService.GetPaymentById(id);
            if (payment == null)
                return NotFound("Payment not found.");
            return Ok(payment);
        }

        //// GET
        //[HttpGet("order/{orderId:int}")]
        //public async Task<IActionResult> GetPaymentByOrderId(int orderId)
        //{
        //    var payment = await _paymentService.GetPaymentByOrderId(orderId);
        //    if (payment == null)
        //        return NotFound("Payment not found.");
        //    return Ok(payment);
        //}


        // POST by customer
        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> AddPayment([FromBody] Payment payment)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _paymentService.AddPayment(payment);
            if (result)
                return CreatedAtAction(nameof(GetPaymentById), new { id = payment.PaymentId }, payment);

            return BadRequest("Failed to add payment.");
        }


        // PUT
        [HttpPut("{id:int}/status")]
        public async Task<IActionResult> UpdatePaymentStatus(int id, [FromBody] string status)
        {
            var result = await _paymentService.UpdatePaymentStatus(id, status);
            if (result)
                return NoContent();

            return BadRequest("Failed to update payment status.");
        }
    }
}
