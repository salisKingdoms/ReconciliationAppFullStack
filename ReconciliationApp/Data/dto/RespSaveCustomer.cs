using ReconciliationApp.Models;

namespace ReconciliationApp.Data.dto
{
    public class RespSaveCustomer
    {
        public string messageUI { get; set; }
        public string messageConsole { get; set; }
        public string messageUIError { get; set; }
        public bool is_ok { get; set; }
    }
    public class RespCustomerList
    {
        public bool is_ok { get; set; }
        public string message { get; set; }
        public List<dt_customer> data { get; set; }
        public int totalRow { get; set; }
    }
}
