using WishBusinessAPI.Models;

namespace JWTWebAuthentication.Repository
{
    public interface IJWTManagerRepository
    {
        Tokens Authenticate(LoginDTO users);
    }

}