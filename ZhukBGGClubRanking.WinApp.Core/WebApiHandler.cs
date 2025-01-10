using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ZhukBGGClubRanking.Core;
using System.Net;
using System.Net.Http.Json;
using System.Security;
using System.Threading;
using ZhukBGGClubRanking.Core.Model;

namespace ZhukBGGClubRanking.WinApp.Core
{
    public static class WebApiHandler
    {
        public static HttpClient GetClientWithAuth(string url, string login, string password, string token=null, int timeOutInSeconds = 0, string mediaType= "application/json")
        {
            NetworkCredential credentials = new NetworkCredential(login, password);
            HttpClientHandler handler = new HttpClientHandler();
            if (!string.IsNullOrWhiteSpace(login) && !string.IsNullOrWhiteSpace(password))
            {
                handler.Credentials = credentials;
            }
            var client = new HttpClient(handler);
            if (timeOutInSeconds > 0)
                client.Timeout = TimeSpan.FromSeconds(timeOutInSeconds);
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
            if (token != null)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return client;
        }

        public static async Task<HttpResponseMessage> Login(string url, string login, string password, string userName, string userPassword)
        {
            var client = GetClientWithAuth(url, login, password);
            HttpContent content = JsonContent.Create(new LoginPrm { UserName = userName.Trim(), PasswordCache = User.GetMD5Hash(userPassword.Trim()) });
            try
            {
                return await client.PostAsync("api/login", content);
            }
            catch (Exception e)
            {
                return new HttpResponseMessage(HttpStatusCode.Moved);
            }
            
        }


        public static async Task<HttpResponseMessage> GetGames(string url, string login, string password, string token)
        {
            var client = GetClientWithAuth(url, login, password, token);
            return await client.GetAsync("api/getgamescollection");
        }

        public static async Task<HttpResponseMessage> GetUserActualRatings(string url, string login, string password, string token)
        {
            var client = GetClientWithAuth(url, login, password, token);
            return await client.GetAsync("api/getusersactualratings");
        }

        public static async Task<HttpResponseMessage> GetUsers(string url, string login, string password, string token)
        {
            var client = GetClientWithAuth(url, login, password, token);
            return await client.GetAsync("api/getusers");
        }

        public static async Task<HttpResponseMessage> GetRawGames(string url, string login, string password, string token)
        {
            var client = GetClientWithAuth(url, login, password, token);
            return await client.GetAsync("api/getrawgames");
        }

        public static async Task<HttpResponseMessage> CreateUserByAdmin(string url, string login, string password, string token, User newUser)
        {
            var client = GetClientWithAuth(url, login, password, token);
            HttpContent content = JsonContent.Create(newUser);
            return await client.PostAsync("api/createuserbyadmin", content);
        }

        public static async Task<HttpResponseMessage> SaveUsersRating(string url, string login, string password, string token, List<RatingItem> ratingItems)
        {
            var client = GetClientWithAuth(url, login, password, token);
            HttpContent content = JsonContent.Create(ratingItems);
            return await client.PostAsync("api/saveusersrating", content);
        }

        public static async Task<HttpResponseMessage> ClearTeseraRawTable(string url, string login, string password, string token)
        {
            var client = GetClientWithAuth(url, login, password, token);
            return await client.PostAsync("api/clearteserarawtable",null);
        }

        public static async Task<HttpResponseMessage> ClearBGGRawTable(string url, string login, string password, string token)
        {
            var client = GetClientWithAuth(url, login, password, token);
            return await client.PostAsync("api/clearbggrawtable", null);
        }

        public static async Task<HttpResponseMessage> ClearBGGTeseraRawTable(string url, string login, string password, string token)
        {
            var client = GetClientWithAuth(url, login, password, token);
            return await client.PostAsync("api/clearbggteserarawtable", null);
        }

        public static async Task<HttpResponseMessage> SaveTeseraGamesRawInfo(string url, string login, string password, string token/*, List<TeseraRawGame> teseraGames*/)
        {
            var client = GetClientWithAuth(url, login, password, token,5000);
            return await client.PostAsync("api/saveteseragamesrawinfo", null);
        }

        public static async Task<HttpResponseMessage> SaveBGGAndTeseraGamesRawInfo(string url, string login, string password, string token)
        {
            var client = GetClientWithAuth(url, login, password, token, 1000);
            //HttpContent content = JsonContent.Create(teseraGames);
            return await client.PostAsync("api/savebggandteseragamesrawinfo", null);
        }

        public static async Task<HttpResponseMessage> GetSimpleWebResponse(string apiUrl, string addUrl)
        {
            var client = GetClientWithAuth(apiUrl, null, null);
            return await client.GetAsync(addUrl);
        }

        public static async Task<HttpResponseMessage> GetGameImage(string url, string login, string password, string token, int bggId)
        {
            var client = GetClientWithAuth(url, login, password, token, 10, "application/octet-stream");
            return await client.GetAsync("api/getgameimagebybggid?bggid=" + bggId);
        }
    }
}

