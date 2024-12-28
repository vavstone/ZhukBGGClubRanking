using ZhukBGGClubRanking.Core;
using ZhukBGGClubRanking.Core.Model;

namespace ZhukBGGClubRanking.WebApi
{
    public static class RequestHandler
    {
        public static BGGCollection GetBggCollection()
        {
            return BGGCollection.LoadFromFile();
        }

        public static List<Game> GetGamesCollection()
        {
            return DBGame.GetGamesCollection();
        }

        public static List<UsersRating> GetUsersActualRatings()
        {
            return DBUsersRating.GetUsersActualRatings();
        }

        public static void SaveUsersRating(UsersRating rating)
        {
            DBUsersRating.SaveRating(rating);
        }

        public static List<User> GetUsers()
        {
            return DBUser.GetUsers();
        }

        public static void CreateNewUser(User newUser)
        {
            DBUser.CreateNewUser(newUser);
        }
    }
}
