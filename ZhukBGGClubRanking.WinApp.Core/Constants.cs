using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhukBGGClubRanking.WinApp.Core
{
    public static class Constants
    {
        public static string RootDir
        {
            get { return Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location); }
        }

        public static string UserSettingsFileFullName
        {
            get { return Path.Combine(RootDir,"settings.xml"); }
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
