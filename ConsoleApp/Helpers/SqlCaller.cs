using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ConsoleApp.Helpers
{
    public class SqlCaller
    {
        private readonly Logger _Logger;

        public SqlCaller(Logger p_Logger)
        {
            _Logger = p_Logger;
        }

        public int GetValue(TimeSpan p_Delay, int p_ReturnValue)
        {
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
            {
                con.Open();
                var com = new SqlCommand($"WAITFOR DELAY '{p_Delay}'; SELECT {p_ReturnValue}", con);
                _Logger.Log($"BeforeSqlCall-{p_ReturnValue:00}");
                var result = (int) com.ExecuteScalar();
                _Logger.Log($"AfterSqlCall--{p_ReturnValue:00}");
                return result;
            }
        }

        public async Task<int> GetValueAsync(TimeSpan p_Delay, int p_ReturnValue)
        {
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
            {
                await con.OpenAsync();
                var com = new SqlCommand($"WAITFOR DELAY '{p_Delay}'; SELECT {p_ReturnValue}", con);
                _Logger.Log($"BeforeSqlCall-{p_ReturnValue:00}");
                var result = (int) await com.ExecuteScalarAsync();
                _Logger.Log($"AfterSqlCall--{p_ReturnValue:00}");
                return result;
            }
        }
    }
}