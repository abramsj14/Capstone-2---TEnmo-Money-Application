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

            string sqlQuery = "SELECT * FROM transfers WHERE transfer_id = @transfer_id;";

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

        //public Transfer RequestTransfer(int userId)
        //{
        //    Transfer transfer = null;

                   
        //}

        
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
