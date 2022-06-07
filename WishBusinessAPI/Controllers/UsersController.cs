﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WishBusinessAPI.Common;
using WishBusinessAPI.Models.Response;
using WishBusinessAPI.Repositories.UserRepository;

namespace WishBusinessAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        TranCodes tranCodes = TranCodes.Exception;
        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [Route("action")]
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            GetUserResponse response = new GetUserResponse();
            try
            {
                response = _userRepository.GetUser();
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.Message = ex.Message.ToString();
               // response.tranCodes = Convert.ToString((int)tranCodes.Exception);
            }

            return Ok(response);
        }
    }
}
