namespace ReconciliationApp.Data.dto
{
    public class ParamSaveUpdateCustomer
    {
        public int code { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public int custType { get; set; }
    }

    public class ParamSearchCustomer
    {
        public int size { get; set; }
        public int page { get; set; }
        public string sortBy { get; set; }
        public string orderBy { get; set; }
        public string name { get; set; }
        public int code { get; set; }
    }
}
