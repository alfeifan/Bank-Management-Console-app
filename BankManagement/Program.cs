using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace BankManagement
{
    class Program
    {
        private const int INDX_ACCOUNT = 0;
        private const int INDX_BALANCE = 1;
        private const int INDX_FNAME = 0;
        private const int INDX_LNAME = 1;
        private const int INDX_ADDRESS = 2;
        private const int INDX_PHONE = 3;
        private const int INDX_EMAIL = 4;
        private static Dictionary<int, Account> accounts;
        static void Main(string[] args)
        {
            accounts = new Dictionary<int, Account>();
            Login login = new Login("login.txt");
            bool loginSuccess = login.signIn();
            while (!loginSuccess)
            {
                Console.WriteLine("Invalid Credentials. Please, try again !\n");
                loginSuccess = login.signIn();
            }
            Console.WriteLine("You successfuly logged in !\n");
            bool quit = false;
            while (!quit)
            {
                string choiceStr = Display.menu();
                int choice = 0;
                if (Validate.isInteger(choiceStr, ref choice))
                {
                    if (choice < 1 || choice > 7) continue;
                    switch (choice)
                    {
                        case 1: createAccount(); break;
                        case 2: searchAccount(); break;
                        case 3: deposit(); break;
                        case 4: withdraw(); break;
                        case 5: statement(); break;
                        case 6: deleteAccount(); break;
                        case 7: quit = true; break;
                        
                    }
                    
                   
                }                
            }           

        }

        static void createAccount()
        {
            List<string> fieldTitles = new List<string>()
                    {"First Name:","Last Name:","Address:","Phone:","Email:" };
            List<string> fields;
            do
            {
                fields = Display.getInfo("CREATE NEW ACCOUNT", "ENTER THE DETAILS", fieldTitles);
            }
            while (!Validate.newAccount(fields));

            Console.Write("Is the information correct (y/n): ");
            string confirm = Console.ReadLine();
            if (confirm == "y")
            {
                Account account = new Account(Validate.generateAccount(), 0,
                    fields[INDX_FNAME], fields[INDX_LNAME], fields[INDX_ADDRESS], 
                    int.Parse(fields[INDX_PHONE]), fields[INDX_EMAIL]);
                account.save();

                Console.WriteLine("Account Created! Details will be provided via email.");
                Console.WriteLine("Account number is {0}",account.AccountNumber);

                // Send email 
                var fromAddress = new MailAddress("bobdoe5418@gmail.com", "Bank System");
                var toAddress = new MailAddress(account.Email);
                const string fromPassword = "gE7yG$@%B$&Y";
                const string subject = "Account Creation Details";
                string body = 
                    string.Format($"Here are your account details!" +
                    $"\nFirst name: {account.FirstName} " +
                    $"\nLast Name: {account.LastName} " +
                    $"\nAddress: {account.Address} " +
                    $"\nPhone number: {account.Phone} " +
                    $"\nEmail: {account.Email}");

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(message);
                }
            }
        }
        static void searchAccount()
        {
            List<string> fieldTitles = new List<string>();                    
            List<string> fields=null;
            while (true)
            {
                fieldTitles.Clear();
                fieldTitles.Add("Account Number:");
                bool found = false;
                string confirm;
                while (!found)
                {
                    do
                    {
                        fields = Display.getInfo("SEARCH NEW ACCOUNT", "ENTER THE DETAILS", fieldTitles);
                    }
                    while (!Validate.searchAccount(fields));

                    found = File.Exists(string.Format("{0}.txt", fields[INDX_ACCOUNT]));
                   
                    if (!found)
                    {
                        Console.WriteLine("Account not found !");
                        Console.Write("Check another account (y/n): ");
                        confirm = Console.ReadLine();
                        if (confirm == "y") continue;
                        else return;
                    }
                }
                Console.WriteLine("Account found !");
                int accountNumber = int.Parse(fields[INDX_ACCOUNT]);
                Account account = new Account(accountNumber);
                account.load();
                fieldTitles.Clear();
                fieldTitles.Add("Account No: " + account.AccountNumber);
                fieldTitles.Add("Account Balance: $" + account.Balance);
                fieldTitles.Add("First Name: " + account.FirstName);
                fieldTitles.Add("Last Name: " + account.LastName);
                fieldTitles.Add("Address: " + account.Address);
                fieldTitles.Add("Phone: " + account.Phone);
                fieldTitles.Add("Email: " + account.Email);

                Display.printInfo("ACCOUNT DETAILS", "", fieldTitles);
                Console.Write("Check another account (y/n): ");
                confirm = Console.ReadLine();
                if (confirm == "y") continue;
                else return;
            }
            
        }
        static void deposit()
        {
            List<string> fieldTitles = new List<string>();
            List<string> fields = null;          
           
            fieldTitles.Add("Account Number:");
            fieldTitles.Add("Amount: $");
            bool found = false;
            string confirm;
            while (!found)
            {
                do
                {
                    fields = Display.getInfo("DEPOSIT", "ENTER THE DETAILS", fieldTitles);
                }
                while (!Validate.deposit(fields));

                found = File.Exists(string.Format("{0}.txt", fields[INDX_ACCOUNT]));

                if (!found)
                {
                    Console.WriteLine("Account not found !");
                    Console.Write("Retry (y/n): ");
                    confirm = Console.ReadLine();
                    if (confirm == "y") continue;
                    else return;
                }
            }
            Console.WriteLine("Account found !");
            int accountNumber = int.Parse(fields[INDX_ACCOUNT]);
            Account account = new Account(accountNumber);
            account.load();
            account.deposit(double.Parse(fields[INDX_BALANCE]));
            account.save();
                
            Console.WriteLine("Amount has been deposited !");
            Console.WriteLine("New Balance is {0:C}",account.Balance);              

        }
        static void withdraw()
        {
            List<string> fieldTitles = new List<string>();
            List<string> fields = null;

            fieldTitles.Add("Account Number:");
            fieldTitles.Add("Amount: $");
            bool found = false;
            string confirm;
            Account account=null;
            bool overdraft = true;
            double amount=0;
            while (overdraft)
            { 
                while (!found)
                {
                    do
                    {
                        fields = Display.getInfo("WITHDRAW", "ENTER THE DETAILS", fieldTitles);
                    }
                    while (!Validate.deposit(fields));

                    found = File.Exists(string.Format("{0}.txt", fields[INDX_ACCOUNT]));

                    if (!found)
                    {
                        Console.WriteLine("Account not found !");
                        Console.Write("Retry (y/n): ");
                        confirm = Console.ReadLine();
                        if (confirm == "y") continue;
                        else return;
                    }
                }
                Console.WriteLine("Account found !");
                int accountNumber = int.Parse(fields[INDX_ACCOUNT]);
                account = new Account(accountNumber);
                account.load();
                amount = double.Parse(fields[INDX_BALANCE]);
                if (amount > account.Balance)
                {
                    Console.WriteLine("Not enough money to withdraw in the account !");
                    found = false;
                }
                else overdraft = false;
            }
            account.withdraw(amount);
            account.save();

            Console.WriteLine("Amount has been withdrew !");
            Console.WriteLine("New Balance is {0:C}", account.Balance);

        }

        static void statement()
        {
            List<string> fieldTitles = new List<string>();
            List<string> fields = null;
            while (true)
            {
                fieldTitles.Clear();
                fieldTitles.Add("Account Number:");
                bool found = false;
                string confirm;
                while (!found)
                {
                    do
                    {
                        fields = Display.getInfo("STATEMENT", "ENTER THE DETAILS", fieldTitles);
                    }
                    while (!Validate.searchAccount(fields));

                    found = File.Exists(string.Format("{0}.txt", fields[INDX_ACCOUNT]));

                    if (!found)
                    {
                        Console.WriteLine("Account not found !");
                        Console.Write("Enter another account (y/n): ");
                        confirm = Console.ReadLine();
                        if (confirm == "y") continue;
                        else return;
                    }
                }
                Console.WriteLine("Account found !");
                Console.WriteLine("Statement is displayed below ...");
                int accountNumber = int.Parse(fields[INDX_ACCOUNT]);
                Account account = new Account(accountNumber);
                account.load();
                fieldTitles.Clear();
                fieldTitles.Add("Account No: " + account.AccountNumber);
                fieldTitles.Add("Account Balance: $" + account.Balance);
                fieldTitles.Add("First Name: " + account.FirstName);
                fieldTitles.Add("Last Name: " + account.LastName);
                fieldTitles.Add("Address: " + account.Address);
                fieldTitles.Add("Phone: " + account.Phone);
                fieldTitles.Add("Email: " + account.Email);

                Display.printInfo("SIMPLE BANKING SYSTEM", "ACCOUNT STATEMENT", fieldTitles);
                Console.Write("Check another account (y/n): ");
                confirm = Console.ReadLine();
                if (confirm == "y") continue;
                else return;
            }

        }
        static void deleteAccount()
        {
            List<string> fieldTitles = new List<string>();
            List<string> fields = null;

            fieldTitles.Add("Account Number:");            
            bool found = false;
            string confirm;
            Account account = null;
           
            double amount = 0;
            while (!found)
            {
                do
                {
                    fields = Display.getInfo("DELETE AN ACCOUNT", "ENTER THE DETAILS", fieldTitles);
                }
                while (!Validate.searchAccount(fields));

                found = File.Exists(string.Format("{0}.txt", fields[INDX_ACCOUNT]));

                if (!found)
                {
                    Console.WriteLine("Account not found !");
                    Console.Write("Retry (y/n): ");
                    confirm = Console.ReadLine();
                    if (confirm == "y") continue;
                    else return;
                }
            }
            Console.WriteLine("Account found !");
            Console.WriteLine("Details displayed below ...");
            int accountNumber = int.Parse(fields[INDX_ACCOUNT]);
            account = new Account(accountNumber);
            account.load();

            fieldTitles.Clear();
            fieldTitles.Add("Account No: " + account.AccountNumber);
            fieldTitles.Add("Account Balance: $" + account.Balance);
            fieldTitles.Add("First Name: " + account.FirstName);
            fieldTitles.Add("Last Name: " + account.LastName);
            fieldTitles.Add("Address: " + account.Address);
            fieldTitles.Add("Phone: " + account.Phone);
            fieldTitles.Add("Email: " + account.Email);

            Display.printInfo("ACCOUNT DETAILS", "", fieldTitles);
            Console.Write("Delete (y/n): ");
            confirm = Console.ReadLine();
            if (confirm == "y")
            {
                File.Delete(string.Format("{0}.txt", accountNumber));
                Console.WriteLine("Account deleted !");
            }
           

        }

       
    }
}
