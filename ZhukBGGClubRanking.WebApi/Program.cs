using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
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
            // указывает, будет ли валидироваться издатель при валидации токена
            ValidateIssuer = true,
            // строка, представляющая издателя
            ValidIssuer =  AuthOptions.ISSUER,
            // будет ли валидироваться потребитель токена
            ValidateAudience = true,
            // установка потребителя токена
            ValidAudience = AuthOptions.AUDIENCE,
            // будет ли валидироваться время существования
            ValidateLifetime = true,
            // установка ключа безопасности
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            // валидация ключа безопасности
            ValidateIssuerSigningKey = true,
        };
    });

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
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

    // создаем JWT-токен
    var jwt = new JwtSecurityToken(
        issuer: AuthOptions.ISSUER,
        audience: AuthOptions.AUDIENCE,
        claims: claims,
        expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(5)),//нужно вычитать 5 минут (значение по умолчанию)
        signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
    var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

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


app.MapPost("/api/saveusersrating", [Authorize] (List<RatingItem> rating, HttpContext context) => {
    var userIdentity = context.User.Identity;
    var user = DBUser.GetUserByName(userIdentity.Name);
    var usersRating = new UsersRating { UserId = user.Id };
    usersRating.Rating.RatingItems = rating;
    RequestHandler.SaveUsersRating(usersRating);
}).WithName("SaveUsersRating");

app.MapPost("/api/createuserbyadmin", [Authorize] (User newUser, HttpContext context) => {
    var userIdentity = context.User.Identity;
    var activeUser = DBUser.GetUserByName(userIdentity.Name);
    if (activeUser == null || !activeUser.IsActive || activeUser.Role != Role.AdminRole)
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        return;
    }
    RequestHandler.CreateNewUser(newUser);
}).WithName("CreateUserByAdmin");

app.MapPost("/api/initiatedb", /*[Authorize]*/ (HttpContext context) => {
    //var userIdentity = context.User.Identity;
    //var activeUser = DBUser.GetUserByName(userIdentity.Name);
    //if (activeUser == null || !activeUser.IsActive || activeUser.Role != "admin")
    //{
    //    context.Response.StatusCode = StatusCodes.Status403Forbidden;
    //    return;
    //}
    var currentUser = new User { Id = 1 };
    RequestHandler.InitiateDB(currentUser);
}).WithName("InitiateDB");

app.MapPost("/api/saveratingstoCSVfiles", /*[Authorize]*/ (HttpContext context) => {
    //var userIdentity = context.User.Identity;
    //var activeUser = DBUser.GetUserByName(userIdentity.Name);
    //if (activeUser == null || !activeUser.IsActive || activeUser.Role != "admin")
    //{
    //    context.Response.StatusCode = StatusCodes.Status403Forbidden;
    //    return;
    //}
    RequestHandler.SaveRatingsToCSVFiles();
}).WithName("SaveRatingsToCSVFiles");



//app.MapPost("/api/updatebggcoll", [Authorize] (HttpContext context) => {

//    TaskWorker.LoadBGGCollectionToDB();
//}).WithName("UpdateBGGColl");




app.MapGet("/test/getteststring", () => "test").WithName("GetTestString");



//app.MapGet("/test/getteststringauth", [Authorize] (HttpContext context) => "test").WithName("GetTestStringAuth");




app.Run();




public class AuthOptions
{
    public const string ISSUER = "zhukbggserver"; // издатель токена
    public const string AUDIENCE = "zhukbggclient"; // потребитель токена
    const string KEY = "riRKMrjJ92n5Foc4X1P5asdfasfdasdfvvxcdsg";   // ключ для шифрации
    public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
}