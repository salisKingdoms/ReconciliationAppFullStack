namespace ReconciliationApp.Data.dto
{
    public class ParamSearchReconsile
    {
        public string tellerID { get; set; }
        public DateTime? trxDateStart { get; set; }
        public DateTime? trxDateEnd { get; set; }
        public string sortBy { get; set; }
        public string orderBy { get; set; }
        public int size { get; set; }
        public int page { get; set; }
    }
}
