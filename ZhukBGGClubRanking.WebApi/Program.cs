using Microsoft.AspNetCore.Authorization;
using ZhukBGGClubRanking.Core;
using ZhukBGGClubRanking.Core.Model;
using ZhukBGGClubRanking.WebApi;
using ZhukBGGClubRanking.WebApi.Code;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();

var jwtSecretKey = "riRKMrjJ92n5Foc4X1P5asdfasfdasdfvvxcdsg";
//var jwtSecretKey = "password123casdsadsaiodiasdsadas";
var tokenExpiryMinutes = WebAppSettings.TokenLifeTimeInMinutes;

builder.Services.AddJwtAuthentication(jwtSecretKey);

var app = builder.Build();

//app.UseDefaultFiles();
//app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

#region ForTestOnly

/*app.MapPost("/token", (HttpContext context) =>
{
    // Generate JWT token
    var generatedToken = TokenGenerator.GenerateToken(jwtSecretKey, tokenExpiryMinutes, "token_user");
    return generatedToken;
    //return  TokenGenerator.GenerateTokenEndpoint(jwtSecretKey, tokenExpiryMinutes);
}).WithName("Token").AllowAnonymous();

app.MapGet("/getteststring", () => "test").WithName("GetTestString").AllowAnonymous();

app.MapGet("/getauthstring", () => "testauth").WithName("GetAuthString").RequireAuthorization();*/

#endregion

app.MapPost("/api/login", (LoginPrm login) =>
{
    var user = DBUser.GetUserByName(login.UserName);
    var loginError = string.Empty;
    var loginResult = DBUser.Validate(user, login.PasswordCache, out loginError);
    if (!loginResult) return Results.Unauthorized();
    //var claims = new List<Claim> { new Claim(ClaimTypes.Name, login.UserName) };

    // создаем JWT-токен
    var encodedJwt = TokenGenerator.GenerateToken(jwtSecretKey, tokenExpiryMinutes, login.UserName);
    // формируем ответ
    var response = new
    {
        access_token = encodedJwt,
        username = login.UserName
    };
    return Results.Json(response);

}).WithName("Login");

app.MapGet("/api/getusers", [Authorize] (HttpContext context) =>
{
    var coll = RequestHandler.GetUsers();
    return coll;
}).WithName("GetUsers");

app.MapGet("/api/getgamescollection", [Authorize] (HttpContext context) =>
{
    var users = DBUser.GetUsers();
    var coll = RequestHandler.GetGamesCollection(users);
    return coll;
}).WithName("GetGamesCollection");

app.MapGet("/api/getusersactualratings", [Authorize] (HttpContext context) =>
{
    var coll = RequestHandler.GetUsersActualRatings();
    return coll;
}).WithName("GetUsersActualRatings");

app.MapGet("/api/getrawgames", [Authorize] (HttpContext context) =>
{
    var games = RequestHandler.GetRawGamesShortInfo();
    return games;
}).WithName("GetRawGames");


app.MapPost("/api/saveusersrating", [Authorize] (List<RatingItem> rating, HttpContext context) => {
    var userIdentity = context.User.Identity;
    var user = DBUser.GetUserByName(userIdentity.Name);
    var usersRating = new UsersRating { UserId = user.Id };
    usersRating.Rating.RatingItems = rating;
    RequestHandler.SaveUsersRating(usersRating);
}).WithName("SaveUsersRating");

app.MapPost("/api/createuserbyadmin", [Authorize] (User newUser, HttpContext context) => {
    if (!AuthUtils.IsUserAdmin(context))
        return;
    RequestHandler.CreateNewUser(newUser);
}).WithName("CreateUserByAdmin");

app.MapPost("/api/initiatedb", /*[Authorize]*/ (HttpContext context) =>
{
    //if (!AuthUtils.IsUserAdmin(context))
    //    return;
    var currentUser = new User { Id = 1 };
    RequestHandler.InitiateDB(currentUser);
}).WithName("InitiateDB");

//app.MapPost("/api/clearlinksbggtables", /*[Authorize]*/ (HttpContext context) => {
//    //if (!AuthUtils.IsUserAdmin(context))
//    //    return;
//    RequestHandler.ClearLinksBGGTables();
//}).WithName("ClearLinksBGGTables");

