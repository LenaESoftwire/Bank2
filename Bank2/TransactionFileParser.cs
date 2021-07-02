using System;
using System.IO;
using System.Collections.Generic;
using NLog;
using System.Linq;
using Newtonsoft.Json;

namespace Bank2
{

    public class TransactionFileParser
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public static List<Transaction> ReadFileJSON(string path)
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

        public static List<Transaction> ReadFileCSV(string path)
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
    }
}
