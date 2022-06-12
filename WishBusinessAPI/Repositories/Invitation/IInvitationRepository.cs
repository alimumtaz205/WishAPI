using WishBusinessAPI.Models.Requests;
using WishBusinessAPI.Models.Response;

namespace WishBusinessAPI.Repositories.Invitation
{
    public interface IInvitationRepository
    {
        public InvitationResponse AddInvitation(InvitationRequest request);
    }
}