using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using ZhukBGGClubRanking.Core.Model;
using ZhukBGGClubRanking.WebApi;
using ZhukBGGClubRanking.WebApi.Core;

namespace ZhukBGGClubRanking.Core
{
    public static class DBUser
    {

        public static User GetUserByName(string name)
        {
            try
            {
                using (var con = DBHelper.CreateConnection())
                {
                    con.Open();
                    using (var cmd = con.CreateCommand())
                    {
                        cmd.CommandText = "select id, password, email, full_name, is_active, create_time, role from users where upper(name)=@name";
                        cmd.CreateParameter(DbType.String, "name", name.ToUpper());
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var result = new User();
                                result.Id = reader.GetFieldValue<Int32>("id");
                                result.Name = name;
                                result.Password = reader.GetFieldValue<string>("password");
                                result.EMail = reader.GetFieldValue<string>("email");
                                result.FullName = reader.GetFieldValue<string>("full_name");
                                result.IsActive = reader.GetFieldValue<bool>("is_active");
                                result.CreateTime = reader.GetFieldValue<DateTime>("create_time");
                                result.Role = reader.GetFieldValue<string>("role");
                                return result;
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

            return null;
        }

        public static List<User> GetUsers()
        {
            var result = new List<User>();
            try
            {
                using (var con = DBHelper.CreateConnection())
                {
                    con.Open();
                    using (var cmd = con.CreateCommand())
                    {
                        cmd.CommandText = "select id, name, email, full_name, is_active, create_time, role from users";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var item = new User();
                                item.Id = reader.GetFieldValue<Int32>("id");
                                item.Name = reader.GetFieldValue<string>("name");
                                item.EMail = reader.GetFieldValue<string>("email");
                                item.FullName = reader.GetFieldValue<string>("full_name");
                                item.IsActive = reader.GetFieldValue<bool>("is_active");
                                item.CreateTime = reader.GetFieldValue<DateTime>("create_time");
                                item.Role = reader.GetFieldValue<string>("role");
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

        public static void CreateNewUser(User newUser)
        {
            try
            {
                using (var con = DBHelper.CreateConnection())
                {
                    con.Open();


                    using (var cmd = con.CreateCommand())
                    {

                        var currentTime = DateTime.Now;
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
                        cmd.CreateParameter(DbType.String, "email", newUser.EMail.ToLower());
                        cmd.CreateParameter(DbType.String, "full_name", newUser.FullName);
                        cmd.CreateParameter(DbType.String, "is_active", newUser.IsActive);
                        cmd.CreateParameter(DbType.String, "create_time", newUser.CreateTime);
                        cmd.CreateParameter(DbType.String, "role", newUser.Role);
                        cmd.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex)
            {
                Log.WriteError(ex);
                throw;
            }
        }

        public static bool Validate(User user, string passwordCacheToCompare, out string error)
        {
            error = null;
            if (user == null)
                error = "Пользователь с таким именем не найден!";
            else if (user.Password!=passwordCacheToCompare)
                error = "Неправильный пароль!";
            else if (!user.IsActive)
                error = "Пользователь отключен!";
            if (string.IsNullOrWhiteSpace(error))
                return true;
            return false;
        }
    }
}