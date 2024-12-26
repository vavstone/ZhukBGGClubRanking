using System.IO;

namespace ZhukBGGClubRanking.Core
{
    public static class CoreSettings
    {
        public static string RootDir
        {
            get { return Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location); }
        }

        public static string ListsDir
        {
            get { return Path.Combine(RootDir, "lists"); }
        }

        public static string CommonCollectionFilePath
        {
            get { return Path.Combine(RootDir, "club_collection", "collection.xml"); }
        }

        public static string GamesNamesTranslateFilePath
        {
            get { return Path.Combine(RootDir, "translate", "collection.csv"); }
        }

        public static string BGGCardPrefixUrl
        {
            get { return "https://boardgamegeek.com/boardgame/"; }
        }

        //public static string UrlForGameBGGId(int id)
        //{
        //    return string.Format("https://boardgamegeek.com/boardgame/{0}", id);
        //}

        public static string TeseraCardPrefixUrl
        {
            get { return "https://tesera.ru/game/"; }
        }
    }
}
