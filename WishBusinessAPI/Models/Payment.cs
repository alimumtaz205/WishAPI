namespace WishBusinessAPI.Models
{
    public class Payment
    {
        public string real_Name { get; set; }
        public string e_mail { get; set; }
        public string phone_no { get; set; }
        public string address { get; set; }
        public string withdrawl_type { get; set; }
        public string network { get; set; }
        public string USDT_address { get; set; }
        public string tran_pass { get; set; }
        public int user_id { get; set; }
        public int payment_id { get; set; }
    }
}
