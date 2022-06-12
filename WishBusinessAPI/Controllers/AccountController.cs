using JWTWebAuthentication.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
        private readonly IJWTManagerRepository _iJWTManagerRepository;
        public AccountController(IAccountRepository accountRepository, IJWTManagerRepository jWTManagerRepository)
        {
            _IaccountRepository = accountRepository;
            _iJWTManagerRepository = jWTManagerRepository;
        }

        [AllowAnonymous]
        [Route("[action]")]
        [HttpPost]
        public IActionResult Authenticate(LoginDTO usersdata)
        {
            var token = _iJWTManagerRepository.Authenticate(usersdata);

            if (token == null)
            {
                return Unauthorized();
            }

            return Ok(token);
        }

        [AllowAnonymous]
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> LoginUser([FromBody] LoginDTO login)
        {
            GetUserResponse response = new GetUserResponse();
            try 
            {
                response = _IaccountRepository.Login(login);

                if (response.isSuccess == true)
                {
                    var token = _iJWTManagerRepository.Authenticate(login);

                    if (token == null)
                    {
                        return Unauthorized();
                    }
                    else
                    {
                        return Ok(token);
                    }
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.Message = ex.Message.ToString();
                //response.tranCodes = tranCodes;
            }
            return Ok(response);
        }
    }
}
