using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using YahooDiscordClient.Extensions;
using YahooDiscordClient.YahooComponents;

namespace YahooDiscordClient.Parsers.Xml.Yahoo
{
    public class YahooScoreboardXmlParser : YahooXmlParser
    {
        protected override string ApiResourcePath => Constants.ScoreboardResourcePath;

        public YahooScoreboardXmlParser() : base()
        {
        }

        protected override void ParseResource(string text)
        {
            League.Matchups = new List<Matchup>();
            var matchupElements = Document.Root.GetChild("league").Descendants(Constants.YahooNs + "matchup").ToList();

            foreach (XElement matchupElement in matchupElements)
            {
                Matchup matchup = new();
                matchup.PreEvent = matchupElement.GetChild("status").Value == "preevent";
                matchup.PostEvent = matchupElement.GetChild("status").Value == "postevent";

                matchup.Teams = new();
                var teamElements = matchupElement.Descendants(Constants.YahooNs + "team").ToList();
                foreach (XElement teamElement in teamElements)
                {
                    Team team = new();
                    team.Name = teamElement.GetChildValue("name");
                    team.Id = int.Parse(teamElement.GetChildValue("team_id"));
                    team.Logo = teamElement.GetChild("team_logos").GetChild("team_logo").GetChildValue("url");
                    team.WaiverPriority = teamElement.GetChildValue("waiver_priority");
                    team.MovesCount = int.Parse(teamElement.GetChildValue("number_of_moves"));
                    team.TradesCount = int.Parse(teamElement.GetChildValue("number_of_trades"));
                    team.DraftGrade = teamElement.GetChildValue("draft_grade");
                    team.WinProbability = float.Parse(teamElement.GetChildValue("win_probability"));

                    team.Managers = new List<Manager>();
                    var managers = teamElement.Descendants(Constants.YahooNs + "managers").ToList();
                    foreach (XElement managerElement in managers)
                    {
                        var tmp = managerElement.GetChild("manager");
                        Manager manager = new();
                        manager.ID = int.Parse(tmp.GetChildValue("manager_id"));
                        manager.Nickname = tmp.GetChildValue("nickname");
                        manager.Image = tmp.GetChildValue("image_url");
                        manager.FeloScore = int.Parse(tmp.GetChildValue("felo_score"));
                        manager.FeloTier = tmp.GetChildValue("felo_tier");
                        team.Managers.Add(manager);
                    }

                    team.MatchupPoints = float.Parse(teamElement.GetChild("team_points").GetChildValue("total"));
                    team.MatchupProjPoints = float.Parse(teamElement.GetChild("team_projected_points").GetChildValue("total"));
                    matchup.Teams.Add(team);
                }

                League.Matchups.Add(matchup);
            }
        }
    }
}