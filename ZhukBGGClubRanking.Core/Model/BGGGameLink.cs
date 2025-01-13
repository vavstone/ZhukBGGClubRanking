using System;
using System.Collections.Generic;
using System.Text;

namespace ZhukBGGClubRanking.Core
{
    public abstract class BGGGameLink
    {
        public int Id { get; set; }
        public string TitleEng { get; set; }
        public string TitleRus { get; set; }
    }
}
