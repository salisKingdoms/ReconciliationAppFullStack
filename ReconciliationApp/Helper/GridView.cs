namespace ReconciliationApp.Helper
{
    public class GridView
    {
        public string draw { get; set; }
        public int recordsFiltered { get; set; }
        public int recordsTotal { get; set; }
        public object data { get; set; }
        public string message { get; set; }
        public bool statusCode { get; set; }
        public int SizeData { get; set; }
    }

    public class GridConfigModel
    {
        //default
        public string sEcho { get; set; }
        public string sSearch { get; set; }
        public int iDisplayLength { get; set; }
        public int iDisplayStart { get; set; }
        public string iColumns { get; set; }
        public string iSortingCols { get; set; }
        public string sColumns { get; set; }
        public int iSortCol { get; set; }
        public string sSortDir { get; set; }

        //transaction filter
        public int custCode { get; set; }
        public int trxID { get; set; }
        public string trxType { get; set; }
        public string currency { get; set; }
        public decimal amount { get; set; }
        public DateTime? trxDateStart { get; set; }
        public DateTime? trxDateEnd { get; set; }
        public string pic { get; set; }
        //search reconcile
        public string tellerID { get; set; }

    }

    public class ParamGridSearch
    {
        public string orderBy { get; set; }
        public string sortBy { get; set; }
        public int page { get; set; }
        public int size { get; set; }
        public string search { get; set; }
    }

    public class RespMessageDB
    {
        public string message { get; set; }
        public string error { get; set; }
    }

    public class RespPost
    {
        public bool is_ok { get; set; }
        public string message { get; set; }
    }
}
