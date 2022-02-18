using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace YahooDiscordClient.Services
{
    public class YahooService
    {
        private HttpClient Client { get; set; }

        public YahooService()
        {
        }

        public async Task ConfigureClient()
        {
            var accessToken = Constants.AccessToken;
            var refreshToken = Constants.RefreshToken;

            if (Client == null)
            {
                Client = new HttpClient();
            }

            string responseText = null;

            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            //var request = new HttpRequestMessage(HttpMethod.Get, "https://fantasysports.yahooapis.com/fantasy/v2/game/nfl");
            HttpResponseMessage response = await Client.GetAsync("https://fantasysports.yahooapis.com/fantasy/v2/league/406.l.942298;season=2021/standings");

            XDocument xml;
            //IEnumerable<XElement> elements;
            //XElement game;

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

                response = await Client.SendAsync(request);

                //var responseString = await newResponse.Content.ReadAsStringAsync();
                var responseString = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseString);

                var newAccessToken = responseData.GetValueOrDefault("access_token");
                var newRefreshToken = responseData.GetValueOrDefault("refresh_token");

                Constants.AccessToken = newAccessToken;
                Constants.RefreshToken = newRefreshToken;

                Client.DefaultRequestHeaders.Authorization = null;
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", newAccessToken);

                response = await Client.GetAsync("https://fantasysports.yahooapis.com/fantasy/v2/league/406.l.942298/standings;");

                //responseText = await response.Content.ReadAsStringAsync();
                /*
                HttpResponseMessage newestResponse = await _client.GetAsync("https://fantasysports.yahooapis.com/fantasy/v2/game/nfl");
                responseText = await newestResponse.Content.ReadAsStringAsync();
                */
                /*
                var newResponseText = await newResponse.Content.ReadAsStringAsync();

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", newAccessToken);
                HttpResponseMessage newestResponse = await client.GetAsync("https://fantasysports.yahooapis.com/fantasy/v2/game/nfl");

                responseText = await newestResponse.Content.ReadAsStringAsync();
                xml = XDocument.Parse(responseText);
                elements = xml.Elements();
                game = xml.Document.Element("game");
                */
            }

            responseText = await response.Content.ReadAsStringAsync();
            xml = XDocument.Parse(responseText);
            XNamespace ns = xml.Root.Attribute("xmlns").Value;
            var league = xml.Root.Element(ns + "league");
            var leagueId = league.Element(ns + "league_id").Value;
            Test = leagueId;
        }

        public void TestDataStore(string value)
        {
            Test = value;
        }

        public async Task<string> FetchTest()
        {
            await ConfigureClient();
            return Test;
        }

        public string Test { get; set; }
    }
}