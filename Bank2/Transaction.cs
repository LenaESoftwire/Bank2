using NLog;
using System;

namespace Bank2
{
    public class Transaction
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        public string Date { get; set; }
        public string To { get; set; }
        public string From { get; set; }
        public string Narrative { get; set; }
        public decimal Amount { get; set; }

        public Transaction(string[] columns)
        {
            Date = columns[0];
            From = columns[1];
            To = columns[2];
            Narrative = columns[3];

            if (decimal.TryParse(columns[4], out var result))
            {
                Amount = result;
            }
            else
            {
                Logger.Error($"Transaction on {Date} from {From} to {To} has a non decimal amount: {columns[4]}");
            }
        }

        public void PrintTransaction() => Console.WriteLine($"{Date} from {From} To {To} Narrative {Narrative} Amount {Amount}");
    }
}