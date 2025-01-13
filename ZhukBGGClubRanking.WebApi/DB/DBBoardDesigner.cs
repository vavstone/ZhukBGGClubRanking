using BoardGamer.BoardGameGeek.BoardGameGeekXmlApi2;
using System.Data;
using System.Data.Common;
using ZhukBGGClubRanking.WebApi;
using ZhukBGGClubRanking.WebApi.Core;

namespace ZhukBGGClubRanking.Core
{
    public static class DBBGGLinks
    {
        public static void SaveLinksForBGGGame(ThingResponse.Item bggGame)
        {
            if (bggGame == null || bggGame.Links==null) return;
            try
            {
                using (var con = DBHelper.CreateConnection())
                {
                    con.Open();


                    using (var cmd = con.CreateCommand())
                    {
                        foreach (var linkGroup in bggGame.Links.GroupBy(c=>c.Type))
                        {
                            SaveLinkGroup(cmd,linkGroup);
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

        static void SaveLinkGroup(DbCommand cmd, IGrouping<string, ThingResponse.Link> linkGroup)
        {
            cmd.Parameters.Clear();
            //TODO сохранить категорию линка, если такой нет
            foreach (var item in linkGroup.ToList())
            {
                cmd.Parameters.Clear();
                //TODO сохранить связи (перезаписать, если не соответствуют)
            }
            /*
            //1. найти актуальную запись (если такая есть) в users
            cmd.CommandText = "select id from users where upper(name)=@name and upper(@email)=@email";
            cmd.CreateParameter(DbType.String, "name", newUser.Name.ToUpper());
            cmd.CreateParameter(DbType.String, "email", newUser.EMail.ToUpper());
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                    throw new Exception("Пользователь с таким именем или email уже существует!");
            }

            //2. вставить запись в users

            cmd.CommandText =
                "insert into users (name,password,email,full_name,is_active,create_time,role) values (@name,@password,@email,@full_name,@is_active,@create_time,@role)";
            cmd.Parameters.Clear();
            cmd.CreateParameter(DbType.String, "name", newUser.Name);
            cmd.CreateParameter(DbType.String, "password", newUser.Password);
            cmd.CreateParameter(DbType.String, "email", newUser.EMail);
            cmd.CreateParameter(DbType.String, "full_name", newUser.FullName);
            cmd.CreateParameter(DbType.Boolean, "is_active", newUser.IsActive);
            cmd.CreateParameter(DbType.DateTime, "create_time", newUser.CreateTime);
            cmd.CreateParameter(DbType.String, "role", newUser.Role);
            cmd.CreateParameter(DbType.Int32, "create_user_id", newUser.CreateUserId);
            cmd.ExecuteNonQuery();*/
        }
    }
}
