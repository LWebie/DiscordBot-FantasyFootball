using System.Threading.Tasks;

namespace YahooDiscordClient.Messages
{
    public interface IMessage<TMessageType>
    {
        public Task Initializer { get; set; }

        public abstract TMessageType CreateMessage();
    }
}