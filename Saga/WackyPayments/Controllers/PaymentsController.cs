using Microsoft.AspNetCore.Mvc;
using WackyPayments.Models;

namespace WackyPayments.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly ILogger<PaymentsController> _logger;

        public PaymentsController(ILogger<PaymentsController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "CapturePayment")]
        public ActionResult<PaymentResponse> CapturePayment([FromBody] PaymentRequest payment)
        {
            PaymentResponse response = new(payment.Id.ToString(), payment.TotalAmount);
            int num = new Random().Next(2);

            if (num <= 1)
            {
                response.PaymentStatus = PaymentStatus.Failed;
                Console.WriteLine($"Order id:{response.OrderId} for {response.Amount} money was a failure :-(");

                return StatusCode(StatusCodes.Status402PaymentRequired, response);
            }
            response.PaymentStatus = PaymentStatus.Success;
            Console.WriteLine($"Order id:{response.OrderId} for {response.Amount} money was successful!");
            response.AuthNumber = response.GetNumber();

            return Ok(response);

        }
    }
}
