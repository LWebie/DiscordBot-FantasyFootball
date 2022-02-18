using YahooDiscordClient.Parsers;

namespace YahooDiscordClient.MessageBuilders.Yahoo
{
    public abstract class YahooMessageBuilder<TMessageType> : MessageBuilder<TMessageType>
    {
        public YahooMessageBuilder() : base()
        {
        }

        protected override abstract string ApiResourcePath { get; }

        protected override abstract IParser CreateParser();
    }
}