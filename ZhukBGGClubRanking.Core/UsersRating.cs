using System;
using System.Collections.Generic;
using System.Linq;

namespace ZhukBGGClubRanking.Core
{
    public class UsersRating:IComparable<UsersRating>
    {
        public List<UserRating> UserRating { get; set; }

        public UsersRating()
        {
            UserRating = new List<UserRating>();
        }

        public int CompareTo(UsersRating other)
        {
            var currentRatings = UserRating.OrderBy(c => c.Rating).ToList();
            var otherRatings = other.UserRating.OrderBy(c => c.Rating).ToList();
            if (!currentRatings.Any() && otherRatings.Any()) return -1;
            if (!otherRatings.Any() && currentRatings.Any()) return 1;
            if (!currentRatings.Any() && !otherRatings.Any()) return 0;
            for (int i = 0; i < currentRatings.Count; i++)
            {
                
                if (otherRatings.Count - 1 < i) return 1;
                if (currentRatings[i].Rating < otherRatings[i].Rating) return 1;
                if (otherRatings[i].Rating < currentRatings[i].Rating) return -1;
            }

            return 0;
        }
    }
}
