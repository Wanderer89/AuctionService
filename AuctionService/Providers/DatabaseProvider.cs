using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using AuctionService.Providers;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using AuctionService.Models;
using System.Data;

namespace AuctionService.Providers
{
    public class DatabaseProvider : IProvider
    {
        private readonly IConfiguration configuration;
        private readonly string connectionString;
        public readonly SqlConnectionManagerBeta dbConnectionManager;
        
        public DatabaseProvider(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.connectionString = GetSqlConnectionString();
            dbConnectionManager =  new SqlConnectionManagerBeta(connectionString, 3, 90000000, 10000000);
        }

        public async Task<bool> ExecuteNonQueryAsync(string sqlQuery)
        {
            try
            {
                SqlCommand cmd = GetCustomSQLCommand(sqlQuery);
                await cmd.ExecuteReaderAsync();
                return true;
            }
            catch (Exception ex)
            {
                //var properties = new Dictionary<string, string>() { { "Exception Context", ex.ToString() } };
                //Logger.LogError($"Exception in {SPName}", properties);
                throw ex;
            }
        }

        public async Task<T> ExecuteScalarReaderAsync<T>(string sqlQuery) where T : IInflatable, new()
        {
            try
            {
                T rowObject = new T();
                SqlCommand cmd = GetCustomSQLCommand(sqlQuery);

                using SqlDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    reader.Read();
                    rowObject.Inflate(reader);
                }
                return rowObject;
            }
            catch (Exception ex)
            {
                //var properties = new Dictionary<string, string>() { { "Exception Context", ex.ToString() } };
                //Logger.LogError($"Exception in {SPName}", properties);
                throw ex;
            }
        }

        public async Task<List<T>> ExecuteMultiReaderAsync<T>(string sqlQuery) where T : IInflatable, new()
        {
            try
            {
                List<T> outputList = new List<T>();
                SqlCommand cmd = GetCustomSQLCommand(sqlQuery);

                using SqlDataReader reader = await cmd.ExecuteReaderAsync();
                while (reader.Read())
                {
                    var dd = new T();
                    //dd.Inflate(reader);
                    outputList.Add(dd);
                }
                return outputList;
            }
            catch (Exception ex)
            {
                //var properties = new Dictionary<string, string>() { { "Exception Context", ex.ToString() } };
                //Logger.LogError($"Exception in {SPName}", properties);
                throw ex;
            }
        }

        private SqlCommand GetCustomSQLCommand(string query)
        {
            using SqlCommand cmd = new SqlCommand();
            var con = dbConnectionManager.GetAvailableCon();
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = query;
            cmd.CommandTimeout = 9999;

            return cmd;
        }

        private string GetSqlConnectionString()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = configuration.GetValue<string>(Constants.DataSource);
            builder.UserID = configuration.GetValue<string>(Constants.UserID);
            builder.Password = configuration.GetValue<string>(Constants.Password);
            builder.InitialCatalog = configuration.GetValue<string>(Constants.Database);
            builder.ConnectTimeout = configuration.GetValue<int>(Constants.ConnectionTimeout);
            builder.IntegratedSecurity = true;
            builder.Encrypt = false;

            return builder.ConnectionString;
        }
    }
}
