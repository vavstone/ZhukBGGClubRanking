using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ZhukBGGClubRanking.Core;
using ZhukBGGClubRanking.Core.Model;

namespace ZhukBGGClubRanking.WinApp
{
    public class GridViewDataSourceWrapper
    {
        public Game GameItem { get; set; }
        public RatingItem RatingItem { get; set; }
        public string Game { get { return GameItem.Name; } }
        public string GameEng { get { return GameItem.NameEng; } }
        //public string GameRus { get { return GameItem.NameRus; } }
        public int Rating { get { return RatingItem.RatingOrder; } }
        public int Weight { get; set; }
        public int CompliancePercent { get { return RatingItem.CompliancePercent; } }
        public string TeseraKey { get { return GameItem.TeseraKey; } }
        public int BGGObjectId { get { return GameItem.BGGObjectId; } }
        public string UserRatingString { get { return RatingItem.UserRatingString; } }
        public string BGGComments { get { return GameItem.OwnersString; } }

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

        public static GridViewDataSourceWrapper CreateFromCoreGame(RatingItem ratingItem, List<Game> gamesCollection/*, List<UsersRating> ratings*/)
        {
            var game = gamesCollection.FirstOrDefault(c => c.Id == ratingItem.GameId);
            var res = new GridViewDataSourceWrapper();
            res.GameItem = game;
            res.RatingItem = ratingItem;
            return res;
        }
    }
}
