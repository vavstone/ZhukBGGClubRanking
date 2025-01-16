using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZhukBGGClubRanking.Core
{
    public static class Extensions
    {
        public static string JoinToString<T>(this IEnumerable<T> list, string separator)
        {
            string res = string.Empty;
            if (list != null)
            {
                for (int i = 0; i < list.Count(); i++)
                {
                    res += list.ElementAt(i).ToString();
                    if (i + 1 < list.Count())
                        res += separator;
                }
            }
            return res;
        }
    }
}
