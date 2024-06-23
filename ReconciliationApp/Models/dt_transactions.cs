namespace ReconciliationApp.Models
{
    public class dt_transactions
    {
        public int TransactionID { get; set; }
        public string TellerID { get; set; }
        public int CustomerCode { get; set; }
        public string TransactionType { get; set; }
        public string Currency { get; set; }
        public decimal Denomination { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDateTime { get; set; }
        public string BankPIC { get; set; }
    }
}
