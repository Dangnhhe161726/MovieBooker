namespace MovieBooker_backend.Helpers
{
    using PayPalCheckoutSdk.Core;
    public class PayPalClient
    {
        public static PayPalEnvironment Environment(IConfiguration configuration)
        {
            return new SandboxEnvironment(
                configuration["PayPal:ClientId"],
                configuration["PayPal:ClientSecret"]
            );
        }

        public static PayPalHttpClient Client(IConfiguration configuration)
        {
            return new PayPalHttpClient(Environment(configuration));
        }
    }
}
