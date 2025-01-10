using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ZhukBGGClubRanking.Core.Code
{
    public static class ExceptionHandler
    {
        public static string GetExceptionDescrtiptionFromWebResult(HttpResponseMessage reqResult)
        {
            //if (reqResult != null)
            //{
            //    if (reqResult.Exception != null)
            //    {
            //        if (reqResult.Exception.InnerException != null)
            //        {
            //            if (reqResult.Exception.InnerException.InnerException != null)
            //            {
            //                return reqResult.Exception.InnerException.InnerException.Message;
            //            }
            //            return reqResult.Exception.InnerException.Message;
            //        }
            //        return reqResult.Exception.Message;
            //    }
            //}
            return string.Empty;
        }
    }
}
