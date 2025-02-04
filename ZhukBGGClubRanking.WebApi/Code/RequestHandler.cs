using System.Net;
using ZhukBGGClubRanking.Core;
using ZhukBGGClubRanking.Core.Code;
using ZhukBGGClubRanking.Core.Model;
using ZhukBGGClubRanking.WebApi.Code;
using ZhukBGGClubRanking.WebApi.Core;
using ZhukBGGClubRanking.WebApi.DB;
using static BoardGamer.BoardGameGeek.BoardGameGeekXmlApi2.UserResponse;
using User = ZhukBGGClubRanking.Core.User;

namespace ZhukBGGClubRanking.WebApi
{
    public static class RequestHandler
    {
        //public static BGGCollection GetBggCollection()
        //{
        //    return BGGCollection.LoadFromFile();
        //}

        public static List<Game> GetGamesCollection(List<User> users)
        {
            return DBGame.GetGamesCollection(users, true);
        }

        public static List<GameFullInfoWrapper> GetGamesWithBGGLinks(List<User> users, GamesFilter filter)
        {
            return DBGameFullInfoWrapper.GetGames(users, filter);
        }

        public static List<TeseraBGGRawGame> GetRawGamesShortInfo()
        {
            return DBTeseraBGGRawGame.GetGamesShortInfo();
        }
        

        public static List<UsersRating> GetUsersActualRatings()
        {
            return DBUsersRating.GetUsersActualRatings();
        }

        public static void SaveUsersRating(UsersRating rating)
        {
            DBUsersRating.SaveRating(rating);
        }

        public static void SaveTeseraRawInfoGames(List<TeseraRawGame> teseraGames)
        {
            foreach (var item in teseraGames)
                DBTeseraRawGame.SaveTeseraRawGame(item);
        }

        public static List<User> GetUsers()
        {
            return DBUser.GetUsers();
        }

        public static void CreateNewUser(User newUser)
        {
            DBUser.CreateNewUser(newUser);
        }

        public static void SaveRatingsToCSVFiles()
        {
            var ratings = DBUsersRating.GetUsersActualRatings();
            var users = DBUser.GetUsers();
            var games = DBGame.GetGamesCollection(users,true);
            foreach (var rating in ratings)
            {
                UserRatingFile.SaveRatingToCSVFile(rating, games, users);
            } 
        }

        public static void ClearLinksBGGTables()
        {
            DBCommon.ClearLinksBGGTables();
        }

