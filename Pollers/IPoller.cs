using YahooDiscordClient.ChatClients;

namespace YahooDiscordClient.Pollers
{
    public interface IPoller
    {
        public IChatClient ChatClient { get; set; }
    }
}