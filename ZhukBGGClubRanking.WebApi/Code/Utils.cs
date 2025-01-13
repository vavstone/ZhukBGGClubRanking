namespace ZhukBGGClubRanking.WebApi
{
    public static class Utils
    {
        public static string GetFileExtensionByContentType(string contentType)
        {
            switch (contentType)
            {
                case "image/jpeg": return ".jpg";
                case "image/png": return ".png";
                default: return contentType.Replace('/','_');
            }
        }

        public static string GetContentTypeByFileExtension(string fileExtension)
        {
            switch (fileExtension)
            {
                case ".jpg": return "image/jpeg";
                case ".png": return "image/png";
                default: return "image/jpeg";
            }
        }
    }
}
