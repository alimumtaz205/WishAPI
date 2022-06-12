namespace WishBusinessAPI.Models
{
    public class Finance
    {
        public int financeId { get; set; }
        public int userId { get; set; }
        public string availabaleBalance { get; set; }
        public string accountFunds { get; set; }
        public string frozenAmount { get; set; }
        public string financialIncome { get; set; }
    }
}
