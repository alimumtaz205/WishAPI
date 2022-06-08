using WishBusinessAPI.Models;
using WishBusinessAPI.Models.Response;

namespace WishBusinessAPI.Repositories.UserRepository
{
    public interface IUserRepository
    {
        public GetUserResponse GetUser();
        public GetUserResponse RegisterUser(User user);
    }
}