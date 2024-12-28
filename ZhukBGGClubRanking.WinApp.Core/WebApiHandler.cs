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
using ZhukBGGClubRanking.Core.Model;

namespace ZhukBGGClubRanking.WinApp.Core
{
    public static class WebApiHandler
    {
        static HttpClient GetClientWithAuth(string url, string login, string password, string token=null)
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

        public static async Task<HttpResponseMessage> Login(string url, string login, string password, string userName, string userPassword)
        {
            var client = GetClientWithAuth(url, login, password);
            HttpContent content = JsonContent.Create(new LoginPrm { UserName = userName.Trim().ToLower(), PasswordCache = User.GetMD5Hash(userPassword.Trim().ToLower()) });
            return await client.PostAsync("api/login", content);
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

        public static async Task<HttpResponseMessage> CreateUserByAdmin(string url, string login, string password, string token, User newUser)
        {
            var client = GetClientWithAuth(url, login, password, token);
            HttpContent content = JsonContent.Create(newUser);
            return await client.PostAsync("api/createuserbyadmin", content);
        }
        
    }
}

