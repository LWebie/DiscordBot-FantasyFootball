using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using YahooDiscordClient.Extensions;
using YahooDiscordClient.YahooComponents;

namespace YahooDiscordClient.Parsers.Xml.Yahoo
{
    public class YahooTransactionsXmlParser : YahooXmlParser
    {
        protected override string ApiResourcePath => Constants.TransactionsResourcePath;

        public YahooTransactionsXmlParser() : base()
        {
        }

        protected override void ParseResource(string text)
        {
            League.LatestTransactions = new();
            var transactionsElements = Document.Root.GetChild("league").Descendants(Constants.YahooNs + "transaction");
            var compareCount = League.TransactionsCount - (League.TransactionsCount - transactionsElements.Count());

            if (League.TransactionsCount != compareCount)
            {
                int transactionIndex = 0;
                foreach (XElement transactionElement in transactionsElements)
                {
                    Transaction transaction = new();
                    transaction.Id = int.Parse(transactionElement.GetChildValue("transaction_id"));
                    transaction.Type = transactionElement.GetChildValue("type");
                    transaction.Status = transactionElement.GetChildValue("status");

                    League.LatestTransactions.Add(transaction);
                    League.LatestTransactions[transactionIndex].Players = new List<Player>();
                    var playerElements = transactionElement.Descendants(Constants.YahooNs + "player").ToList();
                    foreach (XElement playerElement in playerElements)
                    {
                        Player player = new();
                        player.Id = int.Parse(playerElement.GetChildValue("player_id"));
                        player.Name = playerElement.GetChild("name").GetChildValue("full");
                        player.Position = playerElement.GetChildValue("display_position");
                        player.EditorialTeamAbbr = playerElement.GetChildValue("editorial_team_abbr");

                        var transactionData = playerElement.GetChild("transaction_data");
                        player.TransactionType = transactionData.GetChildValue("type");

                        switch (player.TransactionType)
                        {
                            case "add":
                                player.DestinationTeamName = transactionData.GetChildValue("destination_team_name");
                                break;

                            case "drop":
                                player.SourceTeamName = transactionData.GetChildValue("source_team_name");
                                break;

                            case "pending_trade":
                            case "completed_trade":
                                player.DestinationTeamName = transactionData.GetChildValue("destination_team_name");
                                player.SourceTeamName = transactionData.GetChildValue("source_team_name");
                                break;
                        }
                        League.LatestTransactions[transactionIndex].Players.Add(player);
                    }
                    SetTeamLatestTransactions(League.LatestTransactions[transactionIndex]);
                    transactionIndex++;
                }

                League.TransactionsCount = transactionsElements.Count();
            }
        }

        public void SetTeamLatestTransactions(Transaction transaction)
        {
            if (League.Teams == null)
            {
                _ = new YahooStandingsXmlParser();
            }

            foreach (Player player in transaction.Players)
            {
                if (player.DestinationTeamName != null)
                {
                    var matchingTeam = League.Teams.Find(x => x.Name == player.DestinationTeamName);
                    if (matchingTeam.LatestTransactions == null)
                    {
                        matchingTeam.LatestTransactions = new();
                    }

                    foreach (Transaction trans in matchingTeam.LatestTransactions)
                    {
                        if (transaction.Id == trans.Id)
                        {
                            return;
                        }
                    }
                    matchingTeam.LatestTransactions.Add(transaction);
                }
                else if (player.SourceTeamName != null)
                {
                    var matchingTeam = League.Teams.Find(x => x.Name == player.SourceTeamName);
                    if (matchingTeam.LatestTransactions == null)
                    {
                        matchingTeam.LatestTransactions = new();
                    }

                    foreach (Transaction trans in matchingTeam.LatestTransactions)
                    {
                        if (transaction.Id == trans.Id)
                        {
                            return;
                        }
                    }

                    matchingTeam.LatestTransactions.Add(transaction);
                }
            }
        }

        public void TeamNameComparison()
        {
        }
    }
}