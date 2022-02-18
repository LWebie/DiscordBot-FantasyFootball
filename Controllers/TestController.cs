using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using YahooDiscordClient.Handlers;
using YahooDiscordClient.Messages.Yahoo;
using YahooDiscordClient.Pollers;

namespace YahooDiscordClient.Controllers
{
    public class TestController : Controller
    {
        private HttpClient _client = new HttpClient();

        private readonly TestMessageWrapper _tmr;
        private readonly YahooPoller _poller;

        public TestController(IServiceProvider services)
        {
            _tmr = services.GetRequiredService<TestMessageWrapper>();
            _poller = services.GetRequiredService<YahooPoller>();
        }

        //
        // GET: /HelloWorld/
        public async Task<IActionResult> TestIndex()
        {
            if (Constants.AccessToken != await HttpContext.GetTokenAsync("access_token"))
            {
                Constants.AccessToken = await HttpContext.GetTokenAsync("access_token");
                Constants.RefreshToken = await HttpContext.GetTokenAsync("refresh_token");
            }

            await _tmr.SendChatEmbedMessage(new YahooPointsAgainstMessage().CreateMessage());
            _poller.Timer.Start();
            //await _yahoo.HttpService.CallApi("INSERT API CALL");
            //var test = await _yahoo.GetMessage(new YahooStandingsMessageBuilder());
            //await ((DiscordChatClient)_yahoo.ChatClient).SendChatMessage(test);

            var authInfo = await HttpContext.AuthenticateAsync("Cookies");
            authInfo.Properties.UpdateTokenValue("access_token", Constants.AccessToken);
            authInfo.Properties.UpdateTokenValue("refresh_token", Constants.RefreshToken);
            await HttpContext.SignInAsync(authInfo.Principal, authInfo.Properties);

            return View();
        }

        private async Task<string> FetchYahoo()
        {
            string leagueId = "";
            if (User.Identity.IsAuthenticated)
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                var refreshToken = await HttpContext.GetTokenAsync("refresh_token");
                _client = new HttpClient();
                string responseText = null;

                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                //var request = new HttpRequestMessage(HttpMethod.Get, "https://fantasysports.yahooapis.com/fantasy/v2/game/nfl");
                HttpResponseMessage response = await _client.GetAsync("https://fantasysports.yahooapis.com/fantasy/v2/league/INSERTLEAGUE;season=2021/standings");

                XDocument xml;

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    var requestData = new Dictionary<string, string>
                    {
                        ["grant_type"] = "refresh_token",
                        ["refresh_token"] = refreshToken
                    };

                    var request = new HttpRequestMessage(HttpMethod.Post, "https://api.login.yahoo.com/oauth2/get_token")
                    {
                        Content = new FormUrlEncodedContent(requestData)
                    };

                    var basicCredentials = $"{Constants.ClientId}:{Constants.ClientSecret}";
                    var encodedCredentials = Encoding.UTF8.GetBytes(basicCredentials);
                    var base64Credentials = Convert.ToBase64String(encodedCredentials);

                    request.Headers.Add("Authorization", $"Basic {base64Credentials}");

                    //var newResponse = await client.SendAsync(request);

                    response = await _client.SendAsync(request);

                    //var responseString = await newResponse.Content.ReadAsStringAsync();
                    var responseString = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseString);

                    var newAccessToken = responseData.GetValueOrDefault("access_token");
                    var newRefreshToken = responseData.GetValueOrDefault("refresh_token");

                    var authInfo = await HttpContext.AuthenticateAsync("Cookies");
                    authInfo.Properties.GetTokenValue("access_token");
                    authInfo.Properties.GetTokenValue("refresh_token");

                    authInfo.Properties.UpdateTokenValue("access_token", newAccessToken);
                    authInfo.Properties.UpdateTokenValue("refresh_token", newRefreshToken);

                    var testAccessToken = await HttpContext.GetTokenAsync("access_token");
                    var testRefreshToken = await HttpContext.GetTokenAsync("refresh_token");

                    await HttpContext.SignInAsync(authInfo.Principal, authInfo.Properties);

                    _client.DefaultRequestHeaders.Authorization = null;
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", newAccessToken);

                    response = await _client.GetAsync("https://fantasysports.yahooapis.com/fantasy/v2/league/406.l.942298/standings;");
                }

                responseText = await response.Content.ReadAsStringAsync();
                xml = XDocument.Parse(responseText);
                XNamespace ns = xml.Root.Attribute("xmlns").Value;
                var league = xml.Root.Element(ns + "league");
                leagueId = league.Element(ns + "league_id").Value;
            }
            return leagueId;
        }

        //
        // GET: /HelloWorld/Welcome/
        public IActionResult Welcome(string name, int age)
        {
            ViewData["Message"] = $"Hello {name}";
            ViewData["Age"] = age;

            return View();
        }
    }
}