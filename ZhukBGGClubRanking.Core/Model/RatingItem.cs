using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZhukBGGClubRanking.Core.Model
{
    public class RatingItem
    {
        public int Id { get; set; }
        //public int UsersRatingId { get; set; }
        public int GameId { get; set; }
        public int RatingOrder { get; set; }
        public int CompliancePercent { get; set; }
        public int Weight { get; set; }

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

        public List<RatingShortInfo> RatingsByOthersUsers { get; set; } = new List<RatingShortInfo>();
    }
}