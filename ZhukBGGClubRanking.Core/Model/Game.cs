using System;
using System.Collections.Generic;
using System.Linq;
using BoardGamer.BoardGameGeek.BoardGameGeekXmlApi2;
using ZhukBGGClubRanking.Core.Code;
using ZhukBGGClubRanking.Core.Model;
using static BoardGamer.BoardGameGeek.BoardGameGeekXmlApi2.UserResponse;
using static System.Net.Mime.MediaTypeNames;

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
        public int? TeseraId { get; set; }
        public int ParentId { get; set; }
        //public string BGGComments { get; set; } = "";
        public DateTime CreateTime { get; set; }
        public int CreateUserId { get; set; }
        public bool IsActual { get; set; }

        public bool IsAddition { get; set; }

        public string ParentEngName { get; set; }

        public List<GameOwner> Owners { get; set; } = new List<GameOwner>();

        public bool IsStandaloneGame
        {
            get
            {
                if (ParentId>0) return false;
                return !IsAddition;
            }
        }

        public ThingResponse.Item BGGExtendedInfo { get; set; }

        public string Name
        {
            get
            {
                if (string.IsNullOrWhiteSpace(NameRus))
                    return NameEng;
                return string.Format("{0} <{1}>", NameRus, NameEng);
            }
        }

        public string OwnersString
        { 
            get
            {
                var res = string.Empty;
                foreach (var item in Owners.OrderBy(c=>c.UserName))
                {
                    res += item.UserName + " + ";
                }
                if (res.Length > 3)
                    res = res.Substring(0, res.Length - 3);
                return res;
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

        public void SetParents( List<Game> games)
        {
            if (!string.IsNullOrEmpty(ParentEngName))
            {
                var parentGame = games.FirstOrDefault(c=>c.NameEng == ParentEngName);
                if (parentGame!=null)
                    ParentId = parentGame.Id;
            }
        }



        public static Game CreateGame(List<TeseraBGGRawGame> rawGames, List<User> owners, bool getExtendedBGGInfo, int? bggId, int? teseraId, string teseraAlias = null, string parentName = null)
        {
            var currentTime = DateTime.Now;
            var game = new Game();
            game.CreateTime = currentTime;
            game.IsActual = true;
            var firstOwner = owners.FirstOrDefault();
            game.CreateUserId = firstOwner==null ? 1 : firstOwner.Id;

            TeseraBGGRawGame rawItem = null;
            if (bggId!=null && bggId>0)
                rawItem = rawGames.FirstOrDefault(c => c.BGGInfo != null && c.BGGInfo.Id == bggId);
            if (rawItem == null && teseraId!=null && teseraId>0)
            {
                rawItem = rawGames.FirstOrDefault(c =>
                        c.TeseraInfo != null && c.TeseraInfo.TeseraId!=null && c.TeseraInfo.TeseraId.Value==teseraId);
            }
            if (rawItem == null && !string.IsNullOrWhiteSpace(teseraAlias))
            {
                rawItem = rawGames.FirstOrDefault(c =>
                    c.TeseraInfo != null && c.TeseraInfo.Alias != null && c.TeseraInfo.Alias.ToUpper() == teseraAlias.ToUpper());
            }

            if (rawItem != null)
            {
                var teseraInfo = rawItem.TeseraInfo;
                var bggInfo = rawItem.BGGInfo;

                if (bggInfo != null)
                {
                    game.NameEng = game.NameBGG = bggInfo.Name;
                    game.BGGObjectId = bggInfo.Id;
                    if (getExtendedBGGInfo)
                    {
                        game.BGGExtendedInfo = BGGHelper.GetGame(bggInfo.Id);
                        if (game.BGGExtendedInfo != null)
                        {
                            game.ThumbnailBGG = game.BGGExtendedInfo.Thumbnail;
                            game.ImageBGG = game.BGGExtendedInfo.Image;
                            if (game.BGGExtendedInfo.YearPublished != null)
                                game.YearPublished = game.BGGExtendedInfo.YearPublished.Value;
                        }
                    }
                }

                if (teseraInfo != null)
                {
                    game.NameRus = teseraInfo.Title;
                    game.TeseraKey = teseraInfo.Alias;
                    game.TeseraId = teseraInfo.TeseraId;
                    if (game.YearPublished == 0 && teseraInfo.Year != null) game.YearPublished = teseraInfo.Year.Value;
                }
                game.ParentEngName = parentName;
                game.IsAddition = rawItem.IsAddition;
                foreach (var owner in owners)
                {
                    var gameOwner = new GameOwner();
                    gameOwner.UserId = owner.Id;
                    gameOwner.UserName = owner.Name;
                    gameOwner.CreateTime = currentTime;
                    game.Owners.Add(gameOwner);

                }
                return game;
            }
            throw new Exception("Не найдена информация об игре на Tesera и BGG");
        }

        public Game CreateCopy()
        {
            var newGame = new Game();
            newGame.Id = Id;
            newGame.BGGObjectId = BGGObjectId;
            newGame.TeseraId = TeseraId;
            newGame.NameEng = NameEng;
            newGame.NameRus = NameRus;
            newGame.ImageBGG = ImageBGG;
            newGame.ThumbnailBGG = ThumbnailBGG;
            newGame.YearPublished = YearPublished;
            newGame.TeseraKey = TeseraKey;
            newGame.ParentId = ParentId;
            newGame.ParentEngName = ParentEngName;
            newGame.IsAddition = IsAddition;
            foreach (var gameOwner in Owners)
                newGame.Owners.Add(gameOwner);
            return newGame;
        }

        
    }
}