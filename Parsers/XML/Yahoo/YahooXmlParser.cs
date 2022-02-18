using System.Xml.Linq;
using YahooDiscordClient.Extensions;
using YahooDiscordClient.YahooComponents;

namespace YahooDiscordClient.Parsers.Xml.Yahoo
{
    public abstract class YahooXmlParser : XmlParser
    {
        public YahooXmlParser() : base()
        {
        }

        public override void Parse(string text)
        {
            Document = XDocument.Parse(text);
            Constants.YahooNs = Document.Root.Attribute("xmlns").Value;
            var league = Document.Root.GetChild("league");

            League.Name = league.GetChildValue("name");
            League.LogoUrl = league.GetChildValue("logo_url");
            League.EndDate = league.GetChildValue("end_date");
            League.StartDate = league.GetChildValue("start_date");
            League.CurrentWeek = int.Parse(league.GetChildValue("current_week"));
            League.EndWeek = int.Parse(league.GetChildValue("end_week"));
            League.Season = int.Parse(league.GetChildValue("season"));
            League.StartWeek = int.Parse(league.GetChildValue("start_week"));
            League.TeamsCount = int.Parse(league.GetChildValue("num_teams"));

            ParseResource(text);
        }

        protected abstract void ParseResource(string text);
    }
}