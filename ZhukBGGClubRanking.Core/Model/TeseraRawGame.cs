using System;
using System.Collections.Generic;
using System.Text;

namespace ZhukBGGClubRanking.Core.Model
{
    public class TeseraRawGame
    {
        public int Id {  get; set; }
        public string Title { get; set; }
        public int? TeseraId { get; set; } 
        public int? BGGId { get; set; }
        public string Alias { get; set; }
        public string DescriptionShort { get; set; }
        public string Description { get; set; }
        public DateTime? ModificationDateUtc { get; set; }
        public DateTime? CreationDateUtc { get; set; }
        public string PhotoUrl { get; set; }
        public int? Year { get; set; }
        public double? RatingUser { get; set; }
        public double? N10Rating { get; set; }
        public double? N20Rating { get; set; }
        public double? BGGRating { get; set; }
        public double? BGGGeekRating { get; set; }
        public int? BGGNumVotes { get; set; }
        public int? NumVotes { get; set; }
        public int? PlayersMin { get; set; }
        public int? PlayersMax { get; set; }
        public int? PlayersMinRecommend { get; set; }
        public int? PlayersMaxRecommend { get; set; }
        public int? PlayersAgeMin { get; set; }
        public int? TimeToLearn { get; set; }
        public int? PlaytimeMin { get; set; }
        public int? PlaytimeMax { get; set; }
        public int? CommentsTotal { get; set; }
        public int? CommentsTotalNew { get; set; }
        public bool? IsAddition { get; set; }

        public override string ToString()
        {
            return String.Format("{0} {1}", Id, Title);
        }
    }
}
