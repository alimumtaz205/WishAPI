using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WishBusinessAPI.Models;
using WishBusinessAPI.Models.Requests;
using WishBusinessAPI.Models.Response;
using WishBusinessAPI.Repositories.Payment;

namespace WishBusinessAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentMethodController : ControllerBase
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentMethodController(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        [HttpPost]
        [Route("AddPaymentMethod")]
        public async Task<IActionResult> AddPaymentMethod([FromBody]  AddPaymentRequest payment)
        {
            GetPaymentResponse res = new GetPaymentResponse();
            try
            {
                res = _paymentRepository.AddPaymentMethod(payment);
            }
            catch (Exception ex)
            {
                res.isSuccess = false;
                res.Message = ex.Message.ToString();
                //res.tranCodes = tranCodes;
            }
            return Ok(res);
        }
    }
}
