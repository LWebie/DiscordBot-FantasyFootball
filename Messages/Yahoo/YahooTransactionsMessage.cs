using Discord;
using System.Text;
using YahooDiscordClient.Parsers;
using YahooDiscordClient.Parsers.Xml.Yahoo;
using YahooDiscordClient.YahooComponents;

namespace YahooDiscordClient.Messages.Yahoo
{
    public class YahooTransactionsMessage : YahooMessage<Embed>
    {
        public YahooTransactionsMessage() : base()
        {
        }

        public override Embed CreateMessage()
        {
            string title = (League.LatestTransactions.Count > 1) ? "TRANSACTIONS" : "TRANSACTION";
            var embedBuilder = new EmbedBuilder
            {
                Title = title,
                Color = Color.DarkBlue
            };

            embedBuilder.WithAuthor(League.Name);

            foreach (Team team in League.Teams)
            {
                if (team.LatestTransactions != null)
                {
                    StringBuilder s = new();
                    string fieldBody = "```";
                    foreach (Transaction transaction in team.LatestTransactions)
                    {
                        foreach (Player player in transaction.Players)
                        {
                            switch (player.TransactionType)
                            {
                                case "add":
                                    fieldBody += $"[{player.TransactionType.ToUpper()}ED] {player.Name} {player.EditorialTeamAbbr} - {player.Position}";
                                    break;

                                case "drop":
                                    fieldBody += $"[{player.TransactionType.ToUpper()}PED] {player.Name} {player.EditorialTeamAbbr} - {player.Position}";
                                    //s.AppendLine($"```[{player.TransactionType.ToUpper()}ED] {player.Name} {player.EditorialTeamAbbr} - {player.Position}```");
                                    break;
                            }
                            fieldBody += "\n";
                        }
                        fieldBody += "\n";
                    }
                    s.AppendLine(fieldBody + "```");

                    embedBuilder.AddField(team.Name.ToUpper(), s.ToString());
                }
            }

            return embedBuilder.Build();
        }

        protected override IParser CreateParser()
        {
            return new YahooTransactionsXmlParser();
        }
    }
}