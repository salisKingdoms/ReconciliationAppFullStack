namespace ReconciliationApp.Data.dto
{
    public class RespTransaction
    {
        public bool is_ok { get; set; }
        public int TransactionID { get; set; }
        public string TellerID { get; set; }
        public int CustomerCode { get; set; }
        public string TransactionType { get; set; }
        public string Currency { get; set; }
        public decimal Denomination { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDateTime { get; set; }
        public string BankPIC { get; set; }
        public string message { get; set; }
    }
    public class RespDeleteTransaction
    {
        public bool is_ok { get; set; }
        public string message { get; set; }
    }
}
