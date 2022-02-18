using System.Xml.Linq;

namespace YahooDiscordClient.Parsers.Xml
{
    public abstract class XmlParser : Parser<XDocument>
    {
        public override XDocument Document { get; set; }

        public XmlParser() : base()
        {
        }
    }
}