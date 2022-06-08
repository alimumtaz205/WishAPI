using WishBusinessAPI.Models;
using WishBusinessAPI.Models.Response;

namespace WishBusinessAPI.Repositories.Account
{
    public interface IAccountRepository
    {
        public GetUserResponse Login(LoginDTO login);
    }
}