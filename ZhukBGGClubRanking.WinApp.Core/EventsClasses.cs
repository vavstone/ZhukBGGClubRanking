using System;
using System.Collections.Generic;
using System.ComponentModel;
using ZhukBGGClubRanking.Core;
using ZhukBGGClubRanking.Core.Model;

namespace ZhukBGGClubRanking.WinApp.Core
{

    
    public class WebPrmForBW
    {
        public HostingSettings HostingSettings { get; set; } = new HostingSettings();
    }

    public class WebDataResultForBW
    {
        public bool Result { get; set; }
        public string Message { get; set; }
    }
    public class WebDataGamesCollectionResultForBW : WebDataResultForBW
    {
        public List<Game> Games { get; set; }
    }
    public class WebDataUsersRatingsResultForBW : WebDataResultForBW
    {
        public List<UsersRating> UsersRatings { get; set; }
    }
    public class WebDataUsersResultForBW : WebDataResultForBW
    {
        public List<User> Users { get; set; }
    }
    public class WebDataRawGamesResultForBW : WebDataResultForBW
    {
        public List<TeseraBGGRawGame> Games { get; set; }
    }
    

    public class WebDataAllListstResultForBW : WebDataResultForBW
    {
        public List<User> Users { get; set; }
        public List<UsersRating> UsersRatings { get; set; }
        public List<Game> Games { get; set; }
    }

    public class WebResultEventArgs : EventArgs
    {
        public WebDataResultForBW Result { get; set; }
    }

    public class LoginPrmForBW : WebPrmForBW
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }

    public class LoginResultForBW
    {
        public bool Result { get; set; }
        public string Token { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
    }

    public class CreateUserPrmForBW : WebPrmForBW
    {
        public User NewUser { get; set; }
    }

    public class UploadRatingPrmForBW : WebPrmForBW
    {
        public List<RatingItem> RatingItems { get; set; }
    }


    public class WebPrmGetTeseraGames: DoWorkEventArgs
    {
        public WebPrmGetTeseraGames(List<TeseraRawGame> argument) : base(argument)
        {
            games = argument;
        }

        public List<TeseraRawGame> games = new List<TeseraRawGame>();

        public string Url { get; set; }
        public int Offset { get; set; }
        public int Limit { get; set; }
        public int PortionsToGet { get; set; }
    }



    public class WebDataTeseraGamesProgressChangedEventArgs : ProgressChangedEventArgs
    {
        public WebDataTeseraGamesProgressChangedEventArgs(int progressPercentage, TeseraRawInfoData teseraRawInfoData) : 
            base(progressPercentage, teseraRawInfoData)
        {
            PortionOfGames = teseraRawInfoData.GamesPortion;
            RawJson = teseraRawInfoData.RawJson;
        }

        public List<TeseraRawGame> PortionOfGames { get; set; }
        public string RawJson { get; set; }
    }

    public class WebDataTeseraGamesResult : WebDataResultForBW
    {
        public List<TeseraRawGame> Games { get; set; }
    }

    public class WebDataTeseraGamesResultEventArgs : WebDataResultForBW
    {
        public List<TeseraRawGame> Games { get; set; }
    }
}
