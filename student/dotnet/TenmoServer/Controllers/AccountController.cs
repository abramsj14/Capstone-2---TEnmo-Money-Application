using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.DAO;
using TenmoServer.Security;
using Microsoft.AspNetCore.Identity;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("account/")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {

        private readonly IAccountsDao accountsDao;

        public AccountController(IAccountsDao _accountsDao)
        {
            accountsDao = _accountsDao;
        }

        [HttpGet("balance")]
        public ActionResult<decimal> RetrieveAccountBalance()
        {
            return accountsDao.GetBalance(User.Identity.Name);

        }

    }
}
