using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using Dapper;

namespace ReconciliationApp.Data.DataAccess
{
    public class sqlDataAccess : IsqlDataAccess
    {
        private readonly IConfiguration _configuration;
        string connectionId = "ConnectionMUFG";

        public sqlDataAccess(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<T>> GetData<T, P>(string spName, P parameters, bool isAssesmentServer)
        {
            try
            {
                string connectionString = _configuration.GetConnectionString(connectionId);
                using (IDbConnection dbConnection = new SqlConnection(connectionString))
                {
                    return await dbConnection.QueryAsync<T>(spName, parameters, commandType: CommandType.Text);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<bool> SaveData<T>(string spName, T parameters)
        {
            try
            {
                using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString(connectionId));
                await connection.ExecuteAsync(spName, parameters, commandType: CommandType.Text);
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public async Task<T> SaveAndGetData<T, P>(string spName, P parameters, bool isAssesmentServer)
        {
            try
            {
                string connectionString = _configuration.GetConnectionString(connectionId);
                using (IDbConnection dbConnection = new SqlConnection(connectionString))
                {
                    return await dbConnection.ExecuteScalarAsync<T>(spName, parameters, commandType: CommandType.Text);

                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<bool> Delete<T>(string spName, T parameters)
        {
            try
            {
                using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString(connectionId));
                await connection.ExecuteScalarAsync(spName, parameters, commandType: CommandType.Text);
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }


        public Task<DataTable> ExecuteDataTable(string spName, Dictionary<string, object> parameterValues)
        {
            if (string.IsNullOrEmpty(connectionId))
            {
                throw new ArgumentNullException("connectionString");
            }

            if (string.IsNullOrEmpty(spName))
            {
                throw new ArgumentNullException("spName");
            }

            return ExecuteDataTableAsync(connectionId, CommandType.Text, spName, parameterValues);
        }
        public async Task<DataTable> ExecuteDataTableAsync(string connectionString, CommandType commandType, string commandText, Dictionary<string, object> commandParameters)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString(connectionString)))
            {
                await connection.OpenAsync().ConfigureAwait(continueOnCapturedContext: false);
                DataSet DS = await ExecuteDatasetAsync(connection, commandType, commandText, commandParameters).ConfigureAwait(continueOnCapturedContext: false);
                if (DS.Tables[0] != null)
                {
                    return DS.Tables[0];
                }
            }

            return null;
        }

        public async Task<DataSet> ExecuteDatasetAsync(SqlConnection connection, CommandType commandType, string commandText, Dictionary<string, object> commandParameters)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            SqlCommand cmd = new SqlCommand();
            bool mustCloseConnection = await PrepareCommandAsync(cmd, connection, null, commandType, commandText, commandParameters).ConfigureAwait(continueOnCapturedContext: false);
            using SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            cmd.Parameters.Clear();
            if (mustCloseConnection)
            {
                connection.Close();
            }

            return ds;
        }

        private async Task<bool> PrepareCommandAsync(SqlCommand command, SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, Dictionary<string, object> commandParameters)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            if (string.IsNullOrEmpty(commandText))
            {
                throw new ArgumentNullException("commandText");
            }

            bool mustCloseConnection = false;
            if (connection.State != ConnectionState.Open)
            {
                mustCloseConnection = true;
                await connection.OpenAsync().ConfigureAwait(continueOnCapturedContext: false);
            }

            command.Connection = connection;
            command.CommandText = commandText;
            if (transaction != null)
            {
                if (transaction.Connection == null)
                {
                    throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
                }

                command.Transaction = transaction;
            }

            command.CommandType = commandType;
            if (commandParameters != null)
            {
                AttachParameters(command, commandParameters);
            }

            return mustCloseConnection;
        }

        private void AttachParameters(SqlCommand command, Dictionary<string, object> param)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            if (param == null || param.Count <= 0)
            {
                return;
            }

            foreach (string key in param.Keys)
            {
                command.Parameters.AddWithValue(key, param[key]);
            }
        }
    }
}
