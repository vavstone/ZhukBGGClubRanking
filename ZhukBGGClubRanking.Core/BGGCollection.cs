using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace ZhukBGGClubRanking.Core
{
    [XmlRoot("items")]
    public class BGGCollection
    {
        [XmlAttribute("totalitems")]
        public int TotalItems { get; set; }

        [XmlAttribute("termsofuse")]
        public string TermsOfUse { get; set; } = "";

        [XmlAttribute("pubdate")]
        public string PubDate { get; set; } = "";

        [XmlElement("item")]
        public List<ItemElement> Items { get; set; }

        public GamesNamesTranslateList GamesTranslation { get; set; }

        public BGGCollection()
        {
            Items  = new List<ItemElement>();
        }

        public class ItemElement
        {
            [XmlAttribute("objectid")]
            public int ObjectId { get; set; }

            [XmlElement("name")]
            public string Name { get; set; } = "";

            public string NameRus { get; set; } = "";

            [XmlElement("yearpublished")]
            public int YearPublished { get; set; }

            [XmlElement("image")]
            public string Image { get; set; } = "";

            [XmlElement("thumbnail")]
            public string Thumbnail { get; set; } = "";

            [XmlElement("stats")]
            public StatsElement Stats { get; set; }

            [XmlElement("status")]
            public StatusElement Status { get; set; }

            [XmlElement("numplays")]
            public int NumPlays { get; set; }

            [XmlElement("comment")]
            public string Comment { get; set; }

            public string TeseraKey { get; set; }

            public ItemElement()
            {
                Stats = new StatsElement();
                Status = new StatusElement();
            }

            public class StatsElement
            {
                [XmlAttribute("minplayers")]
                public int MinPlayers { get; set; }

                [XmlAttribute("maxplayers")]
                public int MaxPlayers { get; set; }

                [XmlAttribute("minplaytime")]
                public int MinPlaytime { get; set; }

                [XmlAttribute("maxplaytime")]
                public int MaxPlaytime { get; set; }

                [XmlAttribute("playingtime")]
                public int Playtime { get; set; }

                [XmlAttribute("numowned")]
                public int NumberOwned { get; set; }
            }

            public class StatusElement
            {
                [XmlAttribute("own")]
                public bool Own { get; set; }

                [XmlAttribute("prevowned")]
                public bool PreviouslyOwned { get; set; }

                [XmlAttribute("fortrade")]
                public bool ForTrade { get; set; }

                [XmlAttribute("want")]
                public bool Want { get; set; }

                [XmlAttribute("wanttoplay")]
                public bool WantToPlay { get; set; }

                [XmlAttribute("wanttobuy")]
                public bool WantToBuy { get; set; }

                [XmlAttribute("wishlist")]
                public bool Wishlist { get; set; }

                [XmlAttribute("wishlistpriority")]
                public WishlistPriority WishlistPriority { get; set; }

                [XmlAttribute("preordered")]
                public bool Preordered { get; set; }

                [XmlAttribute("lastmodified")]
                public string LastModifiedString { get; set; } = "";
            }


            public Game CreateGame()
            {
                var game = new Game();
                game.NameRus = NameRus;
                game.NameEng = game.NameBGG = Name;
                game.BGGObjectId = ObjectId;
                game.ThumbnailBGG = Thumbnail;
                game.ImageBGG = Image;
                game.YearPublished = YearPublished;
                game.TeseraKey = TeseraKey;
                return game;
            }
        }

        public static BGGCollection LoadFromFile()
        {

            if (File.Exists(CoreSettings.CommonCollectionFilePath))
            {
                var serializer = new XmlSerializer(typeof(BGGCollection));
                using (StreamReader reader = new StreamReader(CoreSettings.CommonCollectionFilePath))
                {
                    var res = (BGGCollection)serializer.Deserialize(reader);
                    return res;
                }
            }
            return null;
        }


        public ItemElement GetItemByName(string name)
        {
            return Items.FirstOrDefault(c => c.Name.ToUpper() == name.ToUpper());
        }

        public void ApplyTranslation(GamesNamesTranslateList gamesTranslation)
        {
            GamesTranslation = gamesTranslation;
            foreach (var game in GamesTranslation.TranslateList)
            {
                var bggItem = GetItemByName(game.NameEng);
                if (bggItem != null)
                {
                    bggItem.NameRus = game.NameRus;
                    bggItem.TeseraKey = game.TeseraName;
                }
            }
        }


    }
}
