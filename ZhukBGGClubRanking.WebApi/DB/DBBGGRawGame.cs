using System;
using System.Collections.Generic;
using System.Data;
using ZhukBGGClubRanking.Core;
using ZhukBGGClubRanking.Core.Model;
using ZhukBGGClubRanking.WebApi.Code;

namespace ZhukBGGClubRanking.WebApi.Core
{
    public static class DBBGGRawGame
    {

        //TODO
        


        public static List<BGGRawGame> GetGames()
        {
            var result = new List<BGGRawGame>();
            try
            {
                using (var con = DBHelper.CreateConnection())
                {
                    con.Open();
                    using (var cmd = con.CreateCommand())
                    {
                        cmd.CommandText = "select id, name, yearpublished, rank, bayesaverage,  average,  usersrated, is_expansion,  abstracts_rank, cgs_rank, " +
                            "childrensgames_rank, familygames_rank, partygames_rank, strategygames_rank, thematic_rank, wargames_rank from bgg_raw_info";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var item = new BGGRawGame();
                                item.Id = reader.GetFieldValue<Int32>("id");
                                item.Name = reader.GetFieldValueNullSafe<string>("name");
                                item.YearPublished = reader.GetFieldValueNullSafe<int?>("yearpublished");
                                item.Rank = reader.GetFieldValueNullSafe<int?>("rank");
                                item.Bayesaverage = reader.GetFieldValueNullSafe<double?>("bayesaverage");
                                item.Average = reader.GetFieldValueNullSafe<double?>("average");
                                item.Usersrated = reader.GetFieldValueNullSafe<int?>("usersrated");
                                item.IsExpansion = reader.GetFieldValueNullSafe<bool>("is_expansion");
                                item.AbstractsRank = reader.GetFieldValueNullSafe<int?>("abstracts_rank");
                                item.CgsRank = reader.GetFieldValueNullSafe<int?>("cgs_rank");
                                item.ChildrensgamesRank = reader.GetFieldValueNullSafe<int?>("childrensgames_rank");
                                item.FamilygamesRank = reader.GetFieldValueNullSafe<int?>("familygames_rank");
                                item.PartygamesRank = reader.GetFieldValueNullSafe<int?>("partygames_rank");
                                item.StrategygamesRank = reader.GetFieldValueNullSafe<int?>("strategygames_rank");
                                item.ThematicRank = reader.GetFieldValueNullSafe<int?>("thematic_rank");
                                item.WargamesRank = reader.GetFieldValueNullSafe<int?>("wargames_rank");
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