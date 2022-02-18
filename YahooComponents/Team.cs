using System.Collections.Generic;

namespace YahooDiscordClient.YahooComponents
{
    public class Team
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string Logo { get; set; }
        public string WaiverPriority { get; set; }
        public int MovesCount { get; set; }
        public int TradesCount { get; set; }
        public string DraftGrade { get; set; }
        public int Rank { get; set; }
        public float Pf { get; set; }
        public float Pa { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Ties { get; set; }
        public float Percentage { get; set; }
        public float MatchupPoints { get; set; }
        public float MatchupProjPoints { get; set; }
        public float WinProbability { get; set; }
        public float BudgetTotal { get; set; }
        public float BudgetSpent { get; set; }
        public List<Manager> Managers { get; set; }
        public List<Transaction> LatestTransactions { get; set; }
    }

    /*
    [XmlRoot("team", Namespace = "http://fantasysports.yahooapis.com/fantasy/v2/base.rng")]
    public class Team
    {
        [XmlElement("name")]
        public string Name { get; set; }
    }
    */
}