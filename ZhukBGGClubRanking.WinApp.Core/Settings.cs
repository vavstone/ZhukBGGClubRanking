using System.IO;

namespace ZhukBGGClubRanking.WinApp.Core
{
    public static class Settings
    {
        public static string RootDir
        {
            get { return Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location); }
        }

        public static string CacheDir
        {
            get { return Path.Combine(RootDir, "cache"); }
        }

        public static string CacheImagesDir
        {
            get { return Path.Combine(CacheDir, "img"); }
        }

        public static string CacheImagesTeseraPicDir
        {
            get { return Path.Combine(CacheImagesDir, "tesera_pic"); }
        }

        public static string CacheImagesBGGPicDir
        {
            get { return Path.Combine(CacheImagesDir, "bgg_pic"); }
        }

        //public static string ListsDir
        //{
        //    get { return Path.Combine(RootDir, "lists"); }
        //}

        //public static string CommonCollectionFilePath
        //{
        //    get { return Path.Combine(RootDir, "club_collection", "collection.xml"); }
        //}

        //public static string GamesNamesTranslateFilePath
        //{
        //    get { return Path.Combine(RootDir, "translate", "collection.csv"); }
        //}

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
