namespace ZhukBGGClubRanking.Core
{
    public class GameRating
    {
        public string Game { get; set; }
        public int Rating { get; set; }
        public int Weight { get; set; }
        public int CompliancePercent { get; set; }
        public BGGCollection.ItemElement BGGItem { get; set; }

        public string BGGComments
        {
            get
            {
                if (BGGItem != null)
                    return BGGItem.Comment;
                return string.Empty;
            }
        }
    }
}
