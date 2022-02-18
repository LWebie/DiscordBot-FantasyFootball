using System.Collections.Generic;

namespace YahooDiscordClient.YahooComponents
{
    public class Transaction
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public List<Player> Players { get; set; }
    }
}