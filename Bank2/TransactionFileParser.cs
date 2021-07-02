using System;
using System.IO;
using System.Collections.Generic;
using NLog;
using System.Linq;
using Newtonsoft.Json;
using System.Xml;

namespace Bank2
{

    public class TransactionFileParser
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public static List<Transaction> GetTransactionsFromJson(string path)
        {
            try
            {
                var json = File.ReadAllText(path);
                Logger.Info("The program has successfully read the file.");
                return JsonConvert.DeserializeObject<List<Transaction>>(json);
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                return new List<Transaction>();
            };
        }

        public static List<Transaction> GetTransactionsFromCsv(string path)
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

        public static List<Transaction> GetTransactionsFromXml(string path)
        {
            try
            {
                var xmlFile = new XmlDocument();
                xmlFile.Load(path);

                var nodes = xmlFile.SelectNodes("TransactionList/SupportTransaction");
                var transactions = new List<Transaction>();

                foreach (XmlElement node in nodes)
                {
                    var transactionInfo = new string[5];
                    transactionInfo[0] = node.GetAttribute("Date");
                    transactionInfo[1] = node.SelectSingleNode("Description")?.InnerText;
                    transactionInfo[2] = node.SelectSingleNode("Value")?.InnerText;
                    transactionInfo[3] = node.SelectSingleNode("Parties/From")?.InnerText;
                    transactionInfo[4] = node.SelectSingleNode("Parties/From")?.InnerText;
                    var transaction = new Transaction(transactionInfo);

                    transactions.Add(transaction);
                }
                return transactions;
               
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                return new List<Transaction>();
            };
        }
    }
}
