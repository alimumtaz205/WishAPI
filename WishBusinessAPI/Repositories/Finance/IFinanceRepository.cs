using WishBusinessAPI.Models.Response;

namespace WishBusinessAPI.Repositories.Finance
{
    public interface IFinanceRepository
    {
        public FinanceResponse GetFinance();
    }
}