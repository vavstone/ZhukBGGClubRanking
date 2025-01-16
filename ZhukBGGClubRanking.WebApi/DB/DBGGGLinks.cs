using BoardGamer.BoardGameGeek.BoardGameGeekXmlApi2;
using System;
using System.Data;
using System.Data.Common;
using ZhukBGGClubRanking.Core.Model;
using ZhukBGGClubRanking.WebApi;
using ZhukBGGClubRanking.WebApi.Core;

namespace ZhukBGGClubRanking.Core
{
    public static class DBGGGLinks
    {
        public static void SaveLinksForBGGGame(ThingResponse.Item bggGame)
        {
            if (bggGame == null || bggGame.Links==null) return;
            try
            {
                using (var con = DBHelper.CreateConnection())
                {
                    con.Open();


                    using (var cmd = con.CreateCommand())
                    {
                        foreach (var linkGroup in bggGame.Links.GroupBy(c=>c.Type))
                        {
                            SaveLinkGroup(cmd,linkGroup, bggGame.Id);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteError(ex);
                throw;
            }
        }

        public static BGGGameLinkCollection GetLinksForBGGGame(int bggId)
        {
            var result = new BGGGameLinkCollection();
            try
            {
                using (var con = DBHelper.CreateConnection())
                {
                    con.Open();
                    using (var cmd = con.CreateCommand())
                    {
                        cmd.CreateParameter(DbType.Int32, "bgg_id", bggId);
                        result.BGDesigners = GetLinksForType(cmd, BoardGameDesigner.TableName).Cast<BoardGameDesigner>().ToList();
                        result.BGArtists = GetLinksForType(cmd, BoardGameArtist.TableName).Cast<BoardGameArtist>().ToList();
                        result.BGAccessories = GetLinksForType(cmd, BoardGameAccessory.TableName).Cast<BoardGameAccessory>().ToList();
                        result.BGCategories = GetLinksForType(cmd, BoardGameCategory.TableName).Cast<BoardGameCategory>().ToList();
                        result.BGGExpansions = GetLinksForType(cmd, BoardGameExpansion.TableName).Cast<BoardGameExpansion>().ToList();
                        result.BGFamilies = GetLinksForType(cmd, BoardGameFamily.TableName).Cast<BoardGameFamily>().ToList();
                        result.BGImplementations = GetLinksForType(cmd, BoardGameImplementation.TableName).Cast<BoardGameImplementation>().ToList();
                        result.BGMechanics = GetLinksForType(cmd, BoardGameMechanic.TableName).Cast<BoardGameMechanic>().ToList();
                        result.BGPublishers = GetLinksForType(cmd, BoardGamePublisher.TableName).Cast<BoardGamePublisher>().ToList();
                        result.BGIntegrations = GetLinksForType(cmd, BoardGameIntegration.TableName).Cast<BoardGameIntegration>().ToList();
                        result.BGCompilations = GetLinksForType(cmd, BoardGameCompilation.TableName).Cast<BoardGameCompilation>().ToList();
                        result.BGUnknownLinks = GetLinksForType(cmd, BoardUnknownLinkType.TableName).Cast<BoardUnknownLinkType>().ToList();
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(ex);
                throw;
            }
        }

        static List<BGGGameLink> GetLinksForType(DbCommand cmd, string linkTable)
        {
            var result = new List<BGGGameLink>();
            if (linkTable == BoardUnknownLinkType.TableName)
                cmd.CommandText = "select t1.id, t1.title_type, t1.title_eng, t1.title_rus from bgunknownlinktype t1 join game_bgunknownlinktype t2 on t2.bgunknownlinktype_id=t1.id where t2.bgg_game_id=@bgg_id";
            else
                cmd.CommandText = string.Format("select t1.id, t1.title_eng, t1.title_rus from {0} t1 join game_{0} t2 on t2.{0}_id=t1.id where t2.bgg_game_id=@bgg_id", linkTable);
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    BGGGameLink item;
                    switch (linkTable)
                    {
                        case BoardGameDesigner.TableName: item = new BoardGameDesigner(); break;
                        case BoardGameAccessory.TableName: item = new BoardGameAccessory(); break;
                        case BoardGameArtist.TableName: item = new BoardGameArtist(); break;
                        case BoardGameCategory.TableName: item = new BoardGameCategory(); break;
                        case BoardGameExpansion.TableName: item = new BoardGameExpansion(); break;
                        case BoardGameFamily.TableName: item = new BoardGameFamily(); break;
                        case BoardGameImplementation.TableName: item = new BoardGameImplementation(); break;
                        case BoardGameMechanic.TableName: item = new BoardGameMechanic(); break;
                        case BoardGamePublisher.TableName: item = new BoardGamePublisher(); break;
                        case BoardGameIntegration.TableName: item = new BoardGameIntegration(); break;
                        case BoardGameCompilation.TableName: item = new BoardGameCompilation(); break;
                        case BoardUnknownLinkType.TableName: item = new BoardUnknownLinkType(); break;
                        default: throw new Exception("Неизвестный тип bbg link");
                    }
                    item.Id = reader.GetFieldValue<Int32>("id");
                    item.TitleEng = reader.GetFieldValue<string>("title_eng");
                    item.TitleRus = reader.IsDBNull("title_rus") ? string.Empty : reader.GetFieldValue<string>("title_rus");
                    if (linkTable == BoardUnknownLinkType.TableName)
                        (item as BoardUnknownLinkType).TitleType = reader.GetFieldValue<string>("title_type");
                    result.Add(item);
                }
            }
            return result;
        }

        static string GetTableNameForLinkType(string linkType)
        {
            switch (linkType)
            {
                case BoardGameDesigner.LinkType: return BoardGameDesigner.TableName;
                case BoardGameAccessory.LinkType: return BoardGameAccessory.TableName;
                case BoardGameArtist.LinkType: return BoardGameArtist.TableName;
                case BoardGameCategory.LinkType: return BoardGameCategory.TableName;
                case BoardGameExpansion.LinkType: return BoardGameExpansion.TableName;
                case BoardGameFamily.LinkType: return BoardGameFamily.TableName;
                case BoardGameImplementation.LinkType: return BoardGameImplementation.TableName;
                case BoardGameMechanic.LinkType: return BoardGameMechanic.TableName;
                case BoardGamePublisher.LinkType: return BoardGamePublisher.TableName;
                case BoardGameIntegration.LinkType: return BoardGameIntegration.TableName;
                case BoardGameCompilation.LinkType: return BoardGameCompilation.TableName;
                default: return BoardUnknownLinkType.TableName;
            }
        }

        static void SaveLinkGroup(DbCommand cmd, IGrouping<string, ThingResponse.Link> linkGroup, int bggId)
        {
            
            var unknownType = true;
            if (new[]
                {
                    BoardGameAccessory.LinkType,
                    BoardGameArtist.LinkType,
                    BoardGameCategory.LinkType,
                    BoardGameExpansion.LinkType,
                    BoardGameFamily.LinkType,
                    BoardGameImplementation.LinkType,
                    BoardGameMechanic.LinkType,
                    BoardGamePublisher.LinkType,
                    BoardGameIntegration.LinkType,
                    BoardGameCompilation.LinkType,
                    BoardGameDesigner.LinkType}.Contains(linkGroup.Key))
            {
                unknownType = false;
            }

            if (!unknownType)
            {
                var linkTableName = GetTableNameForLinkType(linkGroup.Key);
                var gameToLinkTableName =  string.Format("game_{0}", linkTableName);
                var foreignKeyToLinkFieldName = string.Format("{0}_id", linkTableName);

                var sqlInsertLink = string.Format("INSERT OR IGNORE into {0} (title_eng) values (@title_eng)", linkTableName);
                var sqlGetLinkId = string.Format("select id from {0} where upper(title_eng)=upper(@title_eng)", linkTableName);
                var sqlDeleteGameToLink = string.Format("delete from {0} where bgg_game_id=@bgg_game_id", gameToLinkTableName);
                var sqlInsertGameToLink = string.Format("insert into {0} (bgg_game_id,{1}) values (@bgg_game_id,@{1})", gameToLinkTableName, foreignKeyToLinkFieldName);

                //удаляем связи линков c текущей игрой, например, из таблицы game_bgartist
                cmd.Parameters.Clear();
                cmd.CommandText = sqlDeleteGameToLink;
                cmd.CreateParameter(DbType.Int32, "bgg_game_id", bggId);
                cmd.ExecuteNonQuery();

                foreach (var item in linkGroup.ToList().Select(c=>c.Value).Distinct(StringComparer.CurrentCultureIgnoreCase))
                {

                    try
                    {//вставляем линк (например, в таблицу bgartist), если такого еще нет
                        cmd.CommandText = sqlInsertLink;
                        cmd.Parameters.Clear();
                        cmd.CreateParameter(DbType.String, "title_eng", item);
                        cmd.ExecuteNonQuery();

                        //получаем айди линка (вставленного или ранее существовавшего)
                        cmd.CommandText = sqlGetLinkId;
                        var linkId = (Int32)(Int64)cmd.ExecuteScalar();

                        //добавление связи линков с текущей игрой
                        cmd.CommandText = sqlInsertGameToLink;
                        cmd.Parameters.Clear();
                        cmd.CreateParameter(DbType.Int32, "bgg_game_id", bggId);
                        cmd.CreateParameter(DbType.Int32, foreignKeyToLinkFieldName, linkId);
                        cmd.ExecuteNonQuery();

                    }
                    catch (Exception e)
                    {
                        var a = e;
                    }
                        


                }
            }
            else
            {

                var sqlInsertLink =
                    "INSERT OR IGNORE into bgunknownlinktype (title_type,title_eng) values (@title_type,@title_eng)";
                var sqlGetLinkId =
                    "select id from bgunknownlinktype where upper(title_eng)=upper(@title_eng) and upper(title_type)=upper(@title_type)";
                var sqlDeleteGameToLink =
                    "delete from game_bgunknownlinktype where bgg_game_id=@bgg_game_id and bgunknownlinktype_id in (select id from bgunknownlinktype where upper(title_type)=upper(@title_type))";
                var sqlInsertGameToLink =
                    "insert into game_bgunknownlinktype (bgg_game_id,bgunknownlinktype_id) values (@bgg_game_id,@bgunknownlinktype_id)";

                //удаляем связи линков этого типа c текущей игрой из game_bgunknownlinktype
                cmd.Parameters.Clear();
                cmd.CommandText = sqlDeleteGameToLink;
                cmd.CreateParameter(DbType.Int32, "bgg_game_id", bggId);
                cmd.CreateParameter(DbType.String, "title_type", linkGroup.Key);
                cmd.ExecuteNonQuery();

                foreach (var item in linkGroup.ToList().Select(c => c.Value).Distinct())
                {

                    //вставляем линк в bgunknownlinktype, если такого еще нет
                    cmd.CommandText = sqlInsertLink;
                    cmd.Parameters.Clear();
                    cmd.CreateParameter(DbType.String, "title_eng", item);
                    cmd.CreateParameter(DbType.String, "title_type", linkGroup.Key);
                    cmd.ExecuteNonQuery();

                    //получаем айди линка (вставленного или ранее существовавшего)
                    cmd.CommandText = sqlGetLinkId;
                    var linkId = (Int32) (Int64) cmd.ExecuteScalar();

                    //добавление связи линков с текущей игрой
                    cmd.CommandText = sqlInsertGameToLink;
                    cmd.Parameters.Clear();
                    cmd.CreateParameter(DbType.Int32, "bgg_game_id", bggId);
                    cmd.CreateParameter(DbType.Int32, "bgunknownlinktype_id", linkId);
                    cmd.ExecuteNonQuery();


                }
            }




        }
    }
}
