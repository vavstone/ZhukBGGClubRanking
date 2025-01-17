using System.Data;
using ZhukBGGClubRanking.WebApi.Core;

namespace ZhukBGGClubRanking.WebApi.DB
{
    public static class DBTranslate
    {
        public static string GetTranslationCache(string textEng)
        {
            try
            {
                using (var con = DBHelper.CreateConnection())
                {
                    con.Open();
                    using (var cmd = con.CreateCommand())
                    {
                        cmd.CommandText = "select text_rus from translation_cache where text_eng=@text_eng";
                        cmd.CreateParameter(DbType.String, "text_eng", textEng);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())

                                return reader.GetFieldValue<string>("text_rus");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteError(ex);
                throw;
            }
            return null;
        }

        public static void SaveCache(Dictionary<string,string> dictionary)
        {
            try
            {
                using (var con = DBHelper.CreateConnection())
                {
                    con.Open();
                    using (var cmd = con.CreateCommand())
                    {
                        cmd.CommandText =
                            "INSERT OR IGNORE into translation_cache (text_eng,text_rus,dload) values (@text_eng,@text_rus,@dload)";
                        foreach (var p in dictionary)
                        {
                            cmd.Parameters.Clear();
                            cmd.CreateParameter(DbType.String, "text_eng", p.Key);
                            cmd.CreateParameter(DbType.String, "text_rus", p.Value);
                            cmd.CreateParameter(DbType.DateTime, "dload", DateTime.Now);
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
        }
    }
}
