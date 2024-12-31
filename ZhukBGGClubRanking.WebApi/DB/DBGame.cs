using System.Data;
using ZhukBGGClubRanking.Core.Model;
using ZhukBGGClubRanking.WebApi;
using ZhukBGGClubRanking.WebApi.Core;

namespace ZhukBGGClubRanking.Core
{
    public static class DBGame
    {

        public static List<Game> GetGamesCollection(List<User> users)
        {
            var result = new List<Game>();
            try
            {
                using (var con = DBHelper.CreateConnection())
                {
                    con.Open();
                    using (var cmd = con.CreateCommand())
                    {
                        cmd.CommandText = "select id, name_eng, name_bgg, name_rus, parent_id, bgg_object_id, yearpublished, " +
                            "image_bgg, thumbnail_bgg, tesera_key, is_actual, create_time, create_user_id from games";
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
                                item.IsActual = reader.GetFieldValue<bool>("is_actual");
                                if (!reader.IsDBNull("create_time"))
                                    item.CreateTime = reader.GetFieldValue<DateTime>("create_time");
                                item.CreateUserId = reader.IsDBNull("create_user_id") ? 0 : reader.GetFieldValue<int>("create_user_id");
                                result.Add(item);
                            }
                        }

                        foreach (var game in result)
                        {
                            cmd.Parameters.Clear();
                            cmd.CommandText = "select id,user_id,create_time,delete_time from games_owners where game_id=@game_id";
                            cmd.CreateParameter(DbType.Int32, "game_id", game.Id);
                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    var owner = new GameOwner();
                                    owner.Id = reader.GetFieldValue<Int32>("id");
                                    owner.UserId = reader.GetFieldValue<Int32>("user_id");
                                    owner.UserName = users.FirstOrDefault(c => c.Id == owner.UserId).Name;
                                    owner.CreateTime = reader.GetFieldValue<DateTime>("create_time");
                                    if (!reader.IsDBNull("delete_time"))
                                        owner.DeleteTime = reader.GetFieldValue<DateTime>("delete_time");
                                    game.Owners.Add(owner);
                                }
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

        public static int SaveGame(Game game, bool clearOwnersIfExists)
        {
            try
            {
                
                using (var con = DBHelper.CreateConnection())
                {
                    con.Open();
                    using (var cmd = con.CreateCommand())
                    {
                        cmd.CommandText = "select id from games where upper(name_bgg)=@name_bgg";
                        cmd.CreateParameter(DbType.String, "name_bgg", game.NameBGG);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                                game.Id = reader.GetFieldValue<Int32>("id");
                        }
                        cmd.Parameters.Clear();
                        if (game.Id == 0)
                        {
                            cmd.CommandText =
                                "insert into games (name_bgg, name_eng, name_rus, parent_id, bgg_object_id, yearpublished, " +
                                "image_bgg, thumbnail_bgg, tesera_key, is_actual, create_time, create_user_id) " +
                                "values (@name_bgg, @name_eng, @name_rus, @parent_id, @bgg_object_id, @yearpublished, " +
                                "@image_bgg, @thumbnail_bgg, @tesera_key, @is_actual, @create_time, @create_user_id)";
                            cmd.CreateParameter(DbType.String, "name_bgg", game.NameBGG);
                            cmd.CreateParameter(DbType.Int32, "bgg_object_id", game.BGGObjectId);
                            cmd.CreateParameter(DbType.DateTime, "create_time", game.CreateTime);
                            cmd.CreateParameter(DbType.Int32, "create_user_id", game.CreateUserId);
                        }
                        else
                        {
                            cmd.CommandText =
                                "update games set name_eng=@name_eng, name_rus=@name_rus, parent_id=@parent_id, " +
                                "yearpublished=@yearpublished, image_bgg=@image_bgg, thumbnail_bgg=@thumbnail_bgg, " +
                                "tesera_key=@tesera_key, is_actual=@is_actual where id=@id";
                            cmd.CreateParameter(DbType.Int32, "id", game.Id);
                        }

                        cmd.CreateParameter(DbType.String, "name_eng", game.NameEng);
                        cmd.CreateParameter(DbType.String, "name_rus", game.NameRus);
                        cmd.CreateParameter(DbType.Int32, "parent_id", game.ParentId);
                        cmd.CreateParameter(DbType.Int32, "yearpublished", game.YearPublished);
                        cmd.CreateParameter(DbType.String, "image_bgg", game.ImageBGG);
                        cmd.CreateParameter(DbType.String, "thumbnail_bgg", game.ThumbnailBGG);
                        cmd.CreateParameter(DbType.String, "tesera_key", game.TeseraKey);
                        cmd.CreateParameter(DbType.String, "is_actual", game.IsActual);
                        cmd.ExecuteNonQuery();
                        List<GameOwner> currentGameOwners = new List<GameOwner>();
                        if (game.Id==0)
                        {
                            game.Id = DBHelper.GetLastInsertedRowId(cmd);
                        }
                        // удаляем владельцев, если указано
                        if (clearOwnersIfExists)
                        {
                            cmd.Parameters.Clear();
                            cmd.CommandText = "delete from games_owners where game_id=@game_id";
                            cmd.CreateParameter(DbType.Int32, "game_id", game.Id);
                            cmd.ExecuteNonQuery();
                        }
                        foreach (var owner in game.Owners)
                        {
                            cmd.CommandText = string.Format(
                                "insert into games_owners (game_id, user_id, create_time, delete_time) " +
                                "values (@game_id, @user_id, @create_time, {0})",owner.DeleteTime==null?"null": "@delete_time");
                            cmd.Parameters.Clear();
                            cmd.CreateParameter(DbType.Int32, "game_id", game.Id);
                            cmd.CreateParameter(DbType.Int32, "user_id", owner.UserId);
                            cmd.CreateParameter(DbType.DateTime, "create_time", owner.CreateTime);
                            if (owner.DeleteTime!=null)
                                cmd.CreateParameter(DbType.DateTime, "delete_time", owner.DeleteTime);
                            cmd.ExecuteNonQuery();
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

        public static void UpdateParent(Game game)
        {
            try
            {

                using (var con = DBHelper.CreateConnection())
                {
                    con.Open();
                    using (var cmd = con.CreateCommand())
                    {

                        cmd.CommandText =
                            "update games set parent_id=@parent_id where id=@id";
                        cmd.CreateParameter(DbType.Int32, "id", game.Id);
                        cmd.CreateParameter(DbType.Int32, "parent_id", game.ParentId);

                        cmd.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteError(ex);
                throw;
            }

        }
    }
}