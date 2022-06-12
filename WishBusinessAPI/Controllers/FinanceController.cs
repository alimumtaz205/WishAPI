using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WishBusinessAPI.Common;
using WishBusinessAPI.Models;
using WishBusinessAPI.Models.Response;

namespace WishBusinessAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinanceController : ControllerBase
    {
        private readonly IFinanceRepository _financeRepository;
        TranCodes tranCodes = TranCodes.Exception;
        public FinanceController(IFinanceRepository financeRepository)
        {
            _financeRepository = financeRepository;
        }

        [Route("GetFinance")]
        [HttpGet]
        public async Task<IActionResult> GetFinance()
        {
            FinanceResponse response = new FinanceResponse();
            try
            {
                response = _financeRepository.GetFinance();
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.Message = ex.Message.ToString();
                response.tranCodes = tranCodes;
                // response.tranCodes = Convert.ToString((int)tranCodes.Exception);
            }

            return Ok(response);
        }


        [Route("AddFinance")]
        [HttpPost]
        public async Task<IActionResult> AddFinance([FromBody] Finance request)
        {
            FinanceResponse res = new FinanceResponse();
            try
            {
                res = _financeRepository.AddFinance(request);
            }
            catch (Exception ex)
            {
                res.isSuccess = false;
                res.Message = ex.Message.ToString();
                res.tranCodes = tranCodes;
            }
            return Ok(res);
        }
    }
}
