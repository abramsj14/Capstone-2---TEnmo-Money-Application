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