        public static void InitiateDB(User currentUser)
        {
            //1. получаем актуальную версию collection.xml с сайта BGG и обновляем collection.xml в club_collection\collection.xml
            BGGCollection.LoadFromUrlToFile();

            //2. читаем translate\collection.csv
            var translateFile = GamesNamesTranslateFile.LoadFromFile();

            //3. читаем игры в games из club_collection\collection.xml (с использованием информации из translate\collection.csv)
            var bggColl = BGGCollection.LoadFromFile();

            //4. очищаем таблицы БД
            DBCommon.ClearDB();

            //5. получаем список пользователей (по названиям файлов в lists\ и по владельцам игр в комментах БГГ) и создаем пользователей в users 
            var userNames = UserRatingFile.GetFilesNamesWithoutExt();
            var userNamesFromComments = bggColl.Items.SelectMany(c => c.OwnersList).ToList().Distinct();
            foreach (var name in userNamesFromComments)
            {
                if (!userNames.Any(c=>c.ToUpper()==name.ToUpper()))
                    userNames.Add(name);
            }
            foreach (var userName in userNames)
            {
                var user = new User();
                user.Name = user.FullName = user.Password = userName;
                user.EMail = string.Format("{0}@yandex.ru", userName);
                user.IsActive = true;
                user.CreateTime = DateTime.Now;
                user.Role = Role.UserRole;
                user.CreateUserId = currentUser.Id;
                user.HashPassword();
                DBUser.CreateNewUser(user);
            }

            //6. получаем созданных пользователей
            var users = DBUser.GetUsers();

            //7. применяем файл Translate при необходимости
            if (translateFile != null)
            {
                bggColl.ApplyTranslation(translateFile.GamesTranslate);
            }

            //8. получаем информацию из bgg_raw_info,bgg_tesera_raw_info
            var rawGames = DBTeseraBGGRawGame.GetGamesShortInfo();

            //9. применяем информацию из bgg_raw_info,bgg_tesera_raw_info, создаем игру
            List<Game> games = new List<Game>();
            foreach (var item in bggColl.Items)
            {
                if (!games.Any(c => c.BGGObjectId == item.ObjectId))
                {
                    var ownersNameList = item.OwnersList;
                    var owners = new List<User>();
                    foreach (var ownerName in ownersNameList)
                    {
                        var owner = users.FirstOrDefault(c => c.Name.ToUpper() == ownerName.ToUpper());
                        if (owner != null)
                            owners.Add(owner);
                    }

                    var game = Game.CreateGame(rawGames, owners, false, item.ObjectId, null, item.TeseraKey,
                        item.ParentName);
                    if (!string.IsNullOrWhiteSpace(item.Thumbnail))
                        game.ThumbnailBGG = item.Thumbnail;
                    if (!string.IsNullOrWhiteSpace(item.Image))
                        game.ImageBGG = item.Image;
                    games.Add(game);
                }

            }
            //10. сохраняем игры и bgg линки
            foreach (var game in games)
            {
                DBGame.SaveGame(game, false);
                DBGGGLinks.SaveLinksForBGGGame(game.BGGExtendedInfo);
            }
            //11. обновляем парентс
            foreach (var game in games)
            {
                game.SetParents(games);
                DBGame.UpdateParent(game);
            }

            //12. создаем рейтинги (rating_items, users_ratings) из файлов в lists\
            var ratings = UserRatingFile.GetUsersRatingsFromListsFolder(users, games);
            
            foreach (var userRating in ratings)
            {
                userRating.ReCalculateRatingAfterRemoveItems();
                DBUsersRating.SaveRating(userRating);
            }
        }

        

        public static void ClearTeseraRawTable()
        {
            DBCommon.ClearTeseraRawTable();
        }
        public static void ClearBGGRawTable()
        {
            DBCommon.ClearBGGRawTable();
        }
        public static void ClearBGGTeseraRawTable()
        {
            DBCommon.ClearBGGTeseraRawTable();
        }


        public static void SaveBGGAndTeseraRawGames()
        {
            var bggGames = DBBGGRawGame.GetGames();
            var teseraGames = DBTeseraRawGame.GetGames();
            var games = TeseraBGGRawGame.GetMergedTeseraBGGInfoList(bggGames, teseraGames);
            TeseraBGGRawGame.SetParentsAndIsAddition(games);
            DBTeseraBGGRawGame.SaveGames(games);

        }

        internal static string GetGameImagePathByBGGId(int bggid)
        {
            var pathToBGGsmallImages = CoreConstants.ImgCacheBGGSmallFilesDir;
            var pathToBGGlargeImages = CoreConstants.ImgCacheBGGLargeFilesDir;
            if (!Directory.Exists(pathToBGGsmallImages))
                Directory.CreateDirectory(pathToBGGsmallImages);
            if (!Directory.Exists(pathToBGGlargeImages))
                Directory.CreateDirectory(pathToBGGlargeImages);
            foreach (var file in Directory.GetFiles(pathToBGGlargeImages))
            {
                var fileNameWithoutExt = Path.GetFileNameWithoutExtension(file);
                if (fileNameWithoutExt == bggid.ToString())
                    return file;
            }
            foreach (var file in Directory.GetFiles(pathToBGGsmallImages))
            {
                var fileNameWithoutExt = Path.GetFileNameWithoutExtension(file);
                if (fileNameWithoutExt == bggid.ToString())
                    return file;
            }
            var bggItem = BGGHelper.GetGame(bggid);
            if (bggItem != null)
            {
                var pathToNewImage = WebAppSettings.GetFromBGGLargeImagesForThumbnails ? pathToBGGlargeImages : pathToBGGsmallImages;
                var tmpFileName = bggid.ToString();
                var tmpFileFullName = Path.Combine(pathToNewImage, tmpFileName);
                using (var client = new WebClient())
                {
                    var imgUrl = WebAppSettings.GetFromBGGLargeImagesForThumbnails ? bggItem.Image : bggItem.Thumbnail;
                    client.DownloadFile(imgUrl, tmpFileFullName);
                    if (!String.IsNullOrEmpty(client.ResponseHeaders["Content-Type"]))
                    {
                        var fileName = tmpFileName +
                                       Utils.GetFileExtensionByContentType(client.ResponseHeaders["Content-Type"]);
                        var newFileFullName = Path.Combine(pathToNewImage, fileName);
                        File.Move(tmpFileFullName, newFileFullName);
                        return newFileFullName;
                    }
                    return tmpFileFullName;
                }
            }

            return CoreConstants.ImgNotFound;
        }

