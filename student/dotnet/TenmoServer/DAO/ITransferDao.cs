using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface ITransferDao
    {
        Transfer GetTransfers(int userId);
        Transfer GetTransferStatus(int transferStatusId);
        Transfer AddTransfer(Transfer transfer, int fromAccountId, int toAccountId);
        List<Transfer> GetTransfersByUserId(int userId);
    }
}
