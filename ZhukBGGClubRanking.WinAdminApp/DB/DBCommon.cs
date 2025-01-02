using System;
using System.Data.Common;

namespace ZhukBGGClubRanking.WinAdminApp
{
    public static class DBCommon
    {

        public static void ClearTable(DbCommand cmd, string tableToClear)
        {
            cmd.CommandText = string.Format("delete from {0}", tableToClear);
            cmd.ExecuteNonQuery();
            cmd.CommandText = string.Format("UPDATE SQLITE_SEQUENCE SET SEQ = 0 WHERE NAME = '{0}'", tableToClear);
            cmd.ExecuteNonQuery();
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
                            ClearTable(cmd, "tesera_raw_info");
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
