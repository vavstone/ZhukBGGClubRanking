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
using System.Windows.Forms;
using ZhukBGGClubRanking.Core.Model;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace TestWebAPI
{
    public static class TestWebApi
    {
        public static async void Test()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5116/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await client.GetStringAsync("utils/updatebggcoll");
        }

        public static HttpClient GetClient(string url, string login, string password)
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
            return client;
        }

        public static HttpClient GetClientWithAuth(string url, string login, string password, string key)
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
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", key);
            return client;
        }

        public static async Task<string> GetTestString(string url, string login, string password)
        {
            var client = GetClient(url, login, password);
            var response = client.GetAsync("test/getteststring").Result;
            if (response.StatusCode.ToString() != "OK")
                MessageBox.Show(response.StatusCode.ToString());
            return await response.Content.ReadAsStringAsync();
        }

        public static async Task<string> GetTestStringAuth(string url, string login, string password, string key)
        {
            var client = GetClientWithAuth(url, login, password, key);
            var response = client.GetAsync("test/getteststringauth").Result;
            if (response.StatusCode.ToString() != "OK")
                MessageBox.Show(response.StatusCode.ToString());
            return await response.Content.ReadAsStringAsync();
        }

        public static async Task<string> Login(string url, string login, string password, string userName, string userPassword)
        {
            var client = GetClient(url, login, password);
            HttpContent content = JsonContent.Create(new LoginPrm { UserName = userName, PasswordCache = User.GetMD5Hash(userPassword) });
            //content.Headers.Add("username", userName);
            //content.Headers.Add("passwordcache", User.GetMD5Hash(userPassword));
            var response = client.PostAsync("api/login", content).Result;
            if (response.StatusCode.ToString() != "OK")
                MessageBox.Show(response.StatusCode.ToString());
            return await response.Content.ReadAsStringAsync();
        }

        public static async Task<List<Game>> GetGames(string url,string login, string password, string key)
        {
            var client = GetClientWithAuth(url, login, password, key);
            var response = client.GetAsync("api/getgamescollection").Result;
            if (response.StatusCode.ToString() != "OK")
                MessageBox.Show(response.StatusCode.ToString());
            return await response.Content.ReadAsAsync<List<Game>>();
        }

        public static async Task<List<UsersRating>> GetUserActualRatings(string url, string login, string password, string key)
        {
            var client = GetClientWithAuth(url, login, password, key);
            var response = client.GetAsync("api/getusersactualratings").Result;
            if (response.StatusCode.ToString() != "OK")
                MessageBox.Show(response.StatusCode.ToString());
            return await response.Content.ReadAsAsync<List<UsersRating>>();
        }


        

        public static async Task<string> SaveUsersRating(string url, string login, string password, string key, List<RatingItem> ratingItems)
        {
            var client = GetClientWithAuth(url, login, password, key);
            HttpContent content = JsonContent.Create(ratingItems);
            var response = client.PostAsync("api/saveusersrating", content).Result;
            if (response.StatusCode.ToString() != "OK")
                MessageBox.Show(response.StatusCode.ToString());
            return await response.Content.ReadAsStringAsync();
        }

    }
}

