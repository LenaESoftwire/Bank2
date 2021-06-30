using NLog;

namespace Bank2
{
    public class UserAccount
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        public string Name { get; set; }
        public decimal Debt { get; set; }
        public decimal Lend { get; set; }

        public UserAccount(string name)
        {
            Name = name;
        }
    }
}