using WishBusinessAPI.Models.Requests;
using WishBusinessAPI.Models.Response;

namespace WishBusinessAPI.Repositories.Recharge
{
    public interface IRechargeRepository
    {
        public RechargeResponse GetRechargeDetails();
        public RechargeResponse AddRechargeDetails(RechargeRequest request);
    }
}