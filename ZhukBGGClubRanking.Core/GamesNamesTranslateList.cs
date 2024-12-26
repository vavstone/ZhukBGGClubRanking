using System.Collections.Generic;
using System.Linq;

namespace ZhukBGGClubRanking.Core
{
    public class GamesNamesTranslateList
    {
        public List<GamesNamesTranslate> TranslateList { get; set; }

        public GamesNamesTranslateList()
        {
            TranslateList = new List<GamesNamesTranslate>();
        }

        public string GetNameRus(string nameEng)
        {
            var item = TranslateList.FirstOrDefault(c => c.NameEng == nameEng);
            if (item != null)
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