using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Bank2
{
    internal class Program
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private static void Main()
        {
            var config = new LoggingConfiguration();
            var target = new FileTarget
            {
                FileName = @"C:\Users\eleevd\Training\Bank2\Bank2\Logs\Bank.log",
                Layout = @"${longdate} ${level} - ${logger}: ${message}"
            };
            config.AddTarget("File Logger", target);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, target));
            LogManager.Configuration = config;

            Logger.Info("Program started");

            var bank = new Bank();
            var path = "./Data/Transactions2015.csv";
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
