using System.Net.Http;
using System.Threading.Tasks;
using YahooDiscordClient.Managers;
using YahooDiscordClient.Parsers;

namespace YahooDiscordClient.MessageBuilders
{
    public abstract class MessageBuilder<TMessageType> : IMessageBuilder<TMessageType>
    {
        public MessageBuilder()
        {
            Initializer = Initialize();
        }

        public Task Initializer { get; set; }

        protected abstract string ApiResourcePath { get; }

        protected IParser Parser { get; private set; }

        public string HttpResponseText { get; set; }

        private async Task Initialize()
        {
            Parser = CreateParser();
            var httpResponseText = await GetApiResource().Result.Content.ReadAsStringAsync();
            Parser.Parse(httpResponseText);
        }

        public abstract TMessageType CreateMessage();

        protected abstract IParser CreateParser();

        private async Task<HttpResponseMessage> GetApiResource()
        {
            return await HttpManager.CallApi(ApiResourcePath);
        }
    }
}