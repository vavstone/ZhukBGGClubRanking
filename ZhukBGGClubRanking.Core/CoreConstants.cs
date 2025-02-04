using System.IO;

namespace ZhukBGGClubRanking.Core
{
    public static class CoreConstants
    {
        public static string RootDir
        {
            get { return Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location); }
        }
        public static string AppFilesDir
        {
            get { return Path.Combine(RootDir, "App_Files"); }
        }
        public static string InitiateFilesDir
        {
            get { return Path.Combine(AppFilesDir, "InitiateDB"); }
        }
        public static string InitiateUserRatingFilesDir
        {
            get { return Path.Combine(InitiateFilesDir, "lists"); }
        }
        public static string InitiateCommonCollectionFilePath
        {
            get { return Path.Combine(InitiateFilesDir, "club_collection", "collection.xml"); }
        }
        public static string InitiateGamesNamesTranslateFilePath
        {
            get { return Path.Combine(InitiateFilesDir, "translate", "collection.csv"); }
        }
        public static string ExportFilesDir
        {
            get { return Path.Combine(AppFilesDir, "Export"); }
        }
        public static string ExportTranslateFilePath
        {
            get { return Path.Combine(ExportFilesDir, "translate", "collection.csv"); }
        }
        public static string OuterRawInfoFilesDir
        {
            get { return Path.Combine(AppFilesDir, "OuterRawInfo"); }
        }
        public static string ImgCacheFilesDir
        {
            get { return Path.Combine(AppFilesDir, "ImgCache"); }
        }
        public static string ImgCacheBGGFilesDir
        {
            get { return Path.Combine(ImgCacheFilesDir, "BGG"); }
        }
        public static string ImgCacheBGGLargeFilesDir
        {
            get { return Path.Combine(ImgCacheBGGFilesDir, "large"); }
        }
        public static string ImgCacheBGGSmallFilesDir
        {
            get { return Path.Combine(ImgCacheBGGFilesDir, "small"); }
        }
        public static string ImgagesFilesDir
        {
            get { return Path.Combine(AppFilesDir, "Images"); }
        }
        public static string ImgNotFound
        {
            get { return Path.Combine(ImgagesFilesDir, "NotFound.webp"); }
        }


        public static string BGGCardPrefixUrl
        {
            get { return "https://boardgamegeek.com/boardgame/"; }
        }

        public static string BGGCollectionUrl
        {
            get { return "https://boardgamegeek.com/xmlapi2/collection?username=ZhukBGclub"; }
        }

        

        //public static string UrlForGameBGGId(int id)
        //{
        //    return string.Format("https://boardgamegeek.com/boardgame/{0}", id);
        //}

        public static string TeseraCardPrefixUrl
        {
            get { return "https://tesera.ru/game/"; }
        }

        public static string YandexTranslationAPIUrl
        {
            get { return "https://translate.api.cloud.yandex.net/translate/v2/translate"; }
        }

        public const string CommonUserErrorMessage = "Произошла ошибка. Обратитесь к администратору системы.";
    }
}
