namespace ReconciliationApp.Models
{
    public class dt_daily_balances
    {
        public DateTime DateTransaction { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal ClosingBalance { get; set; }
    }
}
