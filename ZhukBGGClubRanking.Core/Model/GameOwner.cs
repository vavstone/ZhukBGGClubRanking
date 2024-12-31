using System;

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
