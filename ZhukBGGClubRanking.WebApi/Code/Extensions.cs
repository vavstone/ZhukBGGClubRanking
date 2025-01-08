using System.Data;
using System.Data.Common;

namespace ZhukBGGClubRanking.WebApi.Code
{
    public static class Extensions
    {
        public static T GetFieldValueNullSafe <T>(this DbDataReader reader, string fieldName)
        {
            return reader.IsDBNull(fieldName) ? default(T) : reader.GetFieldValue<T>(fieldName);
        }
    }
}
