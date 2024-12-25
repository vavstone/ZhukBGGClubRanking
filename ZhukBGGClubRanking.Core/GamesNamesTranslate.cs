using Csv;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ZhukBGGClubRanking.Core
{
    public class GamesNamesTranslate
    {
        public string NameEng { get; set; }
        public string NameRus { get; set; }
        public string TeseraName { get; set; }
        internal string ParentEngName { get; set; }

        public GamesNamesTranslate Parent
        {
            get;
            set;
        }
    }
}
