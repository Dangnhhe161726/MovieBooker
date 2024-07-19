using Microsoft.AspNetCore.Mvc;
using MovieBooker_backend.Helpers;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;

namespace MovieBooker_backend.Repositories.PayPalRepository
{
    public class PayPalRepository : IPayPalRepository
    {
        private readonly PayPalHttpClient _payPalClient;
        private readonly IConfiguration _configuration;

        public PayPalRepository(IConfiguration configuration)
        {
            _payPalClient = PayPalClient.Client(configuration);
            _configuration = configuration;
        }

        public async Task<string> CreatePayment(double amount)
        {
            var order = new OrderRequest()
            {
                CheckoutPaymentIntent = "CAPTURE",
                PurchaseUnits = new List<PurchaseUnitRequest>
                {
                    new PurchaseUnitRequest
                    {
                        AmountWithBreakdown = new AmountWithBreakdown
                        {
                            CurrencyCode = "USD",
                            Value = amount.ToString("F2"),
                        }
                    }
                },
                ApplicationContext = new ApplicationContext
                {
                    ReturnUrl = _configuration["PayPal:ReturnUrl"],
                    CancelUrl = _configuration["PayPal:CancelUrl"]
                }
            };

            var request = new OrdersCreateRequest();
            request.RequestBody(order);

            var response = await _payPalClient.Execute(request);
            var result = response.Result<Order>();

            var approvalUrl = result.Links.FirstOrDefault(link => link.Rel.Equals("approve")).Href;
            return approvalUrl;
        }

        public async Task<IActionResult> ExecutePayment(string token)
        {
            try
            {
                var request = new OrdersCaptureRequest(token);
                request.RequestBody(new OrderActionRequest());

                var response = await _payPalClient.Execute(request);
                var result = response.Result<Order>();

                if (result != null && result.Status == "COMPLETED")
                {
                    return new RedirectResult("https://localhost:7175/Users/Cart/BookedSuccess");
                }
                else
                {
                    return new RedirectResult("https://localhost:7175");
                }
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        public IActionResult CancelPayment(string token)
        {
            return new RedirectResult("https://localhost:7175");
        }
    }
}
