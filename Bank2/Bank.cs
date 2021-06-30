using System.Collections.Generic;
using System;

namespace Bank2
{
    public class Bank
    {
        public List<Transaction> Transactions { get; set; }
        public List<UserAccount> Users { get; set; }

        public Bank()
        {
            Transactions = new List<Transaction>();
            Users = new List<UserAccount>();
        }

        public void ListAll()
        {
            foreach (var user in Users)
            {
                Console.WriteLine(user.Name + " debt " + user.Debt + " lend " + user.Lend);
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
        public void CountDebtLend(UserAccount user)
        {
            foreach (var transaction in Transactions)
            {
                if (user.Name == transaction.To)
                {
                    user.Debt += transaction.Amount;
                }

                if (user.Name == transaction.From)
                {
                    user.Lend += transaction.Amount;
                }
            }
        }
    }
}