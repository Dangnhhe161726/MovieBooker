using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieBooker_backend.Helpers;
using MovieBooker_backend.Repositories.PayPalRepository;
using Newtonsoft.Json;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;

namespace MovieBooker_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayPalController : ControllerBase
    {
        private readonly IPayPalRepository _payPalRepository;

        public PayPalController(IPayPalRepository payPalRepository)
        {
            _payPalRepository = payPalRepository;
        }

        [HttpPost("create-payment")]
        public async Task<IActionResult> CreatePayment([FromBody] double amount)
        {
            var approvalUrl = await _payPalRepository.CreatePayment(amount);
            return Ok(new { approvalUrl });
        }

        [HttpGet("payment-success")]
        public async Task<IActionResult> ExecutePayment(string token)
        {
            return await _payPalRepository.ExecutePayment(token);
        }

        [HttpGet("payment-cancel")]
        public IActionResult CancelPayment(string token)
        {
            return _payPalRepository.CancelPayment(token);
        }
    }
}
