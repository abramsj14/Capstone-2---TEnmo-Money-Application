using System;
using System.Collections.Generic;
using TenmoClient.Models;

namespace TenmoClient
{
    class Program
    {
        private static readonly ConsoleService consoleService = new ConsoleService();
        private static readonly AuthService authService = new AuthService();
        private static readonly AccountService accountService = new AccountService();
        private static readonly TransferService transferService = new TransferService();


        static void Main(string[] args)
        {
            Run();
        }

        private static void Run()
        {
            int loginRegister = -1;
            while (loginRegister != 1 && loginRegister != 2)
            {
                Console.WriteLine("Welcome to TEnmo!");
                Console.WriteLine("1: Login");
                Console.WriteLine("2: Register");
                Console.Write("Please choose an option: ");

                if (!int.TryParse(Console.ReadLine(), out loginRegister))
                {
                    Console.WriteLine("Invalid input. Please enter only a number.");
                }
                else if (loginRegister == 1)
                {
                    while (!UserService.IsLoggedIn()) //will keep looping until user is logged in
                    {
                        LoginUser loginUser = consoleService.PromptForLogin();
                        ApiUser user = authService.Login(loginUser);
                        if (user != null)
                        {
                            UserService.SetLogin(user);
                        }
                    }
                }
                else if (loginRegister == 2)
                {
                    bool isRegistered = false;
                    while (!isRegistered) //will keep looping until user is registered
                    {
                        LoginUser registerUser = consoleService.PromptForLogin();
                        isRegistered = authService.Register(registerUser);
                        if (isRegistered)
                        {
                            Console.WriteLine("");
                            Console.WriteLine("Registration successful. You can now log in.");
                            loginRegister = -1; //reset outer loop to allow choice for login
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Invalid selection.");
                }
            }

            MenuSelection();
        }

        private static void MenuSelection()
        {
            int menuSelection = -1;
            while (menuSelection != 0)
            {
                Console.WriteLine("");
                Console.WriteLine("Welcome to TEnmo! Please make a selection: ");
                Console.WriteLine("1: View your current balance");
                Console.WriteLine("2: View your past transfers");
                Console.WriteLine("3: View your pending requests");
                Console.WriteLine("4: Send TE bucks");
                Console.WriteLine("5: Request TE bucks");
                Console.WriteLine("6: Log in as different user");
                Console.WriteLine("0: Exit");
                Console.WriteLine("---------");
                Console.Write("Please choose an option: ");

                if (!int.TryParse(Console.ReadLine(), out menuSelection))
                {
                    Console.WriteLine("Invalid input. Please enter only a number.");
                }
                else if (menuSelection == 1)
                {
                    decimal balance = accountService.GetBalance();
                    Console.WriteLine($"${balance}");
                }
                else if (menuSelection == 2)
                {
                    List<Transfer> transfers = transferService.GetPastTransfers(UserService.GetUserId());
                    foreach (Transfer transfer in transfers)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"Transfer Id: {transfer.TransferId}");
                        Console.WriteLine($"Account From: {transfer.AccountFrom}");
                        Console.WriteLine($"Account To: {transfer.AccountTo}");
                        Console.WriteLine($"Amount: {transfer.Amount}");
                        string transferStatus = "";
                        if(transfer.TransferStatusId == 3)
                        {
                            transferStatus = "Rejected";
                        }
                        else if(transfer.TransferStatusId == 2)
                        {
                            transferStatus = "Approved";
                        }
                        Console.WriteLine($"Transfer Status: {transferStatus}");
                        Console.WriteLine("Transfer Type: Send");
                        Console.WriteLine("---------------------------------------");
                    }
                }
                else if (menuSelection == 3)
                {
                    ApproveOrDeny();
                }
                else if (menuSelection == 4)
                {
                    Transfer transfer = CreateTransferObject();
                    transferService.CreateTransfer(transfer);
                    
                }
                else if (menuSelection == 5)
                {
                    Transfer transfer = CreateRequestObject();
                    transferService.CreateTransfer(transfer);

                }
                else if (menuSelection == 6)
                {
                    Console.WriteLine("");
                    UserService.SetLogin(new ApiUser()); //wipe out previous login info
                    Run(); //return to entry point
                }
                else
                {
                    Console.WriteLine("Goodbye!");
                    Environment.Exit(0);
                }
            }
        }

