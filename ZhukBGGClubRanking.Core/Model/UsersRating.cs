using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZhukBGGClubRanking.Core.Model
{
    public class UsersRating : IComparable<UsersRating>
    {
        public int Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? ExpireTime { get; set; }
        public int UserId { get; set; }
        public RatingCollection Rating { get; set; } = new RatingCollection();

        public int CompareTo(UsersRating other)
        {
            var currentRatings = Rating.RatingItems.OrderBy(c => c.RatingOrder).ToList();
            var otherRatings = other.Rating.RatingItems.OrderBy(c => c.RatingOrder).ToList();
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

        public void CalculateWeightByRating(int maxUsersCollectionSize = 0)
        {
            if (Rating.RatingItems.Count == 0) return;
            var curWeigth = 0;
            if (maxUsersCollectionSize > 0)
                curWeigth = maxUsersCollectionSize;
            else
                curWeigth = Rating.RatingItems.Count;
            //GameList.Select(c => c.Rating).Max();
            foreach (var item in Rating.RatingItems.OrderBy(c => c.RatingOrder))
            {
                item.Weight = curWeigth--;
            }
        }

        public void CalculateRatingByWeight()
        {
            if (Rating.RatingItems.Count == 0) return;
            var curRating = 1;
            foreach (var item in Rating.RatingItems.OrderByDescending(c => c.Weight))
            {
                item.RatingOrder = curRating++;
            }
        }

        public void ReCalculateRatingAfterRemoveItems()
        {
            if (Rating.RatingItems.Count == 0) return;
            var curRating = 1;
            foreach (var item in Rating.RatingItems.OrderBy(c => c.RatingOrder))
            {
                item.RatingOrder = curRating++;
            }
        }


        public static UsersRating CalculateAvarageRating(List<UsersRating> inList, List<Game> commonCollection, int topXPos)
        {
            if (inList.Any())
            {
                var selectedRatings = new List<UsersRating>();
                var maxCollSize = inList.Select(c => c.Rating.RatingItems.Count).Max();
                var onlyTop = topXPos < maxCollSize;

                var gamesToSkip = new List<int>();
                if (onlyTop)
                {

                    foreach (var gameRatingList in inList)
                    {
                        var list = gameRatingList.Rating.RatingItems.OrderBy(c => c.RatingOrder).ToList();
                        if (onlyTop)
                            gamesToSkip.AddRange(list.Skip(topXPos).Select(c => c.GameId));
                    }
                }

                gamesToSkip = gamesToSkip.Distinct().ToList();

                foreach (var gameRatingList in inList)
                {
                    var newRating = new UsersRating();
                    selectedRatings.Add(newRating);
                    var list = gameRatingList.Rating.RatingItems.Where(c => gamesToSkip.All(c1 => c1 != c.GameId))
                        .OrderBy(c => c.RatingOrder).ToList();
                    newRating.Rating.RatingItems = list;
                }

                if (onlyTop)
                {
                    foreach (var gameRatingList in selectedRatings)
                    {
                        var newRating = new List<RatingItem>();
                        foreach (var rating in gameRatingList.Rating.RatingItems)
                        {
                            var gameInOtherRatings =
                                selectedRatings.Select(c => c.Rating.RatingItems)
                                    .All(c => c.Any(c1 => c1.GameId == rating.GameId));
                            if (gameInOtherRatings)
                                newRating.Add(rating);
                        }

                        gameRatingList.Rating.RatingItems = newRating;
                    }
                }

                var newMaxCollectionSize = selectedRatings.Select(c => c.Rating.RatingItems.Count).Max();

                foreach (var gameRatingList in selectedRatings)
                {
                    gameRatingList.CalculateWeightByRating(newMaxCollectionSize);
                }

                var result = new UsersRating();
                var commonRating = new List<RatingItem>();
                foreach (var item in selectedRatings)
                {
                    commonRating.AddRange(item.Rating.RatingItems);
                }

                var commonGamesListUnigue = commonRating.Select(c => c.GameId).Distinct();
                foreach (var gameId in commonGamesListUnigue)
                {
                    var sumWeigth = commonRating.Where(c => c.GameId == gameId).Sum(c => c.Weight);
                    var gameItem = new RatingItem();
                    gameItem.GameId = gameId;
                    gameItem.Weight = sumWeigth;
                    result.Rating.RatingItems.Add(gameItem);
                }

                result.CalculateRatingByWeight();
                result.Rating.RatingItems = result.Rating.RatingItems.OrderBy(c => c.RatingOrder).ToList();
                //result.UserNames.AddRange(selectedRatings.SelectMany(c => c.UserNames));
                return result;
            }

            return null;
        }

        public void UpdateGamesCrossRatings(IEnumerable<UsersRating> allLists, IEnumerable<User> users)
        {
            foreach (var ratingItem in Rating.RatingItems)
            {
                //var currentListUser = Cache.Users.FirstOrDefault(c => c.Id == currentList.UserId);

                foreach (var othersRating in allLists.Where(c => c.UserId != UserId))
                {
                    var tmpItem = othersRating.Rating.RatingItems.FirstOrDefault(c => c.GameId == ratingItem.GameId);
                    
                    if (tmpItem != null)
                    {
                        var tmpUser = users.FirstOrDefault(c => c.Id == othersRating.UserId);
                        ratingItem.RatingsByOthersUsers.Add(new RatingShortInfo()
                            { UserName = tmpUser.Name, RatingOrder = tmpItem.RatingOrder });
                    }
                }
            }
        }
    }
}