using NLog;
using NLog.Config;
using NLog.Targets;
using System;

namespace Bank2
{
    internal class Program
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private static void Main()
        {
            SetupLogging();

            Logger.Info("Program started");

            var pathCSV = "./Data/Transactions2015.csv";
            var transactionsCSV = ParseTransactionFile.ReadFileCSV(pathCSV);


            var pathJSON = "./Data/Transactions2013.json";
            var transactionsJSON = ParseTransactionFile.ReadFileJSON(pathJSON);
            var bank = new Bank(transactionsJSON);

            switch (GetReportOption())
            {
                case "1":
                    Logger.Info("User called all users debts and lends report.");
                    bank.ListAll();
                    break;

                case "2":
                    var username = GetUserName(bank);
                    if (username != "")
                    {
                        bank.ListAccount(username);
                        Logger.Info($"{username}'s transaction report was given.");
                    }
                    break;
            }
        }

        private static void SetupLogging()
        {
            var config = new LoggingConfiguration();
            var target = new FileTarget
            {
                FileName = "../../../Logs/Bank.log",
                Layout = @"${longdate} ${level} - ${logger}: ${message}"
            };
            config.AddTarget("File Logger", target);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, target));
            LogManager.Configuration = config;
        }

        private static string GetReportOption()
        {
            Console.Write("Which report would you like to call? 1) List All 2) User's transactions  :");
            var userInput = Console.ReadLine();
            if (userInput is "1" or "2")
            {
                return userInput;
            }

            Logger.Error($"User input an incorrect option, userInput: {userInput}");
            Console.WriteLine("Invalid input, try again");
            return GetReportOption();
        }

        private static string GetUserName(Bank bank)
        {
            Console.Write("Please input username: ");
            var username = Console.ReadLine();
            Logger.Info($"User called List of {username}'s transactions report.");

            if (bank.Users.Contains(username))
            {
                return username;
            }

            Logger.Error($"{username} does not exist in our database... sorry...");
            Console.Write($"There is no user with name {username} in our database, sorry. Do you want to try again? Type y for yes: ");

            if ((Console.ReadLine() == "y"))
            {
                return GetUserName(bank);
            }

            Logger.Error("User decided not to continue");
            return "";
        }
    }
}