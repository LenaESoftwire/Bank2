using System;
using System.Collections.Generic;
using System.IO;
using NLog;
using System.Linq;

namespace Bank2
{
    public class Bank
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        public List<Transaction> Transactions { get; set; } 
        public List<string> Users { get; set; }

        public Bank (string path)
        {
            Transactions = ReadFile(path);
            Users = GetUsernames();
        }

        private static List<Transaction> ReadFile(string path)
        {
            try
            {
                var lines = File.ReadAllLines(path);
                lines = lines.Skip(1).ToArray();
                Logger.Info("The program has successfully read the file.");
                return lines.Select(line => new Transaction(line.Split(','))).ToList();
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                return new List<Transaction>();
            };
        }

        private List<string> GetUsernames()
        {
            var transactionToNames = Transactions.Select(t => t.To);
            var transactionFromNames = Transactions.Select(t => t.From);
            return transactionToNames.Concat(transactionFromNames).Distinct().ToList();
        }

        public void ListAll()
        {
            foreach (var user in Users)
            {
                var debt = 0M;
                var lend = 0M;
                foreach (var transaction in Transactions)
                {
                    if (user == transaction.To)
                    {
                        debt += transaction.Amount;
                    }

                    if (user == transaction.From)
                    {
                        lend += transaction.Amount;
                    }
                }
                Console.WriteLine($"{user} debt is {debt} lend {lend}");
            }
        }

        public void ListAccount(string username)
        {
            foreach (var transaction in Transactions)
            {
                if (username == transaction.To || username == transaction.From)
                {
                    transaction.PrintTransaction();
                }
            }
        }
    }

}
