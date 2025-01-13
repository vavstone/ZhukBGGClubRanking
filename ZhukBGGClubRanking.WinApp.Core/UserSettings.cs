using System;
using System.IO;
using System.Xml.Serialization;
using ZhukBGGClubRanking.Core.Model;

namespace ZhukBGGClubRanking.WinApp.Core
{
    [XmlRoot]
    public class UserSettings
    {
        [XmlAttribute] public bool TeseraPreferable { get; set; } = true;
        [XmlElement] public TablesSettings Tables { get; set; } = new TablesSettings();
        [XmlElement] public HostingSettings Hosting { get; set; } = new HostingSettings();

        public static UserSettings GetUserSettings()
        {
            if (!File.Exists(Constants.UserSettingsFileFullName))
            {
                var settings = new UserSettings();
                settings.SaveUserSettings();
                return settings;
            }
            var serializer = new XmlSerializer(typeof(UserSettings));
            using (var reader = new StreamReader(Constants.UserSettingsFileFullName))
            {
                var userSettings = (UserSettings) serializer.Deserialize(reader);
                return userSettings;
            }
        }

        public void SaveUserSettings()
        {
            var serializer = new XmlSerializer(typeof(UserSettings));
            using (FileStream fs = new FileStream(Constants.UserSettingsFileFullName, FileMode.Create))
            {
                serializer.Serialize(fs, this);
            }
        }

    }

    public class TablesSettings:IComparable<TablesSettings>
    {
        [XmlElement] public TableSettings UserRatingTable { get; set; } = new TableSettings();
        [XmlElement] public TableSettings AverageRatingTable { get; set; } = new TableSettings();

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

    public class HostingSettings
    {
        [XmlAttribute] public string Url { get; set; } = "http://localhost:5116";
        [XmlAttribute] public string Login { get; set; }
        [XmlAttribute] public string Password { get; set; }

        //[XmlAttribute] public string Url { get; set; } = "http://zhukbggtest-001-site1.ltempurl.com";
        //[XmlAttribute] public string Login { get; set; } = "11210871";
        //[XmlAttribute] public string Password { get; set; } = "60-dayfreetrial";
    }
}
