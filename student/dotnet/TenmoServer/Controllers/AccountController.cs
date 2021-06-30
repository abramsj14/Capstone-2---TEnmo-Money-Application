using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.DAO;
using TenmoServer.Security;

namespace TenmoServer.Controllers
{
    [Route("account/")]
    [ApiController]   
    public class AccountController : ControllerBase
    {       
        public class LoginController : ControllerBase
        {
            private readonly IAccountsDao accountsDao;         

            public LoginController(IAccountsDao _accountsDao)
            {               
                accountsDao = _accountsDao;
            }

            [HttpGet("balance")]
            public ActionResult<decimal> RetrieveAccountBalance(int userId)
            {
                               
                return accountsDao.GetBalance(userId);         
            }
        }

    }
}
