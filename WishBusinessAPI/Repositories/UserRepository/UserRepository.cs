using Microsoft.Extensions.Configuration;

namespace WishBusinessAPI.Repositories.UserRepository
{
    public class UserRepository
    {
        private readonly IConfiguration _configuration;

        bool isSuccess=false;
        string Message =string.Empty;

        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    }
}
