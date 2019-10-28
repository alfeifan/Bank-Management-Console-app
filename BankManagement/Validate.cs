using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BankManagement
{
    class Validate
    {
        private static Random rnd = new Random();
        private  const int LOWER = 100000;
        private  const int UPPER = 10000000;
        public static bool newAccount(List<string> fields)
        {
            int phone = 0;
            if (!isInteger(fields[3], ref phone) || fields[3].Length > 10)
            {
                Console.WriteLine("Invalid phone !");
                return false;
            }
            if (!fields[4].Contains("@"))
            {
                Console.WriteLine("Invalid email !");
                return false;
            }
            return true;
        }
        public static bool searchAccount(List<string> fields)
        {
            int account = 0;
            if (!isInteger(fields[0], ref account) || fields[0].Length > 10)
            {
                Console.WriteLine("Invalid account !");
                return false;
            }            
            return true;
        }
        public static bool deposit(List<string> fields)
        {
            int account = 0;
            if (!isInteger(fields[0], ref account) || fields[0].Length > 10)
            {
                Console.WriteLine("Invalid account !");
                return false;
            }
            double amount = 0;
            if (!isDouble(fields[1], ref amount))
            {
                Console.WriteLine("Invalid amount !");
                return false;
            }
            if (amount <= 0)
            {
                Console.WriteLine("Invalid amount !");
                return false;
            }
            return true;
        }
        public static bool isInteger(string s, ref int number)
        {
            try
            {
                number = int.Parse(s);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public static bool isDouble(string s, ref double number)
        {
            try
            {
                number = double.Parse(s);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public static int generateAccount()
        {
            int id = rnd.Next(LOWER, UPPER);
            while(File.Exists(String.Format("{0}.txt", id)))
                id = rnd.Next(LOWER, UPPER);
            return id;
        }
    }
}
