using Csv;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ZhukBGGClubRanking.Core
{
    public class GameRatingListFile
    {
        public GameRatingList File { get; set; }
        public string FileNameWithoutExt { get; set; }

        public static List<GameRatingListFile> LoadFromFolder()
        {
            var bggcoll = BGGCollection.LoadFromFile();
            string folderFullName = Settings.ListsDir;
            var result = new List<GameRatingListFile>();
            foreach (var item in Directory.GetFiles(folderFullName).Where(c=>c.ToLower().EndsWith("csv")))
            {
                var file = LoadFromFile(item);
                file.File.SetBGGCollection(bggcoll);
                result.Add(file);
            }
            return result;
        }

        

        public static GameRatingListFile LoadFromFile(string fileFullName)
        {
            var item = new GameRatingListFile();
            item.FileNameWithoutExt = Path.GetFileNameWithoutExtension(fileFullName);
            var csvArray = Csv.CsvReader.ReadFromText(System.IO.File.ReadAllText(fileFullName));
            var grList = new GameRatingList();
            grList.UserNames.Add(item.FileNameWithoutExt);
            item.File = grList;
            foreach (var arItem in csvArray.OrderBy(c => c.Values[0]))
            {
                var gr = new GameRating();
                gr.Game =  arItem[1];
                gr.Rating = Int32.Parse(arItem[0]);
                grList.GameList.Add(gr);
            }
            grList.CalculateWeightByRating();
            return item;
        }

        public void SaveToFile()
        {
            string fileName = Path.Combine(Settings.ListsDir, this.FileNameWithoutExt + ".csv");
            var txt = CsvWriter.WriteToText(
                new[] {"Rank", "Item"},
                File.GameList.OrderBy(c => c.Rating).Select(c => new[] {c.Rating.ToString(), c.Game}).ToList()
            );
            System.IO.File.WriteAllText(fileName, txt);
        }
    }
}
