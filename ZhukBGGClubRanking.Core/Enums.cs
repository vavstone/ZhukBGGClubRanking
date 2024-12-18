using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ZhukBGGClubRanking.Core
{
    public enum WishlistPriority
    {
        NotSet = 0,
        [XmlEnum("1")]
        MustHave = 1,
        [XmlEnum("2")]
        LoveToHave = 2,
        [XmlEnum("3")]
        LikeToHave = 3,
        [XmlEnum("4")]
        ThinkingAboutIt = 4,
        [XmlEnum("5")]
        DontBuyThis = 5
    }
}
