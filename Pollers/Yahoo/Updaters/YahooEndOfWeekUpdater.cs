using System;
using System.Threading.Tasks;
using YahooDiscordClient.ChatClients;
using YahooDiscordClient.Pollers.Yahoo.Updaters;
using YahooDiscordClient.YahooComponents;

namespace YahooDiscordClient.Pollers.Yahoo
{
    public class YahooEndOfWeekUpdater : Updater
    {
        public YahooEndOfWeekUpdater(IChatClient client) : base(client)
        {
            ConfigureScheduledTime();
        }

        public DateTime ScheduledTime { get; private set; }

        public DateTime StartDateTime { get; private set; }

        public DateTime EndDateTime { get; private set; }

        public override async Task CheckIfUpdated()
        {
            // implement the logic
        }

        private void ConfigureScheduledTime()
        {
            char[] delimiter = new char[] { '-' };
            string[] startDateArray = League.StartDate.Split(delimiter, StringSplitOptions.None);
            string[] endDateArray = League.EndDate.Split(delimiter, StringSplitOptions.None);

            StartDateTime = new DateTime(int.Parse(startDateArray[0]), int.Parse(startDateArray[1]), int.Parse(startDateArray[2]));
            EndDateTime = new DateTime(int.Parse(endDateArray[0]), int.Parse(endDateArray[1]), int.Parse(endDateArray[2]));

            if ((DateTime.Now.DayOfYear > StartDateTime.DayOfYear)
                || (DateTime.Now.DayOfYear > EndDateTime.DayOfYear))
            {
                // implement logic
            }
        }
    }
}