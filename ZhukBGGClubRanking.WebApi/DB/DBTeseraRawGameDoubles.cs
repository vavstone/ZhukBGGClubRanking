using System;
using System.Collections.Generic;
using System.Data;
using ZhukBGGClubRanking.Core.Model;

namespace ZhukBGGClubRanking.WebApi.Core
{
    public static class DBTeseraRawGameDoubles
    {

        public static int SaveTeseraRawGameDoubles(TeseraRawGame game)
        {
            try
            {   
                using (var con = DBHelper.CreateConnection())
                {
                    con.Open();
                    using (var cmd = con.CreateCommand())
                    {
                        cmd.CommandText =
                                "insert into tesera_raw_info_doubles " +
                                "(id, title, tesera_id, bgg_id, alias, description_short, description, modification_date_utc, creation_date_utc, " +
                                "photo_url, year, rating_user, n10rating, n20rating, bgg_rating, bgg_geek_rating, bgg_num_votes, num_votes, players_min, players_max, " +
                                "players_min_recommend, players_max_recommend, players_age_min, time_to_learn, playtime_min, playtime_max, comments_total, comments_total_new, " +
                                "is_addition, dload) values " +
                                "(@id, @title, @tesera_id, @bgg_id, @alias, @description_short, @description, @modification_date_utc, @creation_date_utc, " +
                                "@photo_url, @year, @rating_user, @n10rating, @n20rating, @bgg_rating, @bgg_geek_rating, @bgg_num_votes, @num_votes, @players_min, @players_max, " +
                                "@players_min_recommend, @players_max_recommend, @players_age_min, @time_to_learn, @playtime_min, @playtime_max, @comments_total, @comments_total_new, " +
                                "@is_addition, @dload)";
                        cmd.CreateParameter(DbType.Int32, "id", game.Id);
                        cmd.CreateParameter(DbType.String, "title", game.Title);
                        cmd.CreateParameter(DbType.Int32, "tesera_id", game.TeseraId);
                        cmd.CreateParameter(DbType.Int32, "bgg_id", game.BGGId);
                        cmd.CreateParameter(DbType.String, "alias", game.Alias);
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