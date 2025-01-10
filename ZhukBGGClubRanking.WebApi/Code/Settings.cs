﻿using System.IO;

namespace ZhukBGGClubRanking.WebApi
{
    public static class WebApppSettings
    {
        private static IConfigurationRoot Configuration;
        static IConfigurationRoot Config
        {
            get
            {
                if (Configuration == null)
                {
                    var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json");
                    Configuration = builder.Build();
                }
                return Configuration;
            }
        }

        public static bool GetFromBGGLargeImagesForThumbnails
        {
            get { return bool.Parse(Config["AppSettings:GetFromBGGLargeImagesForThumbnails"]); }
        }

        public static int TokenLifeTimeInMinutes
        {
            get { return int.Parse(Config["AppSettings:TokenLifeTimeInMinutes"]); }
        }

        public static string ConnectionString
        {
            get { return Config["ConnectionStrings:ConnectionString"]; }
        }
    }
}
