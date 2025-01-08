using System.Data;
using System.Data.Common;
using System.Transactions;
using ZhukBGGClubRanking.Core.Model;
using ZhukBGGClubRanking.WebApi.Core;

namespace ZhukBGGClubRanking.WebApi.DB
{
    public class DBTeseraBGGRawGame
    {
        public static int SaveBGGRawGame(TeseraBGGRawGame game, DbCommand cmd)
        {

            var bggPart1 = "";
            var bggPart2 = "";
            var teseraPart1 = "";
            var teseraPart2 = "";
            if (game.BGGInfo != null)
            {
                bggPart1 = "id_bgg, name_bgg, yearpublished_bgg, rank_bgg, bayesaverage_bgg, average_bgg, usersrated_bgg, is_expansion_bgg, " +
                    "abstracts_rank_bgg, cgs_rank_bgg, childrensgames_rank_bgg, familygames_rank_bgg, partygames_rank_bgg, strategygames_rank_bgg, " +
                    "thematic_rank_bgg, wargames_rank_bgg,";
                bggPart2 = "@id_bgg, @name_bgg, @yearpublished_bgg, @rank_bgg, @bayesaverage_bgg, @average_bgg, @usersrated_bgg, @is_expansion_bgg, @abstracts_rank_bgg, " +
                    "@cgs_rank_bgg, @childrensgames_rank_bgg, @familygames_rank_bgg, @partygames_rank_bgg, @strategygames_rank_bgg, @thematic_rank_bgg, @wargames_rank_bgg,";
            }
            if (game.TeseraInfo != null)
            {
                teseraPart1 = "id_tesera, " +
                    "title_tesera, tesera_id_tesera, alias_tesera, description_short_tesera, description_tesera, modification_date_utc_tesera, creation_date_utc_tesera, " +
                    "photo_url_tesera, year_tesera, rating_user_tesera, n10rating_tesera, n20rating_tesera, bgg_rating_tesera, bgg_geek_rating_tesera, " +
                    "bgg_num_votes_tesera, num_votes_tesera, players_min_tesera, players_max_tesera, players_min_recommend_tesera, players_max_recommend_tesera, " +
                    "players_age_min_tesera, time_to_learn_tesera, playtime_min_tesera, playtime_max_tesera, comments_total_tesera, comments_total_new_tesera, " +
                    "is_addition_tesera,";
                teseraPart2 = "@id_tesera, " +
                    "@title_tesera, @tesera_id_tesera, @alias_tesera, @description_short_tesera, @description_tesera, @modification_date_utc_tesera, @creation_date_utc_tesera, " +
                    "@photo_url_tesera, @year_tesera, @rating_user_tesera, @n10rating_tesera, @n20rating_tesera, @bgg_rating_tesera, @bgg_geek_rating_tesera, " +
                    "@bgg_num_votes_tesera, @num_votes_tesera, @players_min_tesera, @players_max_tesera, @players_min_recommend_tesera, @players_max_recommend_tesera, " +
                    "@players_age_min_tesera, @time_to_learn_tesera, @playtime_min_tesera, @playtime_max_tesera, @comments_total_tesera, @comments_total_new_tesera, " +
                    "@is_addition_tesera,";
            }
            cmd.CommandText =
                    string.Format("insert into bgg_tesera_raw_info ({0} {1} is_addition, parent_id, dload) values " +
                    "({2} {3} @is_addition, @parent_id, @dload)",
                    bggPart1, teseraPart1, bggPart2, teseraPart2);
            cmd.Parameters.Clear();
            if (game.BGGInfo != null)
            {
                cmd.CreateParameter(DbType.Int32, "id_bgg", game.BGGInfo.Id);
                cmd.CreateParameter(DbType.Int32, "name_bgg", game.BGGInfo.Name);
                cmd.CreateParameter(DbType.Int32, "yearpublished_bgg", game.BGGInfo.YearPublished);
                cmd.CreateParameter(DbType.Int32, "rank_bgg", game.BGGInfo.Rank);
                cmd.CreateParameter(DbType.Double, "bayesaverage_bgg", game.BGGInfo.Bayesaverage);
                cmd.CreateParameter(DbType.Double, "average_bgg", game.BGGInfo.Average);
                cmd.CreateParameter(DbType.Int32, "usersrated_bgg", game.BGGInfo.Usersrated);
                cmd.CreateParameter(DbType.Boolean, "is_expansion_bgg", game.BGGInfo.IsExpansion);
                cmd.CreateParameter(DbType.Int32, "abstracts_rank_bgg", game.BGGInfo.AbstractsRank);
                cmd.CreateParameter(DbType.Int32, "cgs_rank_bgg", game.BGGInfo.CgsRank);
                cmd.CreateParameter(DbType.Int32, "childrensgames_rank_bgg", game.BGGInfo.ChildrensgamesRank);
                cmd.CreateParameter(DbType.Int32, "familygames_rank_bgg", game.BGGInfo.FamilygamesRank);
                cmd.CreateParameter(DbType.Int32, "partygames_rank_bgg", game.BGGInfo.PartygamesRank);
                cmd.CreateParameter(DbType.Int32, "strategygames_rank_bgg", game.BGGInfo.StrategygamesRank);
                cmd.CreateParameter(DbType.Int32, "thematic_rank_bgg", game.BGGInfo.ThematicRank);
                cmd.CreateParameter(DbType.Int32, "wargames_rank_bgg", game.BGGInfo.WargamesRank);
            }
            if (game.TeseraInfo != null)
            {
                cmd.CreateParameter(DbType.Int32, "id_tesera", game.TeseraInfo.Id);
                cmd.CreateParameter(DbType.String, "title_tesera", game.TeseraInfo.Title);
                cmd.CreateParameter(DbType.Int32, "tesera_id_tesera", game.TeseraInfo.TeseraId);
                cmd.CreateParameter(DbType.String, "alias_tesera", game.TeseraInfo.Alias);
                cmd.CreateParameter(DbType.String, "description_short_tesera", game.TeseraInfo.DescriptionShort);
                cmd.CreateParameter(DbType.String, "description_tesera", game.TeseraInfo.Description);
                cmd.CreateParameter(DbType.DateTime, "modification_date_utc_tesera", game.TeseraInfo.ModificationDateUtc);
                cmd.CreateParameter(DbType.DateTime, "creation_date_utc_tesera", game.TeseraInfo.CreationDateUtc);
                cmd.CreateParameter(DbType.String, "photo_url_tesera", game.TeseraInfo.PhotoUrl);
                cmd.CreateParameter(DbType.Int32, "year_tesera", game.TeseraInfo.Year);
                cmd.CreateParameter(DbType.Double, "rating_user_tesera", game.TeseraInfo.RatingUser);
                cmd.CreateParameter(DbType.Double, "n10rating_tesera", game.TeseraInfo.N10Rating);
                cmd.CreateParameter(DbType.Double, "n20rating_tesera", game.TeseraInfo.N20Rating);
                cmd.CreateParameter(DbType.Double, "bgg_rating_tesera", game.TeseraInfo.BGGRating);
                cmd.CreateParameter(DbType.Double, "bgg_geek_rating_tesera", game.TeseraInfo.BGGGeekRating);
                cmd.CreateParameter(DbType.Int32, "bgg_num_votes_tesera", game.TeseraInfo.BGGNumVotes);
                cmd.CreateParameter(DbType.Int32, "num_votes_tesera", game.TeseraInfo.NumVotes);
                cmd.CreateParameter(DbType.Int32, "players_min_tesera", game.TeseraInfo.PlaytimeMin);
                cmd.CreateParameter(DbType.Int32, "players_max_tesera", game.TeseraInfo.PlayersMax);
                cmd.CreateParameter(DbType.Int32, "players_min_recommend_tesera", game.TeseraInfo.PlayersMinRecommend);
                cmd.CreateParameter(DbType.Int32, "players_max_recommend_tesera", game.TeseraInfo.PlayersMaxRecommend);
                cmd.CreateParameter(DbType.Int32, "players_age_min_tesera", game.TeseraInfo.PlayersAgeMin);
                cmd.CreateParameter(DbType.Int32, "time_to_learn_tesera", game.TeseraInfo.TimeToLearn);
                cmd.CreateParameter(DbType.Int32, "playtime_min_tesera", game.TeseraInfo.PlaytimeMin);
                cmd.CreateParameter(DbType.Int32, "playtime_max_tesera", game.TeseraInfo.PlaytimeMax);
                cmd.CreateParameter(DbType.Int32, "comments_total_tesera", game.TeseraInfo.CommentsTotal);
                cmd.CreateParameter(DbType.Int32, "comments_total_new_tesera", game.TeseraInfo.CommentsTotalNew);
                cmd.CreateParameter(DbType.Boolean, "is_addition_tesera", game.TeseraInfo.IsAddition);
            }
            cmd.CreateParameter(DbType.Boolean, "is_addition", game.IsAddition);
            cmd.CreateParameter(DbType.Int32, "parent_id", game.ParentId);
            cmd.CreateParameter(DbType.DateTime, "dload", DateTime.Now);
            cmd.ExecuteNonQuery();
            game.Id = DBHelper.GetLastInsertedRowId(cmd);

            return game.Id;
        }

        public static void SaveGames(List<TeseraBGGRawGame> games)
        {
            using (var con = DBHelper.CreateConnection())
            {
                con.Open();
                using (var transaction = con.BeginTransaction())
                {
                    using (var cmd = con.CreateCommand())
                    {
                        cmd.Transaction = transaction;
                        try
                        {
                            foreach (var game in games)
                                SaveBGGRawGame(game, cmd);
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            Log.WriteError(ex);
                            throw;
                        }
                        transaction.Commit();
                    }
                }
            }
        }
    }
}
