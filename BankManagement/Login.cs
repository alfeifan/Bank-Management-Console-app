using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BankManagement
{
    class Login
    {
        private string fileName;
        private Dictionary<string, string> users;
        public Login(string fileName)
        {
            this.fileName = fileName;
            //read users and passwords to dictionary
            users = new Dictionary<string, string>();
            TextReader reader = new StreamReader(fileName);
            string line=reader.ReadLine(); ;
            while (line != null)
            {
                string[] s = line.Split(new char[] { '|', '\n', '\r' });
                if(!users.ContainsKey(s[0])) users.Add(s[0], s[1]);
                line = reader.ReadLine();                
            }
            reader.Close();
        }
        public bool signIn()
        {            
            Display.header();
            Display.center("LOGIN TO START");
            Display.print("");
            Display.print("User Name:");
            Display.print("Password:");
            Display.line();           
            
            Console.SetCursorPosition(17, Console.CursorTop - 3);
            string userName = Console.ReadLine();
            Console.SetCursorPosition(17, Console.CursorTop);
            string password = readPassword();
            Console.SetCursorPosition(1, Console.CursorTop+2);
            return (users.ContainsKey(userName) && users[userName] == password);
        }
        private string readPassword()
        {
            string password = "";
            ConsoleKeyInfo kb = Console.ReadKey(true);
            while (kb.Key != ConsoleKey.Enter)
            {
                if (kb.Key != ConsoleKey.Backspace)
                {
                    Console.Write("*");
                    password += kb.KeyChar;
                }
                else if (kb.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    // delete last letter from string
                    password = password.Substring(0, password.Length - 1);                        
                    int pos = Console.CursorLeft;
                    // move the cursor to the left
                    Console.SetCursorPosition(pos - 1, Console.CursorTop);
                    // delete last letter from screen
                    Console.Write(" ");
                    // move the cursor to the left
                    Console.SetCursorPosition(pos - 1, Console.CursorTop);                   
                }
                kb = Console.ReadKey(true);
            }           
            Console.WriteLine();
            return password;
        }
    }
}
