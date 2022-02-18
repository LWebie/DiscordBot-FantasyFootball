using System.Xml.Linq;

namespace YahooDiscordClient.Extensions
{
    public static class XmlLinqExtensions
    {
        public static XElement GetChild(this XElement element, string name)
        {
            return element.Element(Constants.YahooNs + name);
        }

        public static string GetChildValue(this XElement element, string name)
        {
            return element.Element(Constants.YahooNs + name).Value;
        }
    }
}