        private static Transfer CreateTransferObject()
        {
            Transfer transfer = new Transfer();
            Console.WriteLine("Select a user to send to: ");
            List<string> users = transferService.ReturnAllUsers();
            foreach(string user in users)
            {
                Console.WriteLine(user);
            }
            User userToSendTo = transferService.ReturnAUser(Console.ReadLine());
            transfer.AccountTo = userToSendTo.UserId;
            Console.WriteLine("How much would you like to send?: ");
            transfer.Amount = decimal.Parse(Console.ReadLine());
            transfer.AccountFrom = UserService.GetUserId();
            transfer.TransferStatusId = 2;
            transfer.TransferTypeId = 2;
            return transfer;
        }

        private static Transfer CreateRequestObject()
        {
            Transfer transfer = new Transfer();
            Console.WriteLine("Select a user to request TE Bucks from: ");
            List<string> users = transferService.ReturnAllUsers();
            foreach (string user in users)
            {
                Console.WriteLine(user);
            }
            User userToRequestFrom= transferService.ReturnAUser(Console.ReadLine());
            transfer.AccountFrom = userToRequestFrom.UserId;
            Console.WriteLine("How much would you like to request?: ");
            transfer.Amount = decimal.Parse(Console.ReadLine());
            transfer.AccountTo = UserService.GetUserId();
            transfer.TransferStatusId = 1;
            transfer.TransferTypeId = 1;
            return transfer;
        }

        private static void ApproveOrDeny()
        {
            List<Transfer> transfers = transferService.GetPendingRequests(UserService.GetUserId());
            foreach (Transfer transfer in transfers)
            {
                Console.WriteLine();
                Console.WriteLine($"Transfer Id: {transfer.TransferId}");
                Console.WriteLine($"Account From: {transfer.AccountFrom}");
                Console.WriteLine($"Account To: {transfer.AccountTo}");
                Console.WriteLine($"Amount: {transfer.Amount}");
                Console.WriteLine($"Transfer Status: Pending");
                Console.WriteLine("Transfer Type: Request");
                Console.WriteLine("---------------------------------------");
                Console.WriteLine();
            }

            int selection = 0;
            while(selection != 3)
            {
                Console.WriteLine("1: Approve a request");
                Console.WriteLine("2: Reject a request");
                Console.WriteLine("3: Exit request menu");
                selection = Convert.ToInt32(Console.ReadLine());
                if (selection == 1)
                {
                    Console.WriteLine("Please enter the transfer ID of the request you would like to approve");
                    string transferId = Console.ReadLine();
                    int newTransferId = Convert.ToInt32(transferId);
                    Transfer transferToApprove = transferService.GetTransfer(newTransferId);
                    transferToApprove.TransferStatusId = 2;
                    transferToApprove.TransferTypeId = 2;
                    transferService.UpdateTransfer(transferToApprove);
                    Console.WriteLine($"Transfer {transferId} Approved");
                }
                else if(selection == 2)
                {
                    Console.WriteLine("Please enter the transfer ID of the request you would like to reject");
                    string transferId = Console.ReadLine();
                    int newTransferId = Convert.ToInt32(transferId);
                    Transfer transferToReject = transferService.GetTransfer(newTransferId);
                    transferToReject.TransferStatusId = 3;                  
                    transferService.UpdateTransfer(transferToReject);
                    Console.WriteLine($"Transfer {transferId} Rejected");
                }
            }
  
        }

    }
}
