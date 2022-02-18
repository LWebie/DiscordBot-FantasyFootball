using System.Collections.Generic;

namespace YahooDiscordClient.YahooComponents
{
    public class Matchup
    {
        public int Week { get; set; }
        public bool PreEvent { get; set; }
        public bool PostEvent { get; set; }
        public int IsPlayoffs { get; set; }
        public int IsConsolation { get; set; }
        public List<Team> Teams { get; set; }
    }
}