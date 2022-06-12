using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WishBusinessAPI.Models.Requests;
using WishBusinessAPI.Models.Response;
using WishBusinessAPI.Repositories.Recharge;

namespace WishBusinessAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RechargeController : ControllerBase
    {
        private readonly IRechargeRepository _rechargeRepository;

        public RechargeController(IRechargeRepository rechargeRepository)
        {
           _rechargeRepository = rechargeRepository;
        }


        //GetRechargeDetails
        [Route("GetRechargeDetails")]
        [HttpGet]
        public async Task<IActionResult> GetRechargeDetails()
        {
            RechargeResponse response = new RechargeResponse();
            try
            {
                response = _rechargeRepository.GetRechargeDetails();            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.Message = ex.Message.ToString();
               // response.tranCodes = tranCodes;
                // response.tranCodes = Convert.ToString((int)tranCodes.Exception);
            }

            return Ok(response);
        }


        [HttpPost]
        [Route("AddRechargeDetails")]
        public async Task<IActionResult> AddRechargeDetails([FromBody] RechargeRequest request)
        {
            RechargeResponse res = new RechargeResponse();
            try
            {
                res = _rechargeRepository.AddRechargeDetails(request);
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
