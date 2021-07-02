using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TenmoClient.Models;

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
            else
            {
                Console.WriteLine("An error response was received from the server. The status code is " + (int)response.StatusCode);
            }
            return response.Data;
        }
    }
}
