using System.Collections.Generic;

namespace YahooDiscordClient.YahooComponents
{
    public static class League
    {
        public static string Name { get; set; }
        public static string LogoUrl { get; set; }
        public static int TeamsCount { get; set; }
        public static int CurrentWeek { get; set; }
        public static int StartWeek { get; set; }
        public static string StartDate { get; set; }
        public static int EndWeek { get; set; }
        public static string EndDate { get; set; }
        public static int Season { get; set; }

        public static int TransactionsCount { get; set; }

        public static List<Team> Teams { get; set; }

        public static List<Matchup> Matchups { get; set; }
        public static List<Transaction> LatestTransactions { get; set; }
    }

    /*
    [XmlRoot("league", Namespace = "http://fantasysports.yahooapis.com/fantasy/v2/base.rng")]
    public class League
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("logo_url")]
        public string LogoUrl { get; set; }

        [XmlElement("num_teams")]
        public int TeamsCount { get; set; }
    }
    */
}