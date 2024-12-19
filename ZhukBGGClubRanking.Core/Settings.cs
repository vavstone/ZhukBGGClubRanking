using System.IO;

namespace ZhukBGGClubRanking.Core
{
    public static class Settings
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

        public static string UrlForGameBGGId(int id)
        {
            return string.Format("https://boardgamegeek.com/boardgame/{0}", id);
        }
    }
}
