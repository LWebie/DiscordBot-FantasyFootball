using System.Threading.Tasks;

namespace YahooDiscordClient.MessageBuilders
{
    public interface IMessageBuilder<TMessageType>
    {
        public Task Initializer { get; set; }

        public abstract TMessageType CreateMessage();
    }
}