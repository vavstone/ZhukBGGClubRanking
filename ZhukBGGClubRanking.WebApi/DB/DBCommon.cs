﻿using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using ZhukBGGClubRanking.Core.Model;
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
    }
}