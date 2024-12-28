using System;
using System.Collections.Generic;
using System.Text;

namespace ZhukBGGClubRanking.Core.Model
{
    public class RatingShortInfo
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int GameId { get; set; }
        public int RatingOrder { get; set; }
    }
}