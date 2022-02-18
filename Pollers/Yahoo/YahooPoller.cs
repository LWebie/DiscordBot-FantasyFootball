using System.Threading.Tasks;
using YahooDiscordClient.Parsers.Xml.Yahoo;
using YahooDiscordClient.Pollers.Yahoo;

namespace YahooDiscordClient.Pollers
{
    public class YahooPoller : Poller
    {
        public YahooPoller(int msDelay) : base(msDelay)
        {
        }

        public bool Initialized { get; private set; }

        public YahooTransactionsUpdater TransactionsUpdater { get; private set; }
        public YahooEndOfWeekUpdater EndOfWeekUpdater { get; private set; }

        public async void InitializeDbValues()
        {
            var standingsParser = new YahooStandingsXmlParser();
            await standingsParser.Initializer;
            var scoreboardParser = new YahooScoreboardXmlParser();
            await scoreboardParser.Initializer;
            var transactionsParser = new YahooTransactionsXmlParser();
            await transactionsParser.Initializer;

            TransactionsUpdater = new(ChatClient);
            EndOfWeekUpdater = new(ChatClient);

            Initialized = true;
        }

        public override async Task Poll()
        {
            if (Initialized)
            {
                await TransactionsUpdater.CheckIfUpdated();
                await EndOfWeekUpdater.CheckIfUpdated();
                //CheckTransactionsUpdate();
            }
            else
            {
                InitializeDbValues();
            }

            //var test = new YahooStandingsMessage().CreateMessage();
            //await ((DiscordChatClient)ChatClient).SendChatMessage(test);
        }
    }
}