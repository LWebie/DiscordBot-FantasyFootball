using System.Threading.Tasks;
using YahooDiscordClient.ChatClients;
using YahooDiscordClient.Messages.Yahoo;
using YahooDiscordClient.Pollers.Yahoo.Updaters;
using YahooDiscordClient.YahooComponents;

namespace YahooDiscordClient.Pollers.Yahoo
{
    public class YahooTransactionsUpdater : Updater
    {
        public YahooTransactionsUpdater(IChatClient client) : base(client)
        {
        }

        public override async Task CheckIfUpdated()
        {
            int preParseTransactionsCount = League.TransactionsCount;

            if (preParseTransactionsCount != League.TransactionsCount)
            {
                await ((DiscordChatClient)ChatClient).SendChatMessage(new YahooTransactionsMessage().CreateMessage());
            }
        }
    }
}