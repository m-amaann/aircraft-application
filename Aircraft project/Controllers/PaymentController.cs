using Microsoft.AspNetCore.Mvc;
using Stripe;

public class PaymentController : Controller
{
    private readonly string _stripeSecretKey = "your-secret-key"; // Replace with your actual Stripe secret key

    public ActionResult Index()
    {
        ViewBag.StripePublicKey = "your-public-key"; // Replace with your actual Stripe public key
        return View();
    }

    [HttpPost]
    public JsonResult CreatePaymentIntent()
    {
        StripeConfiguration.ApiKey = _stripeSecretKey;

        var options = new PaymentIntentCreateOptions
        {
            Amount = 1000, // amount in cents
            Currency = "usd",
        };

        var service = new PaymentIntentService();
        var paymentIntent = service.Create(options);

        return Json(new { clientSecret = paymentIntent.ClientSecret });
    }
}
