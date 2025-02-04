using System.Data.Common;
using ZhukBGGClubRanking.WebApi.Core;

namespace ZhukBGGClubRanking.WebApi.DB
{
    public static class DBCommon
    {

        public static void ClearTable(DbCommand cmd, string tableToClear, int idToSkip)
        {
            cmd.CommandText = string.Format("delete from {0}{1}", tableToClear, idToSkip > 0 ? " where id!=" + idToSkip:"");
            cmd.ExecuteNonQuery();
            cmd.CommandText = string.Format("UPDATE SQLITE_SEQUENCE SET SEQ = {0} WHERE NAME = '{1}'", idToSkip > 0 ? idToSkip : "0", tableToClear);
            cmd.ExecuteNonQuery();
            //delete from sqlite_sequence where name = 'your_table';

        }
        public static void ClearDB()
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
                                //ClearLinksBGGTables(cmd);
                                
                                //очищаем таблицы БД: rating_items_history, rating_items, users_ratings, games, users(кроме записи 1 - admin), ...
                                ClearTable(cmd, "rating_items_history",0);
                                ClearTable(cmd, "rating_items", 0);
                                ClearTable(cmd, "users_ratings", 0);
                                ClearTable(cmd, "games_owners", 0);
                                ClearTable(cmd, "games", 0);
                                ClearTable(cmd, "users", 1);

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

        static void ClearLinksBGGTables(DbCommand cmd)
        {
            ClearTable(cmd, "bgunknownlinktype", 0);
            ClearTable(cmd, "game_bgunknownlinktype", 0);
            ClearTable(cmd, "game_bgaccessory", 0);
            ClearTable(cmd, "game_bgartist", 0);
            ClearTable(cmd, "game_bgcategory", 0);
            ClearTable(cmd, "game_bgdesigner", 0);
            ClearTable(cmd, "game_bgexpansion", 0);
            ClearTable(cmd, "game_bgfamily", 0);
            ClearTable(cmd, "game_bgimplementation", 0);
            ClearTable(cmd, "game_bgmechanic", 0);
            ClearTable(cmd, "game_bgpublisher", 0);
            ClearTable(cmd, "game_bgintegration", 0);
            ClearTable(cmd, "game_bgcompilation", 0);
        }

        public static void ClearLinksBGGTables()
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
                                ClearLinksBGGTables(cmd);
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

        public static void ClearTeseraRawTable()
        {
            try
            {
                using (var con = DBHelper.CreateConnection())
                {
                    con.Open();
                    using (var cmd = con.CreateCommand())
                    {
                        try
                        {
                            ClearTable(cmd, "tesera_raw_info", 0);
                        }
                        catch (Exception ex)
                        {
                            Log.WriteError(ex);
                            throw;
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

        public static void ClearBGGRawTable()
        {
            try
            {
                using (var con = DBHelper.CreateConnection())
                {
                    con.Open();
                    using (var cmd = con.CreateCommand())
                    {
                        try
                        {
                            ClearTable(cmd, "bgg_raw_info", 0);
                        }
                        catch (Exception ex)
                        {
                            Log.WriteError(ex);
                            throw;
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

        public static void ClearBGGTeseraRawTable()
        {
            try
            {
                using (var con = DBHelper.CreateConnection())
                {
                    con.Open();
                    using (var cmd = con.CreateCommand())
                    {
                        try
                        {
                            ClearTable(cmd, "bgg_tesera_raw_info", 0);
                        }
                        catch (Exception ex)
                        {
                            Log.WriteError(ex);
                            throw;
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
