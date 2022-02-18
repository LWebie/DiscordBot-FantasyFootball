using System.Collections.Generic;
using System.Threading.Tasks;

namespace YahooDiscordClient.ChatClients
{
    public abstract class ChatClient<TClient, TChannelId> : IChatClientGeneric<TClient, TChannelId>
    {
        public abstract TClient Client { get; set; }
        public abstract List<TChannelId> ChannelIds { get; }

        public abstract Task SendChatMessage(string text);
    }
}