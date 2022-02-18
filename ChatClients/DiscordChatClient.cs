using Discord;
using Discord.WebSocket;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YahooDiscordClient.ChatClients
{
    public class DiscordChatClient : ChatClient<DiscordSocketClient, ulong>
    {
        public override DiscordSocketClient Client { get; set; }

        public override List<ulong> ChannelIds => Constants.ChannelIds;

        public override async Task SendChatMessage(string text)
        {
            var channel = Client.GetChannel(ChannelIds[0]) as IMessageChannel;
            await channel.SendMessageAsync(text);
        }

        public async Task SendChatMessage(Embed embed)
        {
            var channel = Client.GetChannel(ChannelIds[0]) as IMessageChannel;
            await channel.SendMessageAsync(embed: embed);
        }
    }
}