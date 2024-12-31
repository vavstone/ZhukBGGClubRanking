using System;
using System.Collections.Generic;
using System.Linq;

namespace ZhukBGGClubRanking.Core.Model
{
    public class RatingItem : IComparable<RatingItem>
    {
        public int Id { get; set; }
        //public int UsersRatingId { get; set; }
        public int GameId { get; set; }
        public int RatingOrder { get; set; }
        public int CompliancePercent { get; set; }
        public int Weight { get; set; }
        public List<RatingShortInfo> RatingsByOthersUsers { get; set; } = new List<RatingShortInfo>();

        public string UserRatingString
        {
            get
            {
                if (RatingsByOthersUsers.Any())
                {
                    var splitter = "; ";
                    var resultStr = "";
                    foreach (var item in RatingsByOthersUsers.OrderBy(c => c.RatingOrder))
                        resultStr += string.Format("{0} - {1}{2}", item.RatingOrder, item.UserName, splitter);
                    return resultStr.Substring(0, resultStr.Length - splitter.Length);
                }

                return string.Empty;
            }
        }

        public int CompareTo(RatingItem other)
        {
            var currentRatings = RatingsByOthersUsers.OrderBy(c => c.RatingOrder).ToList();
            var otherRatings = other.RatingsByOthersUsers.OrderBy(c => c.RatingOrder).ToList();
            if (!currentRatings.Any() && otherRatings.Any()) return -1;
            if (!otherRatings.Any() && currentRatings.Any()) return 1;
            if (!currentRatings.Any() && !otherRatings.Any()) return 0;
            for (int i = 0; i < currentRatings.Count; i++)
            {

                if (otherRatings.Count - 1 < i) return 1;
                if (currentRatings[i].RatingOrder < otherRatings[i].RatingOrder) return 1;
                if (otherRatings[i].RatingOrder < currentRatings[i].RatingOrder) return -1;
            }
            return 0;
        }


    }
}