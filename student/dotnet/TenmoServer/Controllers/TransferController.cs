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
<<<<<<< HEAD

=======
>>>>>>> f96d1544e6462f55cda597f2339ffa47847c5cf4
        public TransferController(ITransferDao _transferDao)
=======
        private static IUserDao userDao;

        public TransferController(ITransferDao _transferDao, IUserDao _userDao)
>>>>>>> 3558b177eebc12adfd727d5aec16a639cf88a56b
        {
            transferDao = _transferDao;
            userDao = _userDao;
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

<<<<<<< HEAD
<<<<<<< HEAD
=======
        [HttpPost("send")]
        public ActionResult<Transfer> NewSendTransfer(string toUser, decimal amount)
=======
        [HttpPost]
        public ActionResult<Transfer> NewTransfer(Transfer transfer)
>>>>>>> 3558b177eebc12adfd727d5aec16a639cf88a56b
        {
            //CREATE SEND             From_User           to_user amount  send_id
            Transfer newTransfer = transferDao.AddTransfer(transfer, transfer.AccountFrom, transfer.AccountTo);
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
>>>>>>> f96d1544e6462f55cda597f2339ffa47847c5cf4
        [HttpGet]
=======
        [HttpGet("{userId}")]
>>>>>>> 3558b177eebc12adfd727d5aec16a639cf88a56b
        public ActionResult<Transfer> GetTransferByUserId(int userId)
        {
            Transfer transfer = transferDao.GetTransfers(userId);
<<<<<<< HEAD

=======
>>>>>>> f96d1544e6462f55cda597f2339ffa47847c5cf4
            if (transfer != null)
            {
                return transfer;
            }
            else
            {
                return NotFound();
            }
        }
<<<<<<< HEAD

=======
>>>>>>> f96d1544e6462f55cda597f2339ffa47847c5cf4
        [HttpGet]
        public ActionResult<Transfer> GetTransferStatusByTransferStatusId(int transferStatusId)
        {
            Transfer transfer = transferDao.GetTransferStatus(transferStatusId);
<<<<<<< HEAD

=======
>>>>>>> f96d1544e6462f55cda597f2339ffa47847c5cf4
            if (transfer == null)
            {
                return NotFound("Transfer Status Id is invalid");
            }
            return transferDao.GetTransfers(transferStatusId);
        }
    }
}
