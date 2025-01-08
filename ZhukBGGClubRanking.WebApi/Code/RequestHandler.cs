using ZhukBGGClubRanking.Core;
using ZhukBGGClubRanking.Core.Model;
using ZhukBGGClubRanking.WebApi.Core;
using ZhukBGGClubRanking.WebApi.DB;

namespace ZhukBGGClubRanking.WebApi
{
    public static class RequestHandler
    {
        public static BGGCollection GetBggCollection()
        {
            return BGGCollection.LoadFromFile();
        }

        public static List<Game> GetGamesCollection(List<User> users)
        {
            return DBGame.GetGamesCollection(users);
        }

        public static List<UsersRating> GetUsersActualRatings()
        {
            return DBUsersRating.GetUsersActualRatings();
        }

        public static void SaveUsersRating(UsersRating rating)
        {
            DBUsersRating.SaveRating(rating);
        }

        public static void SaveTeseraRawInfoGames(List<TeseraRawGame> teseraGames)
        {
            foreach (var item in teseraGames)
                DBTeseraRawGame.SaveTeseraRawGame(item);
        }

        public static List<User> GetUsers()
        {
            return DBUser.GetUsers();
        }

        public static void CreateNewUser(User newUser)
        {
            DBUser.CreateNewUser(newUser);
        }

        public static void SaveRatingsToCSVFiles()
        {
            var ratings = DBUsersRating.GetUsersActualRatings();
            var users = DBUser.GetUsers();
            var games = DBGame.GetGamesCollection(users);
            foreach (var rating in ratings)
            {
                UserRatingFile.SaveRatingToCSVFile(rating, games, users);
            } 
        }

        public static void InitiateDB(User currentUser)
        {
            //1. получаем актуальную версию collection.xml с сайта BGG и обновляем collection.xml в club_collection\collection.xml
            BGGCollection.LoadFromUrlToFile();

            //2. читаем translate\collection.csv
            var translateFile = GamesNamesTranslateFile.LoadFromFile();

            //3. очищаем таблицы БД
            DBCommon.ClearDB();

            //4. создаем пользователей в users по названиям файлов в lists\
            foreach (var userName in UserRatingFile.GetFilesNamesWithoutExt())
            {
                var user = new User();
                user.Name = user.FullName = user.Password = userName;
                user.EMail = string.Format("{0}@yandex.ru", userName);
                user.IsActive = true;
                user.CreateTime = DateTime.Now;
                user.Role = Role.UserRole;
                user.CreateUserId = currentUser.Id;
                user.HashPassword();
                DBUser.CreateNewUser(user);
            }

            //5. получаем созданных пользователей
            var users = DBUser.GetUsers();

            //6. читаем игры в games из club_collection\collection.xml (с использованием информации из translate\collection.csv)
            var bggColl = BGGCollection.LoadFromFile();
            if (translateFile != null)
            {
                bggColl.ApplyTranslation(translateFile.GamesTranslate);
            }

            //7. созданием игры в БД с использованием инфо пользователей из БД
            List<Game> games = new List<Game>();
            foreach (var bggGame in bggColl.Items)
            {
                var game = bggGame.CreateGame(users, currentUser);
                games.Add(game);
            }
            foreach (var game in games)
            {
                DBGame.SaveGame(game, false);
            }
            foreach (var game in games)
            {
                game.SetParents(games);
                DBGame.UpdateParent(game);
            }

            //8. создаем рейтинги (rating_items, users_ratings) из файлов в lists\
            var ratings = UserRatingFile.GetUsersRatingsFromListsFolder(users, games);
            foreach (var userRating in ratings)
            {
                userRating.ReCalculateRatingAfterRemoveItems();
                DBUsersRating.SaveRating(userRating);
            }
        }

        public static void ClearTeseraRawTable()
        {
            DBCommon.ClearTeseraRawTable();
        }
        public static void ClearBGGRawTable()
        {
            DBCommon.ClearBGGRawTable();
        }
        public static void ClearBGGTeseraRawTable()
        {
            DBCommon.ClearBGGTeseraRawTable();
        }


        public static void SaveBGGAndTeseraRawGames()
        {
            var bggGames = DBBGGRawGame.GetGames();
            var teseraGames = DBTeseraRawGame.GetGames();
            var games = TeseraBGGRawGame.GetMergedTeseraBGGInfoList(bggGames, teseraGames);
            TeseraBGGRawGame.SetParentsAndIsAddition(games);
            DBTeseraBGGRawGame.SaveGames(games);

        }
    }
}
