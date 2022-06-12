using WishBusinessAPI.Models.Requests;
using WishBusinessAPI.Models.Response;

namespace WishBusinessAPI.Repositories.Payment
{
    public interface IPaymentRepository
    {
        public GetPaymentResponse AddPaymentMethod(AddPaymentRequest request);
    }
}