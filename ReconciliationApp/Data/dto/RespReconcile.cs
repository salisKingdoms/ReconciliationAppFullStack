using ReconciliationApp.Models;

namespace ReconciliationApp.Data.dto
{
    public class RespReconcile
    {
        public string TellerID { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal TotalDeposits { get; set; }
        public decimal TotalWithdrawals { get; set; }
    }
    public class RespReconcileList
    {
        public bool is_ok { get; set; }
        public string message { get; set; }
        public List<RespReconcile> data { get; set; }
        public int totalRow { get; set; }
    }
    public class RespReconcilenList
    {
        public bool is_ok { get; set; }
        public string message { get; set; }
        public List<dt_daily_balances> data { get; set; }
        public int totalRow { get; set; }
    }
}
