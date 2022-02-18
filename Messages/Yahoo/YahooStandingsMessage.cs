using Discord;
using System.Linq;
using System.Text;
using YahooDiscordClient.Parsers.Xml;
using YahooDiscordClient.Parsers.Xml.Yahoo;
using YahooDiscordClient.YahooComponents;

namespace YahooDiscordClient.Messages.Yahoo
{
    public class YahooStandingsMessage : YahooMessage<Embed>
    {
        public YahooStandingsMessage() : base()
        {
        }

        public override Embed CreateMessage()
        {
            IOrderedEnumerable<Team> sortedTeams;
            sortedTeams = League.Teams.OrderBy(team => team.Rank);

            var embedBuilder = new EmbedBuilder
            {
                Title = "STANDINGS",
                Color = Color.DarkBlue
            };

            embedBuilder.WithAuthor(League.Name);

            for (int i = 0; i < League.Teams.Count; i++)
            {
                StringBuilder s = new();
                string rank = string.Format("{0, -3}", $"{i + 1}.");
                var team = sortedTeams.ElementAt(i);
                string managers = team.Managers[0].Nickname;

                if (team.Managers.Count > 1)
                {
                    managers = $"{managers}/{team.Managers[1].Nickname}";
                }

                s.AppendLine(
                    $"```" +
                    $"[PCT]:{team.Percentage,17:0.000}\n" +
                    $"[PF]:{team.Pf,18:0.00}\n" +
                    $"[PA]:{team.Pa,18:0.00}\n" +
                    $"[PF/G]:{team.Pf / 12,16:0.00}\n" +
                    $"[PA/G]:{team.Pa / 12,16:0.00}" +
                    $"```");

                embedBuilder.AddField($"{rank} {managers.ToUpper()} ({team.Wins}-{team.Losses}-{team.Ties})", s.ToString());
            }
            return embedBuilder.Build();
        }

        protected override XmlParser CreateParser()
        {
            return new YahooStandingsXmlParser();
        }
    }
}