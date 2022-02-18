using System.Net.Http;
using System.Threading.Tasks;
using YahooDiscordClient.Managers;

namespace YahooDiscordClient.Parsers
{
    public abstract class Parser<TDocType> : IParser
    {
        public Parser()
        {
            Initializer = Initialize();
        }

        protected abstract string ApiResourcePath { get; }

        public string HttpResponseText { get; set; }
        public abstract TDocType Document { get; set; }

        public abstract void Parse(string text);

        public Task Initializer { get; private set; }

        protected async Task Initialize()
        {
            HttpResponseText = await GetApiResource().Result.Content.ReadAsStringAsync();
            Parse(HttpResponseText);
        }

        private async Task<HttpResponseMessage> GetApiResource()
        {
            return await HttpManager.CallApi(ApiResourcePath);
        }
    }
}