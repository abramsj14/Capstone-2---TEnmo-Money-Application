using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.Controllers
{
    public class TransferController
    {
        [Route("transfers/")]
        [ApiController]
        [Authorize]
        public class TransferController : ControllerBase
        {
            private static ITransferDao transferDao;
            public TransferController(ITransferDao _transferDao)
            {
                transferDao = _transferDao;
            }
            [HttpGet]
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

}
}
