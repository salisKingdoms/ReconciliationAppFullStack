namespace ReconciliationApp.Data.dto
{
    public class ParamSaveUpdateTransaction
    {
        public int transID { get; set; }
        public string tellerID { get; set; }
        public int customerCode { get; set; }
        public string transType { get; set; }
        public string currency { get; set; }
        public decimal denomination { get; set; }
        public decimal amount { get; set; }
        public DateTime transDate { get; set; }
        public string pic { get; set; }
    }

    public class ParamSearchTransaction
    {
        public int size { get; set; }
        public int page { get; set; }
        public string sortBy { get; set; }
        public string orderBy { get; set; }
        public int custCode { get; set; }
        public int trxID { get; set; }
        public string trxType { get; set; }
        public string currency { get; set; }
        public decimal amount { get; set; }
        public DateTime? trxDateStart { get; set; }
        public DateTime? trxDateEnd { get; set; }
        public string pic { get; set; }
    }
}
