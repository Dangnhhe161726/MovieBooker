using Microsoft.AspNetCore.Mvc;

namespace MovieBooker_backend.Repositories.PayPalRepository
{
    public interface IPayPalRepository
    {
        public Task<string> CreatePayment(double amount);
        public Task<IActionResult> ExecutePayment(string token);
        public IActionResult CancelPayment(string token);
    }
}
