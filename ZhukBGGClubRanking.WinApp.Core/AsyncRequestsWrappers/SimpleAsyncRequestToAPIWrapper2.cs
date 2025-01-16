using System;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;

namespace ZhukBGGClubRanking.WinApp.Core
{
    public class SimpleAsyncRequestToAPIWrapper2<T1,T2>
    {
        public EventHandler WorkCompleted { get; set; }
        public Func<string, string, string, string, T1, Task<HttpResponseMessage>> MethodToExecute { get; set; }
        public Task<T2> ResultContent;

        public async void DoWork(string url, string login, string password, string token, T1 prm)
        {
            var reqResult = await MethodToExecute(url, login, password, token, prm);
            var result = new WebDataResultForBW();
            if (reqResult.StatusCode.ToString() == "OK")
            {
                ResultContent = reqResult.Content.ReadAsAsync<T2>();
                result.Result = true;
            }
            else
            {
                result.Result = false;
                result.Message = reqResult.StatusCode.ToString();
            }
            if (WorkCompleted != null)
                WorkCompleted(this, new WebResultEventArgs { Result = result });
        }
    }
}
