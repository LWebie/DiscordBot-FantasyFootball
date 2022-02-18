using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace YahooDiscordClient
{
    public class Constants
    {
        public const string Issuer = Audiance;
        public static string AccessToken { get; set; }
        public static string RefreshToken { get; set; }
        public static XNamespace YahooNs { get; set; }

        /* These will be made more secure. For testing purposes hardcoded values are used. */
        // *************************************
        public const string Audiance = "https://localhost:44375/";
        public const string ClientId = "";
        public const string ClientSecret = "";
        public const string StandingsResourcePath = "https://fantasysports.yahooapis.com/fantasy/v2/league/406.l.INSERTLEAGUEID/standings";
        public const string TestStandingsResourcePath = "https://fantasysports.yahooapis.com/fantasy/v2/league/.l.INSERTLEAGUEID/standings";
        public const string ScoreboardResourcePath = "https://fantasysports.yahooapis.com/fantasy/v2/league/406.l.INSERTLEAGUEID/scoreboard";
        public const string TestTransactionsResourcePath = "https://fantasysports.yahooapis.com/fantasy/v2/league/.l.INSERTLEAGUEID/transactions";
        public const string TransactionsResourcePath = "https://fantasysports.yahooapis.com/fantasy/v2/league/406.l.INSERTLEAGUEID/transactions";
        //public const string MatchupsResourcePath = "https://fantasysports.yahooapis.com/fantasy/v2/league/406.l.INSERTLEAGUEID/matchups";
        public const int LeagueId = 0;
        public static List<ulong> ChannelIds = new() { 0 };
        // *************************************
    }
}
