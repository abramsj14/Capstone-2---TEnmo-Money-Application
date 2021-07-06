using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("transfer/")]
    [ApiController]
    [Authorize]
    public class TransferController : ControllerBase
    {
        private static ITransferDao transferDao;
        private static IUserDao userDao;
        private static IAccountsDao accountsDao;

        public TransferController(ITransferDao _transferDao, IUserDao _userDao, IAccountsDao _accountsDao)
        {
            transferDao = _transferDao;
            userDao = _userDao;
            accountsDao = _accountsDao;
        }

        [HttpGet("user")]
        public ActionResult<User> FetchUsers(string userName)
        {
            User user = userDao.GetUser(userName);
            return user;
        }

        [HttpGet("users")]
        public ActionResult<List<string>> FetchUsers()
        {
            List<User> users = userDao.GetUsers();
            List<string> usernames = new List<string>();
            foreach(User user in users)
            {
                usernames.Add(user.Username);
            }
            return usernames;
        }

        [HttpPost]
        public ActionResult<Transfer> NewTransfer(Transfer transfer)
        {          
            if (accountsDao.GetBalance(userDao.GetUserName(transfer.AccountFrom)) < transfer.Amount)
            {
                transfer.TransferStatusId = 3;
            }
            Transfer newTransfer = transferDao.AddTransfer(transfer, transfer.AccountFrom, transfer.AccountTo);    
            
            if(transfer.TransferStatusId == 2)
            {
                accountsDao.RemoveBalanceFromAccount(newTransfer.AccountFrom, newTransfer.Amount);
                accountsDao.AddBalanceToAccount(newTransfer.AccountTo, newTransfer.Amount);
            }
            return newTransfer;
        }

        [HttpPut]
        public ActionResult<Transfer> ApproveOrDenyRequest(Transfer transfer)
        {
            if (accountsDao.GetBalanceFromAccount(transfer.AccountFrom) < transfer.Amount)
            {
                transfer.TransferStatusId = 3;
            }
            Transfer newTransfer = transferDao.UpdateTransfer(transfer);

            if (transfer.TransferStatusId == 2)
            {
                accountsDao.RemoveBalanceFromAccount(newTransfer.AccountFrom, newTransfer.Amount);
                accountsDao.AddBalanceToAccount(newTransfer.AccountTo, newTransfer.Amount);
            }
            return newTransfer;
        }

        [HttpGet("{userId}")]
        public ActionResult<List<Transfer>> GetTransfersByUserId(int userId)
        {
            List<Transfer> transfers = transferDao.GetTransfersByUserId(userId);          
            if (transfers != null)
            {
                return transfers;
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("request/{userId}")]
        public ActionResult<List<Transfer>> GetPendingRequestsByUserId(int userId)
        {
            List<Transfer> transfers = transferDao.GetPendingRequestsUserId(userId);
            if (transfers != null)
            {
                return transfers;
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("approveorreject/{transferId}")]
        public ActionResult<Transfer> GetTransferByTransferId(int transferId)
        {
            Transfer transfer = transferDao.GetTransfers(transferId);
            if (transfer == null)
            {
                return NotFound("Transfer Status Id is invalid");
            }
            return transferDao.GetTransfers(transferId);
        }
    }
}
