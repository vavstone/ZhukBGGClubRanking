using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ZhukBGGClubRanking.Core
{
    [XmlRoot]
    public class UserSettings
    {
        [XmlAttribute] public bool TeseraPreferable { get; set; } = true;
        [XmlElement] public TablesSettings Tables { get; set; } = new TablesSettings();

        public static UserSettings GetUserSettings()
        {
            return new UserSettings();
        }
    }

    public class TablesSettings:IComparable<TablesSettings>
    {
        [XmlAttribute] public TableSettings UserRatingTable { get; set; } = new TableSettings();
        [XmlAttribute] public TableSettings AverageRatingTable { get; set; } = new TableSettings();

        public TablesSettings CreateCopy()
        {
            var res = new TablesSettings();
            res.AverageRatingTable = this.AverageRatingTable.CreateCopy();
            res.UserRatingTable = this.UserRatingTable.CreateCopy();
            return res;
        }

        public int CompareTo(TablesSettings other)
        {
            if (other == null) return 1;
            if (UserRatingTable != other.UserRatingTable) return -1;
            if (AverageRatingTable != other.AverageRatingTable) return -1;
            return 0;
        }
    }

    public class TableSettings:IComparable<TableSettings>
    {
        [XmlAttribute] public bool ShowRating { get; set; } = true;
        [XmlAttribute] public bool ShowUsersRating { get; set; } = true;
        [XmlAttribute] public bool ShowOwners { get; set; } = true;

        public TableSettings CreateCopy()
        {
            var res = new TableSettings();
            res.ShowRating = this.ShowRating;
            res.ShowUsersRating = this.ShowUsersRating;
            res.ShowOwners = this.ShowOwners;
            return res;
        }


        public int CompareTo(TableSettings other)
        {
            if (other == null) return 1;
            if (ShowRating!=other.ShowRating) return -1;
            if (ShowUsersRating != other.ShowUsersRating) return -1;
            if (ShowOwners != other.ShowOwners) return -1;
            return 0;
        }
    }

}
