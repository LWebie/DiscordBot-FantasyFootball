using System.Collections.Generic;

namespace YahooDiscordClient.ChatClients
{
    public interface IChatClientGeneric<TClient, TChannelId> : IChatClient
    {
        TClient Client { get; set; }
        List<TChannelId> ChannelIds { get; }
    }
}