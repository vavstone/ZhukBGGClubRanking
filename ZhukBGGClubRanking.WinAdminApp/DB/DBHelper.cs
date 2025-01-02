using Microsoft.Data.Sqlite;
using System;
using System.Configuration;
using System.Data;
using System.Data.Common;

namespace ZhukBGGClubRanking.WinAdminApp
{
    public static class DBHelper
    {
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
            return ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        }

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