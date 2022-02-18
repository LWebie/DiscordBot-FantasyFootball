using Discord;
using System.Linq;
using System.Text;
using YahooDiscordClient.Parsers;
using YahooDiscordClient.Parsers.Xml.Yahoo;
using YahooDiscordClient.YahooComponents;

namespace YahooDiscordClient.MessageBuilders.Yahoo
{
    public class YahooWinLossMessageBuilder : YahooMessageBuilder<Embed>
    {
        public YahooWinLossMessageBuilder() : base()
        {
        }

        protected override string ApiResourcePath => Constants.StandingsResourcePath;

        public override Embed CreateMessage()
        {
            var sortedTeams = League.Teams.OrderByDescending(team => team.Wins);

            var embedBuilder = new EmbedBuilder
            {
                Title = "W/L STANDINGS",
                Color = Color.DarkBlue
            };
            embedBuilder.WithAuthor(League.Name);

            for (int i = 0; i < League.Teams.Count; i++)
            {
                StringBuilder s = new();
                var rank = string.Format("{0, -3}", $"{i + 1}.");
                var team = sortedTeams.ElementAt(i);
                var managers = team.Managers[0].Nickname;

                if (team.Managers.Count > 1)
                {
                    managers = $"{managers}/{team.Managers[1].Nickname}";
                }
                s.AppendLine($"```{team.Wins}-{team.Losses}-{team.Ties}```");
                embedBuilder.AddField($"{rank} {managers.ToUpper()}", s.ToString());
            }

            return embedBuilder.Build();
        }

        protected override IParser CreateParser()
        {
            return new YahooStandingsXmlParser();
        }
    }
}