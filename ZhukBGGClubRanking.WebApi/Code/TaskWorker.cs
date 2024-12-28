using ZhukBGGClubRanking.Core;

namespace ZhukBGGClubRanking.WebApi
{
    public static class TaskWorker
    {
        public static void LoadBGGCollectionToDB()
        {
            var translateFile = GamesNamesTranslateFile.LoadFromFile();
            var bggColl = BGGCollection.LoadFromFile();
            if (translateFile != null)
                bggColl.ApplyTranslation(translateFile.GamesTranslate);

            foreach (var bggGame in bggColl.Items)
            {
                var game = bggGame.CreateGame();
                DBGame.SaveGame(game);
            }
        }
    }
}
