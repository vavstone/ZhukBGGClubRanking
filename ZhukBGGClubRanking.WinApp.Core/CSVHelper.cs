using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhukBGGClubRanking.Core;
using ZhukBGGClubRanking.Core.Model;

namespace ZhukBGGClubRanking.WinApp.Core
{
    public static class CSVHelper
    {

        public static List<RatingItem> GetRatingItemsFromCSVFile(string csvFileFullName, List<Game> gamesCollection)
        {
            var csvArray = Csv.CsvReader.ReadFromText(System.IO.File.ReadAllText(csvFileFullName));
            var ratingItems = new List<RatingItem>();
            foreach (var arItem in csvArray)
            {
                var game = gamesCollection.FirstOrDefault(c => c.NameEng == arItem[1]);
                if (game != null && game.IsStandaloneGame)
                {
                    ratingItems.Add(new RatingItem
                    {
                        GameId = game.Id,
                        RatingOrder = Int32.Parse(arItem[0])
                    });
                }
            }
            return ratingItems;
        }
    }
}
