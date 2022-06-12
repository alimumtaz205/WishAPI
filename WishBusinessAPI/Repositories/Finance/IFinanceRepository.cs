using WishBusinessAPI.Models;
using WishBusinessAPI.Models.Response;

namespace WishBusinessAPI
{
    public interface IFinanceRepository
    {
        public FinanceResponse GetFinance();

        public FinanceResponse AddFinance(Finance request);
    }
}