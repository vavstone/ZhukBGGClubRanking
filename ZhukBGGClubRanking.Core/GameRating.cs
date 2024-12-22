using System.Collections.Generic;
using System.Linq;

namespace ZhukBGGClubRanking.Core
{
    public class GameRating
    {
        public string Game
        {
            get
            {
                if (string.IsNullOrWhiteSpace(GameRus))
                    return GameEng;
                return string.Format("{0} <{1}>", GameRus, GameEng);
            }
        }
        public string GameEng { get; set; }
        public string GameRus { get; set; }
        public int Rating { get; set; }
        public int Weight { get; set; }
        public int CompliancePercent { get; set; }
        public BGGCollection.ItemElement BGGItem { get; set; }
        public List<UserRating> UserRating { get; set; }

        public GameRating()
        {
            UserRating = new List<UserRating>();
        }

        public string BGGComments
        {
            get
            {
                if (BGGItem != null)
                    return BGGItem.Comment;
                return string.Empty;
            }
        }

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
    }
}
