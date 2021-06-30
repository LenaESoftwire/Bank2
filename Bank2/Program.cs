using System.Transactions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;

namespace Bank2

{
    class Program
    {
         
        static void Main()
        {
            var bank = new Bank();
            var path = "./Data/Transactions2014.csv";
            var lines = File.ReadAllLines(path);
            lines = lines.Skip(1).ToArray();
            bank.Transactions = lines.Select(line => new Transaction(line.Split(','))).ToList();

            var userNames = new List<string>();
            foreach (var transaction in bank.Transactions)
            {
                if (!userNames.Contains(transaction.To))
                {
                    userNames.Add(transaction.To);
                }

                if (!userNames.Contains(transaction.From))
                {
                    userNames.Add(transaction.From);
                }
            }

            foreach (var username in userNames)
            {
                var user = new UserAccount(username);
                bank.Users.Add(user);
                bank.CountDebtLend(user);
            }

            Console.Write("Which report would you like to call? 1) List All 2) User's transactions  :");
            var userInput = Console.ReadLine();
            if (userInput == "1")
            {
                bank.ListAll();
            }
           if (userInput == "2")
            {
                Console.Write("Please input username: ");
                var username = Console.ReadLine();
                bank.ListAccount(username);
            }

        }  
    }
}
