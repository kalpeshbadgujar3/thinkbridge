using Helper;
using MasterInterface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Managers
{
    /// <summary>
    /// Database manager is a middle layer between feature specific manager and DB 
    /// Contains all generic methods so can be reused in the application for various implemenations, eliminates duplicate code
    /// </summary>
    public class DatabaseManager : IDatabaseManager
    {
        /// <summary>
        /// Get connection string from config
        /// </summary>
        /// <returns></returns>
        private string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings[Constants.DatabaseName].ConnectionString;
        }

        /// <summary>
        /// Get all records from DB
        /// This is the generic method responsible to get all records based on storedprocedure name
        /// Result is retrieved in json format for better performance
        /// </summary>
        /// <param name="nameOfStoredProcedure"></param>
        /// <returns></returns>
        public async Task<string> GetAllRecords(StoredProcedure nameOfStoredProcedure)
        {
            using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString())) 
            {
                SqlCommand command = new SqlCommand();

                command.CommandText = nameOfStoredProcedure.ToString();
                command.Connection = sqlConnection;
                command.CommandType = CommandType.StoredProcedure;
                await sqlConnection.OpenAsync().ConfigureAwait(false);

                object jsonResult = await command.ExecuteScalarAsync().ConfigureAwait(false);
                sqlConnection.Close();
                return Convert.ToString(jsonResult);
            }
        }

        /// <summary>
        /// Add record to the DB
        /// </summary>
        /// <param name="nameOfStoredProcedure"></param>
        /// <param name="storedProcedureObject"></param>
        /// <returns></returns>
        public async Task<int> AddRecord(StoredProcedure nameOfStoredProcedure, IStoredProcedure storedProcedureObject)
        {
            using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
            {
                SqlCommand command = new SqlCommand();

                command.CommandText = nameOfStoredProcedure.ToString();
                command.Connection = sqlConnection;
                command.CommandType = CommandType.StoredProcedure;
                await sqlConnection.OpenAsync().ConfigureAwait(false);

                command = await GetSqlParameterCollection(storedProcedureObject, command);

                object jsonResult;
                try
                {
                    jsonResult = await command.ExecuteScalarAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                sqlConnection.Close();

                int itemId = Convert.ToInt32(jsonResult);

                return itemId;
            }
        }

        /// <summary>
        /// Construct Params dynamic from stored procedure object
        /// </summary>
        /// <param name="StoredProcedureObject"></param>
        /// <param name="sqlCommand"></param>
        /// <returns></returns>
        private async Task<SqlCommand> GetSqlParameterCollection(IStoredProcedure StoredProcedureObject,
            SqlCommand sqlCommand)
        {
            foreach (var prop in StoredProcedureObject.GetType().GetProperties())
            {
                var propertyValue = prop.GetValue(StoredProcedureObject, null);

                if (IsNullable(prop.PropertyType) == false || propertyValue != null)
                {
                    sqlCommand.Parameters.Add(new SqlParameter(prop.Name, propertyValue));
                }
                else
                {
                    sqlCommand.Parameters.Add(new SqlParameter(prop.Name, DBNull.Value));
                }
            }

            return sqlCommand;
        }

        /// <summary>
        /// Check if param is nullable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool IsNullable<T>(T value)
        {
            return Nullable.GetUnderlyingType(typeof(T)) != null;
        }
    }
}
