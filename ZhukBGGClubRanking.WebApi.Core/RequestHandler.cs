using ZhukBGGClubRanking.Core;

namespace ZhukBGGClubRanking.WebApi.Core
{
    public static class RequestHandler
    {
        public static BGGCollection GetBggCollection()
        {
            return BGGCollection.LoadFromFile();
        }
    }
}
