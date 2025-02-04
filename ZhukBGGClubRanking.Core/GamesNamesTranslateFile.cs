using Csv;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using ZhukBGGClubRanking.Core.Model;

namespace ZhukBGGClubRanking.Core
{
    public class GamesNamesTranslateFile
    {
        public GamesNamesTranslateList GamesTranslate { get; set; }

        public GamesNamesTranslateFile()
        {
            GamesTranslate = new GamesNamesTranslateList();
        }

        public static GamesNamesTranslateFile LoadFromFile()
        {
            var result = new GamesNamesTranslateFile();
            var list = new List<GamesNamesTranslate>();
            
            var csvArray = CsvReader.ReadFromText(System.IO.File.ReadAllText(CoreConstants.InitiateGamesNamesTranslateFilePath));
            foreach (var arItem in csvArray.OrderBy(c => c.Values[0]))
            {
                var gr = new GamesNamesTranslate();
                gr.NameEng =  arItem[0];
                gr.NameRus = arItem[1];
                gr.TeseraName = arItem[2];
                gr.ParentEngName = arItem[3];
                list.Add(gr);
            }

            foreach (var item in list)
            {
                if (!string.IsNullOrWhiteSpace(item.ParentEngName))
                {
                    var parent = list.FirstOrDefault(c => c.NameEng == item.ParentEngName);
                    if (parent != null)
                        item.Parent = parent;
                }
            }
            result.GamesTranslate.TranslateList = list;
            return result;
        }

        public static void UpdateTranslateFile(List<Game> commonGames)
        {
            var list = new List<GamesNamesTranslate>();
            var currentCsvArray = CsvReader.ReadFromText(System.IO.File.ReadAllText(CoreConstants.InitiateGamesNamesTranslateFilePath));

            foreach (var arItem in currentCsvArray.OrderBy(c => c.Values[0]))
            {
                var gr = new GamesNamesTranslate();
                gr.NameEng = arItem[0];
                gr.NameRus = arItem[1];
                gr.TeseraName = arItem[2];
                gr.ParentEngName = arItem[3];
                list.Add(gr);
            }

            foreach (var commonGame in commonGames.OrderBy(c=>c.NameEng))
            {
                if (!list.Any(c=>c.NameEng.ToUpper() == commonGame.NameEng.ToUpper()))
                {

                        var gr = new GamesNamesTranslate();
                        gr.NameEng = commonGame.NameEng ?? "";
                        gr.NameRus = commonGame.NameRus ?? "";
                        gr.TeseraName = commonGame.TeseraKey ?? "";
                        gr.ParentEngName = commonGame.ParentEngName ?? "";
                        list.Add(gr);
                }
            }
            var newCsvFileFullName = CoreConstants.ExportTranslateFilePath;
            if (File.Exists(newCsvFileFullName))
                File.Delete(newCsvFileFullName);
            if (!Directory.Exists(Path.GetDirectoryName(newCsvFileFullName)))
                Directory.CreateDirectory(Path.GetDirectoryName(newCsvFileFullName));
            var header = new[] { "NameEng", "NameRus", "TeseraName", "ParentEngName" };
            var data = new List<string[]>();
            foreach (var item in list)
            {
                var arData = new[] { item.NameEng, item.NameRus, item.TeseraName, item.ParentEngName };
                data.Add(arData);
            }
            File.WriteAllText(newCsvFileFullName, CsvWriter.WriteToText(header, data, ',', false));
        }
    }
}
