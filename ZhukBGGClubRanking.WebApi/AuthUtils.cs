using ZhukBGGClubRanking.Core;

namespace ZhukBGGClubRanking.WebApi
{
    public static class AuthUtils
    {
        public static bool IsUserAdmin(HttpContext context)
        {
            var userIdentity = context.User.Identity;
            var activeUser = DBUser.GetUserByName(userIdentity.Name);
            if (activeUser == null || !activeUser.IsActive || activeUser.Role != "admin")
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return false;
            }
            return true;
        }
    }
}
