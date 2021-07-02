using System;
using System.Collections.Generic;
using RestSharp;
using RestSharp.Authenticators;
using TenmoClient.Models;

namespace TenmoClient
{
    public class TransferService
    {
        private readonly static string API_BASE_URL = "https://localhost:44315/";
        private readonly IRestClient client = new RestClient();

        public List<string> ReturnAllUsers()
        {
            RestRequest request = new RestRequest(API_BASE_URL + "transfer/users");
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());
            IRestResponse<List<string>> response = client.Get<List<string>>(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                Console.WriteLine("An error occurred communicating with the server.");
                return null;
            }
            else if (!response.IsSuccessful)
            {
                Console.WriteLine("An error message was received: ");
                return null;
            }

            return response.Data;
        }

        public User ReturnAUser()
        {
            RestRequest request = new RestRequest(API_BASE_URL + "transfer/user");
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());
            IRestResponse<User> response = client.Get<User>(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                Console.WriteLine("An error occurred communicating with the server.");
                return null;
            }
            else if (!response.IsSuccessful)
            {
                Console.WriteLine("An error message was received: ");
                return null;
            }

            return response.Data;
        }

        public Transfer CreateSendTransfer(Transfer transfer)
        {
            RestRequest request = new RestRequest(API_BASE_URL + "transfer");
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());
            IRestResponse<Transfer> response = client.Get<Transfer>(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                Console.WriteLine("An error occurred communicating with the server.");
                return null;
            }
            else if (!response.IsSuccessful)
            {
                Console.WriteLine("An error message was received: ");
                return null;
            }

            return response.Data;
        }

        public List<Transfer> GetPastTransfers(int userId)
        {
            RestRequest request = new RestRequest(API_BASE_URL + "transfer/" + userId);
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());
            IRestResponse<List<Transfer>> response = client.Get<List<Transfer>>(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                Console.WriteLine("An error occurred communicating with the server.");
                return null;
            }
            else if (!response.IsSuccessful)
            {
                Console.WriteLine("An error message was received: ");
                return null;
            }

            return response.Data;
        }


    }
}
