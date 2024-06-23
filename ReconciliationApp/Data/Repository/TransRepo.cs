using ReconciliationApp.Data.DataAccess;
using ReconciliationApp.Data.dto;
using ReconciliationApp.Helper;
using ReconciliationApp.Models;

namespace ReconciliationApp.Data.Repository
{
    public class TransRepo : ITransRepo
    {
        private readonly IsqlDataAccess _dataAccess;
        private readonly IConfiguration _configuration;

        public TransRepo(IsqlDataAccess db, IConfiguration configuration)
        {
            _dataAccess = db;
            _configuration = configuration;
        }
        private async Task<IEnumerable<msCustomer>> GetCustomers()
        {
            List<msCustomer> result = new List<msCustomer>();
            try
            {
                string query = string.Empty;
                var param = new Dictionary<string, object>
                {

                };
                query = @"select CustomerCode,CustomerName from dt_customer";
                string orderby = " order by CustomerCode asc";
                query += orderby;
                return await _dataAccess.GetData<msCustomer, dynamic>(query, param, true); ;

            }
            catch (Exception ex)
            {
                result = null;
                string message = ex.Message;
            }
            return result;
        }

        public List<msCustomer> GetAllCustomerCode()
        {
            var data = GetCustomers().Result.ToList();
            return data;
        }

        public async Task<RespMessageDB> SaveorUpdateTransaction(ParamSaveUpdateTransaction data)
        {
            RespMessageDB result = new RespMessageDB();
            try
            {
                string query = string.Empty;

                var param = new Dictionary<string, object> {

                    {"trxID",data.transID },
                    { "teller", data.tellerID },
                    { "custCode", data.customerCode },
                    { "type", data.transType},
                    { "currencies", data.currency },
                    { "denom" ,data.denomination },
                    { "amounts" ,data.amount },
                    { "trDate" ,data.transDate },
                    { "picBank" ,data.pic }
                    };
                if (data.transID == 0)
                {
                    //save
                    query = @"EXEC create_Transaction @tellerID=@teller,@customerCode=@custCode,@transactionType=@type,@currency=@currencies,
                                @denomination=@denom,@amount=@amounts,@transDate=@trDate,@pic=@picBank;";
                    var submit = await _dataAccess.SaveAndGetData<string, dynamic>(query, param, true);
                    if (submit.Contains("submited"))
                    {
                        result.message = submit;
                        result.error = "";
                    }
                    else
                    {
                        result.error = submit;
                        result.message = "";
                    }
                }
                else
                {
                    //update
                    query = @"EXEC update_Transaction @TransactionID=@trxID,@customerCode=@custCode,@transactionType=@type,@currency=@currencies,
                                        @denomination=@denom,@amount=@amounts,@transDate=@trDate,@pic=@picBank;";
                    var submit = await _dataAccess.SaveAndGetData<string, dynamic>(query, param, true);
                    if (submit.Contains("success"))
                    {
                        result.message = submit;
                        result.error = "";
                    }
                    else
                    {
                        result.error = submit;
                        result.message = "";
                    }
                }

            }
            catch (Exception ex)
            {
                //result = false;
                string message = ex.Message;
                result.message = message;
            }
            return result;
        }

        public async Task<bool> DeleteTransaction(int transID)
        {
            bool result = false;
            try
            {
                string query = string.Empty;
                var param = new Dictionary<string, object> {
                    {"id", transID }
                    };
                query = @"EXEC delete_Transaction @TransactionID=@id ;";
                await _dataAccess.SaveData(query, param);
                result = true;

            }
            catch (Exception ex)
            {
                result = false;
                string message = ex.Message;
            }
            return result;
        }

        private async Task<IEnumerable<dt_transactions>> GetTransactionById(int transID)
        {
            List<dt_transactions> result = new List<dt_transactions>();
            try
            {
                string query = string.Empty;
                var param = new Dictionary<string, object> {
                    {"id", transID }
                    };
                query = @"EXEC get_detail_Transaction @TransactionID=@id ;";

                return await _dataAccess.GetData<dt_transactions, dynamic>(query, param, true); ;

            }
            catch (Exception ex)
            {
                result = null;
                string message = ex.Message;
            }
            return result;
        }

        public dt_transactions GetTransactionDetail(int transID)
        {
            dt_transactions result = new dt_transactions();
            try
            {
                result = GetTransactionById(transID).Result.FirstOrDefault();

            }
            catch (Exception ex)
            {
                result = null;
                string message = ex.Message;
            }
            return result;
        }

