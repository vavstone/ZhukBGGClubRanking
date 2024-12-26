using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ZhukBGGClubRanking.Core;

namespace ZhukBGGClubRanking.WinApp.Core
{
    public static class TestWebApi
    {
        public static async Task<BGGCollection> Test()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5116/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await client.GetStringAsync("bggcollection");
            var data = JsonConvert.DeserializeObject<BGGCollection>(response);
            return data;
        }
    }
}

