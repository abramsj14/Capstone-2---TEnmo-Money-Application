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
<<<<<<< HEAD

        public TransferController(ITransferDao _transferDao)
        { }
        private static IUserDao userDao;
  
        public TransferController(ITransferDao _transferDao, IUserDao _userDao)

=======
        private static IUserDao userDao;
        private static IAccountsDao accountsDao;

        public TransferController(ITransferDao _transferDao, IUserDao _userDao, IAccountsDao _accountsDao)
>>>>>>> ad83b77856acee52041e9cc23d9a33d063ebbe1c
        {
            transferDao = _transferDao;
            userDao = _userDao;
            accountsDao = _accountsDao;
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

        [HttpPost("send")]
        public ActionResult<Transfer> NewSendTransfer(string toUser, decimal amount)
        {
            return null;
        }

        [HttpPost]
        public ActionResult<Transfer> NewTransfer(Transfer transfer)
        {          
    
            //CREATE SEND             From_User           to_user amount  send_id
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

        /*
        [HttpPost("request")]
        public ActionResult<Transfer> NewRequestTransfer(string fromUser, decimal amount)
        {
            //CREATE SEND             From_User           to_user amount  request_id
            Transfer transfer = transferDao.StoreTransfer(fromUser, User.Identity.Name, amount, 2);
            return transfer;
        }
        */

<<<<<<< HEAD
        [HttpGet]

        [HttpGet("{userId}")]

        public ActionResult<Transfer> GetTransferByUserId(int userId)
        {
            Transfer transfer = transferDao.GetTransfers(userId);

            if (transfer != null)
=======
        [HttpGet("{accountId}")]
        public ActionResult<List<Transfer>> GetTransferByUserId(int accountId)
        {
            List<Transfer> transfers = transferDao.GetTransfersByAccount(accountId);
            if (transfers != null)
>>>>>>> ad83b77856acee52041e9cc23d9a33d063ebbe1c
            {
                return transfers;
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        public ActionResult<Transfer> GetTransferStatusByTransferStatusId(int transferStatusId)
        {
            Transfer transfer = transferDao.GetTransferStatus(transferStatusId);
            if (transfer == null)
            {
                return NotFound("Transfer Status Id is invalid");
            }
            return transferDao.GetTransfers(transferStatusId);
        }
    }
}
