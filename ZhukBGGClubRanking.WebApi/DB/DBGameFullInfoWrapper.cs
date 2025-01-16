using System.Data;
using ZhukBGGClubRanking.WebApi.DB;

namespace ZhukBGGClubRanking.Core
{
    public static class DBGameFullInfoWrapper
    {

        public static List<GameFullInfoWrapper> GetGames(List<User> users, GamesFilter filter)
        {
            var result = new List<GameFullInfoWrapper>();
            var gamesCollection = DBGame.GetGamesCollection(users, true);
            if (!filter.GetOnlyClubCollection)
            {
                var rawGames = DBTeseraBGGRawGame.GetGamesShortInfo();
                foreach (var game in rawGames)
                {
                    result.Add(new GameFullInfoWrapper { RawGame = game});
                }
            }

            foreach (var game in gamesCollection)
            {
                GameFullInfoWrapper existingItem = result.FirstOrDefault(c =>
                    (c.BGGObjectId != null && c.BGGObjectId>0 && c.BGGObjectId.Value == game.BGGObjectId) ||
                    (c.TeseraId != null && game.TeseraId != null && c.TeseraId>0 && c.TeseraId.Value == game.TeseraId.Value));
                if (existingItem != null)
                {
                    existingItem.Game = game;
                }
                else
                {
                    result.Add(new GameFullInfoWrapper { Game = game});
                }
            }

            foreach (var resGame in result.Where(c=>c.BGGObjectId!=null))
            {
                resGame.BGGLinkCollection = DBGGGLinks.GetLinksForBGGGame(resGame.BGGObjectId.Value);
            }

            return result.OrderBy(c => c.NameWithYear).ToList();
        }

       
    }
}