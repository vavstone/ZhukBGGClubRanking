using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using BoardGamer.BoardGameGeek.BoardGameGeekXmlApi2;

namespace ZhukBGGClubRanking.Core.Code
{
    public static class BGGHelper
    {
        public static ThingResponse.Item GetGame(int bggId)
        {
            var bgg = new BoardGameGeekXmlApi2Client(new HttpClient());
            var request = new ThingRequest(new[] { bggId });
            var response = bgg.GetThingAsync(request);
            return response.Result.Result.FirstOrDefault();
        }
    }
}
