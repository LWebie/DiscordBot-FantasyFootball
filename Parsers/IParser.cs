using System.Threading.Tasks;

namespace YahooDiscordClient.Parsers
{
    public interface IParser
    {
        public abstract void Parse(string text);

        public Task Initializer { get; }
    }
}