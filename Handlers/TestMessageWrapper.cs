using Discord;
using System.Threading.Tasks;
using YahooDiscordClient.ChatClients;

namespace YahooDiscordClient.Handlers
{
    public class TestMessageWrapper
    {
        public IChatClient Client { get; set; }

        public TestMessageWrapper()
        {
        }

        public void InitializeChatClient()
        {
        }

        public async Task SendChatMessage(string text)
        {
            ulong id = ((DiscordChatClient)Client).ChannelIds[0];
            var channel = ((DiscordChatClient)Client).Client.GetChannel(id) as IMessageChannel;
            await channel.SendMessageAsync(text);
        }

        public async Task SendChatEmbedMessage(Embed embed)
        {
            ulong id = ((DiscordChatClient)Client).ChannelIds[0];
            var channel = ((DiscordChatClient)Client).Client.GetChannel(id) as IMessageChannel;
            await channel.SendMessageAsync(embed: embed);
        }
    }
}