        //public static void UpdateBGGLinksForClubGames(int sleepInterval)
        //{
        //    var users = DBUser.GetUsers();
        //    var games = DBGame.GetGamesCollection(users, true);
        //    foreach (var game in games)
        //    {
        //        var existingLinks = DBGGGLinks.GetLinksForBGGGame(game.BGGObjectId);
        //        if (!existingLinks.Any())
        //        {
        //            Thread.Sleep(sleepInterval);
        //            game.BGGExtendedInfo = BGGHelper.GetGame(game.BGGObjectId);
        //            if (game.BGGExtendedInfo != null)
        //            {
        //                DBGGGLinks.SaveLinksForBGGGame(game.BGGExtendedInfo);
        //            }
        //        }
        //    }
        //}

        public static void UpdateBGGLinksForGames(int sleepInterval, int portionSize)
        {
            var users = DBUser.GetUsers();
            var colgames = DBGame.GetGamesCollection(users, true);
            var cnt = 0;

            foreach (var game in colgames)
            {
                UpdateLinkForBGGGame(game.BGGObjectId, sleepInterval, ref cnt);

                //var existingLinks = DBGGGLinks.GetLinksForBGGGame(game.BGGObjectId);
                //if (!existingLinks.Any())
                //{
                //    Thread.Sleep(sleepInterval);
                //    game.BGGExtendedInfo = BGGHelper.GetGame(game.BGGObjectId);
                //    if (game.BGGExtendedInfo != null)
                //    {
                //        DBGGGLinks.SaveLinksForBGGGame(game.BGGExtendedInfo);
                //    }
                //}
            }


            var games = DBTeseraBGGRawGame.GetGamesShortInfo();
            foreach (var game in games.Where
                         (c=>c.BGGInfo!=null && c.BGGObjectId!=null && c.BGGObjectId>0 && 
                             !colgames.Any(c1=> c1.BGGObjectId!=null && c1.BGGObjectId==c.BGGObjectId)).
                         OrderByDescending(c=>c.BGGInfo.Usersrated))
            {
                UpdateLinkForBGGGame(game.BGGObjectId.Value, sleepInterval, ref cnt);
                if (cnt >= portionSize) break;

                /*var existingLinks = DBGGGLinks.GetLinksForBGGGame(game.BGGObjectId.Value);
                if (!existingLinks.Any())
                {
                    if (cnt>=2000) break;
                    Thread.Sleep(sleepInterval);
                    var info = BGGHelper.GetGame(game.BGGObjectId.Value);
                    if (info != null)
                    {
                        DBGGGLinks.SaveLinksForBGGGame(info);
                    }
                    cnt++;
                }*/
            }
        }

        static void UpdateLinkForBGGGame(int bggid, int sleepInterval, ref int cnt)
        {
            var existingLinks = DBGGGLinks.GetLinksForBGGGame(bggid);
            if (!existingLinks.Any())
            {
                
                Thread.Sleep(sleepInterval);
                var info = BGGHelper.GetGame(bggid);
                if (info != null)
                    DBGGGLinks.SaveLinksForBGGGame(info);
                cnt++;
            }
        }

