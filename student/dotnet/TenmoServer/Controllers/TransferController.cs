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
    [Route("transfers/")]
    [ApiController]
    [Authorize]
    public class TransferController : ControllerBase
    {
        private static ITransferDao transferDao;
<<<<<<< HEAD

=======
>>>>>>> f96d1544e6462f55cda597f2339ffa47847c5cf4
        public TransferController(ITransferDao _transferDao)
        {
            transferDao = _transferDao;
        }

<<<<<<< HEAD
=======
        [HttpPost("send")]
        public ActionResult<Transfer> NewSendTransfer(string toUser, decimal amount)
        {
            //CREATE SEND             From_User           to_user amount  send_id
            transferDao.StoreTransfer(User.Identity.Name, toUser, amount, 1);
        }

        [HttpPost("request")]
        public ActionResult<Transfer> NewRequestTransfer(string fromUser, decimal amount)
        {
            //CREATE SEND             From_User           to_user amount  send_id
            transferDao.StoreTransfer(fromUser, User.Identity.Name, amount, 2);
        }

>>>>>>> f96d1544e6462f55cda597f2339ffa47847c5cf4
        [HttpGet]
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
