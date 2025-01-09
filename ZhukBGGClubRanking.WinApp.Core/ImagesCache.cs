using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ZhukBGGClubRanking.Core.Model;
using static System.Net.Mime.MediaTypeNames;

namespace ZhukBGGClubRanking.WinApp.Core
{
    public static class ImagesCache
    {
        //public static string GetTeseraPicPath(TeseraRawGame game)
        //{
        //    if (game == null) return null;
        //    var teseraPicPath = Settings.CacheImagesTeseraPicDir;
        //    if (!Directory.Exists(teseraPicPath))
        //        Directory.CreateDirectory(teseraPicPath);
        //    foreach (var picFile in Directory.GetFiles(teseraPicPath))
        //    {
        //        if (Path.GetFileNameWithoutExtension(picFile) == game.TeseraId.Value.ToString())
        //            return picFile;
        //    }
        //    using (var client = new WebClient())
        //    {
        //        client.Headers.Add("Referer", "https://tesera.ru/game/fleet-dice");
        //        string imgFileName = game.TeseraId.Value.ToString();
        //        var fileExt = ".jpg";
        //        var imgNewFileName = Path.Combine(teseraPicPath, imgFileName+fileExt);
        //        client.DownloadFile(game.PhotoUrl, imgNewFileName);
        //        return imgNewFileName;
        //    }
        //}

        public static string GetGamePicPath(TeseraBGGRawGame game)
        {
            if (game == null) return null;
            var teseraPicPath = Settings.CacheImagesTeseraPicDir;
            var bggPicPath = Settings.CacheImagesBGGPicDir;
            if (!Directory.Exists(teseraPicPath))
                Directory.CreateDirectory(teseraPicPath);
            if (!Directory.Exists(bggPicPath))
                Directory.CreateDirectory(bggPicPath);
            if (game.TeseraInfo != null)
            {
                foreach (var picFile in Directory.GetFiles(teseraPicPath))
                {
                    if (Path.GetFileNameWithoutExtension(picFile) == game.TeseraInfo.TeseraId.Value.ToString())
                        return picFile;
                }
            }
            if (game.BGGInfo != null)
            {
                foreach (var picFile in Directory.GetFiles(bggPicPath))
                {
                    if (Path.GetFileNameWithoutExtension(picFile) == game.BGGInfo.Id.ToString())
                        return picFile;
                }
            }

            using (var client = new WebClient())
            {
                client.Headers.Add("Referer", "https://tesera.ru/game/fleet-dice");
                string imgFileName = game.TeseraId.Value.ToString();
                var fileExt = ".jpg";
                var imgNewFileName = Path.Combine(teseraPicPath, imgFileName + fileExt);
                client.DownloadFile(game.PhotoUrl, imgNewFileName);
                return imgNewFileName;
            }
        }
    }
}
