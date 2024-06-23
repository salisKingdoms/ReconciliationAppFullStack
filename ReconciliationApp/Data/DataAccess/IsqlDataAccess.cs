using System.Data;

namespace ReconciliationApp.Data.DataAccess
{
    public interface IsqlDataAccess
    {
        public Task<IEnumerable<T>> GetData<T, P>(string spName, P parameters, bool isAssesmentServer);
        public Task<bool> SaveData<T>(string spName, T parameters);
        public Task<DataTable> ExecuteDataTable(string spName, Dictionary<string, object> parameterValues);
        public Task<bool> Delete<T>(string spName, T parameters);
        public Task<T> SaveAndGetData<T, P>(string spName, P parameters, bool isAssesmentServer);
    }
}
