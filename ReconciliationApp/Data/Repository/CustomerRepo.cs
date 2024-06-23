using ReconciliationApp.Data.DataAccess;
using ReconciliationApp.Data.dto;
using ReconciliationApp.Models;
using ReconciliationApp.Helper;

namespace ReconciliationApp.Data.Repository
{
    public class CustomerRepo :ICustomerRepo
    {
        private readonly IsqlDataAccess _dataAccess;
        private readonly IConfiguration _configuration;

        public CustomerRepo(IsqlDataAccess db, IConfiguration configuration)
        {
            _dataAccess = db;
            _configuration = configuration;
        }

        public int GetExistCustomers(ParamSaveUpdateCustomer filter)
        {
            var data = GetExistCustomer(filter).Result.FirstOrDefault();
            return data;
        }

        private async Task<IEnumerable<int>> GetExistCustomer(ParamSaveUpdateCustomer filter)
        {
            List<int> customerIds = new List<int>();
            try
            {
                var param = new Dictionary<string, object> {
                    {"names", filter.name},
                    { "mail", filter.email },
                    { "phones", filter.phone},
                    { "address", filter.address },
                    };
                string query = @"select 1 from dt_customer where lower (CustomerName)=lower(@names) and lower(email)=lower(@mail) and phone=@phones and lower(CustomerAddress) =lower(@address) ";

                return await _dataAccess.GetData<int, dynamic>(query, param, true);
            }
            catch (Exception ex)
            {

            }
            return customerIds;
        }

        public async Task<RespMessageDB> SaveorUpdateCustomer(ParamSaveUpdateCustomer data)
        {
            RespMessageDB result = new RespMessageDB();
            try
            {
                string query = string.Empty;
                string empID = string.Empty;

                var param = new Dictionary<string, object> {

                    {"names",data.name },
                    { "types", 1 },
                    { "mail", data.email },
                    { "phones", data.phone},
                    { "address", data.address },
                    { "code" ,data.code }
                    };
                if (data.code == 0)
                {
                    //save

                    query = @"EXEC create_customer @customerName=@names,@customerType=@types,@email=@mail,@phone=@phones,@address=@address;";
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
                    query = @"EXEC update_customer  @customerCode =@code,@customerName=@names,@customerType=@types,@email=@mail,@phone=@phones,@address=@address;";
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

        public async Task<bool> DeleteCustomer(int codeCust)
        {
            bool result = false;
            try
            {
                string query = string.Empty;
                var param = new Dictionary<string, object> {
                    {"code", codeCust }
                    };
                query = @"EXEC delete_customer @customerCode=@code";
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

        private async Task<IEnumerable<dt_customer>> GetCustomerById(int codeCust)
        {
            List<dt_customer> result = new List<dt_customer>();
            try
            {
                string query = string.Empty;
                var param = new Dictionary<string, object> {
                    {"code", codeCust }
                    };
                query = @"EXEC get_detail_customer @customerCode=@code";

                return await _dataAccess.GetData<dt_customer, dynamic>(query, param, true); ;

            }
            catch (Exception ex)
            {
                result = null;
                string message = ex.Message;
            }
            return result;
        }

        public dt_customer GetCustomerByCode(int customerCode)
        {
            dt_customer result = new dt_customer();
            try
            {
                result = GetCustomerById(customerCode).Result.FirstOrDefault();

            }
            catch (Exception ex)
            {
                result = null;
                string message = ex.Message;
            }
            return result;
        }

        private async Task<IEnumerable<dt_customer>> GetListCustomers(ParamSearchCustomer filter)
        {
            List<dt_customer> result = new List<dt_customer>();
            try
            {
                string query = string.Empty;
                var param = new Dictionary<string, object> {
                    {"code", filter.code },
                    {"names", filter.name}
                    };
                query = @"select * from dt_customer";
                string orderby = (string.IsNullOrEmpty(filter.orderBy) ? " order by CustomerCode asc" : (" order by " + filter.orderBy + " " + filter.sortBy));
                if (!string.IsNullOrEmpty(filter.name))
                {
                    query += " where lower(CustomerName) = @names";
                }
                if (filter.code > 0)
                {
                    query += " where CustomerCode=@code";
                }

                query += orderby;
                return await _dataAccess.GetData<dt_customer, dynamic>(query, param, true); ;

            }
            catch (Exception ex)
            {
                result = null;
                string message = ex.Message;
            }
            return result;
        }

        public List<dt_customer> GetListCustomer(ParamSearchCustomer filter)
        {
            var data = GetListCustomers(filter).Result.ToList();
            return data;
        }
    }
}
