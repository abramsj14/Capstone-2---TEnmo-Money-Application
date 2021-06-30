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

        public decimal GetBalance(int userId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT account_id, user_id, balance FROM accounts WHERE user_id = @user_id;", conn);
                    cmd.Parameters.AddWithValue("@user_id", userId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        Account account = GetAccount(reader);
                        return account.Balance;
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
