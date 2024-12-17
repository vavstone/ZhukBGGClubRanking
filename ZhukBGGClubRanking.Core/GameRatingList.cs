using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZhukBGGClubRanking.Core
{
    public class GameRatingList
    {
        public List<string> UserNames { get; set; }
        public List<GameRating> GameList { get; set; }

        public GameRatingList()
        {
            GameList = new List<GameRating>();
            UserNames = new List<string>();
        }

        public void CalculateWeightByRating(int maxUsersCollectionSize=0)
        {
            if (GameList.Count==0) return;
            var curWeigth = 0;
            if (maxUsersCollectionSize > 0)
                curWeigth = maxUsersCollectionSize;
            else
                curWeigth = GameList.Count;
            //GameList.Select(c => c.Rating).Max();
            foreach (var item in GameList.OrderBy(c=>c.Rating))
            {
                item.Weight = curWeigth--;
            }
        }

        public void CalculateRatingByWeight()
        {
            if (GameList.Count == 0) return;
            var curRating = 1;
            foreach (var item in GameList.OrderByDescending(c => c.Weight))
            {
                item.Rating = curRating++;
            }
        }


        public static GameRatingList CalculateAvarageRating(List<GameRatingList> inList)
        {
            var maxCollectionSize = inList.Select(c => c.GameList.Count).Max();
            foreach (var gameRatingList in inList)
            {
                gameRatingList.CalculateWeightByRating(maxCollectionSize);
            }
            var result = new GameRatingList();
            var commonRating = new List<GameRating>();
            foreach (var item in inList)
            {
                commonRating.AddRange(item.GameList);
            }

            var commonGamesListUnigue = commonRating.Select(c => c.Game).Distinct();
            foreach (var item in commonGamesListUnigue)
            {
                var sumWeigth = commonRating.Where(c => c.Game == item).Sum(c => c.Weight);
                result.GameList.Add(new GameRating {Game = item, Weight = sumWeigth});
            }
            result.CalculateRatingByWeight();
            result.UserNames.AddRange(inList.SelectMany(c=>c.UserNames));
            return result;
        }
    }
}