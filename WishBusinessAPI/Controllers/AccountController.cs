using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WishBusinessAPI.Models;
using WishBusinessAPI.Models.Response;
using WishBusinessAPI.Repositories.Account;

namespace WishBusinessAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly IAccountRepository _IaccountRepository;
        public AccountController(IAccountRepository accountRepository)
        {
            _IaccountRepository = accountRepository;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> LoginUser([FromBody] LoginDTO login)
        {
            GetUserResponse response = new GetUserResponse();
            try
            {
                response = _IaccountRepository.Login(login);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message.ToString();
                //response.tranCode = Common.TransactionCodes.LOGIN_USER_ERROR;
            }
            return Ok(response);
        }
    }
}
