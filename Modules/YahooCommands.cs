using Discord.Commands;
using System.Threading.Tasks;
using YahooDiscordClient.Messages.Yahoo;

namespace YahooDiscordClient.Modules
{
    public class YahooCommands : ModuleBase<SocketCommandContext>
    {
        [Command("pf")]
        public async Task FetchPFStats()
        {
            await ReplyAsync(embed: new YahooPointsForMessage().CreateMessage());
        }

        [Command("pa")]
        public async Task FetchPaStats()
        {
            await ReplyAsync(embed: new YahooPointsAgainstMessage().CreateMessage());
        }

        [Command("wl")]
        public async Task FetchWinLossStats()
        {
            await ReplyAsync(embed: new YahooWinLossMessage().CreateMessage());
        }

        [Command("ovr")]
        public async Task FetchOverallStats()
        {
            await ReplyAsync("warse");
        }

        [Command("standings")]
        public async Task FetchStandings()
        {
            await ReplyAsync(embed: new YahooStandingsMessage().CreateMessage());
        }

        [Command("scores")]
        public async Task FetchScoreboard()
        {
            await ReplyAsync(embed: new YahooScoreboardMessage().CreateMessage());
        }

        [Command("transactionstats")]
        public async Task FetchTransactions()
        {
            await ReplyAsync(embed: new YahooTransactionStatsMessage().CreateMessage());
        }

        [Command("transactions")]
        public async Task Reply()
        {
            await ReplyAsync(embed: new YahooTransactionsMessage().CreateMessage());
        }
    }
}