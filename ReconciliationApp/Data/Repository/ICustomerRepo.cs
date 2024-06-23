using ReconciliationApp.Data.dto;
using ReconciliationApp.Helper;
using ReconciliationApp.Models;

namespace ReconciliationApp.Data.Repository
{
    public interface ICustomerRepo
    {
        public Task<RespMessageDB> SaveorUpdateCustomer(ParamSaveUpdateCustomer data);
        public Task<bool> DeleteCustomer(int codeCust);

        public dt_customer GetCustomerByCode(int customerCode);

        public List<dt_customer> GetListCustomer(ParamSearchCustomer filter);
    }
}
