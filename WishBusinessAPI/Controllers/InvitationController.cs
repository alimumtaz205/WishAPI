using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WishBusinessAPI.Models.Requests;
using WishBusinessAPI.Models.Response;
using WishBusinessAPI.Repositories.Invitation;

namespace WishBusinessAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvitationController : ControllerBase
    {
        private readonly IInvitationRepository _invitationRepositry;

        public InvitationController(IInvitationRepository invitationRepository)
        {
            _invitationRepositry = invitationRepository;
        }


        [HttpPost]
        [Route("AddPaymentMethod")]
        public async Task<IActionResult> AddInvitation([FromBody] InvitationRequest request)
        {
            InvitationResponse res = new InvitationResponse();
            try
            {
                res = _invitationRepositry.AddInvitation(request);
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
