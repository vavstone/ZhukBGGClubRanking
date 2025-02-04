using System.Collections.Generic;
using System.Linq;

namespace ZhukBGGClubRanking.Core
{
    public class GameRatingList
    {
        public List<string> UserNames { get; set; }
        public List<GameRating> GameList { get; set; }
        public BGGCollection CommonCollection { get; set; }

        public void SetBGGCollection(BGGCollection bgGCollection)
        {
            CommonCollection = bgGCollection;
            foreach (var game in GameList)
            {
                game.BGGItem = CommonCollection.GetItemByName(game.GameEng);
                //if (game.BGGItem != null)
                //{
                //    game.GameRus = game.BGGItem.NameRus;
                //}
                game.GameRus = CommonCollection.GamesTranslation.GetNameRus(game.GameEng);
            }
        }

        public GameRatingList()
        {
            GameList = new List<GameRating>();
            UserNames = new List<string>();
        }

        public void CalculateWeightByRating(int maxUsersCollectionSize=0)
        {
            if (GameList.Count==0) return;
            var curWeigth = 0;
            if (maxUsersCollectionSize > 0)
                curWeigth = maxUsersCollectionSize;
            else
                curWeigth = GameList.Count;
            //GameList.Select(c => c.Rating).Max();
            foreach (var item in GameList.OrderBy(c=>c.Rating))
            {
                item.Weight = curWeigth--;
            }
        }

        public void CalculateRatingByWeight()
        {
            if (GameList.Count == 0) return;
            var curRating = 1;
            foreach (var item in GameList.OrderByDescending(c => c.Weight))
            {
                item.Rating = curRating++;
            }
        }

        public void ReCalculateRatingAfterRemoveItems()
        {
            if (GameList.Count == 0) return;
            var curRating = 1;
            foreach (var item in GameList.OrderBy(c => c.Rating))
            {
                item.Rating = curRating++;
            }
        }


        public static GameRatingList CalculateAvarageRating(List<GameRatingList> inList, BGGCollection commonCollection,
            int topXPos)
        {
            var selectedRatings = new List<GameRatingList>();
            var maxCollSize = inList.Select(c => c.GameList.Count).Max();
            var onlyTop = topXPos < maxCollSize;

            var gamesToSkip = new List<string>();
            if (onlyTop)
            {

                foreach (var gameRatingList in inList)
                {
                    var list = gameRatingList.GameList.OrderBy(c => c.Rating).ToList();
                    if (onlyTop)
                        gamesToSkip.AddRange(list.Skip(topXPos).Select(c => c.GameEng));
                }
            }

            gamesToSkip = gamesToSkip.Distinct().ToList();

            foreach (var gameRatingList in inList)
            {
                var newRating = new GameRatingList();
                selectedRatings.Add(newRating);
                var list = gameRatingList.GameList.Where(c => gamesToSkip.All(c1 => c1 != c.GameEng))
                    .OrderBy(c => c.Rating).ToList();
                newRating.GameList = list;
            }

            if (onlyTop)
            {
                foreach (var gameRatingList in selectedRatings)
                {
                    var newRating = new List<GameRating>();
                    foreach (var rating in gameRatingList.GameList)
                    {
                        var gameInOtherRatings =
                            selectedRatings.Select(c => c.GameList).All(c => c.Any(c1 => c1.GameEng == rating.GameEng));
                        if (gameInOtherRatings)
                            newRating.Add(rating);
                    }

                    gameRatingList.GameList = newRating;
                }
            }



            var newMaxCollectionSize = selectedRatings.Select(c => c.GameList.Count).Max();

            foreach (var gameRatingList in selectedRatings)
            {
                gameRatingList.CalculateWeightByRating(newMaxCollectionSize);
            }

            var result = new GameRatingList();
            var commonRating = new List<GameRating>();
            foreach (var item in selectedRatings)
            {
                commonRating.AddRange(item.GameList);
            }

            var commonGamesListUnigue = commonRating.Select(c => c.GameEng).Distinct();
            foreach (var gameName in commonGamesListUnigue)
            {
                var sumWeigth = commonRating.Where(c => c.GameEng == gameName).Sum(c => c.Weight);
                var bggItem = commonCollection.GetItemByName(gameName);
                var gameItem = new GameRating();
                if (bggItem != null)
                {
                    gameItem.GameEng = bggItem.Name;
                    gameItem.GameRus = bggItem.NameRus;
                    gameItem.BGGItem = bggItem;
                }
                else
                {
                    gameItem.GameEng = gameName;
                }

                gameItem.Weight = sumWeigth;
                result.GameList.Add(gameItem);
            }

            result.CalculateRatingByWeight();
            result.GameList = result.GameList.OrderBy(c => c.Rating).ToList();
            result.UserNames.AddRange(selectedRatings.SelectMany(c => c.UserNames));
            return result;
        }

        public List<GameRating> GetGamesNotInCollectionButExistingInOthers(List<GameRatingList> otherCollections)
        {
            var lastRating = GameList.Select(c => c.Rating).Max();
            var result = new List<GameRating>();
            var gamesInCommonColl = new List<string>();
            var allGames = new List<string>();
            if (CommonCollection != null)
            {
                gamesInCommonColl = CommonCollection.Items.Where(c => c.Status.Own).Select(c => c.Name).ToList();
                foreach (var commonGame in gamesInCommonColl)
                {
                    var translateGame =
                        CommonCollection.GamesTranslation.TranslateList.FirstOrDefault(c => c.NameEng == commonGame);
                    if (translateGame == null || translateGame.Parent == null)
                        allGames.Add(commonGame);
                }

            }
            allGames.AddRange(otherCollections.SelectMany(c => c.GameList).Select(c => c.GameEng).Distinct());


            foreach (var game in allGames.Distinct().OrderBy(c => c))
            {
                if (GameList.All(c => c.GameEng != game))
                    result.Add(new GameRating { GameEng = game, GameRus = CommonCollection.GamesTranslation.GetNameRus(game), Rating = lastRating++ });
            }
            return result;
        }
    }
}