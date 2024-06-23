using ReconciliationApp.Models;

namespace ReconciliationApp.Data.dto
{
    public class RespSaveTransaction
    {
        public string messageUI { get; set; }
        public string messageConsole { get; set; }
        public string messageUIError { get; set; }
        public bool is_ok { get; set; }
    }
    public class RespTransactionList
    {
        public bool is_ok { get; set; }
        public string message { get; set; }
        public List<dt_transactions> data { get; set; }
        public int totalRow { get; set; }
    }
}
