using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BankManagement
{
    class Account
    {
        private int accountNumber;
        private double balance;
        private string firstName;
        private string lastName;
        private string address;
        private int phone;
        private string email;
        public Account(int accountNumber, double balance, string firstName,
            string lastName, string address, int phone, string email)
        {
            this.accountNumber = accountNumber;
            this.balance = balance;
            this.firstName = firstName;
            this.lastName = lastName;
            this.address = address;
            this.phone = phone;
            this.email = email;
        }
        public Account(int accountNumber)
        {
            this.accountNumber = accountNumber;            
        }
        public int AccountNumber
        {
            get { return accountNumber;   }
            set { accountNumber = value;  }
        }
        public double Balance
        {
            get { return balance; }
            set { balance = value; }
        }
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }       
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }
        public string Address
        {
            get { return address; }
            set { address = value; }
        }
        public int Phone
        {
            get { return phone; }
            set { phone = value; }
        }
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        public void save()
        {
            TextWriter writer = new StreamWriter(string.Format("{0}.txt",accountNumber));
            //no need to write account number, since file name already contains it           
            writer.WriteLine(balance);
            writer.WriteLine(firstName);
            writer.WriteLine(lastName);
            writer.WriteLine(address);
            writer.WriteLine(phone);
            writer.WriteLine(email);
            writer.Close();
        }
        public void load()
        {
            TextReader reader = new StreamReader(string.Format("{0}.txt", accountNumber));
            balance = double.Parse(reader.ReadLine());
            firstName = reader.ReadLine();
            lastName = reader.ReadLine();
            address = reader.ReadLine();
            phone = int.Parse(reader.ReadLine());
            email = reader.ReadLine();            
            reader.Close();
        }
        public void deposit(double amount)
        {
            balance += amount;
        }
        public void withdraw(double amount)
        {
            balance -= amount;
        }
    }
}
