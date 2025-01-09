using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ZhukBGGClubRanking.Core;
using ZhukBGGClubRanking.Core.Model;

namespace ZhukBGGClubRanking.WinApp.Core
{
    public class AppCache
    {
        public List<Game> Games { get; set; } = new List<Game>();
        public List<UsersRating> UsersRating { get; set; } = new List<UsersRating>();
        public List<User> Users { get; set; } = new List<User>();
        public List<TeseraBGGRawGame> RawGames = new List<TeseraBGGRawGame>();

        public EventHandler GamesLoaded { get; set; }
        public EventHandler UsersLoaded { get; set; }
        public EventHandler UsersRatingLoaded { get; set; }
        public EventHandler RawGamesLoaded { get; set; }

        public EventHandler AllLoaded { get; set; }

        private BackgroundWorker bwGamesCollection = new BackgroundWorker();
        private BackgroundWorker bwUsersRatings = new BackgroundWorker();
        private BackgroundWorker bwUsers = new BackgroundWorker();
        private BackgroundWorker bwRawGames = new BackgroundWorker();

        private WebDataGamesCollectionResultForBW gamesLoadingResult;
        private WebDataUsersResultForBW usersLoadingResult;
        private WebDataUsersRatingsResultForBW usersRatingsLoadingResult;
        private WebDataRawGamesResultForBW rawGamesLoadingResult;

        public AppCache()
        {
            bwGamesCollection.WorkerSupportsCancellation = true;
            bwGamesCollection.WorkerReportsProgress = true;
            bwGamesCollection.DoWork += BwGamesCollection_DoWork;
            bwGamesCollection.RunWorkerCompleted += BwGamesCollection_RunWorkerCompleted;

            bwUsersRatings.WorkerSupportsCancellation = true;
            bwUsersRatings.WorkerReportsProgress = true;
            bwUsersRatings.DoWork += BwUsersRatings_DoWork;
            bwUsersRatings.RunWorkerCompleted += BwUsersRatings_RunWorkerCompleted;

            bwUsers.WorkerSupportsCancellation = true;
            bwUsers.WorkerReportsProgress = true;
            bwUsers.DoWork += BwUsers_DoWork;
            bwUsers.RunWorkerCompleted += BwUsers_RunWorkerCompleted;

            bwRawGames.WorkerSupportsCancellation = true;
            bwRawGames.WorkerReportsProgress = true;
            bwRawGames.DoWork += BwRawGames_DoWork;
            bwRawGames.RunWorkerCompleted += BwRawGames_RunWorkerCompleted;
        }

        private async void BwUsers_DoWork(object sender, DoWorkEventArgs e)
        {
            var result = new WebDataUsersResultForBW();
            var options = e.Argument as WebPrmForBW;
            var reqResult = WebApiHandler.GetUsers(
                options.HostingSettings.Url,
                options.HostingSettings.Login,
                options.HostingSettings.Password, 
                JWTPrm.Token);

            if (reqResult.Result.StatusCode.ToString() == "OK")
            {
                result.Result = true;
                result.Users = await reqResult.Result.Content.ReadAsAsync<List<User>>();
            }
            else
            {
                result.Result = false;
                result.Message = reqResult.Result.StatusCode.ToString();
            }
            e.Result = result;
        }

        private void BwUsers_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var result = e.Result as WebDataUsersResultForBW;
            if (result.Result)
            {
                Users = result.Users;
            }
            usersLoadingResult = result;
            if (UsersLoaded != null)
                UsersLoaded(this, new WebResultEventArgs {Result = result});
            FireAllLoaded();
        }

        
        
        private async void BwUsersRatings_DoWork(object sender, DoWorkEventArgs e)
        {
            var result = new WebDataUsersRatingsResultForBW();
            var options = e.Argument as WebPrmForBW;
            var reqResult = WebApiHandler.GetUserActualRatings(
                options.HostingSettings.Url,
                options.HostingSettings.Login,
                options.HostingSettings.Password,
                JWTPrm.Token);

            if (reqResult.Result.StatusCode.ToString() == "OK")
            {
                result.Result = true;
                result.UsersRatings = await reqResult.Result.Content.ReadAsAsync<List<UsersRating>>();
            }
            else
            {
                result.Result = false;
                result.Message = reqResult.Result.StatusCode.ToString();
            }
            e.Result = result;
        }

