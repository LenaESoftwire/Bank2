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

        public static void GetTransactionsFromXml(string path)
        {
            try
            {
                XmlReader xmlReader = new XmlTextReader(path);
                while (xmlReader.Read())
                {
                    if ((xmlReader.NodeType == XmlNodeType.Element) && (xmlReader.Name == "SupportTransaction"))
                    {
                        if (xmlReader.HasAttributes)
                            Console.WriteLine("Date: " + xmlReader.GetAttribute("Date"));
                    }
                   
                    if ((xmlReader.NodeType == XmlNodeType.Element) && (xmlReader.Name == "From"))
                    {
                        Console.WriteLine("From: " + xmlReader.ReadInnerXml());
                    }

                    if ((xmlReader.NodeType == XmlNodeType.Element) && (xmlReader.Name == "To"))
                    {
                        Console.WriteLine("To: " + xmlReader.ReadInnerXml());
                    }

                    if ((xmlReader.NodeType == XmlNodeType.Element) && (xmlReader.Name == "Description"))
                    {
                        Console.WriteLine("Narrative: " + xmlReader.ReadInnerXml());
                    }

                    if ((xmlReader.NodeType == XmlNodeType.Element) && (xmlReader.Name == "Value"))
                    {
                        Console.WriteLine("Amount: " + xmlReader.ReadInnerXml());
                    }
                }
                //XmlTextReader xmlReader = new XmlTextReader(path);
               
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                //return new List<Transaction>();
            };
        }
    }
}
