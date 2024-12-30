using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace ZhukBGGClubRanking.Core
{
    public class Game
    {
        public int Id { get; set; }
        public string NameEng { get; set; } = "";
        public string NameBGG { get; set; } = "";
        public string NameRus { get; set; } = "";
        public int YearPublished { get; set; }
        public string ImageBGG { get; set; } = "";
        public string ThumbnailBGG { get; set; } = "";
        public int BGGObjectId { get; set; }
        public string TeseraKey { get; set; } = "";
        public int ParentId { get; set; }
        public string BGGComments { get; set; } = "";

        public bool IsStandaloneGame { get { return ParentId <= 0; } }

        public string Name
        {
            get
            {
                if (string.IsNullOrWhiteSpace(NameRus))
                    return NameEng;
                return string.Format("{0} <{1}>", NameRus, NameEng);
            }
        }

        public static string GetGameNameEngFromFormattedName(string formattedName)
        {
            if (string.IsNullOrWhiteSpace(formattedName))
                return formattedName;
            var indexOfStartQuot = formattedName.IndexOf('<');
            var indexOfEndQuot = formattedName.IndexOf('>');
            if (indexOfStartQuot >= 0 && indexOfEndQuot > 2)
            {
                return formattedName.Substring(indexOfStartQuot + 1, indexOfEndQuot - indexOfStartQuot - 1);
            }

            return formattedName;
        }

        public override string ToString()
        {
            var result = Name;
            if (YearPublished>0)
                result +=
            string.Format(" {0}", YearPublished);
            return result;
        }
    }
}