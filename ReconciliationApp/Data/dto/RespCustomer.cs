namespace ReconciliationApp.Data.dto
{
    public class RespCustomer
    {
        public bool is_ok { get; set; }
        public int CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public int CustomerType { get; set; }
        public string CustomerAddress { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string message { get; set; }
    }
    public class RespDeleteCustomer
    {
        public bool is_ok { get; set; }
        public string message { get; set; }
    }

    public class msCustomer
    {
        public int CustomerCode { get; set; }
        public string CustomerName { get; set; }

    }
}