//app.MapPost("/api/updatebgglinks", /*[Authorize]*/ (HttpContext context) => {
//    //if (!AuthUtils.IsUserAdmin(context))
//    //    return;
//    RequestHandler.UpdateBGGLinks();
//}).WithName("UpdateBGGLinks");

app.MapPost("/api/saveratingstoCSVfiles", [Authorize] (HttpContext context) => {
    if (!AuthUtils.IsUserAdmin(context))
        return;
    RequestHandler.SaveRatingsToCSVFiles();
}).WithName("SaveRatingsToCSVFiles");


app.MapPost("/api/clearteserarawtable", [Authorize] (HttpContext context) => {
    if (!AuthUtils.IsUserAdmin(context))
        return;
    RequestHandler.ClearTeseraRawTable();
}).WithName("ClearTeseraRawTable");

app.MapPost("/api/clearbggrawtable", [Authorize] (HttpContext context) => {
    if (!AuthUtils.IsUserAdmin(context))
        return;
    RequestHandler.ClearBGGRawTable();
}).WithName("ClearBGGRawTable");

app.MapPost("/api/clearbggteserarawtable", [Authorize] (HttpContext context) => {
    if (!AuthUtils.IsUserAdmin(context))
        return;
    RequestHandler.ClearBGGTeseraRawTable();
}).WithName("ClearBGGTeseraRawTable");


app.MapPost("/api/saveteseragamesrawinfo", [Authorize] (HttpContext context) => {
    if (!AuthUtils.IsUserAdmin(context))
        return;
    var teseraGames = TeseraRawGameParseWrapper.LoadGamesFromJsonFiles();
    RequestHandler.SaveTeseraRawInfoGames(teseraGames);
}).WithName("SaveTeseraGamesRawInfo");

app.MapPost("/api/savebggandteseragamesrawinfo", [Authorize] (HttpContext context) => {
    if (!AuthUtils.IsUserAdmin(context))
        return;
    RequestHandler.SaveBGGAndTeseraRawGames();
}).WithName("SaveBGGAndTeseraGamesRawInfo");

app.MapPost("/api/addgamesforuser", [Authorize] (List<Game> games, HttpContext context) => {
    var userIdentity = context.User.Identity;
    var user = DBUser.GetUserByName(userIdentity.Name);
    RequestHandler.AddGamesForUser(games,user);
}).WithName("AddGamesForUser");

app.MapPost("/api/removegamesfromuser", [Authorize] (List<Game> games, HttpContext context) => {
    var userIdentity = context.User.Identity;
    var user = DBUser.GetUserByName(userIdentity.Name);
    RequestHandler.RemoveGamesFromUser(games, user);
}).WithName("RemoveGamesFromUser");


app.MapGet("/api/getgameimagebybggid", [Authorize] (HttpContext context, int bggid) =>
{
    var path = RequestHandler.GetGameImagePathByBGGId(bggid);
    var mimeType = Utils.GetContentTypeByFileExtension(Path.GetExtension(path));
    return Results.File(path, contentType: mimeType);
}).WithName("GetGameImageByBGGId");


app.MapGet("/test/getteststring", () => "test").WithName("GetTestString");




app.Run();

//app.MapGet("/test/getteststringauth", [Authorize] (HttpContext context) => "test").WithName("GetTestStringAuth");

//app.MapPost("/api/updatebggcoll", [Authorize] (HttpContext context) => {

//    TaskWorker.LoadBGGCollectionToDB();
//}).WithName("UpdateBGGColl");

//app.MapPost("/api/saveteseragamesrawinfo", [Authorize] (List<TeseraRawGame> teseraGames, HttpContext context) => {
//    if (!AuthUtils.IsUserAdmin(context))
//        return;
//    RequestHandler.SaveTeseraRawInfoGames(teseraGames);
//}).WithName("SaveTeseraGamesRawInfo");






//public class AuthOptions
//{
//    public const string ISSUER = "zhukbggserver"; // издатель токена
//    public const string AUDIENCE = "zhukbggclient"; // потребитель токена
//    const string KEY = "riRKMrjJ92n5Foc4X1P5asdfasfdasdfvvxcdsg";   // ключ для шифрации
//    public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
//        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
//}

//app.MapGet("/api/bggcollection", () =>
//{
//    var coll = RequestHandler.GetBggCollection();
//    return coll;
//}).WithName("GetBGGCollection");