        public static void AddGamesForUser(List<Game> games, User user)
        {
            foreach (var game in games)
            {
                game.Owners.Clear();
                game.Owners.Add(new GameOwner() {CreateTime = DateTime.Now, UserId = user.Id});
                DBGame.SaveGame(game, false);
            }
        }

        public static void RemoveGamesFromUser(List<Game> games, User user)
        {
            foreach (var game in games)
            {
                game.Owners.Clear();
                game.Owners.Add(new GameOwner() { CreateTime = DateTime.Now, DeleteTime = DateTime.Now, UserId = user.Id });
                DBGame.SaveGame(game, false);
            }
        }

        public static void TranslateBGGLinks(int itemsInPartToYandexCount, int sleepInterval)
        {
            TranslateBGGLinksForTable(BoardGameMechanic.TableName, itemsInPartToYandexCount, sleepInterval);
            TranslateBGGLinksForTable(BoardGameCategory.TableName, itemsInPartToYandexCount, sleepInterval);
            TranslateBGGLinksForTable(BoardGameDesigner.TableName, itemsInPartToYandexCount, sleepInterval);
        }

        static void TranslateBGGLinksForTable(string tableName, int itemsInPartToYandexCount, int sleepInterval)
        {
            var linksDict = DBGGGLinks.GetBggGameLinksDictionaryForTable(tableName);
            var linksToTranslate = linksDict.Where(c => string.IsNullOrWhiteSpace(c.TitleRus)).ToList();
            foreach (var bggGameLink in linksToTranslate)
            {
                var cacheTextRus = DBTranslate.GetTranslationCache(bggGameLink.TitleEng);
                if (!string.IsNullOrWhiteSpace(cacheTextRus))
                    bggGameLink.TitleRus = cacheTextRus;
            }
            var linksToSendToYandexTranslate = linksToTranslate.Where(c => string.IsNullOrWhiteSpace(c.TitleRus));
            var cnt = 0;
            var portion = new List<BGGGameLink>();
            foreach (var bggGameLink in linksToSendToYandexTranslate)
            {
                if (cnt >= itemsInPartToYandexCount || linksToSendToYandexTranslate.Count()< itemsInPartToYandexCount)
                {
                    if (linksToSendToYandexTranslate.Count() < itemsInPartToYandexCount)
                        portion.AddRange(linksToSendToYandexTranslate);
                    var textsEng = portion.Select(c => c.TitleEng).ToArray();
                    System.Threading.Thread.Sleep(sleepInterval);
                    var textsRus = YandexTranslateHelper.GetTranslationFromEng(textsEng).Result;
                    var dic = new Dictionary<string, string>();
                    for (int i = 0; i <= textsEng.Length; i++)
                    {
                        if (textsRus.Count > i)
                        {
                            dic[textsEng[i]] = textsRus[i];
                            var bgglinkToUpdate = linksToTranslate.FirstOrDefault(c => c.TitleEng == textsEng[i] && string.IsNullOrWhiteSpace(c.TitleRus));
                            if (bgglinkToUpdate!=null)
                                bgglinkToUpdate.TitleRus = textsRus[i];
                        }
                    }
                    DBTranslate.SaveCache(dic);
                    if (linksToSendToYandexTranslate.Count() < itemsInPartToYandexCount) 
                        break;
                    portion.Clear();
                    cnt = 0;
                }
                else
                {
                    portion.Add(bggGameLink);
                    cnt++;
                }
            }
            DBGGGLinks.UpdateBggGameLinksDictionaryForTable(tableName, linksToTranslate);
        }

        internal static void UpdateTranslateFile()
        {
            var bggColl = BGGCollection.LoadFromFile();
            var rawGames = DBTeseraBGGRawGame.GetGamesShortInfo();
            List<Game> games = new List<Game>();
            foreach (var item in bggColl.Items)
            {
                if (!games.Any(c => c.BGGObjectId == item.ObjectId))
                {
                    var game = Game.CreateGame(rawGames, new List<User>(), false, item.ObjectId, null, item.TeseraKey,
                        item.ParentName);
                    games.Add(game);
                }
            }
            GamesNamesTranslateFile.UpdateTranslateFile(games);
        }
    }
}
