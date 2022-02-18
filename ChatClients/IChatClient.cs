using System.Threading.Tasks;

namespace YahooDiscordClient.ChatClients
{
    public interface IChatClient
    {
        public Task SendChatMessage(string text);
    }
}