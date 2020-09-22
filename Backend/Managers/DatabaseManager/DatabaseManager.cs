using Helper;
using MasterInterface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Managers
{
    public class DatabaseManager : IDatabaseManager
    {
        public DatabaseManager()
        {
            
        }

        /// <summary>
        /// Get connection string from config
        /// </summary>
        /// <returns></returns>
        private string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings[Constants.DatabaseName].ConnectionString;
        }

        
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

        public async Task<bool> AddRecord(StoredProcedure nameOfStoredProcedure, IStoredProcedure storedProcedureObject)
        {
            using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
            {
                SqlCommand command = new SqlCommand();

                command.CommandText = nameOfStoredProcedure.ToString();
                command.Connection = sqlConnection;
                command.CommandType = CommandType.StoredProcedure;
                await sqlConnection.OpenAsync().ConfigureAwait(false);

                GetSqlParameterCollection(storedProcedureObject, ref command);

                object jsonResult = await command.ExecuteScalarAsync().ConfigureAwait(false);
                sqlConnection.Close();

                bool isAdded = false;

                isAdded = Convert.ToBoolean(jsonResult) ? true : false;

                return isAdded;
            }
        }

        /// <summary>
        /// Construct Params dynamic from stored procedure object
        /// </summary>
        /// <param name="StoredProcedureObject"></param>
        /// <param name="sqlCommand"></param>
        /// <returns></returns>
        private List<SqlParameter> GetSqlParameterCollection(IStoredProcedure StoredProcedureObject,
            ref SqlCommand sqlCommand)
        {
            List<SqlParameter> parameterList = new List<SqlParameter>();
            foreach (var prop in StoredProcedureObject.GetType().GetProperties())
            {
                var propertyValue = prop.GetValue(StoredProcedureObject, null);

                if (IsNullable(prop.PropertyType) == false || propertyValue != null)
                {
                    parameterList.Add(new SqlParameter(prop.Name, propertyValue));
                    sqlCommand.Parameters.Add(new SqlParameter(prop.Name, propertyValue));
                }
                else
                {
                    parameterList.Add(new SqlParameter(prop.Name, DBNull.Value));
                    sqlCommand.Parameters.Add(new SqlParameter(prop.Name, DBNull.Value));
                }
            }

            return parameterList;
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
