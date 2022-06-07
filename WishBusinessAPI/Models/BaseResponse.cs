using WishBusinessAPI.Common;

namespace WishBusinessAPI.Models
{
    public class BaseResponse
    {
        public TranCodes tranCodes { get; set; }
        public bool isSuccess { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
