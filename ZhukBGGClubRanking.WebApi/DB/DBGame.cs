using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using ZhukBGGClubRanking.WebApi;
using ZhukBGGClubRanking.WebApi.Core;

namespace ZhukBGGClubRanking.Core
{
    public static class DBGame
    {

        public static List<Game> GetGamesCollection()
        {
            var result = new List<Game>();
            try
            {
                using (var con = DBHelper.CreateConnection())
                {
                    con.Open();
                    using (var cmd = con.CreateCommand())
                    {
                        cmd.CommandText = "select id, name_eng, name_bgg, name_rus, parent_id, bgg_object_id, yearpublished, image_bgg, thumbnail_bgg, tesera_key from games";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var item = new Game();
                                item.Id = reader.GetFieldValue<Int32>("id");
                                item.NameEng = reader.IsDBNull("name_eng") ? string.Empty : reader.GetFieldValue<string>("name_eng");
                                item.NameEng = reader.IsDBNull("name_eng") ? string.Empty : reader.GetFieldValue<string>("name_eng");
                                item.NameBGG = reader.IsDBNull("name_bgg") ? string.Empty : reader.GetFieldValue<string>("name_bgg");
                                item.NameRus = reader.IsDBNull("name_rus") ? string.Empty : reader.GetFieldValue<string>("name_rus");
                                item.ParentId = reader.IsDBNull("parent_id") ? 0 : reader.GetFieldValue<int>("parent_id");
                                item.BGGObjectId = reader.IsDBNull("bgg_object_id") ? 0 : reader.GetFieldValue<int>("bgg_object_id");
                                item.YearPublished = reader.IsDBNull("yearpublished") ? 0 : reader.GetFieldValue<int>("yearpublished");
                                item.ImageBGG = reader.IsDBNull("image_bgg") ? string.Empty : reader.GetFieldValue<string>("image_bgg");
                                item.ThumbnailBGG = reader.IsDBNull("thumbnail_bgg") ? string.Empty : reader.GetFieldValue<string>("thumbnail_bgg");
                                item.TeseraKey = reader.IsDBNull("tesera_key") ? string.Empty : reader.GetFieldValue<string>("tesera_key");
                                result.Add(item);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteError(ex);
                throw;
            }

            return result;
        }

        public static int SaveGame(Game game)
        {
            try
            {
                bool isExist = game.Id > 0;
                using (var con = DBHelper.CreateConnection())
                {
                    con.Open();
                    using (var cmd = con.CreateCommand())
                    {
                        if (!isExist)
                            cmd.CommandText =
                                "insert into games (name_eng, name_bgg, name_rus, parent_id, bgg_object_id, yearpublished, image_bgg, thumbnail_bgg, tesera_key) " +
                                "values (@name_eng, @name_bgg, @name_rus, @parent_id, @bgg_object_id, @yearpublished, @image_bgg, @thumbnail_bgg, @tesera_key)";
                        //else
                        //    cmd.CommandText =
                        //        "update facultets set name=@name, is_active=@is_active where id=" + Id;

                        cmd.CreateParameter(DbType.String, "name_eng", game.NameEng);
                        cmd.CreateParameter(DbType.String, "name_bgg", game.NameBGG);
                        cmd.CreateParameter(DbType.String, "name_rus", game.NameRus);
                        cmd.CreateParameter(DbType.Int32, "parent_id", game.ParentId);
                        cmd.CreateParameter(DbType.Int32, "bgg_object_id", game.BGGObjectId);
                        cmd.CreateParameter(DbType.Int32, "yearpublished", game.YearPublished);
                        cmd.CreateParameter(DbType.String, "image_bgg", game.ImageBGG);
                        cmd.CreateParameter(DbType.String, "thumbnail_bgg", game.ThumbnailBGG);
                        cmd.CreateParameter(DbType.String, "tesera_key", game.TeseraKey);
                        cmd.ExecuteNonQuery();
                        if (!isExist)
                        {
                            game.Id = DBHelper.GetLastInsertedRowId(cmd);
                            return game.Id;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteError(ex);
                throw;
            }

            return game.Id;
        }
    }
}