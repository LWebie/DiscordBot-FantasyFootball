using System.Threading.Tasks;
using System.Timers;
using YahooDiscordClient.ChatClients;

namespace YahooDiscordClient.Pollers
{
    public abstract class Poller : IPoller
    {
        public Poller(int msDelay)
        {
            Initializer = ConfigureTimer(msDelay);
        }

        public Task Initializer { get; set; }

        public IChatClient ChatClient { get; set; }

        public Timer Timer { get; set; }

        public Task ConfigureTimer(int msDelay)
        {
            Timer = new Timer(msDelay);
            Timer.Elapsed += async (sender, e) => await Poll();
            return Task.CompletedTask;
        }

        public abstract Task Poll();
    }
}