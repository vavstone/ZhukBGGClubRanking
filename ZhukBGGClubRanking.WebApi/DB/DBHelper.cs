using System.Configuration;
using System.Data.Common;
using System.Data;
using Microsoft.Data.Sqlite;
using ZhukBGGClubRanking.Core;

namespace ZhukBGGClubRanking.WebApi.Core
{
    public static class DBHelper
    {
        public static IConfigurationRoot Configuration;


        //private static readonly string FactoryName = GetFactoryName();
        static readonly string ConnectionString = GetConnectionString();
        //static readonly DbProviderFactory Factory = DbProviderFactories.GetFactory(FactoryName);

        public static DbConnection CreateConnection()
        {
            return new SqliteConnection(ConnectionString);
        }

        public static void CreateParameter(this DbCommand command, DbType type, string name, object value)
        {
            if (value == null && (command.CommandType == CommandType.Text))
            {
                command.CommandText = command.CommandText.Replace("@" + name, "null");
                return;
            }
            var prm = command.CreateParameter();
            prm.DbType = type;
            prm.ParameterName = name;
            prm.Value = value;
            command.Parameters.Add(prm);
        }

        public static string GetConnectionString()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            return Configuration["ConnectionStrings:ConnectionString"];
        }
        //public static string GetFactoryName()
        //{
        //    var builder = new ConfigurationBuilder()
        //        .SetBasePath(Directory.GetCurrentDirectory())
        //        .AddJsonFile("appsettings.json");
        //    Configuration = builder.Build();
        //    return Configuration["FactoryName"];
        //}

        public static int GetLastInsertedRowId(DbCommand cmd)
        {
            cmd.CommandText = "SELECT last_insert_rowid()";
            cmd.Parameters.Clear();
            var res = cmd.ExecuteScalar();
            var resVal = (Int32)(Int64)res;
            return resVal;
        }
    }
}