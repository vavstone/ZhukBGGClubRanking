﻿using Csv;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ZhukBGGClubRanking.Core;

namespace ZhukBGGClubRanking.WinApp.Core
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
            
            var csvArray = CsvReader.ReadFromText(System.IO.File.ReadAllText(Settings.GamesNamesTranslateFilePath));
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

        
    }
}
