using System;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace ZhukBGGClubRanking.WinAdminApp
{
    public class WebApiHandler
    {
        public static HttpClient GetClientWithAuth(string url, string login = null, string password = null, string token = null)
        {
            NetworkCredential credentials = new NetworkCredential(login, password);
            HttpClientHandler handler = new HttpClientHandler();
            if (!string.IsNullOrWhiteSpace(login) && !string.IsNullOrWhiteSpace(password))
            {
                handler.Credentials = credentials;
            }
            var client = new HttpClient(handler);
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (token != null)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return client;
        }


        public static async Task<HttpResponseMessage> GetGames(string url, int? offset, int? limit)
        {
            var client = GetClientWithAuth(url);
            var parameters = new Dictionary<string, string>();
            string addQueryString = string.Empty;
            if (limit != null)
                parameters.Add("limit", limit.Value.ToString());
            if (offset != null)
                parameters.Add("offset", offset.Value.ToString());
            if (parameters.Any())
                addQueryString = "?" + string.Join("&", parameters.Select(p => $"{p.Key}={Uri.EscapeDataString(p.Value)}"));
            return await client.GetAsync("games" + addQueryString);
        }

        public static List<TeseraRawGame> GetAllGames(string url, int limit, int portionsToGet=0)
        {
            var client = GetClientWithAuth(url);
            var currentOffset = 0;
            var games = new List<TeseraRawGame>();
            var prm = new WebPrmGetGames { Limit = limit, Offset = currentOffset, PortionsToGet = portionsToGet, Url = url };
            DoGamesRequest(client, games, prm);
            return games;
        }

        static void DoGamesRequest(HttpClient client, List<TeseraRawGame> games, WebPrmGetGames prm)
        {
            var t = Task.Run(
            async () =>
            {
                var parameters = new Dictionary<string, string>();
                string addQueryString = string.Empty;
                parameters.Add("offset", prm.Offset.ToString());
                parameters.Add("limit", prm.Limit.ToString());
                if (parameters.Any())
                    addQueryString = "?" + string.Join("&", parameters.Select(p => $"{p.Key}={Uri.EscapeDataString(p.Value)}"));
                var res = await client.GetAsync("games" + addQueryString).Result.Content.ReadAsAsync<List<TeseraRawGame>>();
                if (res.Count > 0 && (prm.PortionsToGet==0 || prm.Offset < prm.PortionsToGet))
                {
                    games.AddRange(res);
                    prm.Offset++;
                    DoGamesRequest(client, games, prm);
                }
            });
        }
    }


   


    public class WebPrmGetGames
    {
        public string Url { get; set; }
        public int Offset { get; set; }
        public int Limit { get; set; }
        public int PortionsToGet { get; set; }
    }

    public class WebDataResult
    {
        public bool Result { get; set; }
        public string Message { get; set; }
    }
    public class WebDataGamesCollectionResult : WebDataResult
    {
        public List<TeseraRawGame> Games { get; set; }
    }
}