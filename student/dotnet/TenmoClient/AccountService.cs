using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TenmoClient.Models;
using RestSharp.Authenticators;

namespace TenmoClient
{
    public class AccountService
    {
        private readonly static string API_BASE_URL = "https://localhost:44315/";
        private readonly IRestClient client = new RestClient();


        //get balance
        public decimal GetBalance()
        {
            RestRequest request = new RestRequest(API_BASE_URL + "account/balance");
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());
            IRestResponse<decimal> response = client.Get<decimal>(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                Console.WriteLine("An error occurred communicating with the server.");
                return 0M;
            }
            else if (!response.IsSuccessful)
            {
                Console.WriteLine("An error message was received: ");
            }
          
            return response.Data;
        }

        /*
        public List<Transfer> GetPastTransfer(string authToken, int accountId)
        {
            RestRequest request = new RestRequest(API_BASE_URL + accountId);
            client.Authenticator = new JwtAuthenticator(authToken);
            IRestResponse<decimal> response = client.Get<decimal>(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                Console.WriteLine("An error occurred communicating with the server.");               
            }
            else if (!response.IsSuccessful)
            {
                Console.WriteLine("An error message was received: ");
            }

            return response.Data;
        }
        */
    }
}