        private async Task<IEnumerable<dt_transactions>> GetListTransactions(ParamSearchTransaction filter)
        {
            List<dt_transactions> result = new List<dt_transactions>();
            try
            {
                string query = string.Empty;
                var param = new Dictionary<string, object> {

                    {"trxID",filter.trxID },
                    { "custCode", filter.custCode },
                    { "type", filter.trxType },
                    { "currencies", filter.currency},
                    { "amounts", filter.amount },
                    { "trDateStart" ,filter.trxDateStart },
                    { "trDateEnd" ,filter.trxDateEnd != null ?filter.trxDateEnd.Value.AddHours(23) : filter.trxDateEnd},
                    { "picBank" ,filter.pic }

                    };

                query = @"select * from dt_transactions";
                string filters = string.Empty;
                string orderby = (string.IsNullOrEmpty(filter.orderBy) ? " order by TransactionID asc" : (" order by " + filter.orderBy + " " + filter.sortBy));
                List<string> queryFilter = new List<string>();
                if (filter.trxID > 0)
                {
                    queryFilter.Add(" TransactionID = @trxID");
                }
                if (filter.custCode > 0)
                {
                    queryFilter.Add("  CustomerCode=@custCode");

                }
                if (!string.IsNullOrEmpty(filter.trxType) && filter.trxType != "0")
                {
                    queryFilter.Add("  TransactionType=@type");

                }
                if (!string.IsNullOrEmpty(filter.currency) && filter.currency != "0")
                {
                    queryFilter.Add("  Currency=@currencies");

                }
                if (filter.amount > 0)
                {
                    queryFilter.Add("  Amount=@amounts");

                }
                if (filter.trxDateStart != null && filter.trxDateEnd != null)
                {
                    queryFilter.Add("  TransactionDateTime BETWEEN @trDateStart AND @trDateEnd");
                }
                if (!string.IsNullOrEmpty(filter.pic) && filter.pic != "0")
                {
                    queryFilter.Add("  BankPIC=@picBank");

                }

                for (var i = 0; i < queryFilter.Count; i++)
                {
                    if (i == 0) filters += " where ";
                    if (!string.IsNullOrEmpty(queryFilter[i]))
                    {
                        filters += queryFilter[i];

                    }
                    if (i >= 0 && i < queryFilter.Count - 1)
                        filters += " and ";
                }

                query += filters + orderby;
                return await _dataAccess.GetData<dt_transactions, dynamic>(query, param, true); ;

            }
            catch (Exception ex)
            {
                result = null;
                string message = ex.Message;
            }
            return result;
        }

        public List<dt_transactions> GetListTransaction(ParamSearchTransaction filter)
        {
            var data = GetListTransactions(filter).Result.ToList();
            return data;
        }

        public async Task<bool> SetOpeningBalance()
        {
            bool result = false;
            try
            {
                string query = string.Empty;
                var param = new Dictionary<string, object>
                {

                };
                query = @"EXEC set_opening_balance ;";
                await _dataAccess.SaveData(query, param);
                result = true;

            }
            catch (Exception ex)
            {
                result = false;
                string message = ex.Message;
            }
            return result;
        }

        public async Task<bool> SetClosingBalance()
        {
            bool result = false;
            try
            {
                string query = string.Empty;
                var param = new Dictionary<string, object>
                {

                };
                query = @"EXEC set_closing_balance ;";
                await _dataAccess.SaveData(query, param);
                result = true;

            }
            catch (Exception ex)
            {
                result = false;
                string message = ex.Message;
            }
            return result;
        }
        private async Task<IEnumerable<dt_daily_balances>> GetClosingBalance()
        {
            List<dt_daily_balances> result = new List<dt_daily_balances>();
            try
            {
                string queryDataClose = string.Empty;
                var param = new Dictionary<string, object>
                {

                };
                queryDataClose = @"SELECT * FROM dt_daily_balances";
                return await _dataAccess.GetData<dt_daily_balances, dynamic>(queryDataClose, param, true);

            }
            catch (Exception ex)
            {
                result = null;
                string message = ex.Message;
            }
            return result;
        }

        public List<dt_daily_balances> ClosingBalance()
        {
            var data = GetClosingBalance().Result.ToList();
            return data;
        }

        private async Task<IEnumerable<RespReconcile>> GetListReconcile(ParamSearchReconsile filter)
        {
            List<RespReconcile> result = new List<RespReconcile>();
            try
            {
                string query = string.Empty;
                var param = new Dictionary<string, object> {

                    { "Teller", filter.tellerID },
                    { "Start" ,filter.trxDateStart },
                    { "End" ,filter.trxDateEnd != null ?filter.trxDateEnd.Value.AddHours(23) : filter.trxDateEnd},

                    };

                query = @"select * from daily_reconciliation";
                string filters = string.Empty;
                string orderby = (string.IsNullOrEmpty(filter.orderBy) ? " order by TellerID asc" : (" order by " + filter.orderBy + " " + filter.sortBy));
                List<string> queryFilter = new List<string>();
                if (!string.IsNullOrEmpty(filter.tellerID) && filter.tellerID != "0")
                {
                    queryFilter.Add("  TellerID=@Teller");

                }
                if (filter.trxDateStart != null && filter.trxDateEnd != null)
                {
                    queryFilter.Add("  TransactionDate BETWEEN @Start AND @End");
                }


                for (var i = 0; i < queryFilter.Count; i++)
                {
                    if (i == 0) filters += " where ";
                    if (!string.IsNullOrEmpty(queryFilter[i]))
                    {
                        filters += queryFilter[i];

                    }
                    if (i >= 0 && i < queryFilter.Count - 1)
                        filters += " and ";
                }

                query += filters + orderby;
                return await _dataAccess.GetData<RespReconcile, dynamic>(query, param, true); ;

            }
            catch (Exception ex)
            {
                result = null;
                string message = ex.Message;
            }
            return result;
        }

        public List<RespReconcile> GetListReconsiliation(ParamSearchReconsile filter)
        {
            var data = GetListReconcile(filter).Result.ToList();
            return data;
        }
    }
}
