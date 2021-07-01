using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class TransferSqlDao : ITransferDao
    {
        private readonly string connectionString;
        public Transfer GetTransfers(int userId)
        {
            Transfer transfer = null;

            string sqlQuery = "SELECT * FROM transfers JOIN accounts ON accounts.account_id = transfers.account_from WHERE user_id = @user_id";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@user_id", userId);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    transfer = CreateTransferFromReader(reader);
                }
            }
            return transfer;
        }

        public Transfer GetTransferStatus(int transferStatusId)
        {
            Transfer transfer = null;

            string sqlQuery = "SELECT transfer_status_id FROM transfers WHERE transfer_status_id = @transfer_status_id";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@transfer_status_id", transferStatusId);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    transfer = CreateTransferFromReader(reader);
                }
            }
            return transfer;
        }

        public Transfer RequestTransfer(int userId)
        {
            Transfer transfer = null;

                   
        }

        
        public Transfer StoreTransfer(string accountFrom, string accountTo, decimal amount, int transferTypeId)
        {
            int accountFromId = 0;
            int accountToId = 0;
            int newTransferId = 0;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT accounts.user_id FROM accounts JOIN users ON users.user_id = accounts.user_id WHERE users.username = @account_from;", conn);
                cmd.Parameters.AddWithValue("@account_from", accountFrom);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    accountFromId = Convert.ToInt32(reader["user_id"]);
         
                }            
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT accounts.user_id FROM accounts JOIN users ON users.user_id = accounts.user_id WHERE users.username = @account_to;", conn);
                cmd.Parameters.AddWithValue("@account_to", accountTo);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    accountToId = Convert.ToInt32(reader["user_id"]);

                }
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {               
                
                conn.Open();

                SqlCommand cmd = new SqlCommand("INSERT INTO transfers (transfer_type_id, transfer_status_id, account_from, account_to, amount) " + "OUTPUT INSERTED.transfer_id " + "VALUES (@transfer_type_id, @transfer_status_id, @account_from, @account_to, @amount);",conn);
                cmd.Parameters.AddWithValue("@transfer_type_id", transferTypeId);
                cmd.Parameters.AddWithValue("@transfer_status_id", 1);
                cmd.Parameters.AddWithValue("@account_from", accountFromId);
                cmd.Parameters.AddWithValue("@account_to", accountToId);
                cmd.Parameters.AddWithValue("@ammount", amount);
                newTransferId = Convert.ToInt32(cmd.ExecuteScalar());
            }

            return GetTransfers(newTransferId);
        }
        private Transfer CreateTransferFromReader(SqlDataReader reader)
        {
            Transfer transfer = new Transfer();
            transfer.TransferId = Convert.ToInt32(reader["transfer_id"]);
            transfer.TransferTypeId = Convert.ToInt32(reader["transfer_type_id"]);
            transfer.TransferStatusId = Convert.ToInt32(reader["transfer_status_id"]);
            transfer.AccountFrom = Convert.ToInt32(reader["account_from"]);
            transfer.AccountTo = Convert.ToInt32(reader["account_to"]);
            transfer.Amount = Convert.ToDecimal(reader["amount"]);

            return transfer;
        }
    }
}
