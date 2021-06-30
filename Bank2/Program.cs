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
            try
            {
                var lines = File.ReadAllLines(path);
                Logger.Info("The program has successfully read the file.");
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


                var userInput = "";
                while (!(userInput == "1" || userInput == "2"))
                {
                    Console.Write("Which report would you like to call? 1) List All 2) User's transactions  :");
                    userInput = Console.ReadLine();
                    if (userInput == "1")
                    {
                        Logger.Info("User called all users debts and lends report.");
                        bank.ListAll();
                    }

                    else if (userInput == "2")
                    {
                        bool tryAgain=true;
                        var username = "";

                        while (!userNames.Contains(username) && tryAgain)
                        {
                            Console.Write("Please input username: ");
                            username = Console.ReadLine();
                            bank.ListAccount(username);
                            Logger.Info($"User called List of {username}'s transactions report.");
                            if (userNames.Contains(username))
                            {
                                bank.ListAccount(username);
                                Logger.Info($"{username}'s transaction report was given.");
                            }
                            else
                            {
                                Logger.Error($"{username} does not exist in our database... sorry...");
                                Console.Write($"There is no user with name {username} in our database, sorry. Do you want to try again? Type y for yes");
                                tryAgain = (Console.ReadLine() == "y");
                                if (!tryAgain)
                                {
                                    Logger.Error("User decided not to continue");
                                }
                            }
                        }
                    }
                    else
                    {
                        Logger.Error($"User input an incorrect option, userInput: {userInput}");
                        Console.WriteLine("Invalid input, please try again!");
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
            };
        }
    }
}
