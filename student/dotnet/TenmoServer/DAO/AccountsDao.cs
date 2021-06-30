using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TenmoServer.Models;
using TenmoServer.Security;
using TenmoServer.Security.Models;
using System.Threading.Tasks;

namespace TenmoServer.DAO
{
    public class AccountsDao : IAccountsDao
    {
        private readonly string connectionString;

        public AccountsDao(string dbConnectionString)
        {
            connectionString = dbConnectionString; 
        }

        public decimal GetBalance(int userId)
        {
            decimal balance = 0M;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT user_id, balance FROM accounts WHERE user_id = @user_id;", conn);
                    cmd.Parameters.AddWithValue("@user_id", userId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        return Convert.ToDecimal(reader["balance"]);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return
        }

        private
    }
}
