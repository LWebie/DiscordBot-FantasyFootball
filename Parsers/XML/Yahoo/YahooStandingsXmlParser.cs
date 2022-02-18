using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using YahooDiscordClient.Extensions;
using YahooDiscordClient.YahooComponents;

namespace YahooDiscordClient.Parsers.Xml.Yahoo
{
    public class YahooStandingsXmlParser : YahooXmlParser
    {
        protected override string ApiResourcePath => Constants.StandingsResourcePath;

        public YahooStandingsXmlParser() : base()
        {
        }

        protected override void ParseResource(string text)
        {
            League.Teams = new List<Team>();
            var teamElements = Document.Root.GetChild("league").Descendants(Constants.YahooNs + "team").ToList();

            foreach (XElement teamElement in teamElements)
            {
                Team team = new();
                team.Name = teamElement.GetChildValue("name");
                team.Id = int.Parse(teamElement.GetChildValue("team_id"));
                team.Logo = teamElement.GetChild("team_logos").GetChild("team_logo").GetChildValue("url");
                team.WaiverPriority = teamElement.GetChildValue("waiver_priority");
                team.MovesCount = int.Parse(teamElement.GetChildValue("number_of_moves"));
                team.TradesCount = int.Parse(teamElement.GetChildValue("number_of_trades"));
                if (teamElement.GetChildValue("has_draft_grade") == "1") { team.DraftGrade = teamElement.GetChildValue("draft_grade"); }
                //team.BudgetTotal = float.Parse(teamElement.GetChildValue("auction_budget_total"));
                //team.BudgetSpent = float.Parse(teamElement.GetChildValue("auction_budget_spent"));

                team.Managers = new List<Manager>();
                var managers = teamElement.Descendants(Constants.YahooNs + "managers").ToList();
                foreach (XElement managerElement in managers)
                {
                    var tmp = managerElement.GetChild("manager");
                    Manager manager = new();
                    manager.ID = int.Parse(tmp.GetChildValue("manager_id"));
                    manager.Nickname = tmp.GetChildValue("nickname");
                    //manager.Image = tmp.GetChildValue("image_url");
                    manager.FeloScore = int.Parse(tmp.GetChildValue("felo_score"));
                    manager.FeloTier = tmp.GetChildValue("felo_tier");
                    team.Managers.Add(manager);
                }

                var standings = teamElement.GetChild("team_standings");
                team.Rank = int.TryParse(standings.GetChildValue("rank"), out int rank) ? rank : 0;
                team.Pf = float.Parse(standings.GetChildValue("points_for"));
                team.Pa = float.Parse(standings.GetChildValue("points_against"));
                team.Wins = int.Parse(standings.GetChild("outcome_totals").GetChildValue("wins"));
                team.Losses = int.Parse(standings.GetChild("outcome_totals").GetChildValue("losses"));
                team.Ties = int.Parse(standings.GetChild("outcome_totals").GetChildValue("ties"));
                team.Percentage = float.TryParse(standings.GetChild("outcome_totals").GetChildValue("percentage"), out float percentage)
                    ? percentage
                    : 0.00f;
                League.Teams.Add(team);
            }
        }
    }
}