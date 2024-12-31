using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ZhukBGGClubRanking.Core;
using ZhukBGGClubRanking.Core.Model;

namespace ZhukBGGClubRanking.WinApp
{
    public class GridViewDataSourceWrapper
    {

        public string Game { get; set; }
        public string GameEng { get; set; }
        public string GameRus { get; set; }
        public string ParentGameEng { get; set; }
        public int Rating { get; set; }
        public int Weight { get; set; }
        public int CompliancePercent { get; set; }
        //public BGGCollection.ItemElement BGGItem { get; set; }
        public UsersRating UsersRating { get; set; }
        public string TeseraKey { get; set; }
        public int BGGObjectId { get; set; }

        public GridViewDataSourceWrapper()
        {
            UsersRating = new UsersRating();
        }

        public string BGGComments { get; set; }

        public static string GetGameNameEngFromFormattedName(string formattedName)
        {
            if (string.IsNullOrWhiteSpace(formattedName))
                return formattedName;
            var indexOfStartQuot = formattedName.IndexOf('<');
            var indexOfEndQuot = formattedName.IndexOf('>');
            if (indexOfStartQuot >= 0 && indexOfEndQuot > 2)
            {
                return formattedName.Substring(indexOfStartQuot + 1, indexOfEndQuot - indexOfStartQuot - 1);
            }

            return formattedName;
        }

        public string UserRatingString { get; set; }


        public static GridViewDataSourceWrapper CreateFromCoreGame(RatingItem ratingItem, List<Game> gamesCollection/*, List<UsersRating> ratings*/)
        {
            var game = gamesCollection.FirstOrDefault(c => c.Id == ratingItem.GameId);
            var res = new GridViewDataSourceWrapper();
            res.Game = game.Name;
            res.GameEng = game.NameEng;
            res.GameRus = game.NameRus;
            res.BGGObjectId = game.BGGObjectId;
            res.BGGComments = game.OwnersString;
            //res.ParentGameEng = gamesCollection
            res.Rating = ratingItem.RatingOrder;
            res.TeseraKey = game.TeseraKey;
            res.CompliancePercent = ratingItem.CompliancePercent;
            //foreach (var rating in ratings)
            //{
            //    var allRatingItems = rating.Rating.RatingItems.Where(c => c.GameId == game.Id).OrderBy(c => c.RatingOrder)
            //        .ToList();
            //    res.UsersRating.Rating.RatingItems = allRatingItems;
            //}
            res.UserRatingString = ratingItem.UserRatingString;
            return res;
        }
    }
}
