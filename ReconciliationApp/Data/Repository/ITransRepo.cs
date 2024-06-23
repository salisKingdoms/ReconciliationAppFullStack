using ReconciliationApp.Data.dto;
using ReconciliationApp.Helper;
using ReconciliationApp.Models;

namespace ReconciliationApp.Data.Repository
{
    public interface ITransRepo
    {
        public List<msCustomer> GetAllCustomerCode();
        public Task<RespMessageDB> SaveorUpdateTransaction(ParamSaveUpdateTransaction data);
        public Task<bool> DeleteTransaction(int transID);
        public dt_transactions GetTransactionDetail(int transID);
        public List<dt_transactions> GetListTransaction(ParamSearchTransaction filter);
        public Task<bool> SetOpeningBalance();
        public Task<bool> SetClosingBalance();
        public List<dt_daily_balances> ClosingBalance();
        public List<RespReconcile> GetListReconsiliation(ParamSearchReconsile filter);
    }
}
