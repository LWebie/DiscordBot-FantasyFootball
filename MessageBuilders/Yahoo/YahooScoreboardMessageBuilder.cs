using Discord;
using System.Linq;
using System.Text;
using YahooDiscordClient.Parsers;
using YahooDiscordClient.Parsers.Xml.Yahoo;
using YahooDiscordClient.YahooComponents;

namespace YahooDiscordClient.MessageBuilders.Yahoo
{
    public class YahooScoreboardMessageBuilder : YahooMessageBuilder<Embed>
    {
        protected override string ApiResourcePath => Constants.ScoreboardResourcePath;

        public override Embed CreateMessage()
        {
            var title = League.Matchups.Any(matchup => !matchup.PreEvent) ? $"WEEK {League.CurrentWeek} SCOREBOARD" : $"WEEK {League.CurrentWeek} MATCHUPS";
            var embedBuilder = new EmbedBuilder
            {
                Title = title,
                Color = Color.DarkBlue
            };
            embedBuilder.WithAuthor(League.Name);

            for (int i = 0; i < League.Matchups.Count; i++)
            {
                StringBuilder s = new();
                string matchupHeader = $"{League.Matchups[i].Teams[0].Name} "
                                    + $"({League.Matchups[i].Teams[0].Wins}-{League.Matchups[i].Teams[0].Losses}-{League.Matchups[i].Teams[0].Ties})"
                                    + " vs "
                                    + $"{League.Matchups[i].Teams[1].Name} "
                                    + $"({League.Matchups[i].Teams[1].Wins}-{League.Matchups[i].Teams[1].Losses}-{League.Matchups[i].Teams[1].Ties})";

                var sortedTeamsByPf = League.Matchups[i].Teams.OrderByDescending(team => team.MatchupPoints);

                string lineVal = $"```" + $"{sortedTeamsByPf.ElementAt(0).Name}\n";
                if (!League.Matchups[i].PreEvent)
                {
                    lineVal += $"- {sortedTeamsByPf.ElementAt(0).MatchupPoints}\n";
                }

                lineVal += $"- {sortedTeamsByPf.ElementAt(0).MatchupProjPoints} [Proj]\n\n" + $"{sortedTeamsByPf.ElementAt(1).Name}\n";

                if (!League.Matchups[i].PreEvent)
                {
                    lineVal += $"- {sortedTeamsByPf.ElementAt(1).MatchupPoints}\n";
                }

                lineVal += $"- {sortedTeamsByPf.ElementAt(1).MatchupProjPoints} [Proj]" + "```";

                s.AppendLine(lineVal);
                embedBuilder.AddField(matchupHeader, s.ToString());
                /*
                s.AppendLine(
                $"```" +
                $"[]:{League.Key.WaiverRank,12}\n" +
                $"[Waiver Rank]:{League.Key.WaiverRank,12}\n" +
                $"```");
                */
            }

            return embedBuilder.Build();
        }

        protected override IParser CreateParser()
        {
            return new YahooScoreboardXmlParser();
        }
    }
}