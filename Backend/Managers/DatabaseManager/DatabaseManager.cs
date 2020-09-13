using Helper;
using MasterInterface;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Managers
{
    public class DatabaseManager : IDatabaseManager
    {
        SqlConnection sqlConnection;

        public DatabaseManager()
        {
            sqlConnection = new SqlConnection(GetConnectionString()); 
        }


        /// <summary>
        /// Get connection string from config
        /// </summary>
        /// <returns></returns>
        private string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings[Constants.DatabaseName].ConnectionString;
        }

        
        public async Task<string> GetAllItems(StoredProcedure nameOfStoredProcedure)
        {
            using (SqlCommand command = new SqlCommand()) 
            {
                command.CommandText = nameOfStoredProcedure.ToString();
                command.Connection = sqlConnection;
                command.CommandType = CommandType.StoredProcedure;
                await sqlConnection.OpenAsync().ConfigureAwait(false);

                object jsonResult = await command.ExecuteScalarAsync().ConfigureAwait(false);
                sqlConnection.Close();
                return Convert.ToString(jsonResult);
            }
        }
    }
}
