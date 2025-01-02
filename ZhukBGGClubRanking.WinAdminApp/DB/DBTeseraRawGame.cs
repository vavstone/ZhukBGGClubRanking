using System;
using System.Collections.Generic;
using System.Data;
using ZhukBGGClubRanking.Core.Model;

namespace ZhukBGGClubRanking.WinAdminApp
{
    public static class DBTeseraRawGame
    {

        //public static List<TeseraRawGame> GetGamesCollection(List<User> users)
        //{
        //    var result = new List<TeseraRawGame>();
        //    try
        //    {
        //        using (var con = DBHelper.CreateConnection())
        //        {
        //            con.Open();
        //            using (var cmd = con.CreateCommand())
        //            {
        //                cmd.CommandText = "select id, name_eng, name_bgg, name_rus, parent_id, bgg_object_id, yearpublished, " +
        //                    "image_bgg, thumbnail_bgg, tesera_key, is_actual, create_time, create_user_id from games";
        //                using (var reader = cmd.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        var item = new TeseraRawGame();
        //                        item.Id = reader.GetFieldValue<Int32>("id");
        //                        item.NameEng = reader.IsDBNull("name_eng") ? string.Empty : reader.GetFieldValue<string>("name_eng");
        //                        item.NameEng = reader.IsDBNull("name_eng") ? string.Empty : reader.GetFieldValue<string>("name_eng");
        //                        item.NameBGG = reader.IsDBNull("name_bgg") ? string.Empty : reader.GetFieldValue<string>("name_bgg");
        //                        item.NameRus = reader.IsDBNull("name_rus") ? string.Empty : reader.GetFieldValue<string>("name_rus");
        //                        item.ParentId = reader.IsDBNull("parent_id") ? 0 : reader.GetFieldValue<int>("parent_id");
        //                        item.BGGObjectId = reader.IsDBNull("bgg_object_id") ? 0 : reader.GetFieldValue<int>("bgg_object_id");
        //                        item.YearPublished = reader.IsDBNull("yearpublished") ? 0 : reader.GetFieldValue<int>("yearpublished");
        //                        item.ImageBGG = reader.IsDBNull("image_bgg") ? string.Empty : reader.GetFieldValue<string>("image_bgg");
        //                        item.ThumbnailBGG = reader.IsDBNull("thumbnail_bgg") ? string.Empty : reader.GetFieldValue<string>("thumbnail_bgg");
        //                        item.TeseraKey = reader.IsDBNull("tesera_key") ? string.Empty : reader.GetFieldValue<string>("tesera_key");
        //                        item.IsActual = reader.GetFieldValue<bool>("is_actual");
        //                        if (!reader.IsDBNull("create_time"))
        //                            item.CreateTime = reader.GetFieldValue<DateTime>("create_time");
        //                        item.CreateUserId = reader.IsDBNull("create_user_id") ? 0 : reader.GetFieldValue<int>("create_user_id");
        //                        result.Add(item);
        //                    }
        //                }

        //                foreach (var game in result)
        //                {
        //                    cmd.Parameters.Clear();
        //                    cmd.CommandText = "select id,user_id,create_time,delete_time from games_owners where game_id=@game_id";
        //                    cmd.CreateParameter(DbType.Int32, "game_id", game.Id);
        //                    using (var reader = cmd.ExecuteReader())
        //                    {
        //                        while (reader.Read())
        //                        {
        //                            var owner = new GameOwner();
        //                            owner.Id = reader.GetFieldValue<Int32>("id");
        //                            owner.UserId = reader.GetFieldValue<Int32>("user_id");
        //                            owner.UserName = users.FirstOrDefault(c => c.Id == owner.UserId).Name;
        //                            owner.CreateTime = reader.GetFieldValue<DateTime>("create_time");
        //                            if (!reader.IsDBNull("delete_time"))
        //                                owner.DeleteTime = reader.GetFieldValue<DateTime>("delete_time");
        //                            game.Owners.Add(owner);
        //                        }
        //                    }
        //                }
                        
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.WriteError(ex);
        //        throw;
        //    }

        //    return result;
        //}

