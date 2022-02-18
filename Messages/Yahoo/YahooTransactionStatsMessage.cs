using Discord;
using System.Linq;
using System.Text;
using YahooDiscordClient.Parsers;
using YahooDiscordClient.Parsers.Xml.Yahoo;
using YahooDiscordClient.YahooComponents;

namespace YahooDiscordClient.Messages.Yahoo
{
    public class YahooTransactionStatsMessage : YahooMessage<Embed>
    {
        public YahooTransactionStatsMessage() : base()
        {
        }

        public override Embed CreateMessage()
        {
            var sortedTeams = League.Teams.OrderByDescending(team => team.Rank);

            var embedBuilder = new EmbedBuilder
            {
                Title = "TRANSACTION STATS",
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
                $"[Waiver Rank]:{team.WaiverPriority,10}\n" +
                $"[Budget Rem]:{team.BudgetTotal,11:C0}\n" +
                $"[Moves]:{team.MovesCount,16}\n" +
                $"[Trades]:{team.TradesCount,15}" +
                $"```");

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