        private void BwUsersRatings_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var result = e.Result as WebDataUsersRatingsResultForBW;
            if (result.Result)
            {
                UsersRating = result.UsersRatings;
            }
            usersRatingsLoadingResult = result;
            if (UsersRatingLoaded != null)
                UsersRatingLoaded(this, new WebResultEventArgs { Result = result });
            FireAllLoaded();
        }

        

        private async void BwRawGames_DoWork(object sender, DoWorkEventArgs e)
        {
            var result = new WebDataRawGamesResultForBW();
            var options = e.Argument as WebPrmForBW;
            var reqResult = WebApiHandler.GetRawGames(
                options.HostingSettings.Url,
                options.HostingSettings.Login,
                options.HostingSettings.Password,
                JWTPrm.Token);

            if (reqResult.Result.StatusCode.ToString() == "OK")
            {
                result.Result = true;
                result.Games = await reqResult.Result.Content.ReadAsAsync<List<TeseraBGGRawGame>>();
            }
            else
            {
                result.Result = false;
                result.Message = reqResult.Result.StatusCode.ToString();
            }
            e.Result = result;
        }

        private void BwRawGames_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var result = e.Result as WebDataRawGamesResultForBW;
            if (result.Result)
            {
                RawGames = result.Games;
            }
            rawGamesLoadingResult = result;
            if (RawGamesLoaded != null)
                RawGamesLoaded(this, new WebResultEventArgs { Result = result });
            //FireAllLoaded();
        }

        private async void BwGamesCollection_DoWork(object sender, DoWorkEventArgs e)
        {
            var result = new WebDataGamesCollectionResultForBW();
            var options = e.Argument as WebPrmForBW;
            var reqResult = WebApiHandler.GetGames(
                options.HostingSettings.Url,
                options.HostingSettings.Login,
                options.HostingSettings.Password,
                JWTPrm.Token);

            if (reqResult.Result.StatusCode.ToString() == "OK")
            {
                result.Result = true;
                result.Games = await reqResult.Result.Content.ReadAsAsync<List<Game>>();
            }
            else
            {
                result.Result = false;
                result.Message = reqResult.Result.StatusCode.ToString();
            }
            e.Result = result;
        }

        private void BwGamesCollection_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var result = e.Result as WebDataGamesCollectionResultForBW;
            if (result.Result)
            {
                Games = result.Games;
            }
            gamesLoadingResult = result;
            if (GamesLoaded != null)
                GamesLoaded(this, new WebResultEventArgs { Result = result });
            FireAllLoaded();
        }

        void FireAllLoaded()
        {
            if (AllLoaded != null && usersLoadingResult != null && gamesLoadingResult != null && usersRatingsLoadingResult != null)
            {
                var result2 = new WebDataAllListstResultForBW();
                result2.Result = true;
                if (!usersLoadingResult.Result)
                {
                    result2.Result = false;
                    result2.Message = usersLoadingResult.Message;
                }
                else if (!gamesLoadingResult.Result)
                {
                    result2.Result = false;
                    result2.Message = gamesLoadingResult.Message;
                }
                else if (!usersRatingsLoadingResult.Result)
                {
                    result2.Result = false;
                    result2.Message = usersRatingsLoadingResult.Message;
                }
                result2.Users = usersLoadingResult.Users;
                result2.UsersRatings = usersRatingsLoadingResult.UsersRatings;
                result2.Games = gamesLoadingResult.Games;
                usersRatingsLoadingResult.UsersRatings.ForEach(c =>
                    c.UpdateGamesCrossRatings(
                        usersRatingsLoadingResult.UsersRatings,
                        usersLoadingResult.Users));
                AllLoaded(this, new WebResultEventArgs { Result = result2 });
            }
        }

        public async void LoadAll(HostingSettings hostingSettings)
        {
            usersLoadingResult = null;
            usersRatingsLoadingResult = null;
            gamesLoadingResult = null;
            var prm = new WebPrmForBW { HostingSettings = hostingSettings };
            if (!bwGamesCollection.IsBusy) bwGamesCollection.RunWorkerAsync(prm);
            if (!bwUsersRatings.IsBusy) bwUsersRatings.RunWorkerAsync(prm);
            if (!bwUsers.IsBusy) bwUsers.RunWorkerAsync(prm);
        }

        public async void LoadUsers(HostingSettings hostingSettings)
        {
            usersLoadingResult = null;
            usersRatingsLoadingResult = null;
            gamesLoadingResult = null;
            var prm = new WebPrmForBW { HostingSettings = hostingSettings };
            if (!bwUsers.IsBusy) bwUsers.RunWorkerAsync(prm);
        }

        public async void LoadRawGames(HostingSettings hostingSettings)
        {
            //usersLoadingResult = null;
            //usersRatingsLoadingResult = null;
            //gamesLoadingResult = null;
            var prm = new WebPrmForBW { HostingSettings = hostingSettings };
            if (!bwRawGames.IsBusy) bwRawGames.RunWorkerAsync(prm);
        }

    }

}
