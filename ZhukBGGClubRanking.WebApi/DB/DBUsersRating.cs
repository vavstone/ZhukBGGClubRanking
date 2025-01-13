using System.Data;
using ZhukBGGClubRanking.Core.Model;
using ZhukBGGClubRanking.WebApi;
using ZhukBGGClubRanking.WebApi.Core;

namespace ZhukBGGClubRanking.Core
{
    public static class DBUsersRating
    {

        

        public static List<UsersRating> GetUsersActualRatings()
        {
            var result = new List<UsersRating>();
            try
            {
                using (var con = DBHelper.CreateConnection())
                {
                    con.Open();
                    using (var cmd = con.CreateCommand())
                    {
                        cmd.CommandText = "select r.id rating_id, r.user_id, r.create_time, i.id item_id, i.game_id, i.rating_order from users_ratings r join rating_items i on r.id=i.users_rating_id where r.expire_time is null order by r.id";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var rating_id = reader.GetFieldValue<Int32>("rating_id");
                                var rating = result.FirstOrDefault(c => c.Id == rating_id);
                                if (rating == null)
                                {
                                    rating = new UsersRating();
                                    rating.Id = rating_id;
                                    rating.CreateTime = reader.GetFieldValue<DateTime>("create_time");
                                    rating.UserId = reader.GetFieldValue<Int32>("user_id");
                                    result.Add(rating);
                                }
                                var item = new RatingItem();
                                item.Id = reader.GetFieldValue<Int32>("item_id");
                                item.GameId = reader.GetFieldValue<Int32>("game_id");
                                item.RatingOrder = reader.GetFieldValue<Int32>("rating_order");
                                rating.Rating.RatingItems.Add(item);
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

        public static void SaveRating(UsersRating rating)
        {
            try
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
                                var currentTime = DateTime.Now;
                                //1. найти актуальную запись (если такая есть) в users_ratings
                                cmd.CommandText = "select id from users_ratings where expire_time is null and user_id=@user_id";
                                cmd.CreateParameter(DbType.Int32, "user_id", rating.UserId);
                                var currentRatingId = 0;
                                using (var reader = cmd.ExecuteReader())
                                {
                                    if (reader.Read())
                                        currentRatingId = reader.GetFieldValue<Int32>("id");
                                }

                                //2. пометить актуальную запись (если такая есть) в users_ratings (п.1) устаревшей
                                if (currentRatingId > 0)
                                {
                                    cmd.CommandText =
                                        "update users_ratings set expire_time=@expire_time where id=@currentRatingId";
                                    cmd.Parameters.Clear();
                                    cmd.CreateParameter(DbType.Int32, "currentRatingId", currentRatingId);
                                    cmd.CreateParameter(DbType.DateTime, "expire_time", currentTime);
                                    cmd.ExecuteNonQuery();
                                }

                                //3. вставить записи в rating_items_history из rating_items для родительской строки из п.1 (если такие есть)
                                if (currentRatingId > 0)
                                {
                                    cmd.CommandText =
                                        "insert into rating_items_history (users_rating_id,game_id,rating_order) select i.users_rating_id,i.game_id,i.rating_order from rating_items i where i.users_rating_id=@currentRatingId";
                                    cmd.Parameters.Clear();
                                    cmd.CreateParameter(DbType.Int32, "currentRatingId", currentRatingId);
                                    cmd.ExecuteNonQuery();
                                }

                                //4. удалить записи из rating_items для родительской строки из п.1 (если такие есть)
                                if (currentRatingId > 0)
                                {
                                    cmd.Parameters.Clear();
                                    cmd.CommandText = "delete from rating_items where users_rating_id=@currentRatingId";
                                    cmd.CreateParameter(DbType.Int32, "currentRatingId", currentRatingId);
                                    cmd.ExecuteNonQuery();
                                }

                                //5. вставить новую запись в users_ratings
                                cmd.CommandText =
                                    "insert into users_ratings (create_time,user_id) values (@create_time,@user_id)";
                                cmd.Parameters.Clear();
                                cmd.CreateParameter(DbType.DateTime, "create_time", currentTime);
                                cmd.CreateParameter(DbType.Int32, "user_id", rating.UserId);
                                cmd.ExecuteNonQuery();
                                var newRatingId = DBHelper.GetLastInsertedRowId(cmd);

                                //6. вставить записи в rating_items
                                foreach (var ratingItem in rating.Rating.RatingItems.OrderBy(c => c.RatingOrder))
                                {
                                    cmd.CommandText =
                                        "insert into rating_items (users_rating_id,game_id,rating_order) values (@users_rating_id,@game_id,@rating_order)";
                                    cmd.Parameters.Clear();
                                    cmd.CreateParameter(DbType.Int32, "users_rating_id", newRatingId);
                                    cmd.CreateParameter(DbType.Int32, "game_id", ratingItem.GameId);
                                    cmd.CreateParameter(DbType.Int32, "rating_order", ratingItem.RatingOrder);
                                    cmd.ExecuteNonQuery();
                                }
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
            catch (Exception ex)
            {
                Log.WriteError(ex);
                throw;
            }
        }

    }
}