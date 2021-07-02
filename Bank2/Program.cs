using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Generic;

namespace Bank2
{
    internal class Program
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private static void Main()
        { 
            SetupLogging();

            Logger.Info("Program started");

            string path;
            var transactions = new List<Transaction>();
            switch (GetFileOption())
            {
                case "1":
                    path = "./Data/Transactions2012.xml";
                    transactions = TransactionFileParser.GetTransactionsFromXml(path);
                    break;
                case "2":
                    path = "./Data/Transactions2013.json";
                    transactions = TransactionFileParser.GetTransactionsFromJson(path);
                    break;
                case "3":
                    path = "./Data/Transactions2014.csv";
                    transactions = TransactionFileParser.GetTransactionsFromCsv(path);
                    break;
                case "4":
                    path = "./Data/Transactions2015.csv";
                    transactions = TransactionFileParser.GetTransactionsFromCsv(path);
                    break;
            }

            var bank = new Bank(transactions);
            Console.WriteLine(bank.Transactions.Count);
            Console.WriteLine(transactions.Count);

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

        private static string GetFileOption()
        {
            Console.WriteLine("List of the Transaction files:");
            Console.WriteLine("1) Transaction2012.xml");
            Console.WriteLine("2) Transaction2013.json ");
            Console.WriteLine("3) Transaction2014.csv");
            Console.WriteLine("4) Transaction2015.csv");
            Console.Write("Please select the file (1,2,3,4): ");
            var fileChoice = Console.ReadLine();
            if (fileChoice is "1" or "2" or "3" or "4")
            {
                return fileChoice;
            }

            Logger.Error($"User input an incorrect option, fileChoice: {fileChoice}");
            Console.WriteLine("Invalid input, try again");
            return GetFileOption();
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