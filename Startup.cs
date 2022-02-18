using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using YahooDiscordClient.ChatClients;
using YahooDiscordClient.Handlers;
using YahooDiscordClient.Managers;
using YahooDiscordClient.Pollers;
using YahooDiscordClient.Services;

namespace YahooDiscordClient
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private static DiscordSocketClient _client; // client field

        public IConfiguration Configuration { get; }

        // Keep the CommandService and DI container around for use with commands.
        // These two types require you install the Discord.Net.Commands package.
        private static CommandService _commands;

        private IServiceProvider _services;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(config =>
            {
                config.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                config.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                config.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = "Yahoo!";
            })
            .AddCookie(config =>
            {
                config.LoginPath = "/signin";
                config.LogoutPath = "/signout";
            })
            .AddYahoo(config =>
            {
                config.ClientId = Constants.ClientId;
                config.ClientSecret = Constants.ClientSecret;
                config.SaveTokens = true;
            });

            services.AddControllersWithViews();

            ConfigureDiscord(services);

            var test = MainAsync();
        }

        public async Task MainAsync()
        {
            // init the client and sub to the log method
            //_client = new DiscordSocketClient();

            // grab bot token
            string token = Environment.GetEnvironmentVariable("BOT");

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            _client.Ready += setClientServices;

            Task setClientServices()
            {
                var chatClient = _services.GetRequiredService<TestMessageWrapper>().Client = new DiscordChatClient();
                ((DiscordChatClient)chatClient).Client = _client;
                _services.GetRequiredService<YahooPoller>().ChatClient = chatClient;
                return Task.CompletedTask;
            }

            await _services.GetRequiredService<CommandHandlingService>().InstallCommandsAsync();
        }

        public void ConfigureDiscord(IServiceCollection services)
        {
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                // How much logging do you want to see?
                LogLevel = LogSeverity.Info,

                // If you or another service needs to do anything with messages
                // (eg. checking Reactions, checking the content of edited/deleted messages),
                // you must set the MessageCacheSize. You may adjust the number as needed.
                //MessageCacheSize = 50,

                // If your platform doesn't have native WebSockets,
                // add Discord.Net.Providers.WS4Net from NuGet,
                // add the `using` at the top, and uncomment this line:
                //WebSocketProvider = WS4NetProvider.Instance
            });

            _commands = new CommandService(new CommandServiceConfig
            {
                // Again, log level:
                LogLevel = LogSeverity.Info,

                // There's a few more properties you can set,
                // for example, case-insensitive commands.
                CaseSensitiveCommands = false,
            });

            //HttpManager httpManager = new();
            HttpManager.CreateHttpClient();
            DiscordChatClient cc = new();
            TestMessageWrapper tmw = new();
            YahooPoller poller = new(7500);
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton(_client);
            services.AddSingleton(_commands);
            //services.AddSingleton(httpManager);
            services.AddSingleton(cc);
            services.AddSingleton(tmw);
            services.AddSingleton(poller);
            services.AddSingleton<CommandHandlingService>();

            // Setup your DI container.
            _services = services.BuildServiceProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}