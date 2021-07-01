using NLog;
using System;
using System.Collections.Generic;

namespace Bank2
{
    public class Bank
    {
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
        public List<string> Users { get; set; } = new List<string>();
        public void ListAll()
        {
            foreach (var user in Users)
            {
                decimal debt = 0;
                decimal lend = 0;
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
