using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace YahooDiscordClient.Managers
{
    public static class HttpManager
    {
        private static HttpClient Client { get; set; }

        public static void CreateHttpClient()
        {
            Client = new HttpClient();
        }

        public static async Task<HttpResponseMessage> CallApi(string resourcePath)
        {
            var accessToken = Constants.AccessToken;
            var refreshToken = Constants.RefreshToken;

            if (Client == null)
            {
                Client = new HttpClient();
            }

            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            HttpResponseMessage response = await Client.GetAsync(resourcePath);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return response;

                case HttpStatusCode.Unauthorized:
                    response = await FetchResource(resourcePath);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return response;
                    }
                    else
                    {
                        throw new Exception($"HTTP response returned with status code {response.StatusCode}");
                    }

                default:
                    throw new Exception($"HTTP response returned with status code {response.StatusCode}");
            }
        }

        private static async Task<HttpResponseMessage> FetchResource(string resourcePath)
        {
            var requestData = new Dictionary<string, string>
            {
                ["grant_type"] = "refresh_token",
                ["refresh_token"] = Constants.RefreshToken
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.login.yahoo.com/oauth2/get_token")
            {
                Content = new FormUrlEncodedContent(requestData)
            };

            var basicCredentials = $"{Constants.ClientId}:{Constants.ClientSecret}";
            var encodedCredentials = Encoding.UTF8.GetBytes(basicCredentials);
            var base64Credentials = Convert.ToBase64String(encodedCredentials);

            request.Headers.Add("Authorization", $"Basic {base64Credentials}");

            HttpResponseMessage response = await Client.SendAsync(request);

            var responseString = await response.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseString);

            Constants.AccessToken = responseData.GetValueOrDefault("access_token");
            Constants.RefreshToken = responseData.GetValueOrDefault("refresh_token");

            Client.DefaultRequestHeaders.Authorization = null;
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Constants.AccessToken);

            response = await Client.GetAsync(resourcePath);

            return response;
        }
    }
}