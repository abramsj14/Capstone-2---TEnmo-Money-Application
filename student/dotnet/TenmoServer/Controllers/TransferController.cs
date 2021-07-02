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
        public TransferController(ITransferDao _transferDao)
        {
            transferDao = _transferDao;
        }

        [HttpPost]
        public ActionResult<Transfer> NewTransfer(Transfer transfer)
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

        [HttpGet("{userId}")]
        public ActionResult<Transfer> GetTransferByUserId(int userId)
        {
            Transfer transfer = transferDao.GetTransfers(userId);
            if (transfer != null)
            {
                return transfer;
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
