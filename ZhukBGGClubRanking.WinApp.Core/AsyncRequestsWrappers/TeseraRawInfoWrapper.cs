using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text.Json.Nodes;
using System.Threading;
using ZhukBGGClubRanking.Core.Model;

namespace ZhukBGGClubRanking.WinApp.Core
{
    public class TeseraRawInfoWrapper
    {
        private BackgroundWorker bwTeseraCollection = new BackgroundWorker();
        //private WebDataTeseraGamesResult rawInfoSavingResult;
        public List<TeseraRawGame> TeseraGames { get; set; } = new List<TeseraRawGame>();
        public EventHandler RawInfoLoaded { get; set; }
        public ProgressChangedEventHandler ProgressChanged { get; set; }

        public TeseraRawInfoWrapper()
        {
            bwTeseraCollection.WorkerSupportsCancellation = true;
            bwTeseraCollection.WorkerReportsProgress = true;
            bwTeseraCollection.DoWork += BwTeseraCollection_DoWork;
            bwTeseraCollection.ProgressChanged += BwTeseraCollection_ProgressChanged;
            bwTeseraCollection.RunWorkerCompleted += BwTeseraCollection_RunWorkerCompleted;
        }

        private void BwTeseraCollection_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var userState = e.UserState as TeseraRawInfoData;
            var games = userState.GamesPortion;
            var json = userState.RawJson;
            if (ProgressChanged != null)
            {
                var pcEventArgs = new WebDataTeseraGamesProgressChangedEventArgs(0, userState);
                ProgressChanged(this, pcEventArgs);
            }
        }

        private async void BwTeseraCollection_DoWork(object sender, DoWorkEventArgs e)
        {
            var options = e.Argument as WebPrmGetTeseraGames;
            var result = new WebDataTeseraGamesResult();
            var games = new List<TeseraRawGame>();
            //var client = WebApiHandler.GetClientWithAuth(options.Url,null,null);
            result.Result = true;
            result.Games = games;
            //var optionsToSaveGames = new WebPrmForBW { HostingSettings = this.Settings };
            DoGamesRequest(games, options);
            //foreach (var game in games)
            //    DBTeseraRawGame.SaveTeseraRawGame(game);
            e.Result = result;
        }
        async void DoGamesRequest(List<TeseraRawGame> games, WebPrmGetTeseraGames prm)
        {

            var parameters = new Dictionary<string, string>();
            string addQueryString = "games";
            parameters.Add("offset", prm.Offset.ToString());
            parameters.Add("limit", prm.Limit.ToString());
            if (parameters.Any())
                addQueryString += "?" + string.Join("&", parameters.Select(p => $"{p.Key}={Uri.EscapeDataString(p.Value)}"));
            var content = WebApiHandler.GetSimpleWebResponse(prm.Url, addQueryString).Result.Content;
            //var res = await content.ReadAsAsync<List<TeseraRawGame>>();
            var str = await content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<List<TeseraRawGame>>(str);
            if (res.Count > 0 && (prm.PortionsToGet == 0 || prm.Offset < prm.PortionsToGet))
            {
                games.AddRange(res);
                bwTeseraCollection.ReportProgress(0, new TeseraRawInfoData { GamesPortion = res, RawJson = str });
                if (bwTeseraCollection.CancellationPending == true)
                {
                    prm.Cancel = true;
                    return;
                }
                prm.Offset++;
                //Thread.Sleep(2000);
                DoGamesRequest(games, prm);
            }
        }
        private void BwTeseraCollection_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var result = e.Result as WebDataTeseraGamesResult;
            if (result.Result)
            {
                TeseraGames = result.Games;
            }
            if (RawInfoLoaded != null)
                RawInfoLoaded(this, new WebResultEventArgs { Result = result });
        }

        public async void StartRequests(WebPrmGetTeseraGames prm)
        {
            //var prm = new WebPrmGetTeseraGames {  Url =  };
            if (!bwTeseraCollection.IsBusy) bwTeseraCollection.RunWorkerAsync(prm);
        }



    }

    public class TeseraRawInfoData
    {
        public string RawJson { get; set; }
        public List<TeseraRawGame> GamesPortion { get; set; }
    }

}