        public static int SaveTeseraRawGame(TeseraRawGame game)
        {
            try
            {
                
                using (var con = DBHelper.CreateConnection())
                {
                    con.Open();
                    using (var cmd = con.CreateCommand())
                    {
                        cmd.Parameters.Clear();

                            cmd.CommandText =
                                @"insert into games 
                                (id,
                                title,
                                tesera_id,
                                bgg_id,
                                alias,
                                description_short,
                                description,
                                modification_date_utc, 
                                creation_date_utc,
                                photo_url,
                                year,
                                rating_user,
                                n10rating,
                                n20rating,
                                bgg_rating, 
                                bgg_geek_rating,
                                bgg_num_votes,
                                num_votes,
                                players_min, 
                                players_max, 
                                players_min_recommend,
                                players_max_recommend,
                                players_age_min,
                                time_to_learn,
                                playtime_min,
                                playtime_max,
                                comments_total, 
                                comments_total_new,
                                is_addition,
                                dload) 
                                values
                                (@id,
                                @title,
                                @tesera_id,
                                @bgg_id,
                                @alias,
                                @description_short,
                                @description,
                                @modification_date_utc, 
                                @creation_date_utc,
                                @photo_url,
                                @year,
                                @rating_user,
                                @n10rating,
                                @n20rating,
                                @bgg_rating, 
                                @bgg_geek_rating,
                                @bgg_num_votes,
                                @num_votes,
                                @players_min, 
                                @players_max, 
                                @players_min_recommend,
                                @players_max_recommend,
                                @players_age_min,
                                @time_to_learn,
                                @playtime_min,
                                @playtime_max,
                                @comments_total, 
                                @comments_total_new,
                                @is_addition,
                                @dload)";
                        cmd.CreateParameter(DbType.Int32, "id", game.Id);
                        cmd.CreateParameter(DbType.String, "title", game.Title);
                        cmd.CreateParameter(DbType.Int32, "tesera_id", game.TeseraId);
                        cmd.CreateParameter(DbType.Int32, "bgg_id", game.BGGId);
                        cmd.CreateParameter(DbType.String, "description_short", game.DescriptionShort);
                        cmd.CreateParameter(DbType.String, "description", game.Description);
                        cmd.CreateParameter(DbType.DateTime, "modification_date_utc", game.ModificationDateUtc);
                        cmd.CreateParameter(DbType.DateTime, "creation_date_utc", game.CreationDateUtc);
                        cmd.CreateParameter(DbType.String, "photo_url", game.PhotoUrl);
                        cmd.CreateParameter(DbType.Int32, "year", game.Year);
                        cmd.CreateParameter(DbType.Double, "rating_user", game.RatingUser);
                        cmd.CreateParameter(DbType.Double, "n10rating", game.N10Rating);
                        cmd.CreateParameter(DbType.Double, "n20rating", game.N20Rating);
                        cmd.CreateParameter(DbType.Double, "bgg_rating", game.BGGRating);
                        cmd.CreateParameter(DbType.Double, "bgg_geek_rating", game.BGGGeekRating);
                        cmd.CreateParameter(DbType.Int32, "bgg_num_votes", game.BGGNumVotes);
                        cmd.CreateParameter(DbType.Int32, "num_votes", game.NumVotes);
                        cmd.CreateParameter(DbType.Int32, "players_min", game.PlayersMin);
                        cmd.CreateParameter(DbType.Int32, "players_max", game.PlayersMax);
                        cmd.CreateParameter(DbType.Int32, "players_min_recommend", game.PlayersMinRecommend);
                        cmd.CreateParameter(DbType.Int32, "players_max_recommend", game.PlayersMaxRecommend);
                        cmd.CreateParameter(DbType.Int32, "players_age_min", game.PlayersAgeMin);
                        cmd.CreateParameter(DbType.Int32, "time_to_learn", game.TimeToLearn);
                        cmd.CreateParameter(DbType.Int32, "playtime_min", game.PlaytimeMin);
                        cmd.CreateParameter(DbType.Int32, "playtime_max", game.PlaytimeMax);
                        cmd.CreateParameter(DbType.Int32, "comments_total", game.CommentsTotal);
                        cmd.CreateParameter(DbType.Int32, "comments_total_new", game.CommentsTotalNew);
                        cmd.CreateParameter(DbType.Boolean, "is_addition", game.IsAddition);
                        cmd.CreateParameter(DbType.DateTime, "dload", DateTime.Now);
                        cmd.ExecuteNonQuery();
                        game.Id = DBHelper.GetLastInsertedRowId(cmd);
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

        //public static void UpdateParent(TeseraRawGame game)
        //{
        //    try
        //    {

        //        using (var con = DBHelper.CreateConnection())
        //        {
        //            con.Open();
        //            using (var cmd = con.CreateCommand())
        //            {

        //                cmd.CommandText =
        //                    "update games set parent_id=@parent_id where id=@id";
        //                cmd.CreateParameter(DbType.Int32, "id", game.Id);
        //                cmd.CreateParameter(DbType.Int32, "parent_id", game.ParentId);

        //                cmd.ExecuteNonQuery();

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.WriteError(ex);
        //        throw;
        //    }

        //}
    }
}