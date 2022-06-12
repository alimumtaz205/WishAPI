namespace WishBusinessAPI.Models
{
    public class Recharge
    {
        public int rechargeId { get; set; }
        public string USTDTopup { get; set; }
        public string receiverAddress { get; set; }
        public string tranId { get; set; }
        public string paymentSS { get; set; }
    }
}
