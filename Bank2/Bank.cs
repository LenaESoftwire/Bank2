using System;
using System.Collections.Generic;
using System.Linq;

namespace Bank2
{
    public class Bank
    {
        public List<Transaction> Transactions { get; set; } 
        public List<string> Users { get; set; }

        public Bank (List<Transaction> transactions)
        {
            Transactions = transactions;
            Users = GetUserNames();
        }

        private List<string> GetUserNames()
        {
            var transactionToNames = Transactions.Select(t => t.ToAccount);
            var transactionFromNames = Transactions.Select(t => t.FromAccount);
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
                    if (user == transaction.ToAccount)
                    {
                        debt += transaction.Amount;
                    }

                    if (user == transaction.FromAccount)
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
                if (username == transaction.ToAccount || username == transaction.FromAccount)
                {
                    transaction.PrintTransaction();
                }
            }
        }
    }

}
