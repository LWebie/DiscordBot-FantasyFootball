using System.Threading.Tasks;
using YahooDiscordClient.Parsers;

namespace YahooDiscordClient.Messages
{
    public abstract class Message<TMessageType> : IMessage<TMessageType>
    {
        public Message()
        {
            Initializer = Initialize();
        }

        public Task Initializer { get; set; }

        protected IParser Parser { get; private set; }

        private async Task Initialize()
        {
            Parser = CreateParser();
            await Parser.Initializer;
        }

        public abstract TMessageType CreateMessage();

        protected abstract IParser CreateParser();
    }
}