using System;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;

namespace ZhukBGGClubRanking.WinApp.Core
{
    public class SaveAsyncRequestToDBAPIWrapper<T>
    {
        public EventHandler WorkCompleted { get; set; }
        private BackgroundWorker bw = new BackgroundWorker();
        public Func<string, string, string, string, T, Task<HttpResponseMessage>> MethodToExecute { get; set; }
        public T DataToSave { get; set; }
         
        public SaveAsyncRequestToDBAPIWrapper()
        {
            bw.WorkerSupportsCancellation = true;
            bw.WorkerReportsProgress = true;
            bw.DoWork += Bw_DoWork;
            bw.RunWorkerCompleted += Bw_Completed;
        }

        private async void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            var options = e.Argument as WebPrmForBW;
            var reqResult = MethodToExecute(
                options.HostingSettings.Url,
                options.HostingSettings.Login,
                options.HostingSettings.Password,
                JWTPrm.Token,
                DataToSave);
            var result = new WebDataResultForBW();
            if (reqResult.Result.StatusCode.ToString() == "OK")
            {
                result.Result = true;
            }
            else
            {
                result.Result = false;
                result.Message = reqResult.Result.StatusCode.ToString();
            }
            e.Result = result;
        }

        private void Bw_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            var result = e.Result as WebDataResultForBW;
            if (WorkCompleted != null)
                WorkCompleted(this, new WebResultEventArgs {Result = result});
        }
        
        public void DoWork(HostingSettings hostingSettings)
        {
            var prm = new WebPrmForBW { HostingSettings = hostingSettings };
            //if (!bw.IsBusy) 
                bw.RunWorkerAsync(prm);
        }
    }
}
