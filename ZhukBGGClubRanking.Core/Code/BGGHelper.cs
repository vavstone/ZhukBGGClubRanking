using BoardGamer.BoardGameGeek.BoardGameGeekXmlApi2;
using System;
using System.Linq;
using System.Net.Http;

namespace ZhukBGGClubRanking.Core.Code
{
    public static class BGGHelper
    {
        public static ThingResponse.Item GetGame(int bggId)
        {
            try
            {
                var bgg = new BoardGameGeekXmlApi2Client(new HttpClient());
                var request = new ThingRequest(new[] { bggId });
                var response = bgg.GetThingAsync(request);
                return response.Result.Result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Log.WriteError(ex);
                return null;
            }
        }
    }
}
