namespace ZhukBGGClubRanking.Core
{
    public class GamesNamesTranslate
    {
        public string NameEng { get; set; }
        public string NameRus { get; set; }
        public string TeseraName { get; set; }
        public string ParentEngName { get; set; }

        public GamesNamesTranslate Parent { get; set; }
    }
}
