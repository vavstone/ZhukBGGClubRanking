using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ZhukBGGClubRanking.Core.Model;

namespace ZhukBGGClubRanking.Core
{
    public static class TeseraRawGameParseWrapper
    {
        public static List<TeseraRawGame> LoadGamesFromJsonFiles()
        {
            var res = new List<TeseraRawGame>();
            var pathToFiles = Path.Combine(CoreConstants.OuterRawInfoFilesDir, DateTime.Now.ToString("yyyy-MM-dd"),"Tesera");
            if (Directory.Exists(pathToFiles))
            {
                foreach (var file in Directory.GetFiles(pathToFiles))
                {
                    var content = File.ReadAllText(file);
                    var list = JsonConvert.DeserializeObject<List<TeseraRawGame>>(content);
                    res.AddRange(list);
                }
            }
            return res;
        }
    }
}
