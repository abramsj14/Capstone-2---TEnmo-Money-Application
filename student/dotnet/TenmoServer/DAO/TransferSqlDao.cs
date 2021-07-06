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

        public TransferSqlDao(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public Transfer GetTransfers(int transferId)
        {
            Transfer transfer = null;
            string sqlQuery = "SELECT transfer_id, transfer_type_id, transfer_status_id, account_from, account_to, amount FROM transfers WHERE transfer_id = @transfer_id;";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@transfer_id", transferId);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    transfer = CreateTransferFromReader(reader);
                }
            }
            return transfer;
        }


        public List<Transfer> GetTransfersByUserId(int userId)
        {
            List<Transfer> transfers = new List<Transfer>();

            string sqlQuery = "SELECT transfer_id, transfer_type_id, transfer_status_id, account_from, account_to, amount FROM transfers WHERE (transfer_status_id = 2 OR transfer_status_id = 3) AND transfer_type_id = 2 AND ((SELECT account_id FROM accounts WHERE user_id = @user_id) = account_from OR (SELECT account_id FROM accounts WHERE user_id = @user_id) = account_to);";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@user_id", userId);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                   Transfer transfer = CreateTransferFromReader(reader);
                    transfers.Add(transfer);
                }
            }
            return transfers;
        }

        public List<Transfer> GetPendingRequestsUserId(int userId)
        {
            List<Transfer> transfers = new List<Transfer>();

            string sqlQuery = "SELECT transfer_id, transfer_type_id, transfer_status_id, account_from, account_to, amount FROM transfers WHERE transfer_status_id = 1 AND ((SELECT account_id FROM accounts WHERE user_id = @user_id) = account_from);";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@user_id", userId);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Transfer transfer = CreateTransferFromReader(reader);
                    transfers.Add(transfer);
                }
            }
            return transfers;
        }
            
        public Transfer AddTransfer(Transfer transfer, int fromAccountId, int toAccountId)
        {
            int newTransferId = 0;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {               
                
                conn.Open();

                SqlCommand cmd = new SqlCommand("INSERT INTO transfers (transfer_type_id, transfer_status_id, account_from, account_to, amount) " + 
                    "OUTPUT INSERTED.transfer_id " + 
                    "VALUES (" +
                    "(SELECT transfer_type_id FROM transfer_types WHERE transfer_type_id = @transfer_type_id), " +
                    "(SELECT transfer_status_id FROM transfer_statuses WHERE transfer_status_id = @transfer_status_id), " +
                    "(SELECT account_id FROM accounts WHERE user_id = @account_from), " +
                    "(SELECT account_id FROM accounts WHERE user_id = @account_to), " +
                    "@amount);",conn);
                cmd.Parameters.AddWithValue("@transfer_type_id", transfer.TransferTypeId);
                cmd.Parameters.AddWithValue("@transfer_status_id", transfer.TransferStatusId);
                cmd.Parameters.AddWithValue("@account_from", transfer.AccountFrom);
                cmd.Parameters.AddWithValue("@account_to", transfer.AccountTo);
                cmd.Parameters.AddWithValue("@amount", transfer.Amount);


                newTransferId = Convert.ToInt32(cmd.ExecuteScalar());
            }
            return GetTransfers(newTransferId);
        }

        public Transfer UpdateTransfer(Transfer transfer)
        {           
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("UPDATE transfers SET transfer_type_id = (SELECT transfer_type_id FROM transfer_types WHERE transfer_type_id = @transfer_type_id), transfer_status_id = (SELECT transfer_status_id FROM transfer_statuses WHERE transfer_status_id = @transfer_status_id),account_from = (SELECT account_id FROM accounts WHERE user_id = @account_from), account_to = (SELECT account_id FROM accounts WHERE user_id = @account_to), amount = @amount  WHERE transfer_id = @transfer_id;", conn);
                cmd.Parameters.AddWithValue("@transfer_id", transfer.TransferId);
                cmd.Parameters.AddWithValue("@transfer_type_id", transfer.TransferTypeId);
                cmd.Parameters.AddWithValue("@transfer_status_id", transfer.TransferStatusId);
                cmd.Parameters.AddWithValue("@account_from", transfer.AccountFrom);
                cmd.Parameters.AddWithValue("@account_to", transfer.AccountTo);
                cmd.Parameters.AddWithValue("@amount", transfer.Amount);

            }
            return transfer;
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
