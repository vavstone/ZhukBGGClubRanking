using Csv;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ZhukBGGClubRanking.Core
{
    public class GamesNamesTranslateFile
    {
        public List<GamesNamesTranslate> TranslateList { get; set; }

        public static GamesNamesTranslateFile LoadFromFile()
        {
            var result = new GamesNamesTranslateFile();
            var list = new List<GamesNamesTranslate>();
            
            var csvArray = Csv.CsvReader.ReadFromText(System.IO.File.ReadAllText(Settings.GamesNamesTranslateFilePath));
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
            result.TranslateList = list;
            return result;
        }

        public string GetNameRus(string nameEng)
        {
            var item = TranslateList.FirstOrDefault(c => c.NameEng == nameEng);
            if (item!=null)
                return item.NameRus;
            return string.Empty;
        }

        public string GetTeseraName(string nameEng)
        {
            var item = TranslateList.FirstOrDefault(c => c.NameEng == nameEng);
            if (item != null)
                return item.TeseraName;
            return string.Empty;
        }
    }
}
