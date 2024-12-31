using System;
using System.Collections.Generic;
using System.Text;

namespace ZhukBGGClubRanking.Core.Model
{
    public class GameOwner
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? DeleteTime { get; set; }
    }
}
