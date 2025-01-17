using System.Net.Http.Headers;
using ZhukBGGClubRanking.Core;

namespace ZhukBGGClubRanking.WebApi.Code
{
    public static class YandexTranslateHelper
    {
        public static async Task<List<string>> GetTranslationFromEng(string[] textsEng)
        {
            var request =  new YandexTranslateRequest();
            request.folderId = WebAppSettings.YandexAPIFolderId;
            request.sourceLanguageCode = "en";
            request.targetLanguageCode = "ru";
            request.texts = textsEng;
            var res = await GetYandexTranslateResponse(WebAppSettings.YandexAPIToken, request);
            var content = res.Content.ReadFromJsonAsync<YandexTranslateResult>();
            var result = content.Result;
            if (result != null)
            {
                return result.GetRusTexts();
            }
            return new List<string>();
        }

        static Task<HttpResponseMessage> GetYandexTranslateResponse(string token, YandexTranslateRequest request)
        {
            var url = CoreConstants.YandexTranslationAPIUrl;
            var client = GetClientWithAuth(url, token);
            HttpContent content = JsonContent.Create(request);
            return client.PostAsync(url, content);
        }

        static HttpClient GetClientWithAuth(string url, string token)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return client;
        }

        class YandexTranslateRequest
        {
            public string sourceLanguageCode { get; set; }
            public string targetLanguageCode { get; set; }
            public string[] texts { get; set; }
            public string folderId { get; set; }
        }

        class YandexTranslateResult
        {
            public Dictionary<string, string>[] translations { get; set; }

            public List<string> GetRusTexts()
            {
                var list = new List<string>();
                foreach (var dic in translations)  
                {
                    list.Add(dic["text"]);
                }
                return list;
            }
        }
    }
}
