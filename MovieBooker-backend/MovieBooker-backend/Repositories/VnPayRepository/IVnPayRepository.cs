using MovieBooker_backend.Models;

namespace MovieBooker_backend.Repositories.VnPayRepository
{
    public interface IVnPayRepository
    {
        public string CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model);

        public VnPaymentResponseModel PaymentExecute(IQueryCollection collections);

    }
}
