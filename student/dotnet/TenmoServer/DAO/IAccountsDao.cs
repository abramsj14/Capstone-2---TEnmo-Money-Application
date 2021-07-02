using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.DAO
{
    public interface IAccountsDao
    {
        decimal GetBalance(string userName);
        decimal GetBalanceFromAccount(int accountId);
        decimal RemoveBalanceFromAccount(int accountId, decimal amountToRemove);
        decimal AddBalanceToAccount(int accountId, decimal amountToAdd);

    }
}
