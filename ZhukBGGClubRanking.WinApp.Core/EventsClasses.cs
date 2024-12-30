using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhukBGGClubRanking.Core.Model;
using ZhukBGGClubRanking.Core;

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
}
