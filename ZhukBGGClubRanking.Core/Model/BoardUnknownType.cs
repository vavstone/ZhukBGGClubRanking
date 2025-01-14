namespace ZhukBGGClubRanking.Core
{
    public class BoardUnknownLinkType : BGGGameLink
    {
        public const string LinkType = "unknown";
        public const string TableName = "bgunknownlinktype";

        public string TitleType { get; set; }
    }
}
