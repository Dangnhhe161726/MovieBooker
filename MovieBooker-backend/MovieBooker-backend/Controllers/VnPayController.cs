using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieBooker_backend.Models;
using MovieBooker_backend.Repositories.VnPayRepository;

namespace MovieBooker_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VnPayController : ControllerBase
    {
        private readonly IVnPayRepository _vnPayRepository;

        public VnPayController(IVnPayRepository vnPayRepository)
        {
            _vnPayRepository = vnPayRepository;
        }
        [HttpPost("CreatePaymentUrl")]
        public IActionResult CreatePaymentUrl(VnPaymentRequestModel model)
        {
            var paymentUrl = _vnPayRepository.CreatePaymentUrl(HttpContext, model);
            return Ok(new { PaymentUrl = paymentUrl });
        }
        [HttpGet("PaymentCallback")]
        public IActionResult PaymentCallback()
        {
            var response = _vnPayRepository.PaymentExecute(Request.Query);
            if (response.Success)
            {
                if (response.VnPayResponseCode == "00")
                {
                    return Redirect("https://localhost:7175/Users/Cart/BookedSuccess");
                }
                else if (response.VnPayResponseCode == "24")
                {
                    return Redirect("https://localhost:7175");
                }
                else
                {
                    return BadRequest(new { message = "Payment failed", response });
                }
            }
            else
            {
                return BadRequest(new { message = "Payment execution failed", response });
            }
        }
    }
}
