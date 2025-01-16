using System.Collections.Generic;
using ZhukBGGClubRanking.Core.Model;

namespace ZhukBGGClubRanking.Core
{
    public class GameFullInfoWrapper
    {

        public Game Game { get; set; }
        public TeseraBGGRawGame RawGame { get; set; }

        public BGGGameLinkCollection BGGLinkCollection { get; set; }

        

        public int? RawGameId
        {
            get 
            { 
                if (RawGame!=null) return Game.Id;
                return null;
            }
        }

        public string NameWithYear
        {
            get
            {
                if (Game != null) return Game.NameWithYear;
                if (RawGame != null) return RawGame.NameWithYear;
                return null;
            }
        }

        public string Name
        {
            get
            {
                if (Game != null) return Game.Name;
                if (RawGame != null) return RawGame.Name;
                return null;
            }
        }

        public string NameEng
        {
            get
            {
                if (Game != null) return Game.NameEng;
                if (RawGame != null) return RawGame.NameEng;
                return null;
            }
        }

        public string NameRus
        {
            get
            {
                if (Game != null) return Game.NameRus;
                if (RawGame != null) return RawGame.NameRus;
                return null;
            }
        }
        public int? YearPublished 
        {
            get
            {
                if (Game != null) return Game.YearPublished;
                if (RawGame != null) return RawGame.YearPublished;
                return null;
            }
        }

        public int? BGGObjectId
        {
            get
            {
                if (Game!=null) return Game.BGGObjectId;
                if (RawGame != null) return RawGame.BGGObjectId;
                return null;
            }
        }

        public string TeseraKey
        {
            get
            {
                if (Game != null) return Game.TeseraKey;
                if (RawGame != null) return RawGame.TeseraAlias;
                return null;
            }
        }
        public int? TeseraId
        {
            get
            {
                if (Game != null) return Game.TeseraId;
                if (RawGame != null) return RawGame.TeseraId;
                return null;
            }
        }


        public bool IsAddition
        {
            get
            {
                if (Game != null) return Game.IsAddition;
                if (RawGame != null) return RawGame.IsAddition;
                return false;
            }
        }

        public List<GameOwner> Owners
        {
            get
            {
                if (Game != null) return Game.Owners;
                return new List<GameOwner>();
            }
        }

        public bool IsStandaloneGame
        {
            get
            {
                return !IsAddition;
            }
        }

        public string IsStandaloneGameString
        {
            get
            {
                return IsStandaloneGame ? "да" : "нет";
            }
        }

        public string OwnersString
        { 
            get
            {
                if (Game != null) return Game.OwnersString;
                return null;
            }
        }


        public string DesignersString
        {
            get
            {
                if (BGGLinkCollection != null) return BGGLinkCollection.DesignersString;
                return null;
            }
        }

        public string MechanicsString
        {
            get
            {
                if (BGGLinkCollection != null) return BGGLinkCollection.MechanicsString;
                return null;
            }
        }

        public string CategoriesString
        {
            get
            {
                if (BGGLinkCollection != null) return BGGLinkCollection.CategoriesString;
                return null;
            }
        }

        public string FamiliesString
        {
            get
            {
                if (BGGLinkCollection != null) return BGGLinkCollection.FamiliesString;
                return null;
            }
        }

        



        public override string ToString()
        {
            return NameWithYear;
        }


    }
}