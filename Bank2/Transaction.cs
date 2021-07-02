using System;
using Newtonsoft.Json;
using NLog;

namespace Bank2
{
    public class Transaction
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public string Date { get; set; }

        [JsonProperty("FromAccount")]
        public string FromAccount { get; set; }

        [JsonProperty("ToAccount")]
        public string ToAccount { get; set; }

        public string Narrative { get; set; }

        public decimal Amount { get; set; }

        public Transaction()
        {

        }

        public Transaction(string[] columns)
        {
            Date = columns[0];
            FromAccount = columns[1];
            ToAccount = columns[2];
            Narrative = columns[3];

            if (decimal.TryParse(columns[4], out var result))
            {
                Amount = result;
            }
            else
            {
                Logger.Error($"Transaction on {Date} from {FromAccount} to {ToAccount} has a non decimal amount: {columns[4]}");
            }
        }

        public void PrintTransaction() => Console.WriteLine($"{Date} from {FromAccount} To {ToAccount} Narrative {Narrative} Amount {Amount}");
    }
}