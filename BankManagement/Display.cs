using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankManagement
{
    class Display
    {
        private const int WIDTH = 50;
        
        public static void header(string title = "WELOME TO SIMPLE BANKING SYSTEM")
        {
            line();
            center(title);
            line();
        }
        public static void center(string text)
        {
            Console.Write("|");
            int left = (WIDTH - text.Length - 2) / 2;
            for (int i = 0; i < left; i++) Console.Write(" ");
            Console.Write(text);
            for (int i = 0; i < WIDTH - text.Length - 2 - left; i++) Console.Write(" ");
            Console.WriteLine("|");
        }
        public static void print(string text)
        {
            Console.Write("|");            
            for (int i = 0; i < 5; i++) Console.Write(" ");
            Console.Write(text);
            for (int i = 0; i < WIDTH - text.Length - 2 - 5; i++) Console.Write(" ");
            Console.WriteLine("|");
        }
        public static void line()
        {
            for (int i = 0; i < WIDTH; i++) Console.Write("-"); Console.WriteLine();
        }
        public static string menu()
        {
            header();
            print("1. Create new account");
            print("2. Search for an account");
            print("3. Deposit");
            print("4. Withdraw");
            print("5. A/C statement");
            print("6. Delete account");
            print("7. Exit");
            line();
            print("Enter your choice (1-7):");
            line();
            Console.SetCursorPosition(31, Console.CursorTop - 2);           
            string choiceStr= Console.ReadLine();
            Console.SetCursorPosition(0, Console.CursorTop+2);
            return choiceStr;
        }
        public static List<string> getInfo(string header1, string header2, List<string> fieldTitles)
        {
            header(header1);
            center(header2);
            print("");
            foreach (string field in fieldTitles) print(field);
            line();
            List<string> fields = new List<string>();
            Console.SetCursorPosition(0, Console.CursorTop - fieldTitles.Count - 1);
            for (int i = 0; i < fieldTitles.Count; i++)
            {
                Console.SetCursorPosition(7 + fieldTitles[i].Length, Console.CursorTop);
                fields.Add(Console.ReadLine());
            }          
            Console.SetCursorPosition(0, Console.CursorTop + 2);           
            return fields;
        }
        public static void printInfo(string header1, string header2, List<string> fieldTitles)
        {
            header(header1);
            center(header2);
            print("");
            foreach (string field in fieldTitles) print(field);
            line();
            Console.WriteLine();         
        }
    }
}
