using Csv;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ZhukBGGClubRanking.Core;

namespace ZhukBGGClubRanking.WinApp.Core
{
    public class GameRatingListFile
    {
        public GameRatingList RatingList { get; set; }
        public string FileNameWithoutExt { get; set; }

        public static List<GameRatingListFile> LoadFromFolder(BGGCollection commonCollection)
        {
            //var bggcoll = BGGCollection.LoadFromFile();
            string folderFullName = Settings.ListsDir;
            var result = new List<GameRatingListFile>();
            foreach (var item in Directory.GetFiles(folderFullName).Where(c=>c.ToLower().EndsWith("csv")))
            {
                var file = LoadFromFile(item, commonCollection);
                
                result.Add(file);
            }
            return result;
        }

        

        public static GameRatingListFile LoadFromFile(string fileFullName, BGGCollection commonCollection)
        {
            var item = new GameRatingListFile();
            item.FileNameWithoutExt = Path.GetFileNameWithoutExtension(fileFullName);
            var csvArray = Csv.CsvReader.ReadFromText(System.IO.File.ReadAllText(fileFullName));
            var grList = new GameRatingList();
            grList.UserNames.Add(item.FileNameWithoutExt);
            item.RatingList = grList;
            foreach (var arItem in csvArray.OrderBy(c => c.Values[0]))
            {
                var gr = new GameRating();
                gr.GameEng =  arItem[1];
                gr.Rating = Int32.Parse(arItem[0]);
                var isAdditionToGame = false;
                var gameInfo =
                    commonCollection.GamesTranslation.TranslateList.FirstOrDefault(c => c.NameEng == gr.GameEng);
                if (gameInfo != null && gameInfo.Parent != null)
                    isAdditionToGame = true;
                if (!isAdditionToGame)
                {
                    grList.GameList.Add(gr);
                }
            }
            grList.ReCalculateRatingAfterRemoveItems();
            grList.CalculateWeightByRating();
            grList.SetBGGCollection(commonCollection);
            return item;
        }

        public void SaveToFile()
        {
            string fileName = Path.Combine(Settings.ListsDir, this.FileNameWithoutExt + ".csv");
            var txt = CsvWriter.WriteToText(
                new[] {"Rank", "Item"},
                RatingList.GameList.OrderBy(c => c.Rating).Select(c => new[] {c.Rating.ToString(), c.Game}).ToList()
            );
            System.IO.File.WriteAllText(fileName, txt);
        }
    }
}
