using System.Threading.Tasks;
using YahooDiscordClient.ChatClients;

namespace YahooDiscordClient.Pollers.Yahoo.Updaters
{
    public abstract class Updater
    {
        public Updater(IChatClient client)
        {
            ChatClient = client;
        }

        protected IChatClient ChatClient { get; private set; }

        public abstract Task CheckIfUpdated();
    }
}