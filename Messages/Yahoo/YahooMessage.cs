using YahooDiscordClient.Parsers;

namespace YahooDiscordClient.Messages.Yahoo
{
    public abstract class YahooMessage<TMessageType> : Message<TMessageType>
    {
        public YahooMessage() : base()
        {
        }

        protected override abstract IParser CreateParser();
    }
}