using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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
            get { return Path.Combine(RootDir, "club_collection", "ZhukBGclub.xml"); }
        }
    }
}
