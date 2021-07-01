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
            SetupLogging();

            Logger.Info("Program started");

            var bank = new Bank();
            var path = "./Data/Transactions2015.csv";

            bank.Transactions = ReadFile(path);
            bank.Users = CreateListOfUsers(bank);

            var userChoice = UserChoiceInput();

            switch (userChoice)
            {
                case "1":
                    Logger.Info("User called all users debts and lends report.");
                    bank.ListAll();
                    break;

                case "2":
                    var username = UserNameInput(bank); 
                    bank.ListAccount(username);
                    Logger.Info($"{username}'s transaction report was given.");
                    break;
            }
        }

        private static void SetupLogging()
        {
            var config = new LoggingConfiguration();
            var target = new FileTarget
            {
                //FileName = @"C:\Users\eleevd\Training\Bank2\Bank2\Logs\Bank.log",
                FileName = @"C:\Users\suwtru\Training\Bank2\Bank2\Logs\Bank.log",
                Layout = @"${longdate} ${level} - ${logger}: ${message}"
            };
            config.AddTarget("File Logger", target);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, target));
            LogManager.Configuration = config;
        }

        private static List<Bank2.Transaction> ReadFile(string path)
        {
            var lines = new string[] { };
            try
            {
                lines = File.ReadAllLines(path);
                lines = lines.Skip(1).ToArray();
                Logger.Info("The program has successfully read the file.");
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
            };
            return lines.Select(line => new Transaction(line.Split(','))).ToList();
        }

        private static List<String> CreateListOfUsers(Bank bank)
        {
            var userNames = new List<string>();
            var transactionToNames = bank.Transactions.Select(t => t.To);
            var transactionFromNames = bank.Transactions.Select(t => t.From);
            var users = transactionToNames.Concat(transactionFromNames).Distinct().Select(name => name);
            userNames.AddRange(users);
            return userNames;
        }

        private static string UserChoiceInput()
        {
            Console.Write("Which report would you like to call? 1) List All 2) User's transactions  :");
            var userInput = Console.ReadLine();
            while (!(userInput == "1" || userInput == "2"))
            {
                Logger.Error($"User input an incorrect option, userInput: {userInput}");
                Console.WriteLine("Invalid input, please select 1) List All or 2) User's transactions. Type 1 or 2:  ");
                userInput = Console.ReadLine();
            }
            return userInput;
        }

        private static String UserNameInput(Bank bank)
        {
            var tryAgain = true;

            Console.Write("Please input username: ");
            var username = Console.ReadLine();
            Logger.Info($"User called List of {username}'s transactions report.");

            while (!bank.Users.Contains(username) && tryAgain)
            {
                Logger.Error($"{username} does not exist in our database... sorry...");
                Console.Write($"There is no user with name {username} in our database, sorry. Do you want to try again? Type y for yes: ");
                tryAgain = (Console.ReadLine() == "y");
                if (!tryAgain)
                {
                    Logger.Error("User decided not to continue");
                }
                else
                {
                    Console.Write("Please input username: ");
                    username = Console.ReadLine();
                    Logger.Info($"User called List of {username}'s transactions report.");
                }
            }
            return username;
        }
    }
}