using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TenmoServer.Models;
using TenmoServer.Security;
using TenmoServer.Security.Models;
using System.Threading.Tasks;

namespace TenmoServer.DAO
{
    public class AccountsSqlDao : IAccountsDao
    {
        private readonly string connectionString;

        public AccountsSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString; 
        }

        public decimal GetBalance(string userName)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT balance FROM accounts " +
                                                    "JOIN users ON users.user_id = accounts.user_id " +
                                                    "WHERE username = @username;", conn);
                    cmd.Parameters.AddWithValue("@username", userName);
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

            return 0M;
        }

        private Account GetAccount(SqlDataReader reader)
        {
            Account account = new Account()
            {
                AccountId = Convert.ToInt32(reader["account_id"]),
                UserId = Convert.ToInt32(reader["user_id"]),
                Balance = Convert.ToDecimal(reader["balance"]),
            };

            return account;
        }
    }
}
