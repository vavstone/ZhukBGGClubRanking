using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ZhukBGGClubRanking.Core.Model
{
    public class TeseraBGGRawGame
    {
        public int Id {  get; set; }
        public TeseraRawGame TeseraInfo { get; set; }
        public BGGRawGame BGGInfo { get; set; }
        public bool IsAddition { get; set; }
        public int ParentId { get; set; }

        public string NameEng
        {
            get
            {
                if (BGGInfo != null) return BGGInfo.Name;
                return null;
            }
        }
        public string NameRus
        {
            get
            {
                if (TeseraInfo != null) return TeseraInfo.Title;
                return null;
            }
        }
        public int? YearPublished
        {
            get
            {
                if (BGGInfo!=null && BGGInfo.YearPublished != null && BGGInfo.YearPublished.Value > 0)
                    return BGGInfo.YearPublished.Value;
                if (TeseraInfo != null && TeseraInfo.Year != null && TeseraInfo.Year.Value > 0)
                    return TeseraInfo.Year.Value;
                return null;
            }
        }
        public int? BGGObjectId
        {
            get
            {
                if (BGGInfo != null)
                    return BGGInfo.Id;
                return null;
            }
        }
        public string TeseraAlias
        {
            get
            {
                if (TeseraInfo != null) return TeseraInfo.Alias;
                return null;
            }
        }
        public int? TeseraId
        {
            get
            {
                if (TeseraInfo != null) return TeseraInfo.TeseraId;
                return null;
            }
        }



        public static List<TeseraBGGRawGame> GetMergedTeseraBGGInfoList(List<BGGRawGame> bggGames, List<TeseraRawGame> teseraGames)
        {
            var res = new List<TeseraBGGRawGame>();
            foreach (var bggGame in bggGames)
            {
                var game = new TeseraBGGRawGame();
                game.BGGInfo = bggGame;
                res.Add(game);
            }
            foreach (var teseraGame in teseraGames.Where(c => c.BGGId > 0))
            {
                var bggGame = bggGames.FirstOrDefault(c => c.Id == teseraGame.BGGId);
                if (bggGame != null)
                {
                    var gameWithEmptyTeseraInfo = res.FirstOrDefault(c => c.BGGInfo!=null && c.BGGInfo.Id == teseraGame.BGGId && c.TeseraInfo == null);
                    if (gameWithEmptyTeseraInfo == null)
                    {
                        gameWithEmptyTeseraInfo = new TeseraBGGRawGame();
                        gameWithEmptyTeseraInfo.BGGInfo = bggGame;
                        res.Add(gameWithEmptyTeseraInfo);
                    }
                    gameWithEmptyTeseraInfo.TeseraInfo = teseraGame;
                }
                else
                {
                    var gameWithEmptyBGGInfo = new TeseraBGGRawGame();
                    gameWithEmptyBGGInfo.TeseraInfo = teseraGame;
                    res.Add(gameWithEmptyBGGInfo);
                }
            }
            foreach (var teseraGame in teseraGames.Where(c => c.BGGId == 0))
            {
                var gameWithEmptyBGGInfo = new TeseraBGGRawGame();
                gameWithEmptyBGGInfo.TeseraInfo = teseraGame;
                res.Add(gameWithEmptyBGGInfo);
            }
            return res.OrderBy(c => c.BGGInfo == null ? 9999999 : c.BGGInfo.Id).ThenBy(c => c.TeseraInfo == null ? 9999999 : c.TeseraInfo.Id).ToList();
        }

        public static void SetParentsAndIsAddition(List<TeseraBGGRawGame> games)
        {
            foreach (var game in games)
            {
                if (game.TeseraInfo != null && game.TeseraInfo.IsAddition != null)
                    game.IsAddition = game.TeseraInfo.IsAddition.Value;
                if (game.BGGInfo != null)
                    game.IsAddition = game.BGGInfo.IsExpansion;
                //TODO реализовать определение на основании вспомогательной таблицы база-дополнения
            }
        }

        public override string ToString()
        {
            return NameWithYear;
        }

        public string Name
        {
            get
            {
                if (TeseraInfo != null && BGGInfo != null && TeseraInfo.Title.ToUpper() != BGGInfo.Name.ToUpper())
                    return string.Format("{0} <{1}>", TeseraInfo.Title, BGGInfo.Name);
                if (TeseraInfo != null)
                    return TeseraInfo.Title;
                if (BGGInfo != null)
                    return BGGInfo.Name;
                return "";
            }
        }

        public string NameWithYear
        {
            get
            {
                var yearAddStr = (YearPublished==null || YearPublished == 0) ? "" : " " + YearPublished;
                return Name + yearAddStr;
            }
        }

        public static List<TeseraBGGRawGame> FilterRawGames(IEnumerable<TeseraBGGRawGame> sourceList, string searchMask, int minSearchMaskLength)
        {
            if (minSearchMaskLength==0 || searchMask.Length<minSearchMaskLength)
                return new List<TeseraBGGRawGame>();
            return sourceList.Where(c => c.NameWithYear.ToUpper().Contains(searchMask.ToUpper())).OrderBy(c=>c.NameWithYear).ToList();
        }

    }
}
