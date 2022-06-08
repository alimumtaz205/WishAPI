using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WishBusinessAPI.Common;
using WishBusinessAPI.Models.Response;
using WishBusinessAPI.Repositories.Finance;

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
    }
}
