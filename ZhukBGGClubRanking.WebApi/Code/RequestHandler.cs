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

        public static void InitiateDB()
        {
            //1. получаем актуальную версию collection.xml с сайта BGG и обновляем collection.xml в club_collection\collection.xml
            BGGCollection.LoadFromUrlToFile();


            //2. очищаем таблицы БД: rating_items_history, rating_items, users_ratings, games, users (кроме записи 1 - admin), ...
            //TODO!!!

            //3. создаем пользователей в users по названиям файлов в lists\
            //TODO!!!

            //4. создаем игры в games из club_collection\collection.xml (с использованием информации из translate\collection.csv)
            //TODO!!!
            var translateFile = GamesNamesTranslateFile.LoadFromFile();
            var bggColl = BGGCollection.LoadFromFile();
            if (translateFile != null)
            {
                bggColl.ApplyTranslation(translateFile.GamesTranslate);
                bggColl.SetParents();
            }
            foreach (var bggGame in bggColl.Items)
            {
                var game = bggGame.CreateGame();
                DBGame.SaveGame(game);
            }

            //5. создаем рейтинги (rating_items, users_ratings) из файлов в lists\
            //TODO!!!
        }


    }
}
