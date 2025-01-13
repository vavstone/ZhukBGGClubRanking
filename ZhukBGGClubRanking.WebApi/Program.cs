using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ZhukBGGClubRanking.Core;
using ZhukBGGClubRanking.Core.Model;
using ZhukBGGClubRanking.WebApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // ���������, ����� �� �������������� �������� ��� ��������� ������
            ValidateIssuer = true,
            // ������, �������������� ��������
            ValidIssuer =  AuthOptions.ISSUER,
            // ����� �� �������������� ����������� ������
            ValidateAudience = true,
            // ��������� ����������� ������
            ValidAudience = AuthOptions.AUDIENCE,
            // ����� �� �������������� ����� �������������
            ValidateLifetime = true,
            // ��������� ����� ������������
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            // ��������� ����� ������������
            ValidateIssuerSigningKey = true,
        };
    });

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();





// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}




//app.MapGet("/api/bggcollection", () =>
//{
//    var coll = RequestHandler.GetBggCollection();
//    return coll;
//}).WithName("GetBGGCollection");


app.MapPost("/api/login", (LoginPrm login) =>
{
    var user = DBUser.GetUserByName(login.UserName);
    var loginError = string.Empty;
    var loginResult = DBUser.Validate(user, login.PasswordCache, out loginError);
    if (!loginResult) return Results.Unauthorized();
    var claims = new List<Claim> { new Claim(ClaimTypes.Name, login.UserName) };

    // ������� JWT-�����
    var jwt = new JwtSecurityToken(
        issuer: AuthOptions.ISSUER,
        audience: AuthOptions.AUDIENCE,
        claims: claims,
        expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(WebAppSettings.TokenLifeTimeInMinutes)),//����� �������� 5 ����� (�������� �� ���������)
        signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
    var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

    // ��������� �����
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

app.MapPost("/api/initiatedb", [Authorize] (HttpContext context) => {
    if (!AuthUtils.IsUserAdmin(context))
        return;
    var currentUser = new User { Id = 1 };
    RequestHandler.InitiateDB(currentUser);
}).WithName("InitiateDB");

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

//app.MapPost("/api/saveteseragamesrawinfo", [Authorize] (List<TeseraRawGame> teseraGames, HttpContext context) => {
//    if (!AuthUtils.IsUserAdmin(context))
//        return;
//    RequestHandler.SaveTeseraRawInfoGames(teseraGames);
//}).WithName("SaveTeseraGamesRawInfo");

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

//app.MapPost("/api/updatebggcoll", [Authorize] (HttpContext context) => {

//    TaskWorker.LoadBGGCollectionToDB();
//}).WithName("UpdateBGGColl");

app.MapGet("/api/getgameimagebybggid", [Authorize] (HttpContext context, int bggid) =>
{
    var path = RequestHandler.GetGameImagePathByBGGId(bggid);
    var mimeType = Utils.GetContentTypeByFileExtension(Path.GetExtension(path));
    return Results.File(path, contentType: mimeType);
}).WithName("GetGameImageByBGGId");



app.MapGet("/test/getteststring", () => "test").WithName("GetTestString");



//app.MapGet("/test/getteststringauth", [Authorize] (HttpContext context) => "test").WithName("GetTestStringAuth");




app.Run();




public class AuthOptions
{
    public const string ISSUER = "zhukbggserver"; // �������� ������
    public const string AUDIENCE = "zhukbggclient"; // ����������� ������
    const string KEY = "riRKMrjJ92n5Foc4X1P5asdfasfdasdfvvxcdsg";   // ���� ��� ��������
    public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
}