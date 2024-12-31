using Csv;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ZhukBGGClubRanking.Core;
using ZhukBGGClubRanking.Core.Model;

namespace ZhukBGGClubRanking.Core
{
    public class UserRatingFile
    {
        
        public static List<string> GetFilesNamesWithoutExt()
        {
            var result = new List<string>();
            var path = CoreSettings.InitiateUserRatingFilesDir;
            foreach(var file in Directory.GetFiles(path))
            {
                result.Add(Path.GetFileNameWithoutExtension(file));
            }
            return result;
        }

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

        public static List<UsersRating> GetUsersRatingsFromListsFolder(List<User> users, List<Game> gamesCollection)
        {
            var list = new List<UsersRating>();
            foreach (var csvFile in Directory.GetFiles(CoreSettings.InitiateUserRatingFilesDir))
            {
                var fileNameWithoutExt = Path.GetFileNameWithoutExtension(csvFile);
                var user = users.FirstOrDefault(c => c.Name.ToUpper() == fileNameWithoutExt.ToUpper());
                var ratingItems = GetRatingItemsFromCSVFile(csvFile, gamesCollection);
                list.Add(new UsersRating
                {
                    UserId = user.Id,
                    CreateTime = DateTime.Now,
                    Rating = new RatingCollection { RatingItems = ratingItems }
                });
            }
            return list;
        }
    }
}
